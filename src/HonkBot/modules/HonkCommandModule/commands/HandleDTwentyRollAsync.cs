using Discord;
using Discord.Interactions;
using HonkBot.Models.Tools;
using Microsoft.Extensions.Logging;

namespace HonkBot.Modules;

public partial class HonkCommandModule : InteractionModuleBase
{
    /// <summary>
    /// HonkBot rolls a D20.
    /// </summary>
    [SlashCommand(name: "roll-d20", description: "Roll a D20 dice!")]
    private async Task HandleDTwentyRollAsync()
    {
        await DeferAsync();

        int d20RollVal = RandomGenerator.GetRandomNumber(
            minValue: 1,
            maxValue: 20
        );

        string outputMessage = d20RollVal switch
        {
            1 => "Wow! You rolled a natural 1! Sucks for you!",
            20 => "Wow! You rolled a natural 20! You're either lucky or you're cheating!",
            _ => $"You rolled a {d20RollVal}."
        };

        await FollowupAsync(
            text: outputMessage
        );
    }
}