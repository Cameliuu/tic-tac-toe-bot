using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace tic_tac_toe_bot.Managers;

public class ComponentsManager
{
    private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();

    public static async Task loadComponents()
    {
        _client.ButtonExecuted += ComponentHandler;
    }

 

    private static async Task<bool> IsPlayerInGame(SocketMessageComponent component)
    {
        return (component.User.Username == GameManager.Player1.name ||
                component.User.Username == GameManager.Player2.name)
            ? true
            : false;
        
    }

    private static async Task PopulateChoices(Player player, short pos1, short pos2,[Optional] short pos3, [Optional] short pos4)
    {
        Console.WriteLine($"{pos1}:{player.choices[pos1]}");
        Console.WriteLine($"{pos2}:{player.choices[pos2]}");
        player.choices[pos1]++;
        player.choices[pos2]++;
        if(pos3 !=0)
            player.choices[pos3]++;
        if (pos4 != 0)
            player.choices[pos4]++;
    }
    private static async Task<char> GetChoiceFromComponent(SocketMessageComponent component)
    {
        
        return (component.User.Username == GameManager.Player1.name)
            ? 'X'
            : 'O';
    }

    private static async Task<bool> IsFirstPlayer(SocketMessageComponent component)
    {
        if(component.User.Username == GameManager.Player1.name)
            Console.WriteLine("DA");
        else
        {
            Console.WriteLine("NU");
        }
        return (component.User.Username == GameManager.Player1.name) ? true : false;
    }
    private static async Task<char> GetNextTurn(char turn)
    {
        return (turn == 'X') ? 'O' : 'X';
    }
    
    private static async Task UpdateComponent(SocketMessageComponent component, short row, short col)
    {
        if (!IsPlayerInGame(component).Result)
            await component.Channel.SendMessageAsync(
                $"{component.User.Mention}, nu poti participa intrucat nu faci parte dintre participanti");
        else
        {
            if (GameManager.turn != GetChoiceFromComponent(component).Result)
                await component.Channel.SendMessageAsync($"{component.User.Mention}, nu este randul tau!");
            else
            {
                GameManager.turn = GetNextTurn(GetChoiceFromComponent(component).Result).Result;
                GameManager.board[row, col] = GetChoiceFromComponent(component).Result;
                await component.UpdateAsync(properties =>
                    properties.Components = GameManager.GetBuilder(-1).Result);
                GameManager.tries++;
                Console.WriteLine("PLayer 1");
                foreach(var choice in GameManager.Player1.choices)
                    Console.Write($"{choice}\t");
                Console.WriteLine("PLayer 2");
                foreach (var choice in GameManager.Player2.choices)
                    Console.Write($"{choice}\t");
                if (GameManager.tries > 3)
                {
                    if (GameManager.Player1.choices.Contains(3))
                    {
                        
                        await component.Channel.SendMessageAsync("X castiga");
                        await component.ModifyOriginalResponseAsync(properties =>
                            properties.Components = GameManager.GetBuilder(Array.IndexOf(GameManager.Player1.choices,3)).Result);
                    }
                    if (GameManager.Player2.choices.Contains(3))
                    {
                        await component.Channel.SendMessageAsync("O castiga");
                        await component.ModifyOriginalResponseAsync(properties =>
                            properties.Components = GameManager.GetBuilder(Array.IndexOf(GameManager.Player2.choices,3)).Result);
                    }
                }
            }
        }
    }
   
    public  static async Task ComponentHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "1":
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 0, 3, 6);
                else
                    await PopulateChoices(GameManager.Player2, 0, 3, 6);
                await UpdateComponent(component,0,0);
                
                break;
            case "2":
                if (await IsFirstPlayer(component)) 
                    await PopulateChoices(GameManager.Player1, 0, 4);
                else
                    await PopulateChoices(GameManager.Player2, 0, 4);
                await UpdateComponent(component,0,1);
                
                break;
            case "3":
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 0, 5, 7);
                else
                    await PopulateChoices(GameManager.Player2, 0, 5, 7);
                await UpdateComponent(component,0,2);
                
                break;
            case "4":
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 1, 3);
                else
                    await PopulateChoices(GameManager.Player2, 1, 3);
                await UpdateComponent(component,1,0);
                
                break;
            case "5":
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 1,4 , 6,7);
                else
                    await PopulateChoices(GameManager.Player2, 1, 4, 6,7);
                await UpdateComponent(component,1,1);
                
                break;
            case "6":
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 1, 5);
                else
                    await PopulateChoices(GameManager.Player2, 1, 5);
                await UpdateComponent(component,1,2);
                
                break;
            case "7":
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 2, 3,7);
                else
                    await PopulateChoices(GameManager.Player2, 2,3, 7);
                await UpdateComponent(component,2,0);
                
                break;
            case "8":
                Console.WriteLine("INTRA");
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 2, 4);
                else
                    await PopulateChoices(GameManager.Player2, 2,4);
                await UpdateComponent(component,2,1);
                
                break;
            case "9":
                if (await IsFirstPlayer(component))
                    await PopulateChoices(GameManager.Player1, 2, 3,6);
                else
                    await PopulateChoices(GameManager.Player2, 2,3,6);
                await UpdateComponent(component,2,2);
                break;
            case "yes":
                await component.UpdateAsync(p => p.Components = GameManager.GetBuilder(-1).Result);
                break;

        }
    }
}