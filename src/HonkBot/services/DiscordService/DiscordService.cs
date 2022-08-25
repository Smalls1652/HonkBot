using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HonkBot.Models.Tools;
using HonkBot.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HonkBot.Services;

/// <summary>
/// The primary Discord Service for handling bot calls.
/// </summary>
public class DiscordService : IDiscordService
{
    /// <summary>
    /// <see cref="DiscordSocketClient" /> passed in from dependency injection.
    /// </summary>
    private readonly DiscordSocketClient _discordClient;

    /// <summary>
    /// An <see cref="ILogger" /> for logging.
    /// </summary>
    private readonly ILogger<DiscordService> _logger;

    /// <summary>
    /// Config data passed in from dependency injection.
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// Service objects passed in from dependency injection.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes <see cref="DiscordService" /> for use.
    /// </summary>
    /// <param name="discordClient">The <see cref="DiscordSocketClient" /> used for dependency injection.</param>
    /// <param name="logger">The logger assigned to <see cref="DiscordService" /> for dependency injection.</param>
    /// <param name="config">Config data passed in for dependency injection.</param>
    /// <param name="serviceProvider">Services passed in for dependency injection.</param>
    public DiscordService(DiscordSocketClient discordClient, ILogger<DiscordService> logger, IConfiguration config, IServiceProvider serviceProvider)
    {
        _discordClient = discordClient;
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// The <see cref="InteractionService" /> used for the HonkBot to interact with commands.
    /// </summary>
    private InteractionService? _interactionService;

    /// <inheritdoc cref="IDiscordService.Connect" />
    public async Task Connect()
    {
        _logger.LogInformation("Connecting client.");

        // Ensure that the 'DiscordClientToken' config value is not null.
        if (_config.GetValue<string>("DiscordClientToken") is null)
        {
            throw new Exception("DiscordClientToken environment variable not found.");
        }

        // Start the login and the websocket client.
        await _discordClient.LoginAsync(TokenType.Bot, _config.GetValue<string>("DiscordClientToken"));
        await _discordClient.StartAsync();

        // Initialize the interaction service.
        _logger.LogInformation("Initializing interaction service.");
        _interactionService = new(_discordClient.Rest);

        // Add the following modules to the interaction service:
        // - HonkCommandModule
        // - ImageCommandsModule
        _logger.LogInformation("Adding modules to interaction service.");
        await _interactionService.AddModuleAsync<HonkCommandModule>(_serviceProvider);
        await _interactionService.AddModuleAsync<ImageCommandsModule>(_serviceProvider);

        // Add logging method for the DiscordClient and InteractionService.
        _discordClient.Log += HandleLog;
        _interactionService.Log += HandleLog;

        // Add slash command handling to the DiscordClient.
        _discordClient.InteractionCreated += HandleSlashCommand;

        // Add method to run when the DiscordClient is in a ready state.
        _discordClient.Ready += OnClientReady;

        // Add the guild update method.
        _discordClient.GuildUpdated += HandleGuildUpdate;

        _discordClient.MessageReceived += HandleRandomReactionAsync;
    }

    /// <summary>
    /// Registers commands and sets the initial presence status for HonkBot.
    /// </summary>
    private async Task OnClientReady()
    {
#if DEBUG
        // If we're in a debug environment (development), then register the commands to the
        // configured test server (guild).
        ulong testGuildId = _config.GetValue<ulong>("DiscordTestGuildId");
        _logger.LogInformation("In DEBUG mode. Registering to guild id: {testGuildId}", testGuildId);
        await _interactionService!.RegisterCommandsToGuildAsync(
            guildId: testGuildId,
            deleteMissing: true
        );
#else
        // Otherwise, register the commands globally.
        await _interactionService!.RegisterCommandsGloballyAsync(
            deleteMissing: true
        );
#endif

        // Log the registered commands.
        string slashCommandsLoadedString = string.Join(",", _interactionService.SlashCommands);
        _logger.LogInformation("Slash commands loaded: {commandsLoadedString}", slashCommandsLoadedString);

        // Set the initial status.
        await SetGameStatus(null, ActivityType.Playing);
    }

    /// <inheritdoc cref="IDiscordService.SetGameStatus(string?, ActivityType)" />
    public async Task SetGameStatus(string? status, ActivityType activityType = ActivityType.Playing)
    {
        if (status is null)
        {
            status = "gm simulator 2022";
        }

        await _discordClient.SetGameAsync(
            name: status,
            streamUrl: null,
            type: activityType
        );
    }

    /// <summary>
    /// Handles logging for the DiscordClient.
    /// </summary>
    private Task HandleLog(LogMessage logMessage)
    {
        _logger.LogInformation("[{Severity}] {LogMessage}", logMessage.Severity, logMessage.ToString());

        return Task.CompletedTask;
    }

    private async Task HandleSlashCommand(SocketInteraction interaction)
    {
        SocketInteractionContext interactionContext = new(_discordClient, interaction);
        await _interactionService!.ExecuteCommandAsync(interactionContext, _serviceProvider);
    }

    private async Task HandleRandomReactionAsync(SocketMessage message)
    {
        if (message.Author.Id != _discordClient.CurrentUser.Id)
        {
            int randomPercent = RandomGenerator.GetRandomNumber(0, 100);
            _logger.LogInformation("Should HonkBot randomly add a reaction? {Percent}", randomPercent);

            if (randomPercent <= 10)
            {
                _logger.LogInformation("HonkBot is going to add a random emote reaction to message ID '{MessageId}'.", message.Id);

                SocketGuild? guild = GetChannelGuild(message.Channel);

                if (guild is not null)
                {
                    _logger.LogInformation("Message originated from '{Guild}'. Getting a random emote.", guild.Name);

                    int randomEmoteIndex = RandomGenerator.GetRandomNumber(0, guild.Emotes.ToList().Count - 1);

                    GuildEmote randomEmote = guild.Emotes.ToArray()[randomEmoteIndex];

                    _logger.LogInformation("Adding emote, '{EmoteName}', to message.", randomEmote.Name);
                    await message.AddReactionAsync(randomEmote);
                }
            }
        }
    }

    private SocketGuild? GetChannelGuild(ISocketMessageChannel channel)
    {
        List<SocketGuild> guilds = _discordClient.Guilds.ToList();

        SocketGuild? foundGuild = null;
        bool guildFound = false;

        for (int i = 0; i < guilds.Count || guildFound != true; i++)
        {
            _logger.LogInformation("Seeing if '{Guild}' is where the message was sent from.", guilds[i].Name);
            if (guilds[i].Channels.Contains(channel as SocketChannel))
            {
                _logger.LogInformation("Found in '{Guild}'.", guilds[i].Name);
                foundGuild =  guilds[i];
                guildFound = true;
            }
        }

        return foundGuild;
    }

    private async Task HandleGuildUpdate(SocketGuild guild1, SocketGuild guild2)
    {
        foreach (GuildEmote emote in guild2.Emotes)
        {
            if (!guild1.Emotes.Contains(emote))
            {
                await guild2.SystemChannel.SendMessageAsync(
                    text: $"New emote added! {emote}",
                    embed: new EmbedBuilder()
                    {
                        ImageUrl = emote.Url
                    }
                    .Build()
                );
            }
        }
    }
}