namespace HonkBot.Models.Config;

/// <summary>
/// Interface that defines the configuration for the random fart bomb feature in a specific server.
/// </summary>
public interface IRandomFartBombConfig
{
    /// <summary>
    /// Whether the random fart bomb feature is enabled in the server.
    /// </summary>
    bool Enabled { get; set; }
}