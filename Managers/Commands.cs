using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace tic_tac_toe_bot.Managers;

public class Commands : ModuleBase<SocketCommandContext>
{
    [Command("ttt")]
    public async Task StartAsync([Remainder] string player)
        => await GameManager.StartGame(Context.User, Context.Channel, player,Context.Guild);
}