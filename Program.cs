using Discord;
using Discord.WebSocket;
using tic_tac_toe_bot;

public class Program
{
    private DiscordSocketClient _client;
    public static Task Main(string[] args) => new Bot().MainAsync();
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

}