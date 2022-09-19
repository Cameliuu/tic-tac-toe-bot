using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace tic_tac_toe_bot.Managers;

public class CommandManager
{
    private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
    private static CommandService _commandservice = ServiceManager.GetService<CommandService>();
    public static async Task InstallCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;
        await _commandservice.AddModulesAsync(Assembly.GetEntryAssembly(),ServiceManager.Provider);
        foreach (var command in _commandservice.Commands)
            Console.WriteLine($"Command {command.Name} was loaded!");
    }

    private static async Task HandleCommandAsync(SocketMessage messageParam)
    {
        
        var message = messageParam as SocketUserMessage;    
        if (message == null) return;
        var argPos = 0;
        if (message.Author.IsBot || message.Channel is IDMChannel) return;

        if (!(message.HasStringPrefix(ConfigManager.Config.Prefix, ref argPos)) ||
            (message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            return;
        var context = new SocketCommandContext(_client, message);
        var result = await _commandservice.ExecuteAsync(context,argPos,ServiceManager.Provider);
    }
}