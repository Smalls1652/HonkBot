using Discord;
using Discord.Interactions;
using HonkBot.Models.Config;
using HonkBot.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

/// <summary>
/// Slash command module for configuring HonkBot.
/// </summary>
[EnabledInDm(false)]
public partial class HonkBotConfigCommandModule : InteractionModuleBase
{
    /// <summary>
    /// <see cref="IDiscordService" /> passed in from dependency injection.
    /// </summary>
    private readonly IDiscordService _discordService;

    /// <summary>
    /// <see cref="IOdesliService" /> passed in from dependency injection.
    /// </summary>
    private readonly IOdesliService _odesliService;

    /// <summary>
    /// An <see cref="ILogger" /> for logging.
    /// </summary>
    private readonly ILogger<HonkBotConfigCommandModule> _logger;

    /// <summary>
    /// <see cref="ICosmosDbService" /> passed in from dependency injection.
    /// </summary>
    private readonly ICosmosDbService _cosmosDbService;

    /// <summary>
    /// Initialize <see cref="HonkBotConfigCommandModule" /> for use.
    /// </summary>
    /// <param name="discordService">The <see cref="IDiscordService" /> for dependency injection.</param>
    /// <param name="odesliService">The <see cref="IOdesliService" /> for dependency injection.</param>
    /// <param name="logger">The logger assigned to <see cref="HonkBotConfigCommandModule" /> for dependency injection.</param>
    /// <param name="cosmosDbService">The <see cref="ICosmosDbService" /> for dependency injection.</param>
    public HonkBotConfigCommandModule(IDiscordService discordService, IOdesliService odesliService, ILogger<HonkBotConfigCommandModule> logger, ICosmosDbService cosmosDbService)
    {
        _discordService = discordService;
        _odesliService = odesliService;
        _logger = logger;
        _cosmosDbService = cosmosDbService;
    }
}