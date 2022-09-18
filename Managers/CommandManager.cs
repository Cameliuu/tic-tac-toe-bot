using Discord.Interactions;
using Discord.WebSocket;

namespace tic_tac_toe_bot.Managers;

public class CommandManager
{
    private DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
    private InteractionService _interactionService = ServiceManager.GetService<InteractionService>();
}