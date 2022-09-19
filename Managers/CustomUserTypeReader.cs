using Discord;
using System.Threading.Tasks;
namespace tic_tac_toe_bot.Managers;

public class CustomUserTypeReader
{
    public static async Task<IUser> GetUserFromString(string s, IGuild server)
    {
        if (s.IndexOf('@') == -1 || s.Replace("<", "").Replace(">", "").Length != s.Length - 2)
            throw new System.Exception("Not a valid user mention.");

        string idStr = s.Replace("<", "").Replace(">", "").Replace("@", "");

        try
        {
            ulong id = ulong.Parse(idStr);
            return await server.GetUserAsync(id);
        }
        catch
        {
            throw new System.Exception("Could not parse User ID. Are you sure the user is still on the server?");
        }
    }
    
}