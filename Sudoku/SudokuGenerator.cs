using System;
using System.Text;

class SudokuGenerator
{
    private int[,] board = new int[9, 9];
    private Random rand = new Random();

    public string GeneratePuzzle()
    {
        FillBoard(0, 0);
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sb.Append(board[i, j]);
            }
        }

        return sb.ToString();
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
            int temp = values[i];
            values[i] = values[j];
            values[j] = temp;
        }

        return values;
    }
}
