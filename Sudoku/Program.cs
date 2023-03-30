using Sudoku;

bool game = true;
SudokuBoard board = new SudokuBoard();
Console.WriteLine("press q to quit");
Console.WriteLine("enter a number with the form [row][column] [number] (i.e A9 3)");


while (game)
{
    board.PrintSudoku();

    string? input = Console.ReadLine();

    while(true)
    {
        if (input == "quit")
        {
            game = false;
            break;
        }
        else if (input.Length == 4)
        {
            board.addEntry(input);
            break;
        }
        else
        {
            Console.WriteLine("invalid input");
        }
    }
}
