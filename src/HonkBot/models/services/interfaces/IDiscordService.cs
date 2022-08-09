using Discord;

namespace HonkBot.Models.Services;

public interface IDiscordService
{
    Task Connect();

    Task SetGameStatus(string? status, ActivityType activityType);
}