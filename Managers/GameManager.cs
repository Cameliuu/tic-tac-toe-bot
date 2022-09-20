using System.Runtime.InteropServices;
using Discord;
using Discord.WebSocket;
using Microsoft.VisualBasic;

namespace tic_tac_toe_bot.Managers;

public class GameManager
{
    private DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
    public static  Player Player1 { get; set; } 
    public static Player Player2 { get; set; } 
    public  static Board board { get; set; }
    public  static bool active { get; set; }
    public static char turn { get; set; }
    public static short tries { get; set; }
    public  static async Task StartGame(SocketUser user, ISocketMessageChannel channel,string player,SocketGuild guild)
    {
        var playerS=await CustomUserTypeReader.GetUserFromString(player, guild);
        board = new Board();
        Player1 = new Player();
        Player2 = new Player();
        Player2.name = playerS.Username;
        if (user.Username == player)
        {   
            await channel.SendMessageAsync($"{player} nu te poti provoca singur!");
            return;
        }
    
        var builder = new ComponentBuilder()
            .WithButton("Yes", "yes", style: ButtonStyle.Success)
            .WithButton("No", "no", style: ButtonStyle.Danger);
        await channel.SendMessageAsync($"{player}, {user.Mention} Te provoaca la un joc de X si 0.Accepti?", components:builder.Build());
        
        
        await SetActive(user, playerS, true);

    }

    public  static async Task SetActive([Optional]IUser player1, [Optional]IUser player2, [Optional] bool set)
    {
        if(set)
        {
            active = true;
            turn = 'X';
            Player1.choice = 'X';
            Player2.name = player2.Username;
            Player2.choice = 'O';
            Player1.name = player1.Username;
            Console.WriteLine($"{DateTime.Now}\tGame active");
        }
        else
        {
            active = false;
            turn = ' ';
            Player1.choice = ' ';
            Player2.name = String.Empty;
            Player2.choice = ' ';
            await ResetPLayer(Player1);
            await ResetPLayer(Player2);
            Player1.name = String.Empty;
            tries = 0;
            Console.WriteLine($"{DateTime.Now}\tGame stopped");
        }
    }

    public static async Task ResetPLayer(Player player)
    {
        Array.Fill(player.choices,0);
        foreach(var val in player.choices)
            Console.WriteLine(val);
    }

    public static async Task<MessageComponent> GetBuilder([Optional] int pos)
    {
        Console.WriteLine($"Function called, pos={pos}");
        var builder = new ComponentBuilder();
        switch (pos)
        {
            case 0:
                builder.WithButton(board.choices[0,0].ToString(), "1", row: 0, style: ButtonStyle.Success, disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0,1].ToString(), "2", row: 0,style: ButtonStyle.Success,  disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0,2].ToString(), "3", row: 0, style: ButtonStyle.Success, disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1,0].ToString(), "4", row: 1, disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2,disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2,disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2,disabled:board.disabled[2,2]);
                break;
            case 1:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0 ,disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0 ,disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0 ,disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1, style: ButtonStyle.Success,disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1, style: ButtonStyle.Success,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1, style: ButtonStyle.Success,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2 , disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2 , disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2  ,disabled:board.disabled[2,2]);
                break; 
            case 2:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0, disabled:board.disabled[0,0]);  
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0, disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0, disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1, disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1, disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1, disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2, style: ButtonStyle.Success, disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2, style: ButtonStyle.Success, disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2, style: ButtonStyle.Success, disabled:board.disabled[2,2]);
                break;
            case 3:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0, style: ButtonStyle.Success, disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0,disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0,disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1,style: ButtonStyle.Success, disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2,style: ButtonStyle.Success,disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2,disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2,disabled:board.disabled[2,2]);
                break;
            case 4:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0,disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0, style: ButtonStyle.Success,disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0,disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1,disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1, style: ButtonStyle.Success,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2,disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2, style: ButtonStyle.Success,disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2,disabled:board.disabled[2,2]);
                break;
            case 5:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0,disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0,disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0, style: ButtonStyle.Success,disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1,disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1, style: ButtonStyle.Success,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2,disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2,disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2, style: ButtonStyle.Success,disabled:board.disabled[2,2]);
                break;
            case 6:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0, style: ButtonStyle.Success,disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0,disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0,disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1,disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1, style: ButtonStyle.Success,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2,disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2,disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2, style: ButtonStyle.Success,disabled:board.disabled[2,2]);
                break;
            case 7:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0,disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0,disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0, style: ButtonStyle.Success,disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1,disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1, style: ButtonStyle.Success,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2, style: ButtonStyle.Success,disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2,disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2,disabled:board.disabled[2,2]);
                break;
            default:
                builder.WithButton(board.choices[0, 0].ToString(), "1", row: 0,disabled:board.disabled[0,0]);
                builder.WithButton(board.choices[0, 1].ToString(), "2", row: 0,disabled:board.disabled[0,1]);
                builder.WithButton(board.choices[0, 2].ToString(), "3", row: 0,disabled:board.disabled[0,2]);
                builder.WithButton(board.choices[1, 0].ToString(), "4", row: 1,disabled:board.disabled[1,0]);
                builder.WithButton(board.choices[1, 1].ToString(), "5", row: 1,disabled:board.disabled[1,1]);
                builder.WithButton(board.choices[1, 2].ToString(), "6", row: 1,disabled:board.disabled[1,2]);
                builder.WithButton(board.choices[2, 0].ToString(), "7", row: 2,disabled:board.disabled[2,0]);
                builder.WithButton(board.choices[2, 1].ToString(), "8", row: 2,disabled:board.disabled[2,1]);
                builder.WithButton(board.choices[2, 2].ToString(), "9", row: 2,disabled:board.disabled[2,2]);
                break;
        }
        return builder.Build();
        
    }
}