using Discord;

namespace HonkBot.Services;

public interface IDiscordService
{
    Task Connect();

    Task SetGameStatus(string? status, ActivityType activityType);
}