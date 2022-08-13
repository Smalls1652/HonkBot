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
        // Parse the emote passed through.
        Emote? parsedEmote;
        try
        {
            _logger.LogInformation("Parsing '{emote}'.", emote);
            parsedEmote = Emote.Parse(emote);

            // Throw an exception if the parse returned null.
            if (parsedEmote is null)
            {
                throw new Exception("Emote parsed was null.");
            }
        }
        catch
        {
            // Throw an error back to the client that the emote failed to parse.
            _logger.LogInformation("Failed to parse the emote.");
            await RespondAsync(
                text: "Failed to parse emote. >:(",
                ephemeral: true
            );

            return;
        }

        try
        {
            // Download the emote's image file.
            _logger.LogInformation("Attempting to get emote from '{Url}'.", parsedEmote.Url);
            Stream? emoteImgStream = await DownloadEmoteAsync(parsedEmote);

            if (emoteImgStream is null)
            {
                // Throw an error if emote image stream is null.
                throw new Exception("Emote image has a null file stream.");
            }
            else
            {
                // If the image stream wasn't null, start the resize process.
                _logger.LogInformation("Attempting to resize emote.");

                // Get the image's info and resize it.
                MagickImageInfo emoteImgInfo = await GetEmoteImageInfoAsync(emoteImgStream);
                MemoryStream emoteImgResizedStream = ResizeEmote(emoteImgStream, emoteImgInfo);

                // Send the resized emote to the client.
                _logger.LogInformation("Sending resized emote.");
                await RespondWithFileAsync(
                    fileName: $"{parsedEmote.Name}.{emoteImgInfo.Format.ToString().ToLower()}",
                    fileStream: emoteImgResizedStream
                );

                await emoteImgStream.DisposeAsync();
            }
        }
        catch (Exception e)
        {
            // If any error occurred while resizing the emote,
            // send the base emote image back to the client.
            
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