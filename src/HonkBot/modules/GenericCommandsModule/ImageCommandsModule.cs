using Discord;
using Discord.Interactions;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

/// <summary>
/// A slash command module for having HonkBot manipulate images.
/// </summary>
public partial class ImageCommandsModule : InteractionModuleBase
{
    private readonly ILogger<ImageCommandsModule> _logger;
    public ImageCommandsModule(ILogger<ImageCommandsModule> logger)
    {
        _logger = logger;
    }
}