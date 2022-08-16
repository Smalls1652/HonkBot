using System.Net;
using System.Text.Json;
using HonkBot.Models.Odesli;
using Microsoft.Extensions.Logging;

namespace HonkBot.Services;

/// <summary>
/// A service for handling API calls to Odesli.
/// </summary>
public class OdesliService : IOdesliService
{
    private readonly ILogger<OdesliService> _logger;
    private readonly HttpClient _httpClient = new();
    private readonly Uri _baseUri = new("https://api.song.link/v1-alpha.1/");

    /// <summary>
    /// Initializes <see cref="OdesliService" /> for use.
    /// </summary>
    /// <param name="logger">The logger assigned to <see cref="OdesliService" /> for dependency injection.</param>
    public OdesliService(ILogger<OdesliService> logger)
    {
        _logger = logger;
        _httpClient.BaseAddress = _baseUri;
    }

    /// <inheritdoc cref="IOdesliService.GetShareLinksAsync(string)" />
    public async Task<MusicEntityItem> GetShareLinksAsync(string inputUrl)
    {
        _logger.LogInformation("Getting share links for '{inputUrl}'.", inputUrl);
        string encodedUrl = WebUtility.UrlEncode(inputUrl);
        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"links?url={encodedUrl}"
        );

        HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);
        string responseContentString = await responseMessage.Content.ReadAsStringAsync();

        MusicEntityItem? musicEntityItem = JsonSerializer.Deserialize<MusicEntityItem>(
            json: responseContentString
        );

        if (musicEntityItem is null)
        {
            throw new NullReferenceException("Music entity was null.");
        }

        requestMessage.Dispose();
        responseMessage.Dispose();

        return musicEntityItem;
    }
}