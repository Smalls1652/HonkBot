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

        MusicEntityItem musicEntityItem = await _odesliService.GetShareLinksAsync(musicShareUrl);

        PlatformEntityLink youtubeLink = musicEntityItem.LinksByPlatform!["youtube"];
        PlatformEntityLink itunesLink = musicEntityItem.LinksByPlatform!["itunes"];
        PlatformEntityLink appleMusicLink = musicEntityItem.LinksByPlatform!["appleMusic"];
        PlatformEntityLink spotifyLink = musicEntityItem.LinksByPlatform!["spotify"];

        StreamingEntityItem itunes = musicEntityItem.EntitiesByUniqueId![itunesLink.EntityUniqueId!];

        using HttpClient httpClient = new();
        HttpResponseMessage responseMessage = await httpClient.GetAsync(itunes.ThumbnailUrl);
        Stream imageStream = await responseMessage.Content.ReadAsStreamAsync();

        ButtonBuilder youtubeButton = new(
            label: "YouTube",
            style: ButtonStyle.Link,
            url: youtubeLink.Url!.ToString()
        );

        ButtonBuilder appleMusicButton = new(
            label: "Apple Music",
            style: ButtonStyle.Link,
            url: appleMusicLink.Url!.ToString()
        );

        ButtonBuilder spotifyButton = new(
            label: "Spotify",
            style: ButtonStyle.Link,
            url: spotifyLink.Url!.ToString()
        );

        ButtonBuilder moreLinksButton = new(
            label: "More links",
            style: ButtonStyle.Link,
            url: musicEntityItem.PageUrl!.ToString()
        );

        ComponentBuilder linksComponentBuilder = new ComponentBuilder()
            .WithButton(youtubeButton)
            .WithButton(appleMusicButton)
            .WithButton(spotifyButton)
            .WithButton(moreLinksButton);

        await FollowupWithFileAsync(
            text: $"Streaming music links for **{itunes.Title} by {itunes.ArtistName}**.",
            fileStream: imageStream,
            fileName: $"{itunes.Title}.jpg",
            components: linksComponentBuilder.Build()
        );
    }
}