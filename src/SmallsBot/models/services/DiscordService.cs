using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmallsBot.Models.Services;

public class DiscordService : IDiscordService
{
    private readonly ILogger<DiscordService> _logger;
    private readonly IConfiguration _config;

    public DiscordService (ILogger<DiscordService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public DiscordSocketClient DiscordClient
    {
        get => _discordClient;
    }
    private readonly DiscordSocketClient _discordClient = new();

    public async Task Connect()
    {
        _logger.LogInformation("Connecting client.");

        if (_config.GetValue<string>("DiscordClientToken") is null)
        {
            throw new Exception("DiscordClientToken environment variable not found.");
        }

        await _discordClient.LoginAsync(TokenType.Bot, _config.GetValue<string>("DiscordClientToken"));
        await _discordClient.StartAsync();
    }
}