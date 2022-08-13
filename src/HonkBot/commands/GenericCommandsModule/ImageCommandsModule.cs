using Discord;
using Discord.Interactions;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

public partial class ImageCommandsModule : InteractionModuleBase
{
    private readonly ILogger<ImageCommandsModule> _logger;
    public ImageCommandsModule(ILogger<ImageCommandsModule> logger)
    {
        _logger = logger;
    }
}