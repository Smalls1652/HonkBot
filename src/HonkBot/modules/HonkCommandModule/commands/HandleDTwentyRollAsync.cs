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
    [EnabledInDm(true)]
    [SlashCommand(name: "roll-d20", description: "Roll a D20 dice!")]
    private async Task HandleDTwentyRollAsync(
        [Summary(name: "roll-for", description: "What you want to roll for?")]
        string rollFor
    )
    {
        await DeferAsync();

        _logger.LogInformation("'{User}' rolled a D20 for '{RollFor}'.", Context.User.Username, rollFor);

        int userD20RollVal = RandomGenerator.GetRandomNumber(
            minValue: 1,
            maxValue: 20
        );

        _logger.LogInformation("'{User}' rolled '{RollResult}'.", Context.User.Username, userD20RollVal);

        int dmD20RollVal = RandomGenerator.GetRandomNumber(
            minValue: 1,
            maxValue: 20
        );

        _logger.LogInformation("'HonkBot' rolled '{RollResult}'.", dmD20RollVal);

        bool passesCheck;
        if (userD20RollVal == 1)
        {
            passesCheck = false;
        }
        else if (userD20RollVal == 20)
        {
            passesCheck = true;
        }
        else
        {
            passesCheck = userD20RollVal > dmD20RollVal;
        }

        _logger.LogInformation("'{User}' was successful: {PassCheck}", Context.User.Username, passesCheck);

        string passCheckMessage = passesCheck switch
        {
            false => "**Check result:** Failed",
            _ => "**Check result:** Successful"
        };

        string outputMessage = userD20RollVal switch
        {
            1 => $"Wow! {Context.User.Mention} rolled a natural `1` for '**{rollFor}**'! That sucks! lmao\n\n{passCheckMessage}",
            20 => $"Wow! {Context.User.Mention} rolled a natural `20` for '**{rollFor}**'! {Context.User.Mention} is either lucky or cheating!\n\n{passCheckMessage}",
            _ => $"{Context.User.Mention} rolled `{userD20RollVal}` for '**{rollFor}**'.\n\n{passCheckMessage}"
        };
        
        await FollowupAsync(
            text: outputMessage
        );
    }
}