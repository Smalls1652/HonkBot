using Discord.WebSocket;

namespace HonkBot.Models.Services;

public interface IDiscordService
{
    Task Connect();
}