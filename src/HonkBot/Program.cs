using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HonkBot.Commands;
using HonkBot.Models.Services;

namespace HonkBot;

public class Program
{

    public static async Task Main()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder();

        hostBuilder
            .ConfigureServices(
                (_, services) =>
                {
                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<IDiscordService, DiscordService>();
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

        IDiscordService discordSvc = host.Services.GetRequiredService<IDiscordService>();
        await discordSvc.Connect();

        await host.RunAsync();
    }
}