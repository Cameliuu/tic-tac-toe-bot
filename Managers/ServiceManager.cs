using Microsoft.Extensions.DependencyInjection;

namespace tic_tac_toe_bot.Managers;

public class ServiceManager
{
    public static IServiceProvider Provider { get; private set; }
    public static void SetProvider(ServiceCollection collection) => Provider = collection.BuildServiceProvider();
    public static T GetService<T>() where T : new() => Provider.GetRequiredService<T>();
}