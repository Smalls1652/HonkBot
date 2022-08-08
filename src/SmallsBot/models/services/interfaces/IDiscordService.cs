using Discord.WebSocket;

namespace SmallsBot.Models.Services;

public interface IDiscordService
{
    DiscordSocketClient DiscordClient { get; }

    Task Connect();
}