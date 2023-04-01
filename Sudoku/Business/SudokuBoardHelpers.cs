internal static class SudokuBoardHelpers
{

    

    public static int ColumnToInt(char header)
    {
        char[] rows = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
        int value = -1;

        for (int i = 0; i < rows.Length; ++i)
        {
            if (header == rows[i])
            {
                value = i;
            }
        }
        return value;
    }
}