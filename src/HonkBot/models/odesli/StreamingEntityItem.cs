using System.Text.Json.Serialization;

namespace HonkBot.Models.Odesli;

public class StreamingEntityItem : IStreamingEntityItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("artistName")]
    public string? ArtistName { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public Uri? ThumbnailUrl { get; set; }

    [JsonPropertyName("thumbnailWidth")]
    public int? ThumbnailWidth { get; set; }

    [JsonPropertyName("thumbnailHeight")]
    public int? ThumbnailHeight { get; set; }

    [JsonPropertyName("apiProvider")]
    public string? ApiProvider { get; set; }

    [JsonPropertyName("platform")]
    public string[]? Platform { get; set; }
}