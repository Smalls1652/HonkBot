using Discord;
using Discord.Interactions;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

public partial class ImageCommandsModule : InteractionModuleBase
{
    /// <summary>
    /// Takes an emote and makes it bigger.
    /// </summary>
    /// <param name="emote">The emote to make bigger.</param>
    [RequireUserPermission(ChannelPermission.SendMessages)]
    [SlashCommand(name: "hugemoji", description: "Make an emote honking huge.")]
    private async Task HandleHonkingHugemoji(
        [Summary(description: "The emote to make honking huge.")]
        string emote
    )
    {
        Emote? parsedEmote;
        try
        {
            _logger.LogInformation("Parsing '{emote}'.", emote);
            parsedEmote = Emote.Parse(emote);

            if (parsedEmote is null)
            {
                throw new Exception("Emote parsed was null.");
            }
        }
        catch
        {
            _logger.LogInformation("Failed to parse the emote.");
            await RespondAsync(
                text: "Failed to parse emote. >:(",
                ephemeral: true
            );

            return;
        }

        try
        {
            _logger.LogInformation("Attempting to get emote from '{Url}'.", parsedEmote.Url);
            SocketsHttpHandler httpHandler = new()
            {
                ConnectTimeout = TimeSpan.FromSeconds(2)
            };
            Stream? emoteImgStream;
            using (HttpClient httpClient = new(httpHandler, true))
            {
                HttpRequestMessage requestMessage = new(
                    method: HttpMethod.Get,
                    requestUri: parsedEmote!.Url
                );

                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);

                emoteImgStream = await responseMessage.Content.ReadAsStreamAsync();
            }


            if (emoteImgStream is null)
            {
                throw new Exception("Emote image has a null file stream.");
            }
            else
            {
                _logger.LogInformation("Attempting to resize emote.");
                byte[] emoteImgResizedByteArray;

                // We need to create a copy of the download image's stream.
                // Whenever ImageMagick reads the image, it closes the stream
                // and causes future use of the stream to fail.
                MemoryStream emoteImgStreamCopy = new();
                await emoteImgStream.CopyToAsync(emoteImgStreamCopy);

                // Reset the position of both the original and copy to the beginning.
                emoteImgStream.Position = 0;
                emoteImgStreamCopy.Position = 0;

                MagickImageInfo emoteImgInfo = new(emoteImgStreamCopy);
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

                _logger.LogInformation("Sending resized emote.");
                await RespondWithFileAsync(
                    fileName: $"{parsedEmote.Name}.{emoteImgInfo.Format.ToString().ToLower()}",
                    fileStream: emoteImgResizedStream
                );

                await emoteImgStream.DisposeAsync();
                await emoteImgStreamCopy.DisposeAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation("Failed to resize the emote. Falling back to sending the raw emote image.");

            _logger.LogError(
                exception: e,
                message: "Error message: {Message}",
                args: e.Message
            );
            await RespondAsync(
                embed: new EmbedBuilder()
                {
                    ImageUrl = Emote.Parse(emote).Url
                }.Build()
            );
        }
    }
}