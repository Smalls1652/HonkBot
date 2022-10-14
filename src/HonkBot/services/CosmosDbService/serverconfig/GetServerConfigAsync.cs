using HonkBot.Models.Config;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace HonkBot.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <inheritdoc cref="ICosmosDbService.GetServerConfigAsync(string)"/>
    /// <exception cref="Exception">Thrown when the database fails to return a result.</exception>
    public async Task<ServerConfig> GetServerConfigAsync(string guildId)
    {
        _logger.LogInformation("Getting server config for guild ID '{guildId}'.", guildId);
        List<ServerConfig> serverConfigs = new();

        // Create a query to get the server config from the database.
        Container container = _cosmosDbClient.GetContainer(_databaseId, "server-configs");
        QueryDefinition queryDef = new($"SELECT * FROM c WHERE c.guildId = \"{guildId}\"");
        FeedIterator<ServerConfig> queryResults = container.GetItemQueryIterator<ServerConfig>(queryDef);

        // Iterate through the results and add them to the list.
        while (queryResults.HasMoreResults)
        {
            foreach (ServerConfig serverConfig in await queryResults.ReadNextAsync())
            {
                serverConfigs.Add(serverConfig);
            }
        }

        // If there are no results, throw an exception.
        if (serverConfigs.Count == 0)
        {
            _logger.LogError("No server config found for guild ID '{guildId}'.", guildId);
            throw new Exception($"No server config found for guild ID '{guildId}'.");
        }

        // Only return the first result as there should only be one.
        return serverConfigs.FirstOrDefault()!;
    }
}