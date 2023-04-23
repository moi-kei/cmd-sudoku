using Sudoku.Data;
using System.Diagnostics;

namespace Sudoku.Business
{
    internal static class SudokuGame
    {
        /// <summary>Saves the game.</summary>
        /// <param name="sudoku">The sudoku.</param>
        /// <param name="loadedGame">if set to <c>true</c> [loaded game].</param>
        public static void SaveGame(SudokuBoard sudoku, bool loadedGame)
        {
            // Check if the Sudoku board is null
            if (sudoku is null)
            {
                Console.WriteLine("Couldn't save game");
            }
            else
            {
                // Loop until the user enters a valid input
                while (true)
                {
                    Console.WriteLine("\nDo you want to save the game");
                    Console.WriteLine("1 for yes 2 for no");
                    // Read the user's input
                    var input = Console.ReadLine();
                    // Check if the user wants to save the game
                    if (input == "1")
                    {
                        // Save the game too the csv using the DataLayer class
                        DataLayer.SaveGame(sudoku, loadedGame, "3");
                        break;
                    }
                    // Check if the user does not want to save the game
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

        /// <summary>Loads the game.</summary>
        /// <param name="replay">if set to <c>true</c> [replay].</param>
        /// <returns>loaded game if it exists else null;</returns>
        public static SudokuBoard? LoadGame(bool replay)
        {
            Console.WriteLine("\nEnter game ID of game to be loaded");
            try
            {
                var input = Console.ReadLine();
                // If user types "q", return null to exit
                if (input == "q")
                {
                    return null;
                }

                // Attempt to load the game with the given ID
                try
                {
                    if (input != null)
                    {
                        SudokuBoard? board = DataLayer.LoadGame(input, replay);
                        return board;
                    }
                    else return null;
                }
                // If there was an error while loading the game, return null
                catch
                {
                    return null;
                }
            }
            // If user input is not a number, ask them to enter a valid number and return null
            catch
            {
                Console.WriteLine("\nenter a number for game ID");
                return null;
            }
        }

        /// <summary>Plays the game.</summary>
        /// <param name="sudoku">The sudoku.</param>
        /// <param name="loadedGame">if set to <c>true</c> [loaded game].</param>
        public static void PlayGame(SudokuBoard sudoku, bool loadedGame)
        {
            while (sudoku.IsComplete == false)
            {
                // Display game menu and print the sudoku board
                GameMenu();
                sudoku.PrintSudoku();
                // Take user input and handle different input cases
                var input = Console.ReadLine();

                //check if input is null
                if (input == null)
                {
                    Console.WriteLine("invalid input");
                }
                // Try to add user input to the sudoku board as a new entry
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
                // Check if the sudoku board is completed correctly
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
                // Undo last action on the sudoku board
                else if (input == "u")
                {
                    sudoku.Undo();
                }
                // Redo last undone action on the sudoku board
                else if (input == "i")
                {
                    sudoku.Redo();
                }
                // Restart the sudoku board
                else if (input == "r")
                {
                    sudoku.Restart();
                }
                // Quit the game and ask to save the current state of the sudoku board
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

        /// <summary>Plays the game.</summary>
        /// <param name="sudoku">The sudoku.</param>
        /// 
        public static void PlayTimedGame(SudokuBoard sudoku)
        {
            // get user to input a time limit
            int timeLimit;
            while (true) 
            {
                Console.WriteLine("timed games cannot be saved");
                Console.WriteLine("enter a value for the timer in seconds e.g. 300 for 5 minute timer");
                var input = Console.ReadLine();

                try
                {
                    if (input != null) 
                    { 
                        timeLimit = Int32.Parse(input);
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("timer invalid");
                }
            }
 
            // start stopwatch 
            Stopwatch stopWatch = new();
            stopWatch.Start();

            //play the game
            while (sudoku.IsComplete == false)
            {
                if (timeLimit < stopWatch.Elapsed.TotalSeconds)
                {
                    Console.WriteLine("Time's up!");
                    break;
                }

                // Display game menu and print the sudoku board
                GameMenu();
                Console.WriteLine($"Time: {stopWatch.Elapsed.TotalSeconds}");
                sudoku.PrintSudoku();
                // Take user input and handle different input cases
                var input = Console.ReadLine();

                //check if input is null
                if (input == null)
                {
                    Console.WriteLine("invalid input");
                }
                // Try to add user input to the sudoku board as a new entry
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
                // Check if the sudoku board is completed correctly
                else if (input == "c")
                {
                    if (sudoku.CheckSudoku() == true)
                    {
                        Console.WriteLine("\nGame Completed");
                        sudoku.IsComplete = true;
                    }
                    else
                    {
                        Console.WriteLine("\nGame not Completed");
                    }
                }
                // Undo last action on the sudoku board
                else if (input == "u")
                {
                    sudoku.Undo();
                }
                // Redo last undone action on the sudoku board
                else if (input == "i")
                {
                    sudoku.Redo();
                }
                // Restart the sudoku board
                else if (input == "r")
                {
                    sudoku.Restart();
                }
                // Quit the game and ask to save the current state of the sudoku board
                else if (input == "q")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("invalid input");
                }
            }
        }

        /// <summary>Replays the game step by step.</summary>
        /// <param name="sudoku">The sudoku.</param>
        public static void ReplayGame(SudokuBoard sudoku)
        {
            for (int i = 0; i < sudoku.GetMoves().Count ; i++)
            {
                ReplayMenu();
                sudoku.Board = sudoku.GetMoves()[i];
                sudoku.PrintSudoku();
                Console.WriteLine($"\nMove number: {i}");

                var input = Console.ReadLine();

                // Check if user wants to quit replay
                if ( input == "q")
                {
                    break;
                }
                // Check if user wants to go back one move
                else if (input == "b")
                {
                    i -= 2;
                }
            }
            Console.WriteLine("\nReplay complete");
        }

        /// <summary>Displays the main menu.</summary>
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

        /// <summary>Displays the game menu.</summary>
        private static void GameMenu()
        {
            Console.WriteLine("\nenter a number with the form [column][row][number] (i.e A93)");
            Console.WriteLine("enter a space with the form [column][row][ ] (i.e A9 ) to clear a square");
            Console.WriteLine("case does not matter, sqares encased between _ _ cannot be changed");
            Console.WriteLine("\nenter u to undo last move");
            Console.WriteLine("enter i to redo last undone move");
            Console.WriteLine("enter r to restart");
            Console.WriteLine("enter c to check if the puzzle is complete");
            Console.WriteLine("enter q to exit to main menu\n");
        }

        /// <summary>Displays the replay menu</summary>
        public static void ReplayMenu()
        {
            Console.WriteLine("\npress enter to step through the game");
            Console.WriteLine("enter b to go back");
            Console.WriteLine("enter q to exit\n");
        }
    }
}
