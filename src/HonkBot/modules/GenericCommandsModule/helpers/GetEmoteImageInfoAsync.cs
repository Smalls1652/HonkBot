using Discord.Interactions;
using ImageMagick;

namespace HonkBot.Modules;

public partial class ImageCommandsModule : InteractionModuleBase
{
    /// <summary>
    /// Gets the emote's image file info.
    /// </summary>
    /// <param name="emoteImgStream">The stream of the emote image.</param>
    /// <returns>The emote's image file info.</returns>
    private static async Task<MagickImageInfo> GetEmoteImageInfoAsync(Stream emoteImgStream)
    {
        // Copy the passed Stream to a MemoryStream instance.
        // We need to do this to prevent the passed Stream from being closed too early.
        MemoryStream emoteImgStreamCopy = new();
        await emoteImgStream.CopyToAsync(emoteImgStreamCopy);

        // Reset the position of both the original and copy to the beginning.
        emoteImgStream.Position = 0;
        emoteImgStreamCopy.Position = 0;

        // Return the MagickImageInfo from the copied Stream.
        return new(emoteImgStreamCopy);
    }
}