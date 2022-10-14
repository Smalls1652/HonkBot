using System.Text.Json.Serialization;

namespace HonkBot.Models.Config;

/// <summary>
/// Configuration for the random fart bomb feature in a specific server.
/// </summary>
public class RandomFartBombConfig : IRandomFartBombConfig
{
    /// <summary>
    /// Default constructor for the <see cref="RandomFartBombConfig"/> class.
    /// </summary>
    public RandomFartBombConfig()
    {
        Enabled = false;
    }

    /// <inheritdoc cref="IRandomFartBombConfig.Enabled"/>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}