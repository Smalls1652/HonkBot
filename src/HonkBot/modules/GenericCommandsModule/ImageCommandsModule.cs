using Discord;
using Discord.Interactions;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

/// <summary>
/// A slash command module for having HonkBot manipulate images.
/// </summary>
public partial class ImageCommandsModule : InteractionModuleBase
{
    private readonly ILogger<ImageCommandsModule> _logger;

    /// <summary>
    /// Initialize <see cref="ImageCommandsModule" /> for use.
    /// </summary>
    /// <param name="logger">The logger assigned to <see cref="ImageCommandsModule" /> for dependency injection.</param>
    public ImageCommandsModule(ILogger<ImageCommandsModule> logger)
    {
        _logger = logger;
    }
}