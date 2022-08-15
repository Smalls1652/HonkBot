using Discord;
using Discord.Interactions;

namespace HonkBot.Commands;

public partial class ImageCommandsModule : InteractionModuleBase
{
    /// <summary>
    /// Download the emote.
    /// </summary>
    /// <param name="emote">The emote to resize.</param>
    /// <returns>A <see cref="Stream" /> of the downloaded emote's image file.</returns>
    private static async Task<Stream?> DownloadEmoteAsync(Emote emote)
    {
        // Initialize the Stream of the downloaded emote's image.
        Stream? emoteImgStream;

        // Set the timeout for the HttpClient to 2 seconds
        // and then initialize the HttpClient.
        SocketsHttpHandler httpHandler = new()
        {
            ConnectTimeout = TimeSpan.FromSeconds(10)
        };
        using HttpClient httpClient = new(httpHandler, true);

        // Send an HttpRequest to get the emote's image.
        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: emote!.Url
        );
        HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);

        // Read the contents of the HttpResponseMessage as a Stream.
        emoteImgStream = await responseMessage.Content.ReadAsStreamAsync();

        // Return the emote's image as a Stream.
        return emoteImgStream;
    }
}