using HonkBot.Models.Odesli;

namespace HonkBot.Models.Services;

public interface IOdesliService
{
    Task<MusicEntityItem> GetShareLinksAsync(string inputUrl);
}