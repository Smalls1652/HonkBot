using Discord;
using Discord.Interactions;
using HonkBot.Models.Services;
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
    public HonkCommandModule(IDiscordService discordService, IOdesliService odesliService, ILogger<HonkCommandModule> logger)
    {
        _discordService = discordService;
        _odesliService = odesliService;
        _logger = logger;
    }
}