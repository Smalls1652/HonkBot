using Discord;
using Discord.Interactions;
using HonkBot.Models.Tools;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Send a message, secretly, as HonkBot.
    /// </summary>
    /// <param name="message">The message to send as HonkBot.</param>
    [RequireOwner]
    [SlashCommand(name: "honk-message", description: "Send a message, secretly, as HonkBot.")]
    private async Task HandleSendMessageAsHonk(
        [Summary(name: "message", description: "The message to send as HonkBot.")]
        string message
    )
    {
        await DeferAsync(
            ephemeral: true
        );

        await Context.Channel.SendMessageAsync(
            text: message
        );

        await FollowupAsync(
            text: "Message sent. ;)",
            ephemeral: true
        );
    }
}