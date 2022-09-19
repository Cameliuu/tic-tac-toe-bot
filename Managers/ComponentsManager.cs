using Discord.WebSocket;

namespace tic_tac_toe_bot.Managers;

public class ComponentsManager
{
    private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();

    public static async Task loadComponents()
    {
        _client.ButtonExecuted += ComponentHandler;
    }
    public static async Task ComponentHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "custom-id":
                await component.RespondAsync($"{component.User.Mention} has clicked the button!");
                break;
        }
    }
}