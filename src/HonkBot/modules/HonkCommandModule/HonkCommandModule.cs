using Discord;
using Discord.Interactions;
using HonkBot.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

/// <summary>
/// The primary slash command module for interacting with HonkBot.
/// </summary>
public partial class HonkCommandModule : InteractionModuleBase
{
    private readonly IDiscordService _discordService;
    private readonly IOdesliService _odesliService;
    private readonly ILogger<HonkCommandModule> _logger;

    /// <summary>
    /// Initialize <see cref="HonkCommandModule" /> for use.
    /// </summary>
    /// <param name="discordService">The <see cref="IDiscordService" /> for dependency injection.</param>
    /// <param name="odesliService">The <see cref="IOdesliService" /> for dependency injection.</param>
    /// <param name="logger">The logger assigned to <see cref="HonkCommandModule" /> for dependency injection.</param>
    public HonkCommandModule(IDiscordService discordService, IOdesliService odesliService, ILogger<HonkCommandModule> logger)
    {
        _discordService = discordService;
        _odesliService = odesliService;
        _logger = logger;
    }
}