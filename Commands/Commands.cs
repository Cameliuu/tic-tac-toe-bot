using Discord.Interactions;

namespace tic_tac_toe_bot.Commands;

public class Commands : InteractionModuleBase
{
    [SlashCommand("echo", "Echo an input")]
    public async Task Echo(string input)
    {
        await RespondAsync(input);
    }

}