using System.Text.Json.Serialization;

namespace HonkBot.Models.Config;

/// <summary>
/// Configurations for a specific Discord server.
/// </summary>
public class ServerConfig : IServerConfig
{
    /// <summary>
    /// Default constructor for <see cref="ServerConfig" />.
    /// </summary>
    [JsonConstructor]
    public ServerConfig()
    {
        RandomReactConfig = new();
        RandomFartBombConfig = new();
    }

    /// <summary>
    /// Constructor for <see cref="ServerConfig" /> with a provided guild ID.
    /// </summary>
    public ServerConfig(string guildId)
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "server-config-item";
        GuildId = guildId;
        RandomReactConfig = new RandomReactConfig();
        RandomFartBombConfig = new RandomFartBombConfig();
    }

    /// <inheritdoc cref="IServerConfig.Id"/>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <inheritdoc cref="IServerConfig.PartitionKey"/>
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <inheritdoc cref="IServerConfig.GuildId"/>
    [JsonPropertyName("guildId")]
    public string GuildId { get; set; } = null!;

    /// <inheritdoc cref="IServerConfig.RandomReactConfig"/>
    [JsonPropertyName("randomReactConfig")]
    public RandomReactConfig RandomReactConfig { get; set; }

    /// <inheritdoc cref="IServerConfig.RandomFartBombConfig"/>
    [JsonPropertyName("randomFartBombConfig")]
    public RandomFartBombConfig RandomFartBombConfig { get; set; }
}