using System.Net;
using System.Text.Json;
using HonkBot.Models.Odesli;
using Microsoft.Extensions.Logging;

namespace HonkBot.Models.Services;

public class OdesliService : IOdesliService
{
    private readonly ILogger<OdesliService> _logger;
    private readonly HttpClient _httpClient = new();
    private readonly Uri _baseUri = new("https://api.song.link/v1-alpha.1/");
    public OdesliService(ILogger<OdesliService> logger)
    {
        _logger = logger;
        _httpClient.BaseAddress = _baseUri;
    }

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