using System.Text;

class SudokuGenerator
{
    private readonly int[,] board = new int[9, 9];
    private readonly Random rand = new();

    public string GeneratePuzzle(string difficulty)
    {
        FillBoard(0, 0);
        StringBuilder sb = new();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sb.Append(board[i, j]);
            }
        }
        string completePuzzle = ReplaceChars(sb.ToString(), GameDifficulty(difficulty)[0], GameDifficulty(difficulty)[1]);
        return completePuzzle;
    }

    private bool FillBoard(int row, int col)
    {
        if (col == 9)
        {
            col = 0;
            row++;

            if (row == 9)
            {
                return true;
            }
        }

        int[] values = ShuffleValues();

        foreach (int value in values)
        {
            if (IsValid(row, col, value))
            {
                board[row, col] = value;

                if (FillBoard(row, col + 1))
                {
                    return true;
                }
            }
        }

        board[row, col] = 0;
        return false;
    }

    private bool IsValid(int row, int col, int value)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == value || board[i, col] == value)
            {
                return false;
            }
        }

        int boxRow = (row / 3) * 3;
        int boxCol = (col / 3) * 3;

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

    private int[] ShuffleValues()
    {
        int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        for (int i = values.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (values[j], values[i]) = (values[i], values[j]);
        }

        return values;
    }

    private static string ReplaceChars(string inputString, int minNumOfCharsToReplace, int maxNumOfCharsToReplace)
    {
        if (minNumOfCharsToReplace >= inputString.Length || maxNumOfCharsToReplace >= inputString.Length || minNumOfCharsToReplace > maxNumOfCharsToReplace)
        {
            //return the original input string if the range is invalid
            return inputString;
        }

        //create a StringBuilder object from the input string
        StringBuilder sb = new(inputString);

        //create a new instance of Random class to generate random indices
        Random rnd = new();

        //generate a random number between min and max range (inclusive)
        int numOfCharsToReplace = rnd.Next(minNumOfCharsToReplace, maxNumOfCharsToReplace + 1); 

        for (int i = 0; i < numOfCharsToReplace; i++)
        {
            //generate a random index to replace in the StringBuilder object
            int indexToReplace = rnd.Next(0, sb.Length);
            if (sb[indexToReplace] != ' ')
            {
                //replace the character at the specified index with underscore character
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

    private static int[] GameDifficulty(string difficulty)
    {
        int[] diff = new int[2];

        if (difficulty == "easy")
        {
            diff[0] = 40;
            diff[1] = 45;
        }
        else if (difficulty == "medium")
        {
            diff[0] = 45;
            diff[1] = 50;
        }
        else if (difficulty == "hard")
        {
            diff[0] = 50;
            diff[1] = 55;
        }
        return diff;
    }
}
