using HonkBot.Models.Config;

namespace HonkBot.Services;

/// <summary>
/// Interface that defines the methods for interacting with the Azure CosmosDB database.
/// </summary>
public interface ICosmosDbService
{
    /// <summary>
    /// Gets a server config from the database.
    /// </summary>
    /// <param name="guildId">The Discord Server guild ID to look up.</param>
    /// <returns>The <see cref="ServerConfig" /> item.</returns>
    Task<ServerConfig> GetServerConfigAsync(string guildId);

    /// <summary>
    /// Add or update a server config in the database.
    /// </summary>
    /// <param name="serverConfig">The <see cref="ServerConfig" /> to add/update.</param>
    Task AddOrUpdateServerConfigAsync(ServerConfig serverConfig);
}