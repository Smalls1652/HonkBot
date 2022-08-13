using Discord.Interactions;
using ImageMagick;

namespace HonkBot.Commands;

public partial class ImageCommandsModule : InteractionModuleBase
{
    private static MemoryStream ResizeEmote(Stream emoteImgStream, MagickImageInfo emoteImgInfo)
    {
        byte[] emoteImgResizedByteArray;

        if (emoteImgInfo.Format == MagickFormat.Gif)
        {
            using MagickImageCollection emoteImgCol = new(emoteImgStream);
            emoteImgCol.Coalesce();

            foreach (IMagickImage image in emoteImgCol)
            {
                image.Resize(256, 256);
            }
            emoteImgResizedByteArray = emoteImgCol.ToByteArray();
        }
        else
        {
            using MagickImage emoteImg = new(emoteImgStream);
            emoteImg.Resize(256, 256);
            emoteImgResizedByteArray = emoteImg.ToByteArray();
        }

        MemoryStream emoteImgResizedStream = new(emoteImgResizedByteArray);

        return emoteImgResizedStream;
    }
}