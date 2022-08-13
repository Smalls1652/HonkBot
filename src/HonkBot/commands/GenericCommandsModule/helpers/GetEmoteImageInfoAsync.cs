using Discord.Interactions;
using ImageMagick;

namespace HonkBot.Commands;

public partial class ImageCommandsModule : InteractionModuleBase
{
    private static async Task<MagickImageInfo> GetEmoteImageInfoAsync(Stream emoteImgStream)
    {
        MemoryStream emoteImgStreamCopy = new();
        await emoteImgStream.CopyToAsync(emoteImgStreamCopy);

        // Reset the position of both the original and copy to the beginning.
        emoteImgStream.Position = 0;
        emoteImgStreamCopy.Position = 0;

        return new(emoteImgStreamCopy);
    }
}