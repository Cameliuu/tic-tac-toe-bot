using Discord;
using Discord.WebSocket;

namespace tic_tac_toe_bot;

public class Player
{
    public string name;
    public char choice;
    public int[] choices = { 0, 0, 0, 0, 0, 0 , 0,0 };
}