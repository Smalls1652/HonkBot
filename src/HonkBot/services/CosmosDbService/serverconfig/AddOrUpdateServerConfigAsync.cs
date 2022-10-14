using System.Net;
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
            _logger.LogInformation("Updating server config for guild ID '{guildId}'.", serverConfig.GuildId);

            // Attempt to update the server config in the database.
            await container.ReplaceItemAsync(
                item: serverConfig,
                id: serverConfig.Id,
                partitionKey: new(serverConfig.PartitionKey)
            );
        }
        catch (CosmosException dbException)
        {
            _logger.LogInformation("Server config for guild ID '{guildId}' not found. Creating new entry in database.", serverConfig.GuildId);
            // If the server config doesn't exist in the database already, create it.
            if (dbException.StatusCode == HttpStatusCode.NotFound)
            {
                await container.CreateItemAsync(
                item: serverConfig,
                partitionKey: new(serverConfig.PartitionKey)
            );
            }
        }
    }
}