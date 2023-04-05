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
                    loadedGame = true;
                    SudokuBoard? board = SudokuGame.LoadGame();

                    if (board != null )
                    {
                        SudokuGame.PlayGame(board, loadedGame);
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
                    loadedGame = false;
                    SudokuBoard? board = SudokuGame.LoadGame();

                    if (board != null)
                    {
                        SudokuGame.PlayGame(board, loadedGame);
                    }
                }
                catch
                {
                    Console.WriteLine("\nCouldn't load game");
                }
            }
            else if (input == "9")
            {

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
                SudokuGame.PlayGame(board, loadedGame);
            }
        }
    }

    private static void ReplayGame()
    {

    }
}