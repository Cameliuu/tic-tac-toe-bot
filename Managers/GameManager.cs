using Discord;
using Discord.WebSocket;
using Microsoft.VisualBasic;

namespace tic_tac_toe_bot.Managers;

public class GameManager
{
    private DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
    public static  Player Player1 { get; set; } 
    public static Player Player2 { get; set; } 
    public  static char[,] board=new char[3, 3]{{'1','2','3'},{'4','5','6'},{'7','8','9'}};
    public  static bool active { get; set; }
    public static char turn { get; set; }
    
    public  static async Task StartGame(SocketUser user, ISocketMessageChannel channel,string player,SocketGuild guild)
    {
        
        Player1 = new Player();
        if (user.Username == player)
        {
            await channel.SendMessageAsync($"{player} nu te poti provoca singur!");
            return;
        }
    
        var builder = new ComponentBuilder()
            .WithButton("Yes", "yes", style: ButtonStyle.Success)
            .WithButton("No", "no", style: ButtonStyle.Danger);
        await channel.SendMessageAsync($"{player}, {user.Mention} Te provoaca la un joc de X si 0.Accepti?", components:builder.Build());
        Player2 = new Player();
        var playerS=await CustomUserTypeReader.GetUserFromString(player, guild);
        await SetActive(user, playerS);

    }

    public  static async Task SetActive(IUser player1, IUser player2)
    {
        
        active = true;
        turn = 'X';
        Player1.choice = 'X';
        Player2.name = player2.Username;
        Player2.choice = 'O';
        Player1.name = player1.Username;
        Console.WriteLine($"{DateTime.Now}\tGame active");
    }

    public  static async Task<ComponentBuilder> GetBuilder()
    {
        var builder = new ComponentBuilder()
            .WithButton(board[0,0].ToString(), "1", row: 0)
            .WithButton(board[0,1].ToString(), "2", row: 1)
            .WithButton(board[0,2].ToString(), "3", row: 2)
            .WithButton(board[1,0].ToString(), "4", row: 0)
            .WithButton(board[1,1].ToString(), "5", row: 1)
            .WithButton(board[1,2].ToString(), "6", row: 2)
            .WithButton(board[2,0].ToString(), "7", row: 0)
            .WithButton(board[2,1].ToString(), "8", row: 1)
            .WithButton(board[2,2].ToString(), "9", row: 2);
        return builder;
    }
    
}