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
            MainMenu();
            var input = Console.ReadLine();
            string difficulty = "";
            // get difficulty from user
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
            // load unfinished game
            else if(input == "7")
            {
                try
                {
                    //show game IDs of unfinished games
                    Console.WriteLine("\nGame IDs of unfinished games:");
                    foreach (string s in DataLayer.GetIncompleteGames())
                    {
                        Console.WriteLine(s);
                    }
                    SudokuBoard? board = SudokuGame.LoadGame(false);

                    if (board != null )
                    {
                        SudokuGame.PlayGame(board, true, false);
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
                        SudokuGame.PlayGame(board, false, false);
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
                bool timer = false;

                while (true)
                {
                    Console.WriteLine("Do you want to add a time limit to your game? 1 for yes, 2 for no");
                    input = Console.ReadLine();

                    if (input == "1")
                    {
                        timer = true;
                        break;
                    }
                    else if (input == "2")
                    {
                        timer = false;
                        break;
                    }
                }
                SudokuBoard board = new(difficulty);
                SudokuGame.PlayGame(board, false, timer);
            }
        }
    }

    /// <summary>Displays the main menu.</summary>
    private static void MainMenu()
    {
        Console.WriteLine("\nenter 1 for easy");
        Console.WriteLine("enter 2 for medium");
        Console.WriteLine("enter 3 for hard");
        Console.WriteLine("enter 7 to load unfinished game");
        Console.WriteLine("enter 8 to replay a finished game");
        Console.WriteLine("enter 9 to the sequence of moves from an old game");
        Console.WriteLine("enter q to exit\n");
    }
}