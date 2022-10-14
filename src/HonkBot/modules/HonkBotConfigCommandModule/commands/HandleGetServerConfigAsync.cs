using System.Text;
using Discord;
using Discord.Interactions;
using HonkBot.Models.Config;
using HonkBot.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkBotConfigCommandModule : InteractionModuleBase
{
    /// <summary>
    /// Get the config for the current server.
    /// </summary>
    /// <returns></returns>
    [RequireUserPermission(GuildPermission.Administrator)]
    [SlashCommand(name: "get-honkbot-config", description: "Gets the server configuration.")]
    public async Task HandleGetServerConfigAsync()
    {
        await DeferAsync();

        ServerConfig serverConfig = await _cosmosDbService.GetServerConfigAsync(Context.Guild.Id.ToString());

        StringBuilder outputText = new($"**Randomly add reactions to messages:** `{serverConfig.RandomReactConfig.Enabled}` (`{serverConfig.RandomReactConfig.PercentChanceToHappen}%` chance to occur)");
        outputText.AppendLine("\n");
        outputText.AppendLine($"**Randomly drop a fart bomb on a message:** `{serverConfig.RandomFartBombConfig.Enabled}`");

        await FollowupAsync(
            text: outputText.ToString()
        );
    }
}