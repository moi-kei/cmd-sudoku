using System.Text;

class SudokuGenerator4x4
{
    private readonly int[,] board = new int[16, 16];
    private readonly Random rand = new();

    /// <summary>Generates the completed puzzle.</summary>
    /// <param name="difficulty">The difficulty.</param>
    /// <returns>An 81 character string with the completed sudoku puzzle.</returns>
    public string GeneratePuzzle(string difficulty)
    {
        // Fill the Sudoku board with valid numbers
        FillBoard(0, 0);
        // Create a StringBuilder to hold the generated Sudoku puzzle
        StringBuilder sb = new();
        // Iterate over the Sudoku board to append each value to the StringBuilder
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                if (board[i, j] <= 9)
                {
                    sb.Append(board[i, j]);
                }
                else
                {
                    sb.Append(ConvertToHex(board[i, j]));
                }
            }
        }
        // Replace some of the numbers in the generated puzzle based on the given difficulty level
        string completePuzzle = ReplaceChars(sb.ToString());
        return completePuzzle;
    }

    /// <summary>Fills the board.</summary>
    /// <param name="row">The row.</param>
    /// <param name="col">The column.</param>
    /// <returns>A 2D array with the completed sudoku</returns>
    private bool FillBoard(int row, int col)
    {
        // If we have filled in all columns of the current row, move to the next row

        if (col == 16)
        {
            col = 0;
            row++;

            // If we have filled in all rows, the board is filled and we can return true
            if (row == 16)
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
            if (IsValid(row, col, value))
            {
                board[row, col] = value;

                // If the next position is successfully filled, we can return true
                if (FillBoard(row, col + 1))
                {
                    return true;
                }
            }
        }

        // If none of the values are valid inputs for the current position, backtrack and reset the current position to 0
        board[row, col] = 0;
        return false;
    }

    /// <summary>Checks if the number in the square is a valid input</summary>
    /// <param name="row">The row.</param>
    /// <param name="col">The column.</param>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <c>true</c> if the specified row is valid; otherwise, <c>false</c>.</returns>
    private bool IsValid(int row, int col, int value)
    {
        // Check if the value already exists in the row or column
        for (int i = 0; i < 16; i++)
        {
            if (board[row, i] == value || board[i, col] == value)
            {
                return false;
            }
        }
        // Check if the value already exists in the 3x3 box that contains the position
        int boxRow = (row / 4) * 4;
        int boxCol = (col / 4) * 4;

        for (int i = boxRow; i < boxRow + 4; i++)
        {
            for (int j = boxCol; j < boxCol + 4; j++)
            {
                if (board[i, j] == value)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>Shuffles the values.</summary>
    /// <returns>an array with the values shuffled</returns>
    private int[] ShuffleValues()
    {
        // Create an array of integers with the values 1 to 9.
        int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10, 11, 12, 13, 14, 15, 16 };

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
    /// <returns>
    ///   <br />
    /// </returns>
    private static string ReplaceChars(string inputString)
    {
        StringBuilder sb = new(inputString);
        //create a new instance of Random class to generate random indices
        Random rnd = new();
        //generate a random number between min and max range
        int numOfCharsToReplace = rnd.Next(120, 140);

        for (int i = 0; i < numOfCharsToReplace; i++)
        {
            //generate a random index to replace in the StringBuilder object
            int indexToReplace = rnd.Next(0, sb.Length);
            //check if the square is not already blank to avoid the same number being removed more than once

            if (sb[indexToReplace] != ' ')
            {
                //replace the character at the specified index with space
                sb[indexToReplace] = ' ';
            }
            else i--;
        }
        //convert the StringBuilder object back to a string and return it
        return sb.ToString();
    }

    public static string ConvertToHex(int value)
    {
        if (value == 10)
        {
            return "A";
        }
        if (value == 12)
        {
            return "B";
        }
        if (value == 13)
        {
            return "C";
        }
        if (value == 14)
        {
            return "D";
        }
        if (value == 15)
        {
            return "E";
        }
        if (value == 16)
        {
            return "F";
        }
        else return " ";
    }
}
