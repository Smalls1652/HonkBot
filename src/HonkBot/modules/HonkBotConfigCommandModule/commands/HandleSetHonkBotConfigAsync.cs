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
    /// Enable or disable a HonkBot feature.
    /// </summary>
    /// <param name="configItem">The config item to set.</param>
    /// <param name="enabled">Whether to enable the feature or not.</param>
    /// <returns></returns>
    [RequireUserPermission(GuildPermission.Administrator)]
    [SlashCommand(name: "set-honkbot-config", description: "Set a server configuration item for HonkBot.")]
    public async Task HandleSetHonkBotConfigAsync(
        [Summary(name: "configitem", description: "The configuration item to set."), Choice(name: "randomreact", value: "randomreact"), Choice(name: "randomfartbomb", value: "randomfartbomb")]
        string configItem,
        [Summary("enabled", "Whether to enable the feature or not.")]
        bool enabled
    )
    {
        await DeferAsync();

        ServerConfig serverConfig = await _cosmosDbService.GetServerConfigAsync(Context.Guild.Id);

        switch (configItem)
        {
            case "randomreact":
                _logger.LogInformation("Setting random react config to '{enabled}' for guild ID '{guildId}'.", enabled, Context.Guild.Id);
                serverConfig.RandomReactConfig.Enabled = enabled;
                break;
            case "randomfartbomb":
                _logger.LogInformation("Setting random fart bomb config to '{enabled}' for guild ID '{guildId}'.", enabled, Context.Guild.Id);
                serverConfig.RandomFartBombConfig.Enabled = enabled;
                break;
        }

        StringBuilder outputText = new("Updated config:");
        outputText.AppendLine("\n");
        outputText.AppendLine($"**Randomly add reactions to messages:** `{serverConfig.RandomReactConfig.Enabled}` (`{serverConfig.RandomReactConfig.PercentChanceToHappen}%` chance to occur)");
        outputText.AppendLine("\n");
        outputText.AppendLine($"**Randomly drop a fart bomb on a message:** `{serverConfig.RandomFartBombConfig.Enabled}`");

        _logger.LogInformation("Updating server config for guild ID '{guildId}'.", Context.Guild.Id);
        await _cosmosDbService.AddOrUpdateServerConfigAsync(serverConfig);

        await FollowupAsync(
            text: outputText.ToString()
        );
    }
}