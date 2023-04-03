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
            bool loadedGame = false;
            MainMenu();
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
                    loadedGame = true;
                    SudokuBoard? board = LoadGame();

                    if (board != null )
                    {
                        PlayGame(board, loadedGame);
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
                    loadedGame = true;
                    SudokuBoard? board = LoadGame();

                    if (board != null)
                    {
                        PlayGame(board, loadedGame);
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
                PlayGame(board, loadedGame);
            }
        }
    }

    public static void PlayGame(SudokuBoard board, bool loadedGame)
    {
        while (board.IsComplete == false)
        {
            GameMenu();
            board.PrintSudoku();
            var input = Console.ReadLine();

            if (input == null)
            {
                Console.WriteLine("invalid input");
            }
            else if (input.Length == 4)
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
                    Console.WriteLine("\nGame Completed");
                    board.IsComplete = true;
                    SaveGame(board, loadedGame);
                }
                else
                {
                    Console.WriteLine("\nGame not Completed");
                }
            }
            else if (input == "u")
            {
                board.Undo();
            }
            else if (input == "i")
            {
                board.Redo();
            }
            else if (input == "r")
            {
                board.Restart();
            }
            else if (input == "s")
            {
                DataLayer.SaveGame(board, loadedGame);
            }
            else if (input == "q")
            {
                SaveGame(board, loadedGame);
                break;
            }
            else
            {
                Console.WriteLine("invalid input y");
            }
        }
    }

    private static void SaveGame(SudokuBoard sudoku, bool loadedGame)
    {
        if (sudoku is null)
        {
            Console.WriteLine("Couldn't save game");
        }
        else
        {
            while (true)
            {
                Console.WriteLine("\nDo you want to save the game");
                Console.WriteLine("1 for yes 2 for no");
                var input = Console.ReadLine();

                if (input == "1")
                {
                    DataLayer.SaveGame(sudoku, loadedGame);
                    break;
                }
                else if (input == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("invalid input");
                }
            }
        }
    }

    private static SudokuBoard? LoadGame()
    {
        Console.WriteLine("\nEnter game ID of game to be loaded");
        try
        {
           var input = Console.ReadLine();

            if (input == null)
            {
                return null;
            }
            else if (DataLayer.LoadGame(input) != null)
            {
                SudokuBoard board = DataLayer.LoadGame(input);
                Console.WriteLine(board.GameID);
                return board;
            }
            else return null;
        }
        catch
        {
            Console.WriteLine("\nenter a number for game ID");
            return null;
        }
    }

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

    private static void GameMenu()
    {
        Console.WriteLine("\nenter check to check if the puzzle is complete");
        Console.WriteLine("enter a number with the form [column][row] [number] (i.e A9 3)");
        Console.WriteLine("case does not matter, sqares encased between _ _ cannot be changed");
        Console.WriteLine("\nenter u to undo last move");
        Console.WriteLine("enter i to redo last undone move");
        Console.WriteLine("enter r to restart");
        Console.WriteLine("enter s tosave");
        Console.WriteLine("enter q to exit to main menu\n");
    }
}