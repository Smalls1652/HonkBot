using Discord;
using Discord.Interactions;
using HonkBot.Models.Odesli;
using HonkBot.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Gets share links for a song on multiple music streaming services and returns it to the client.
    /// </summary>
    /// <param name="musicShareUrl">A URL from a music streaming service.</param>
    [EnabledInDm(isEnabled: true)]
    [SlashCommand(name: "sharemusic", description: "Get share links for multiple streaming music services.")]
    private async Task HandleGetMusicShareLinks(
        [Summary(name: "url", description: "A URL of a song/album generated from a music streaming service.")]
        string musicShareUrl
    )
    {
        await DeferAsync();

        // Get the music entity item from Odesli.
        MusicEntityItem musicEntityItem = await GetMusicEntityItemAsync(musicShareUrl);

        // Attempt to get the iTunes platform info from the music entity item.
        PlatformEntityLink? itunesLink;
        try
        {
            itunesLink = musicEntityItem.LinksByPlatform!["itunes"];
        }
        catch
        {
            // If the music entity item doesn't have an iTunes link,
            // throw an error, send a follow-up message to the user, and prematurely end the command.
            _logger.LogError("No iTunes link found for {musicShareUrl}", musicShareUrl);
            await FollowupAsync(
                text: "I was unable to get the necessary information from Odesli. :(",
                ephemeral: true
            );
            return;
        }

        // Get the album art from the streaming entity item for iTunes.
        StreamingEntityItem itunes = musicEntityItem.EntitiesByUniqueId![itunesLink.EntityUniqueId!];
        Stream imageStream = await GetMusicEntityItemAlbumArt(itunes);

        // Generate the music share components.
        ComponentBuilder linksComponentBuilder = GenerateMusicShareComponents(musicEntityItem);

        // Send the follow-up message with the music share links.
        await FollowupWithFileAsync(
            text: $"Streaming music links for **{itunes.Title} by {itunes.ArtistName}**.",
            fileStream: imageStream,
            fileName: $"{itunes.Title}.jpg",
            components: linksComponentBuilder.Build()
        );
    }
}