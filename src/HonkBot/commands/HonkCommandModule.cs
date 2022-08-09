using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HonkBot.Models.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

public class HonkCommandModule : InteractionModuleBase
{
    private readonly IDiscordService _discordService;
    private readonly ILogger<HonkCommandModule> _logger;
    public HonkCommandModule(IDiscordService discordService, ILogger<HonkCommandModule> logger)
    {
        _discordService = discordService;
        _logger = logger;
    }

    [RequireUserPermission(ChannelPermission.SendMessages)]
    [SlashCommand(name: "honk-gm", description: "honk says gm to a user")]
    public async Task HandleHonkGmSlashCommand(
        [Summary(description: "The user to have HonkBot say gm to.")]
        IUser user
    )
    {
        _logger.LogInformation("'{Username}' called the 'honk-gm' command.", Context.User.Username);
        await RespondWithFileAsync(
            text: $"gm {user.Mention}",
            allowedMentions: AllowedMentions.All,
            filePath: "honk.png"
        );
    }

    [RequireUserPermission(ChannelPermission.SendMessages)]
    [SlashCommand(name: "honk-angry", description:"honk mad")]
    public async Task HandleHonkAngryResponseCommand(
        [Summary(description: "A message for HonkBot to angrily send.")]
        string message
    )
    {
        _logger.LogInformation("'{Username}' called the 'honk-angry' command.", Context.User.Username);
        await RespondWithFileAsync(
            text: message,
            filePath: "honk-gon-get-ya.png"
        );
    }

    [DefaultMemberPermissions(GuildPermission.Administrator)]
    [SlashCommand("reset-honk-status", "Reset honk's status")]
    public async Task HandleResetHonkStatus(
        [Summary(description: "The status you want to set for HonkBot.")]
        string status = "honking away",
        [Summary(description: "The type of activity you want to show."), Choice("Playing", 0), Choice("Streaming", 1), Choice("Listening", 2), Choice("Watching", 3), Choice("Competing", 5)]
        int activityType = 0
    )
    {
        ActivityType activityTypeConverted = (ActivityType)activityType;
        await _discordService.SetGameStatus(status, activityTypeConverted);

        await RespondAsync(
            text: "Updated HonkBot's status.",
            ephemeral: true
        );
    }
}