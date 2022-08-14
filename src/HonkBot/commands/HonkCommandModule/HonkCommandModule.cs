using Discord;
using Discord.Interactions;
using HonkBot.Models.Services;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

/// <summary>
/// The primary slash command module for interacting with HonkBot.
/// </summary>
public partial class HonkCommandModule : InteractionModuleBase
{
    private readonly IDiscordService _discordService;
    private readonly ILogger<HonkCommandModule> _logger;
    public HonkCommandModule(IDiscordService discordService, ILogger<HonkCommandModule> logger)
    {
        _discordService = discordService;
        _logger = logger;
    }
}