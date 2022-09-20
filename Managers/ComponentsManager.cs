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

    private static async Task PopulateChoices(SocketMessageComponent component, short pos1, short pos2,[Optional] short pos3, [Optional] short pos4)
    {
        
        if (await IsFirstPlayer(component) && GameManager.Player1.choice == GameManager.turn && GameManager.active)
        {
          
            GameManager.Player1.choices[pos1]++;
            GameManager.Player1.choices[pos2]++;
            if(pos3 !=0)
               GameManager.Player1.choices[pos3]++;
            if (pos4 != 0)
              GameManager.Player1.choices[pos4]++;
        }
        else if (!(await IsFirstPlayer(component))&& GameManager.Player2.choice == GameManager.turn && GameManager.active)
        {
            Console.WriteLine($"{pos1}:{GameManager.Player2.choices[pos1]}");
            Console.WriteLine($"{pos2}:{GameManager.Player2.choices[pos2]}");
            GameManager.Player2.choices[pos1]++;
            GameManager.Player2.choices[pos2]++;
            if(pos3 !=0)
                GameManager.Player2.choices[pos3]++;
            if (pos4 != 0)
                GameManager.Player2.choices[pos4]++;
        }
    }
    private static async Task<char> GetChoiceFromComponent(SocketMessageComponent component)
    {
        
        return (component.User.Username == GameManager.Player1.name)
            ? 'X'
            : 'O';
    }

    private static async Task<bool> PlayerWon(Player player, SocketMessageComponent component)
    {
        if (player.choices.Contains(3))
        {
            await component.Channel.SendMessageAsync($"{player.name} castiga");
            await component.ModifyOriginalResponseAsync(properties =>
                properties.Components = GameManager.GetBuilder(Array.IndexOf(player.choices, 3)).Result);
            await GameManager.SetActive(set: false);
            return true;
        }

        return false;
    }
    private static async Task<bool> IsFirstPlayer(SocketMessageComponent component)
    {
        return (component.User.Username == GameManager.Player1.name) ? true : false;
    }
    private static async Task<char> GetNextTurn(char turn)
    {
        return (turn == 'X') ? 'O' : 'X';
    }
    
    private static async Task UpdateComponent(SocketMessageComponent component, short row, short col)
    {
        if (!IsPlayerInGame(component).Result)
        {
            await component.Channel.SendMessageAsync(
                $"{component.User.Mention}, nu poti participa intrucat nu faci parte dintre participanti");
            return;
        }

        if (GameManager.turn != GetChoiceFromComponent(component).Result)
        {
            await component.Channel.SendMessageAsync($"{component.User.Mention}, nu este randul tau!");
            return;
        }

        if (!GameManager.active)
        {
            await component.Channel.SendMessageAsync($"Nu este niciun joc in desfasurare!");
            return;
        }
        GameManager.turn = GetNextTurn(GetChoiceFromComponent(component).Result).Result;
        GameManager.board.choices[row, col] = GetChoiceFromComponent(component).Result;
        GameManager.board.disabled[row, col] = true;
        await component.UpdateAsync(properties =>
            properties.Components = GameManager.GetBuilder(-1).Result);
        GameManager.tries++;
        if (GameManager.tries > 3)
        {
            if (GameManager.turn == 'O')
                await PlayerWon(GameManager.Player1, component);
            else
                await PlayerWon(GameManager.Player2, component);
        }
        if(GameManager.tries==9 && (!(await PlayerWon(GameManager.Player1,component)) && !(await PlayerWon(GameManager.Player2,component))))
        {
            await component.Channel.SendMessageAsync("It's a tie!");
            await GameManager.SetActive(set: false);
        }
    }
   
    public  static async Task ComponentHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "1":
                await PopulateChoices(component, 0, 3,6);
                await UpdateComponent(component,0,0);
                break;
            case "2":
                await PopulateChoices(component, 0, 4);
                await UpdateComponent(component, 0, 1);
                break;
            case "3":
                await PopulateChoices(component, 0, 5,7);
                await UpdateComponent(component,0,2);
                
                break;
            case "4":
                await PopulateChoices(component, 1, 3);
                await UpdateComponent(component,1,0);
                
                break;
            case "5":
                await PopulateChoices(component, 1, 4,6,7);
                await UpdateComponent(component,1,1);
                
                break;
            case "6":
                await PopulateChoices(component, 1, 5);
                await UpdateComponent(component,1,2);
                
                break;
            case "7":
                await PopulateChoices(component, 2, 3,7);
                await UpdateComponent(component,2,0);
                break;
            case "8":
                await PopulateChoices(component, 2, 4);
                await UpdateComponent(component,2,1);
                
                break;
            case "9":
                await PopulateChoices(component, 2, 3,6);
                await UpdateComponent(component,2,2);
                break;
            case "yes":
            {
                if (component.User.Username != GameManager.Player2.name)
                {
                    await component.Channel.SendMessageAsync($"Doar{GameManager.Player2.name} poate accepta!");
                    return;
                }
                await component.UpdateAsync(p => p.Components = GameManager.GetBuilder(-1).Result);
            }
                break;
            case "no":
                await component.Channel.SendMessageAsync($"{component.User.Mention} a refuzat jocul!");
                await component.Message.DeleteAsync();
                
                break;

    }
    }
}