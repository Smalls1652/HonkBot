using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HonkBot.Models.Odesli;
using HonkBot.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Updates the music share links when a user clicks the refresh button component.
    /// </summary>
    /// <param name="entityUrl">The Odesli URL for the music item entity.</param>
    /// <returns></returns>
    [ComponentInteraction(customId: "refresh-musiclinks-btn-*")]
    private async Task HandleRefreshMusicShareLinksBtnAsync(string entityUrl)
    {
        await Context.Interaction.DeferAsync(
            ephemeral: false
        );

        _logger.LogInformation("Refreshing music share links for {url}", entityUrl);

        // Get the music entity item from Odesli.
        MusicEntityItem musicEntityItem = await GetMusicEntityItemAsync(entityUrl);

        // Generate the music share components.
        ComponentBuilder linksComponentBuilder = GenerateMusicShareComponents(musicEntityItem);

        // Modify the message with the updated components.
        await Context.Interaction.ModifyOriginalResponseAsync(
            messageProps => messageProps.Components = linksComponentBuilder.Build()
        );

        _logger.LogInformation("Refreshed music share links for {url}", entityUrl);
    }
}