using Discord.WebSocket;

namespace SmallsBot.Models.Services;

public interface IDiscordService
{
    Task Connect();
}