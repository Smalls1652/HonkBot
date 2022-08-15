using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Has HonkBot send an angry message.
    /// </summary>
    /// <param name="message">A message for HonkBot to angrily send.</param>
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
}