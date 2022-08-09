using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace SmallsBot.Commands;

public class GenericCommandsModule : InteractionModuleBase
{
    private readonly DiscordSocketClient _discordClient;
    private readonly ILogger<GenericCommandsModule> _logger;
    
    public GenericCommandsModule(DiscordSocketClient discordClient, ILogger<GenericCommandsModule> logger)
    {
        _discordClient = discordClient;
        _logger = logger;
    }

    [SlashCommand(name: "hugemoji", description: "Make an emoji honking huge.")]
    private async Task HandleHonkingHugemogi(IEmote emote)
    {
        await RespondAsync(
            embed: new EmbedBuilder()
            {
                ImageUrl = Emote.Parse(emote.Name).Url
            }.Build()
        );
    }
}