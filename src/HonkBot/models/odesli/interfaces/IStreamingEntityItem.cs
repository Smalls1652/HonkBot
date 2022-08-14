namespace HonkBot.Models.Odesli;

public interface IStreamingEntityItem
{
    string? Id { get; set; }
    string? Title { get; set; }
    string? ArtistName { get; set; }
    Uri? ThumbnailUrl { get; set; }
    int? ThumbnailWidth { get; set; }
    int? ThumbnailHeight { get; set; }
    string? ApiProvider { get; set; }
    string[]? Platform { get; set; }
}