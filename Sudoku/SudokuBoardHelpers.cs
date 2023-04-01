internal static class SudokuBoardHelpers
{

    public static int GetID()
    {
        try
        {
            var lastLine = File.ReadLines(@"GameHistory.csv").Last();
            var splitLine = lastLine.Split(',');
            int nextID = int.Parse(splitLine[0]) + 1;

            return nextID;
        }
        catch
        {
            return 10000000;
        }
    }

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

        public static int GetIndex(string input)
        {
            input = input.ToUpper();
            int row = ColumnToInt(input[0]);
            int column = input[1] - '0';

            return row * 9 + column - 1;
        }
}