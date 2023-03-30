using Sudoku;
using System.Transactions;

bool game = true;
SudokuBoard board = new SudokuBoard();
Console.WriteLine("enter quit to exit");
Console.WriteLine("enter check to check if the puzzle is complete");
Console.WriteLine("enter a number with the form [row][column] [number] (i.e A9 3)");
Console.WriteLine("case does not matter, sqares encased between _ _ cannot be changed");


while (game)
{
    board.PrintSudoku();

    while(true)
    {
        string? input = Console.ReadLine();

        if (input == "quit")
        {
            game = false;
            break;
        }
        else if (input.Length == 4)
        {
            board.AddEntry(input);
            break;
        }
        else if(input == "check")
        {
            if(board.CheckSudoku() == true)
            {
                Console.WriteLine("Game Completed");
                game = false;
            }
            else
            {
                Console.WriteLine("Game not Completed");
            }
        }
        else
        {
            Console.WriteLine("invalid input");
        }
    }
}
