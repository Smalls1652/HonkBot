using System.Text.Json.Serialization;

namespace HonkBot.Models.Odesli;

public class PlatformEntityLink : IPlatformEntityLink
{
    [JsonPropertyName("entityUniqueId")]
    public string? EntityUniqueId { get; set; }

    [JsonPropertyName("url")]
    public Uri? Url { get; set; }

    [JsonPropertyName("nativeAppUriMobile")]
    public Uri? NativeAppUriMobile { get; set; }

    [JsonPropertyName("nativeAppUriDesktop")]
    public Uri? NativeAppUriDesktop { get; set; }
}