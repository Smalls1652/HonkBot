using System.Net;
using System.Text;
using System.Text.Json;
using HonkBot.Models.Config;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace HonkBot.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <inheritdoc cref="ICosmosDbService.AddOrUpdateServerConfigAsync(ServerConfig)"/>
    public async Task AddOrUpdateServerConfigAsync(ServerConfig serverConfig)
    {
        Container container = _cosmosDbClient.GetContainer(_databaseId, "server-configs");

        try
        {
            _logger.LogInformation("Updating server config ({id}) for guild ID '{guildId}'.", serverConfig.Id, serverConfig.GuildId);

            // Attempt to update the server config in the database.
            await container.ReplaceItemAsync(
                item: serverConfig,
                id: serverConfig.Id,
                partitionKey: new(serverConfig.PartitionKey)
            );
        }
        catch (CosmosException dbException) when (dbException.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation("Server config for guild ID '{guildId}' not found. Creating new entry in database.", serverConfig.GuildId);
            Stream serverConfigJsonStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(serverConfig)));
            string serverConfigJson = JsonSerializer.Serialize(serverConfig);
            _logger.LogInformation("Server config JSON: {serverConfigJson}", serverConfigJson);

            // If the server config doesn't exist in the database already, create it.
            await container.CreateItemStreamAsync(
                streamPayload: serverConfigJsonStream,
                partitionKey: new(serverConfig.PartitionKey)
            );
        }
    }
}