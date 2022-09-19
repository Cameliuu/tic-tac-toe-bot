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

    public static async Task updateComponent(SocketMessageComponent component)
    {
        var builder = new ComponentBuilder()
            .WithButton("rahat", "custom-id",style:ButtonStyle.Primary);
        await component.UpdateAsync(properties => properties.Components = builder.Build());
    }
    public static async Task ComponentHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "1":
                updateComponent(component);
                break;
            case "2":
                updateComponent(component);
                break;
            case "3":
                updateComponent(component);
                break;
            case "4":
                updateComponent(component);
                break;
            case "5":
                updateComponent(component);
                break;
            case "6":
                updateComponent(component);
                break;
            case "7":
                updateComponent(component);
                break;
            case "8":
                updateComponent(component);
                break;
            case "9":
                updateComponent(component);
                break;
            
            
        }
    }
}