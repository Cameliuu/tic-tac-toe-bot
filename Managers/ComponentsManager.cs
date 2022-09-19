using System.Threading.Channels;
using Discord;
using Discord.WebSocket;

namespace tic_tac_toe_bot.Managers;

public class ComponentsManager
{
    private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();

    public static async Task loadComponents()
    {
        _client.ButtonExecuted += ComponentHandler;
    }

    
    public static async Task<bool> IsPlayerInGame(SocketMessageComponent component)
    {
        return (component.User.Username == GameManager.Player1.name ||
                component.User.Username == GameManager.Player2.name)
            ? true
            : false;
        
    }

    public static async Task<char> GetChoiceFromComponent(SocketMessageComponent component)
    {
        
        return (component.User.Username == GameManager.Player1.name)
            ? 'X'
            : 'O';
    }

    public static async Task<char> GetNextTurn(char turn)
    {
        return (turn == 'X') ? 'O' : 'X';
    }
    public static async Task UpdateComponent(SocketMessageComponent component, short row, short col)
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
                    properties.Components = GameManager.GetBuilder().Result.Build());
            }
        }
    }
   
    public  static async Task ComponentHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "1":
                await UpdateComponent(component,0,0);
                
                break;
            case "2":
                await UpdateComponent(component,0,1);
                break;
            case "3":
                await UpdateComponent(component,0,2);
                break;
            case "4":
                await UpdateComponent(component,1,0);
                break;
            case "5":
                await UpdateComponent(component,1,1);
                break;
            case "6":
                await UpdateComponent(component,1,2);
                break;
            case "7":
                await UpdateComponent(component,2,0);
                break;
            case "8":
                await UpdateComponent(component,2,1);
                break;
            case "9":
                await UpdateComponent(component,2,2);
                break;
            case "yes":
                component.UpdateAsync(p => p.Components = GameManager.GetBuilder().Result.Build());
                break;

        }
    }
}