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

        PlatformEntityLink? itunesLink;
        try
        {
            itunesLink = musicEntityItem.LinksByPlatform!["itunes"];
        }
        catch
        {
            _logger.LogError("No iTunes link found for {musicShareUrl}", musicShareUrl);
            await FollowupAsync(
                text: "I was unable to get the necessary information from Odesli. :(",
                ephemeral: true
            );
            return;
        }

        PlatformEntityLink? youtubeLink;
        try
        {
            youtubeLink = musicEntityItem.LinksByPlatform!["youtube"];
        }
        catch
        {
            _logger.LogWarning("No YouTube link found for {musicShareUrl}", musicShareUrl);
            youtubeLink = null;
        }

        PlatformEntityLink? appleMusicLink;
        try
        {
            appleMusicLink = musicEntityItem.LinksByPlatform!["appleMusic"];
        }
        catch
        {
            _logger.LogWarning("No Apple Music link found for {musicShareUrl}", musicShareUrl);
            appleMusicLink = null;
        }

        PlatformEntityLink? spotifyLink;
        try
        {
            spotifyLink = musicEntityItem.LinksByPlatform!["spotify"];
        }
        catch
        {
            _logger.LogWarning("No Spotify link found for {musicShareUrl}", musicShareUrl);
            spotifyLink = null;
        }

        StreamingEntityItem itunes = musicEntityItem.EntitiesByUniqueId![itunesLink.EntityUniqueId!];

        using HttpClient httpClient = new();
        HttpResponseMessage responseMessage = await httpClient.GetAsync(itunes.ThumbnailUrl);
        Stream imageStream = await responseMessage.Content.ReadAsStreamAsync();

        ButtonBuilder youtubeButton;
        if (youtubeLink is not null)
        {
            youtubeButton = new(
                label: "YouTube",
                style: ButtonStyle.Link,
                url: youtubeLink.Url!.ToString()
            );
        }
        else
        {
            youtubeButton = new(
                label: "YouTube 🚫",
                style: ButtonStyle.Secondary,
                isDisabled: true,
                customId: $"{musicEntityItem.EntityUniqueId}-youtube-disabled"
            );
        }

        ButtonBuilder appleMusicButton;
        if (appleMusicLink is not null)
        {
            appleMusicButton = new(
                label: "Apple Music",
                style: ButtonStyle.Link,
                url: appleMusicLink.Url!.ToString()
            );
        }
        else
        {
            appleMusicButton = new(
                label: "Apple Music 🚫",
                style: ButtonStyle.Secondary,
                isDisabled: true,
                customId: $"{musicEntityItem.EntityUniqueId}-appleMusic-disabled"
            );
        }

        ButtonBuilder spotifyButton;
        if (spotifyLink is not null)
        {
            spotifyButton = new(
                label: "Spotify",
                style: ButtonStyle.Link,
                url: spotifyLink.Url!.ToString()
            );
        }
        else
        {
            spotifyButton = new(
                label: "Spotify 🚫",
                style: ButtonStyle.Secondary,
                isDisabled: true,
                customId: $"{musicEntityItem.EntityUniqueId}-spotify-disabled"
            );
        }

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