using Discord.Interactions;
using ImageMagick;

namespace HonkBot.Commands;

public partial class ImageCommandsModule : InteractionModuleBase
{
    /// <summary>
    /// Resize the emote image from it's normal 108x108 resolution to 256x256.
    /// </summary>
    /// <param name="emoteImgStream">The stream of the emote image.</param>
    /// <param name="emoteImgInfo">The emote's image info.</param>
    /// <returns>A <see cref="MemoryStream" /> of the resized emote image.</returns>
    private static MemoryStream ResizeEmote(Stream emoteImgStream, MagickImageInfo emoteImgInfo)
    {
        // Initialize the resized emote's byte array.
        byte[] emoteImgResizedByteArray;

        // Resized the image based off the format of the image.
        if (emoteImgInfo.Format == MagickFormat.Gif)
        {
            // If the image is a GIF, we need to handle it differently by 
            // looping through each image in the GIF and resizing them.
            using MagickImageCollection emoteImgCol = new(emoteImgStream);
            emoteImgCol.Coalesce();
            foreach (IMagickImage image in emoteImgCol)
            {
                image.Resize(256, 256);
            }

            // Write the resized GIF to the byte array.
            emoteImgResizedByteArray = emoteImgCol.ToByteArray();
        }
        else
        {
            // If it's any other format (Typically a JPEG or PNG),
            // resize it normally.
            using MagickImage emoteImg = new(emoteImgStream);
            emoteImg.Resize(256, 256);

            // Write the resized image to the byte array.
            emoteImgResizedByteArray = emoteImg.ToByteArray();
        }

        // Return the resized image's MemoryStream.
        return new(emoteImgResizedByteArray);
    }
}