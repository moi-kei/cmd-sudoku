using Sudoku.Data;

namespace Sudoku.Business
{
    internal static class SudokuGame
    {
        public static void SaveGame(SudokuBoard sudoku, bool loadedGame)
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

        public static SudokuBoard? LoadGame(bool replay)
        {
            Console.WriteLine("\nEnter game ID of game to be loaded");
            try
            {
                var input = Console.ReadLine();

                if (input == "q")
                {
                    return null;
                }

                try
                {
                    SudokuBoard? board = DataLayer.LoadGame(input, replay);
                    return board;
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("\nenter a number for game ID");
                return null;
            }
        }

        public static void PlayGame(SudokuBoard sudoku, bool loadedGame)
        {
            while (sudoku.IsComplete == false)
            {
                GameMenu();
                sudoku.PrintSudoku();
                var input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("invalid input");
                }
                else if (input.Length > 1)
                {
                    try
                    {
                        sudoku.AddEntry(input);
                    }
                    catch
                    {
                        Console.WriteLine("invalid input");
                    }
                }
                else if (input == "c")
                {
                    if (sudoku.CheckSudoku() == true)
                    {
                        Console.WriteLine("\nGame Completed");
                        sudoku.IsComplete = true;
                        SudokuGame.SaveGame(sudoku, loadedGame);
                    }
                    else
                    {
                        Console.WriteLine("\nGame not Completed");
                    }
                }
                else if (input == "u")
                {
                    sudoku.Undo();
                }
                else if (input == "i")
                {
                    sudoku.Redo();
                }
                else if (input == "r")
                {
                    sudoku.Restart();
                }
                else if (input == "s")
                {
                    DataLayer.SaveGame(sudoku, loadedGame);
                }
                else if (input == "q")
                {
                    SudokuGame.SaveGame(sudoku, loadedGame);
                    break;
                }
                else
                {
                    Console.WriteLine("invalid input");
                }
            }
        }

        public static void ReplayGame(SudokuBoard sudoku)
        {
            for (int i = 0; i < sudoku.GetMoves().Count ; i++)
            {
                ReplayMenu();
                sudoku.Board = sudoku.GetMoves()[i];
                sudoku.PrintSudoku();
                Console.WriteLine($"\nMove number: {i}");

                var input = Console.ReadLine();

                if ( input == "q")
                {
                    break;
                }
                else if(input == "b")
                {
                    i -= 2;
                }
            }
            Console.WriteLine("\nReplay complete");
        }

        public static void MainMenu()
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
            Console.WriteLine("\nenter a number with the form [column][row][number] (i.e A93)");
            Console.WriteLine("enter a space with the form [column][row][ ] (i.e A9 ) to clear a square");
            Console.WriteLine("case does not matter, sqares encased between _ _ cannot be changed");
            Console.WriteLine("\nenter u to undo last move");
            Console.WriteLine("enter i to redo last undone move");
            Console.WriteLine("enter r to restart");
            Console.WriteLine("enter s to save");
            Console.WriteLine("enter c to check if the puzzle is complete");
            Console.WriteLine("enter q to exit to main menu\n");
        }

        public static void ReplayMenu()
        {
            Console.WriteLine("\npress enter to step through the game");
            Console.WriteLine("enter b to goo back");
            Console.WriteLine("enter q to exit\n");
        }
    }
}
