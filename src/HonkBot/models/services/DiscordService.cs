using System.Text.RegularExpressions;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HonkBot.Commands;

namespace HonkBot.Models.Services;

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

    public async Task Connect()
    {
        _logger.LogInformation("Connecting client.");

        if (_config.GetValue<string>("DiscordClientToken") is null)
        {
            throw new Exception("DiscordClientToken environment variable not found.");
        }

        await _discordClient.LoginAsync(TokenType.Bot, _config.GetValue<string>("DiscordClientToken"));
        await _discordClient.StartAsync();

        _logger.LogInformation("Initializing interaction service.");
        _interactionService = new(_discordClient.Rest);

        _commandService = new();

        _logger.LogInformation("Adding modules to interaction service.");
        await _interactionService.AddModuleAsync<HonkCommandModule>(_serviceProvider);
        await _interactionService.AddModuleAsync<GenericCommandsModule>(_serviceProvider);

        _discordClient.Log += HandleLog;
        //_discordClient.MessageReceived += HandleMessageCommand;
        _interactionService.Log += HandleLog;
        _commandService.Log += HandleLog;
        _discordClient.InteractionCreated += HandleSlashCommand;
        _discordClient.Ready += OnClientReady;
        _discordClient.GuildUpdated += HandleGuildUpdate;

    }

    private async Task OnClientReady()
    {
#if DEBUG
        ulong testGuildId = _config.GetValue<ulong>("DiscordTestGuildId");
        _logger.LogInformation("In DEBUG mode. Registering to guild id: {testGuildId}", testGuildId);
        await _interactionService!.RegisterCommandsToGuildAsync(testGuildId);
#else
        await _interactionService!.RegisterCommandsGloballyAsync();
#endif

        string slashCommandsLoadedString = string.Join(",", _interactionService.SlashCommands);
        _logger.LogInformation("Slash commands loaded: {commandsLoadedString}", slashCommandsLoadedString);

        string messageCommandsLoaded = string.Join(",", _commandService!.Commands.ToList());
        _logger.LogInformation("Mention commands loaded: {commandsLoadedString}", messageCommandsLoaded);
    }

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