namespace HonkBot.Models.Config;

/// <summary>
/// Interface that defines the configurations for a specific Discord server.
/// </summary>
public interface IServerConfig
{
    /// <summary>
    /// A unique identifier for the server and it's config.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The partition key for the database.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// The Discord Server's Guild ID.
    /// </summary>
    ulong  GuildId { get; set; }

    /// <summary>
    /// Configuration for the random react feature in a specific server.
    /// </summary>
    RandomReactConfig RandomReactConfig { get; set; }

    /// <summary>
    /// Configuration for the random fart bomb feature in a specific server.
    /// </summary>
    RandomFartBombConfig RandomFartBombConfig { get; set; }
}