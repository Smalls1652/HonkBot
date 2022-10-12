using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Has HonkBot send a "gm" message to a user in a Discord server.
    /// </summary>
    /// <param name="user">The user to have HonkBot say gm to.</param>
    [RequireUserPermission(ChannelPermission.SendMessages)]
    [EnabledInDm(isEnabled: false)]
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
}