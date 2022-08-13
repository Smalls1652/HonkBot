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

            Stream? emoteImgStream = await DownloadEmoteAsync(parsedEmote);

            if (emoteImgStream is null)
            {
                throw new Exception("Emote image has a null file stream.");
            }
            else
            {
                _logger.LogInformation("Attempting to resize emote.");

                MagickImageInfo emoteImgInfo = await GetEmoteImageInfoAsync(emoteImgStream);
                MemoryStream emoteImgResizedStream = ResizeEmote(emoteImgStream, emoteImgInfo);

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