namespace HonkBot.Models.Odesli;

public interface IMusicEntityItem
{
    string? EntityUniqueId { get; set; }
    string? UserCountry { get; set; }
    Uri? PageUrl { get; set; }
    Dictionary<string, PlatformEntityLink>? LinksByPlatform { get; set; }
    Dictionary<string, StreamingEntityItem>? EntitiesByUniqueId { get; set; }
}