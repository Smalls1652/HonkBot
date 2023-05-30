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
    [EnabledInDm(false)]
    [MessageCommand(name: "Drop fart bomb")]
    public async Task HandleHonkFartAsync(IMessage message)
    {
        // Respond back to the Discord client that the command is being processed.
        await DeferAsync(
            ephemeral: true
        );

        _logger.LogInformation("'{Username}' called the 'Drop fart bomb' command on message '{MessageId}'.", Context.User.Username, message.Id);

        char dirSep = Path.DirectorySeparatorChar;
        
        // Get the content of the fart bomb video.
        FileStream fileContents = File.Open(
            path: Path.Combine(Environment.CurrentDirectory, $"assets{dirSep}video{dirSep}sharding.mp4"),
            mode: FileMode.Open,
            access: FileAccess.Read
        );

        // Send a message with the video as a reply to the message provided.
        await Context.Channel.SendFileAsync(
            text: null,
            stream: fileContents,
            filename: "sharding.mp4",
            messageReference: new MessageReference(
                messageId: message.Id
            )
        );

        // Tell the user that the fart bomb has been dropped.
        await FollowupAsync(
            text: "fart bomb dropped",
            ephemeral: true
        );

        // Close the file stream and dispose it.
        fileContents.Close();
        fileContents.Dispose();
    }
}