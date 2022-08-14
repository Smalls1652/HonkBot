namespace HonkBot.Models.Odesli;

public interface IPlatformEntityLink
{
    string? EntityUniqueId { get; set; }
    Uri? Url { get; set; }
    Uri? NativeAppUriMobile { get; set; }
    Uri? NativeAppUriDesktop { get; set; }
}