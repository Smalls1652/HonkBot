using Discord;
using Discord.Interactions;

namespace HonkBot.Commands;

public partial class ImageCommandsModule : InteractionModuleBase
{
    private static async Task<Stream?> DownloadEmoteAsync(Emote emote)
    {
        Stream? emoteImgStream;

        SocketsHttpHandler httpHandler = new()
        {
            ConnectTimeout = TimeSpan.FromSeconds(2)
        };

        using HttpClient httpClient = new(httpHandler, true);

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: emote!.Url
        );

        HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);

        emoteImgStream = await responseMessage.Content.ReadAsStreamAsync();

        return emoteImgStream;
    }
}