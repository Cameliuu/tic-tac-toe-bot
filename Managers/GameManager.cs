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
       await BuildBoard(channel);
    }

    public static async Task BuildBoard(ISocketMessageChannel channel)
    {
        var builder = new ComponentBuilder()
            .WithButton("label", "custom-id");
        await channel.SendMessageAsync("Here is a button!", components: builder.Build());
    }
}