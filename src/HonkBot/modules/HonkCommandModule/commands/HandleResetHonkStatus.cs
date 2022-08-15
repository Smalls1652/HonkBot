using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Changes HonkBot's presence status on Discord.
    /// </summary>
    /// <param name="status">The status to set.</param>
    /// <param name="activityType">The type of activity to show.</param>
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