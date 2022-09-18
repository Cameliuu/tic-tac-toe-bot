using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using tic_tac_toe_bot.Managers;
using RunMode = Discord.Commands.RunMode;

namespace tic_tac_toe_bot
{

    public class Bot
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private InteractionService _interactionService;

        public Bot()
        {
            
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug
            });
            _commandService = new CommandService(new CommandServiceConfig()
            {
                LogLevel = LogSeverity.Debug,
                CaseSensitiveCommands = true,
                IgnoreExtraArgs = true,
                DefaultRunMode = RunMode.Async
            });
            _interactionService = new InteractionService(_client, new InteractionServiceConfig()
            {
            LogLevel = LogSeverity.Debug,
            EnableAutocompleteHandlers = true,
            DefaultRunMode = Discord.Interactions.RunMode.Async
            });
            var collection = new ServiceCollection();
            collection.AddSingleton(_client);
            collection.AddSingleton(_commandService);
            collection.AddSingleton(_interactionService);
            ServiceManager.SetProvider(collection);


        }


public async Task Client_Ready()
{
    
    var globalCommand = new SlashCommandBuilder();
    globalCommand.WithName("first-global-command");
    globalCommand.WithDescription("This is my first global slash command");

    try
    {
        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
    }
    catch(ApplicationCommandException exception)
    {
  
        var json = JsonConvert.SerializeObject(exception.Errors,Formatting.Indented);


        Console.WriteLine(json);
    }
}


private async Task SlashCommandHandler(SocketSlashCommand command)
{
    await command.RespondAsync($"You executed {command.Data.Name}");
}
        public async Task MainAsync()
        {
            _client.SlashCommandExecuted += SlashCommandHandler;
            _client.Ready += Client_Ready;
            if (string.IsNullOrWhiteSpace(ConfigManager.Config.Token)) return;
            await _client.LoginAsync(TokenType.Bot, ConfigManager.Config.Token);
            await _client.StartAsync();
            // Let's hook the ready event for creating our commands in.
           
            await Task.Delay(-1);
        }
    }
}