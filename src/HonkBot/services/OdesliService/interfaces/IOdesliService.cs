using HonkBot.Models.Odesli;

namespace HonkBot.Services;

/// <summary>
/// Interface for defining a service class for handling API calls to Odesli.
/// </summary>
public interface IOdesliService
{
    /// <summary>
    /// Get data from Odesli for a given music streaming service URL.
    /// </summary>
    /// <param name="inputUrl">A URL from a music streaming service.</param>
    /// <returns><see cref="MusicEntityItem" /></returns>
    Task<MusicEntityItem> GetShareLinksAsync(string inputUrl);
}