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
        private static DiscordSocketClient _client;
        private static CommandService _commandService; 

        public Bot()
        {
            
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
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



        public static async Task OnReady()
        {
            Console.WriteLine($"{DateTime.Now}\tBot is Ready");
            await _client.SetStatusAsync(UserStatus.Online);
            await _client.SetGameAsync($"Prefix: {ConfigManager.Config.Prefix}");
        }
        public async Task MainAsync()
        {
            await ComponentsManager.loadComponents();
            _client.Ready += OnReady;
            if (string.IsNullOrWhiteSpace(ConfigManager.Config.Token)) return;
            await CommandManager.InstallCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, ConfigManager.Config.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}