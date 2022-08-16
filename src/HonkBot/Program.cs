using Discord;
using Discord.WebSocket;
using HonkBot.Services;
using ImageMagick;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HonkBot;

/// <summary>
/// HonkBot's main entrypoint class.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entrypoint method for HonkBot. This is what will run when HonkBot is ran.
    /// </summary>
    public static async Task Main()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder();

        GatewayIntents gatewayIntents = GatewayIntents.AllUnprivileged - GatewayIntents.GuildInvites - GatewayIntents.GuildScheduledEvents;

        DiscordSocketConfig discordSocketConfig = new()
        {
            GatewayIntents = gatewayIntents
        };

        hostBuilder
            .ConfigureServices(
                (_, services) =>
                {
                    services.AddSingleton<DiscordSocketClient>(
                        implementationInstance: new(discordSocketConfig)
                    );
                    services.AddSingleton<IDiscordService, DiscordService>();
                    services.AddSingleton<IOdesliService, OdesliService>();
                }
            )
            .ConfigureLogging(
                (loggerOptions) =>
                {
                    loggerOptions.AddConsole();
                }
            );


        hostBuilder.ConfigureAppConfiguration(
            (hostingContext, config) =>
            {
                config.Sources.Clear();

                IHostEnvironment hostEnv = hostingContext.HostingEnvironment;

                config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(
                        path: "appsettings.json",
                        optional: true,
                        reloadOnChange: true
                    )
                    .AddJsonFile(
                        path: $"appsettings.{hostEnv.EnvironmentName}.json",
                        optional: true,
                        reloadOnChange: true
                    );

                config.AddEnvironmentVariables();
            }
        );

        using IHost host = hostBuilder.Build();

        MagickNET.Initialize();

        IDiscordService discordSvc = host.Services.GetRequiredService<IDiscordService>();
        await discordSvc.Connect();

        await host.RunAsync();
    }
}