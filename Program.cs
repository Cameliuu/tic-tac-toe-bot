using Discord;
using Discord.WebSocket;

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