using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Honk... Drops a fart bomb...
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [RequireUserPermission(ChannelPermission.SendMessages)]
    [MessageCommand(name: "HonkShard")]
    public async Task HandleHonkFartAsync(IMessage message)
    {
        _logger.LogInformation("'{Username}' called the 'HonkFart' command on message '{MessageId}'.", Context.User.Username, message.Id);

        FileStream fileContents = File.Open(
            path: Path.Combine(Environment.CurrentDirectory, "assets/audio/fart.mp3"),
            mode: FileMode.Open,
            access: FileAccess.Read
        );

        await Context.Channel.SendFileAsync(
            text: null,
            stream: fileContents,
            filename: "shard.mp3",
            messageReference: new MessageReference(
                messageId: message.Id
            )
        );

        await RespondAsync(
            text: "fart bomb dropped",
            ephemeral: true
        );

        fileContents.Close();
        fileContents.Dispose();
    }
}