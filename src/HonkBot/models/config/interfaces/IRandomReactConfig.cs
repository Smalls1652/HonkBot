namespace HonkBot.Models.Config;

/// <summary>
/// Defines the configuration for the random react feature in a specific server.
/// </summary>
public interface IRandomReactConfig
{
    /// <summary>
    /// Whether the random react feature is enabled in the server.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// The chance for a message to have a random emote reaction added to it.
    /// </summary>
    int PercentChanceToHappen { get; set; }
}