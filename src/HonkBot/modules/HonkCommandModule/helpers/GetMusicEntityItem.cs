using Discord;
using Discord.Interactions;
using HonkBot.Models.Odesli;
using HonkBot.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Get the <see cref="MusicEntityItem" /> of a given URL.
    /// </summary>
    /// <param name="url">URL for a music share link.</param>
    /// <returns>A <See cref="MusicEntityItem">MusicEntityItem</See>.</returns>
    private async Task<MusicEntityItem> GetMusicEntityItemAsync(string url)
    {
        return await _odesliService.GetShareLinksAsync(url);
    }
}