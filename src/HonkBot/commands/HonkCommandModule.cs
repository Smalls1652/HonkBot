using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using HonkBot.Models.Services;

namespace HonkBot.Commands;

public class HonkCommandModule : InteractionModuleBase
{
    private readonly ILogger<HonkCommandModule> _logger;
    public HonkCommandModule(ILogger<HonkCommandModule> logger)
    {
        _logger = logger;
    }

    [SlashCommand(name: "honk-gm", description: "honk says gm to a user")]
    public async Task HandleHonkGmSlashCommand(IUser user)
    {
        _logger.LogInformation("'{Username}' called the 'honk-gm' command.", Context.User.Username);
        await RespondWithFileAsync(
            text: $"gm {user.Mention}",
            allowedMentions: AllowedMentions.All,
            filePath: "honk.png"
        );
    }

    [SlashCommand(name: "honk-angry", description:"honk mad")]
    public async Task HandleHonkAngryResponseCommand(string message)
    {
        _logger.LogInformation("'{Username}' called the 'honk-angry' command.", Context.User.Username);
        await RespondWithFileAsync(
            text: message,
            filePath: "honk-gon-get-ya.png"
        );
    }
}