using System.Text.Json.Serialization;

namespace HonkBot.Models.Odesli;

public class MusicEntityItem : IMusicEntityItem
{
    [JsonPropertyName("entityUniqueId")]
    public string? EntityUniqueId { get; set; }

    [JsonPropertyName("userCountry")]
    public string? UserCountry { get; set; }

    [JsonPropertyName("pageUrl")]
    public Uri? PageUrl { get; set; }

    [JsonPropertyName("linksByPlatform")]
    public Dictionary<string, PlatformEntityLink>? LinksByPlatform { get; set; }

    [JsonPropertyName("entitiesByUniqueId")]
    public Dictionary<string, StreamingEntityItem>? EntitiesByUniqueId { get; set; }
}