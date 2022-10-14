using Discord;
using Discord.Interactions;
using HonkBot.Models.Odesli;
using HonkBot.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Get the album art of a given <See cref="StreamingEntityItem" /> in a <see cref="MusicEntityItem" />.
    /// </summary>
    /// <param name="streamingEntityItem">The streaming entity item.</param>
    /// <returns>A stream of the album art image.</returns>
    private static async Task<Stream> GetMusicEntityItemAlbumArt(StreamingEntityItem streamingEntityItem)
    {
        // Get the album art from the streaming entity item.
        using HttpClient httpClient = new();
        HttpResponseMessage responseMessage = await httpClient.GetAsync(streamingEntityItem.ThumbnailUrl);
        Stream imageStream = await responseMessage.Content.ReadAsStreamAsync();

        return imageStream;
    }
}