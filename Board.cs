namespace tic_tac_toe_bot.Managers;

public class Board
{
    public  char[,] choices=new char[3, 3]{{' ',' ',' '},{' ',' ',' '},{' ',' ',' '}};
    public  bool[,] disabled = new bool[3,3] { {false, false, false},{false, false, false}, {false, false, false} };
}