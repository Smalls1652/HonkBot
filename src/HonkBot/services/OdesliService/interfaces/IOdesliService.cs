using HonkBot.Models.Odesli;

namespace HonkBot.Services;

public interface IOdesliService
{
    Task<MusicEntityItem> GetShareLinksAsync(string inputUrl);
}