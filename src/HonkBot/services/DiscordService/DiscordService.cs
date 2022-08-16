using System.Text.RegularExpressions;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using HonkBot.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HonkBot.Services;

/// <summary>
/// The primary Discord Service for handling bot calls.
/// </summary>
public class DiscordService : IDiscordService
{
    private readonly DiscordSocketClient _discordClient;
    private readonly ILogger<DiscordService> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;

    public DiscordService(DiscordSocketClient discordClient, ILogger<DiscordService> logger, IConfiguration config, IServiceProvider serviceProvider)
    {
        _discordClient = discordClient;
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
    }

    private InteractionService? _interactionService;
    private CommandService? _commandService;

    /// <summary>
    /// Connects to the Discord API.
    /// </summary>
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

        // Initialize the command service.
        _commandService = new();

        // Add the following modules to the interaction service:
        // - HonkCommandModule
        // - ImageCommandsModule
        _logger.LogInformation("Adding modules to interaction service.");
        await _interactionService.AddModuleAsync<HonkCommandModule>(_serviceProvider);
        await _interactionService.AddModuleAsync<ImageCommandsModule>(_serviceProvider);

        // Add logging method for the DiscordClient, InteractionService, and CommandService.
        _discordClient.Log += HandleLog;
        _interactionService.Log += HandleLog;
        _commandService.Log += HandleLog;

        // Add slash command handling to the DiscordClient.
        _discordClient.InteractionCreated += HandleSlashCommand;

        // Add method to run when the DiscordClient is in a ready state.
        _discordClient.Ready += OnClientReady;

        // Add the guild update method.
        _discordClient.GuildUpdated += HandleGuildUpdate;

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

        string messageCommandsLoaded = string.Join(",", _commandService!.Commands.ToList());
        _logger.LogInformation("Mention commands loaded: {commandsLoadedString}", messageCommandsLoaded);

        // Set the initial status.
        await SetGameStatus(null, ActivityType.Playing);
    }

    /// <summary>
    /// Sets the status of HonkBot.
    /// </summary>
    /// <param name="status">The status message to set.</param>
    /// <param name="activityType">The <see cref="ActivityType" /> to set.</param>
    public async Task SetGameStatus(string? status, ActivityType activityType = ActivityType.Playing)
    {
        if (status is null)
        {
            status = "honking away";
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

    private async Task HandleMessageCommand(SocketMessage messageParam)
    {
        SocketUserMessage message = messageParam as SocketUserMessage;
        if (message is null)
        {
            return;
        }

        int argPos = 0;
        string currentUserString = _discordClient.CurrentUser.Mention;
        currentUserString = currentUserString.Replace("<", "")
            .Replace(">", "")
            .Replace("@", "")
            .Replace("!", "");
        Regex hasMentionRegex = new($".*{currentUserString}.*");
        if (message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos) || hasMentionRegex.IsMatch(message.Content))
        {
            SocketCommandContext context = new(_discordClient, message);

            _logger.LogInformation("'{Username}' mentioned honkbot.", context.User.Username);

            await context.Channel.SendFileAsync(
                filePath: "honk-gon-get-ya.png",
                text: $"pls dont @ me, {context.User.Mention}"
            );
        }
        else
        {
            return;
        }
    }

    private async Task HandleSlashCommand(SocketInteraction interaction)
    {
        SocketInteractionContext interactionContext = new(_discordClient, interaction);
        await _interactionService!.ExecuteCommandAsync(interactionContext, _serviceProvider);
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