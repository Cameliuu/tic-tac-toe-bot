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

    public static async Task UpdateComponent(SocketMessageComponent component, short row, short col)
    {
        if(GameManager.active)
        {
            if(component.User.Username == GameManager.Player1.name || component.User.Username == GameManager.Player2.name)
            {
                if(component.User.Username == GameManager.Player1.name)
                {
                    if(GameManager.turn == 'X')
                    {
                        GameManager.board[row, col] = GameManager.Player1.choice;
                        GameManager.turn = GameManager.Player2.choice;
                        await component.UpdateAsync(properties => properties.Components = GameManager.GetBuilder().Result.Build());
                    }
                    else
                    {
                        await component.Channel.SendMessageAsync($"{component.User.Mention}, nu este randul tau!");
                    }
                }

                if (component.User.Username == GameManager.Player2.name)
                {
                    if(GameManager.turn == 'O')
                    {
                        GameManager.board[row, col] = GameManager.Player2.choice;
                        GameManager.turn = GameManager.Player1.choice;
                        await component.UpdateAsync(properties => properties.Components = GameManager.GetBuilder().Result.Build());
                    }
                    else
                    {
                        await component.Channel.SendMessageAsync($"{component.User.Mention}, nu este randul tau!");
                    }
                }
            }
            else
            {
                await component.Channel.SendMessageAsync(
                    $"{component.User.Mention} nu poti participa intrucat este deja un joc in desfasurare");
            }
        }
        else
        {
            await component.Channel.SendMessageAsync("Nu este niciun joc in desfasurare");
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