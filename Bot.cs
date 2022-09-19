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
            var collection = new ServiceCollection();
            collection.AddSingleton(_client);
            collection.AddSingleton(_commandService);
            ServiceManager.SetProvider(collection);
        }




        public async Task MainAsync()
        {
            if (string.IsNullOrWhiteSpace(ConfigManager.Config.Token)) return;
            await _client.LoginAsync(TokenType.Bot, ConfigManager.Config.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}