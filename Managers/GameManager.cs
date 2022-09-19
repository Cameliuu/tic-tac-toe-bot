using Discord;
using Discord.WebSocket;
using Microsoft.VisualBasic;

namespace tic_tac_toe_bot.Managers;

public class GameManager
{
    private DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();

    public static async Task StartGame(SocketUser user, ISocketMessageChannel channel,string player,SocketGuild guild)
    {

       var playerS=await CustomUserTypeReader.GetUserFromString(player, guild);
       await channel.SendMessageAsync($"{playerS.Mention}");
       await BuildBoard(channel,"label");
    }

    public static async Task BuildBoard(ISocketMessageChannel channel,string text)
    {
        var builder = new ComponentBuilder()
            .WithButton("1", "1", row: 0)
            .WithButton("2", "2", row: 1)
            .WithButton("3", "3", row: 2)
            .WithButton("4", "4", row: 0)
            .WithButton("5", "5", row: 1)
            .WithButton("6", "6", row: 2)
            .WithButton("7", "7", row: 0)
            .WithButton("8", "8", row: 1)
            .WithButton("9", "9", row: 2);
        await channel.SendMessageAsync("Here is a button!", components: builder.Build());
    }
    
}