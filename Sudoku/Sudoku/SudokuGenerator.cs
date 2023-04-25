using System.Text;

class SudokuGenerator
{
    private readonly int[,] board = new int[9, 9];
    private readonly Random rand = new();

    /// <summary>
    /// Generates the completed puzzle.
    /// </summary>
    /// <param name="difficulty">The difficulty.</param>
    /// <returns>An 81 character string with the completed sudoku puzzle.</returns>
    public string GeneratePuzzle(string difficulty)
    {
        // Fill the Sudoku board with valid numbers
        GenerateCompletePuzzle(0, 0);
        // Create a StringBuilder to hold the generated Sudoku puzzle
        StringBuilder sb = new();
        // Iterate over the Sudoku board to append each value to the StringBuilder
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sb.Append(board[i, j]);
            }
        }
        // Replace some of the numbers in the generated puzzle based on the given difficulty level
        string completePuzzle = ReplaceChars(sb.ToString(), GameDifficulty(difficulty)[0], GameDifficulty(difficulty)[1]);
        return completePuzzle;
    }

    /// <summary>
    /// Fills the board.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    /// <returns>A 2D array with the completed sudoku</returns>
    private bool GenerateCompletePuzzle(int row, int column)
    {
        // If we have filled in all columns of the current row, move to the next row

        if (column == 9)
        {
            column = 0;
            row++;

            // If we have filled in all rows, the board is filled and we can return true
            if (row == 9)
            {
                return true;
            }
        }

        // Shuffle the values to try for the current position
        int[] values = ShuffleValues();

        // Try each value in the shuffled list for the current position
        foreach (int value in values)
        {
            // If the current value is a valid input for the current position, fill it in and recursively try to fill in the next position
            if (CheckSquare(row, column, value))
            {
                board[row, column] = value;

                // If the next position is successfully filled, we can return true
                if (GenerateCompletePuzzle(row, column + 1))
                {
                    return true;
                }
            }
        }

        // If none of the values are valid inputs for the current position, backtrack and reset the current position to 0
        board[row, column] = 0;
        return false;
    }

    /// <summary>
    /// Checks if the number in the square is a valid input
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <c>true</c> if the specified row is valid; otherwise, <c>false</c>.</returns>
    private bool CheckSquare(int row, int column, int value)
    {
        // Check if the value already exists in the row or column
        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == value || board[i, column] == value)
            {
                return false;
            }
        }
        // Check if the value already exists in the 3x3 box that contains the position
        int boxRow = (row / 3) * 3;
        int boxCol = (column / 3) * 3;

        for (int i = boxRow; i < boxRow + 3; i++)
        {
            for (int j = boxCol; j < boxCol + 3; j++)
            {
                if (board[i, j] == value)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Shuffles the values.
    /// </summary>
    /// <returns>an array with the values shuffled</returns>
    private int[] ShuffleValues()
    {
        // Create an array of integers with the values 1 to 9.
        int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // Loop through the array from the end to the start.
        for (int i = values.Length - 1; i > 0; i--)
        {
            // Generate a random integer between 0 and i
            int j = rand.Next(i + 1);
            // Swap the values at indices i and j
            (values[j], values[i]) = (values[i], values[j]);
        }
        return values;
    }

    /// <summary>
    /// Replaces the chars in a string with spaces.
    /// used to turn the completed sudoku puzzle into an incomplete one.
    /// </summary>
    /// <param name="inputString">The input string.</param>
    /// <param name="minNumOfCharsToReplace">The minimum number of chars to replace.</param>
    /// <param name="maxNumOfCharsToReplace">The maximum number of chars to replace.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    private static string ReplaceChars(string inputString, int minNumOfCharsToReplace, int maxNumOfCharsToReplace)
    {
        if (minNumOfCharsToReplace >= inputString.Length || maxNumOfCharsToReplace >= inputString.Length || minNumOfCharsToReplace > maxNumOfCharsToReplace)
        {
            // return the original input string if the range is invalid
            return inputString;
        }

        StringBuilder sb = new(inputString);
        // create a new instance of Random class to generate random indices
        Random rnd = new();
        // generate a random number between min and max range
        int numOfCharsToReplace = rnd.Next(minNumOfCharsToReplace, maxNumOfCharsToReplace + 1);

        for (int i = 0; i < numOfCharsToReplace; i++)
        {
            // generate a random index to replace in the StringBuilder object
            int indexToReplace = rnd.Next(0, sb.Length);
            // check if the square is not already blank to avoid the same number being removed more than once

            if (sb[indexToReplace] != ' ')
            {
                //replace the character at the specified index with space
                sb[indexToReplace] = ' ';
            }
            else
            {
                i--;
            }
        }
        //convert the StringBuilder object back to a string and return it
        return sb.ToString();
    }

    /// <summary>
    /// Converts the difficult into an array of 2 ints which represents the cariance of numbers removed from the completed puzzle
    /// </summary>
    /// <param name="difficulty">The difficulty.</param>
    /// <returns>An array of 2 integers</returns>
    private static int[] GameDifficulty(string difficulty)
    {
        int[] diff = new int[2];

        // removes 40 - 45 for easy
        if (difficulty == "easy")
        {
            diff[0] = 40;
            diff[1] = 45;
        }
        // removes 45 - 50 for medium
        else if (difficulty == "medium")
        {
            diff[0] = 45;
            diff[1] = 50;
        }
        // removes 50 - 55 for hard
        else if (difficulty == "hard")
        {
            diff[0] = 50;
            diff[1] = 55;
        }
        return diff;
    }
}
