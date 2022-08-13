using Discord;
using Discord.Interactions;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace HonkBot.Commands;

public partial class GenericCommandsModule : InteractionModuleBase
{
    private readonly ILogger<GenericCommandsModule> _logger;
    public GenericCommandsModule(ILogger<GenericCommandsModule> logger)
    {
        _logger = logger;
    }
}