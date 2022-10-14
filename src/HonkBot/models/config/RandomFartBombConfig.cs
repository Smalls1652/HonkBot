using System.Text.Json.Serialization;

namespace HonkBot.Models.Config;

/// <summary>
/// Configuration for the random fart bomb feature in a specific server.
/// </summary>
public class RandomFartBombConfig : IRandomFartBombConfig
{
    /// <inheritdoc cref="IRandomFartBombConfig.Enabled"/>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}