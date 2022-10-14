using System.Text.Json.Serialization;

namespace HonkBot.Models.Config;

/// <summary>
/// Configurations for a specific Discord server.
/// </summary>
public class ServerConfig : IServerConfig
{
    /// <summary>
    /// Default constructor for the <see cref="ServerConfig"/> class.
    /// </summary>
    public ServerConfig()
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "server-config-item";
        RandomReactConfig = new RandomReactConfig();
        RandomFartBombConfig = new RandomFartBombConfig();
    }

    /// <inheritdoc cref="IServerConfig.Id"/>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <inheritdoc cref="IServerConfig.PartitionKey"/>
    [JsonPropertyName("partitionKey")]
    public string PartitionKey  { get; }

    /// <inheritdoc cref="IServerConfig.GuildId"/>
    [JsonPropertyName("guildId")]
    public ulong GuildId { get; set; }

    /// <inheritdoc cref="IServerConfig.RandomReactConfig"/>
    [JsonPropertyName("randomReactConfig")]
    public RandomReactConfig RandomReactConfig { get; }

    /// <inheritdoc cref="IServerConfig.RandomFartBombConfig"/>
    [JsonPropertyName("randomFartBombConfig")]
    public RandomFartBombConfig RandomFartBombConfig { get; }
}