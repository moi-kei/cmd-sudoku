﻿using Sudoku;

internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            MainMenu();
            string? input = Console.ReadLine();
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
                GameMenu();
                SudokuBoard board = new SudokuBoard(difficulty);

                while (board.IsComplete == false)
                {
                    board.PrintSudoku();
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
                            board.IsComplete = true;
                        }
                        else
                        {
                            Console.WriteLine("Game not Completed");
                        }
                    }
                    else if(input == "q")
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
    }

    private static void MainMenu()
    {
        Console.WriteLine("enter 1 for easy");
        Console.WriteLine("enter 2 for medium");
        Console.WriteLine("enter 3 for hard");
        Console.WriteLine("enter q to exit\n");
    }

    private static void GameMenu()
    {
        Console.WriteLine("enter check to check if the puzzle is complete");
        Console.WriteLine("enter a number with the form [column][row] [number] (i.e A9 3)");
        Console.WriteLine("case does not matter, sqares encased between _ _ cannot be changed");
        Console.WriteLine("enter q to exit to main menu\n");
    }
}