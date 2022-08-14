using Discord;
using Discord.Interactions;
using HonkBot.Models.Odesli;
using HonkBot.Models.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

public partial class HonkCommandModule : InteractionModuleBase
{
    [SlashCommand(name: "sharemusic", description: "Get share links for multiple streaming music services.")]
    private async Task HandleGetMusicShareLinks(string musicShareUrl)
    {
        await DeferAsync();

        MusicEntityItem musicEntityItem = await _odesliService.GetShareLinksAsync(musicShareUrl);

        PlatformEntityLink youtubeLink = musicEntityItem.LinksByPlatform!["youtube"];
        PlatformEntityLink itunesLink = musicEntityItem.LinksByPlatform!["itunes"];
        PlatformEntityLink appleMusicLink = musicEntityItem.LinksByPlatform!["appleMusic"];
        PlatformEntityLink spotifyLink = musicEntityItem.LinksByPlatform!["spotify"];

        StreamingEntityItem itunes = musicEntityItem.EntitiesByUniqueId![itunesLink.EntityUniqueId!];
        StreamingEntityItem youtube = musicEntityItem.EntitiesByUniqueId![youtubeLink.EntityUniqueId!];
        StreamingEntityItem appleMusic = musicEntityItem.EntitiesByUniqueId![appleMusicLink.EntityUniqueId!];
        StreamingEntityItem spotify = musicEntityItem.EntitiesByUniqueId![spotifyLink.EntityUniqueId!];

        using HttpClient httpClient = new();
        HttpResponseMessage responseMessage = await httpClient.GetAsync(itunes.ThumbnailUrl);
        Stream imageStream = await responseMessage.Content.ReadAsStreamAsync();

        List<Embed> embedItems = new();
        embedItems.Add(
            new EmbedBuilder()
            {
                Title = "Play on YouTube!",
                Description = $"Play {itunes.Title} by {itunes.ArtistName} on YouTube.",
                Url = youtubeLink.Url!.ToString(),
                ThumbnailUrl = youtube.ThumbnailUrl!.ToString()
            }.Build()
        );

        embedItems.Add(
            new EmbedBuilder()
            {
                Title = "Play on Apple Music!",
                Description = $"Play {itunes.Title} by {itunes.ArtistName} on Apple Music.",
                Url = appleMusicLink.Url!.ToString(),
                ThumbnailUrl = appleMusic.ThumbnailUrl!.ToString()
            }.Build()
        );

        embedItems.Add(
            new EmbedBuilder()
            {
                Title = "Play on Spotify!",
                Description = $"Play {itunes.Title} by {itunes.ArtistName} on Spotify.",
                Url = spotifyLink.Url!.ToString(),
                ThumbnailUrl = spotify.ThumbnailUrl!.ToString()
            }.Build()
        );

        await FollowupWithFileAsync(
            text: $"Share links for **{itunes.Title}** by {itunes.ArtistName}.",
            fileStream: imageStream,
            fileName: $"{itunes.Title}.jpg",
            embeds: embedItems.ToArray()
        );
    }
}