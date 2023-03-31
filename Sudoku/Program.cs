using Sudoku;
using System.Transactions;

internal class Program
{
    private static void Main(string[] args)
    {
        bool game = true;
        Console.WriteLine("enter quit to exit");
        Console.WriteLine("enter check to check if the puzzle is complete");
        Console.WriteLine("enter a number with the form [row][column] [number] (i.e A9 3)");
        Console.WriteLine("case does not matter, sqares encased between _ _ cannot be changed");


        while (game)
        {
            string input = Console.ReadLine();
            if (input == "quit")
            {
                game = false;
            }
            else if (input == "p")
            {
                SudokuBoard board = new SudokuBoard();
                board.PrintSudoku();

                SudokuBoard board2 = new SudokuBoard();
                board2.PrintSudoku();

                SudokuBoard board3 = new SudokuBoard();
                board3.PrintSudoku();

                input = Console.ReadLine();

                if (input.Length == 4)
                {
                    try
                    {
                        board.AddEntry(input);
                    }
                    catch
                    {
                        Console.WriteLine("invalid input");
                    }
                }
                else if (input == "check")
                {
                    if (board.CheckSudoku() == true)
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
            else
            {
                Console.WriteLine("invalid input");
            }

        }
    }
}