using Sudoku.Business;
using Sudoku.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        while (true)
        {
            SudokuGame.MainMenu();
            var input = Console.ReadLine();
            string difficulty = "";

            if (input == "1")
            {
                difficulty = "easy";
            }
            else if (input == "2")
            {
                difficulty = "medium";
            }
            else if (input == "3")
            {
                difficulty = "hard";
            }
            else if(input == "7")
            {
                try
                {
                    Console.WriteLine("\nGame IDs of unfinished games:");
                    foreach (string s in DataLayer.GetIncompleteGames())
                    {
                        Console.WriteLine(s);
                    }
                    SudokuBoard? board = SudokuGame.LoadGame(false);

                    if (board != null )
                    {
                        SudokuGame.PlayGame(board, true);
                    }
                }
                catch
                {
                    Console.WriteLine("\nCouldn't load game");
                }
            }
            else if(input == "8")
            {
                try
                {
                    Console.WriteLine("\nGame IDs of finished games:");
                    foreach (string s in DataLayer.GetCompleteGames())
                    {
                        Console.WriteLine(s);
                    }

                    SudokuBoard? board = SudokuGame.LoadGame(false);

                    if (board != null)
                    {
                        SudokuGame.PlayGame(board, false);
                    }
                }
                catch
                {
                    Console.WriteLine("\nCouldn't load game");
                }
            }
            else if (input == "9")
            {
                try
                {
                    Console.WriteLine("\nGame IDs of finished games:");
                    foreach (string s in DataLayer.GetCompleteGames())
                    {
                        Console.WriteLine(s);
                    }

                    SudokuBoard? board = SudokuGame.LoadGame(true);

                    if (board != null)
                    {
                        SudokuGame.ReplayGame(board);
                    }
                }
                catch
                {
                    Console.WriteLine("\nCouldn't load game");
                }
            }
            else if (input == "q")
            {
                break;
            }
            else
            {
                Console.WriteLine("invalid input");
            }

            if(difficulty != "")
            {
                SudokuBoard board = new(difficulty);
                SudokuGame.PlayGame(board, false);
            }
        }
    }
}