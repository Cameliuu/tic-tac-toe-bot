using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;

namespace tic_tac_toe_bot.Managers;

public class CommandManager
{
    private DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
    private CommandService _commandservice = ServiceManager.GetService<CommandService>();
    public async Task InstallCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;
        
        await _commandservice.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),ServiceManager.Provider);
        foreach (var command in _commandservice.Commands)
            Console.WriteLine($"Command {command} was loaded!");
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;
        if (message == null) return;
        
        int argPos = 0;
        
        if (!(message.HasCharPrefix('!', ref argPos) || 
              message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
            return;
        
        var context = new SocketCommandContext(_client, message);
        await _commandservice.ExecuteAsync(
            context: context, 
            argPos: argPos,
            services: null);
    }
}