using Discord;

namespace HonkBot.Services;

/// <summary>
/// Interface that defines properties for a Discord Service class that handles bot calls.
/// </summary>
public interface IDiscordService
{
    /// <summary>
    /// Connects to the Discord API.
    /// </summary>
    Task Connect();

    /// <summary>
    /// Sets the status of HonkBot.
    /// </summary>
    /// <param name="status">The status message to set.</param>
    /// <param name="activityType">The <see cref="ActivityType" /> to set.</param>
    Task SetGameStatus(string? status, ActivityType activityType);
}