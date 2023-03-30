using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class SudokuBoard
    {
        private string initialboard = "0347060000000502007500000049702610483820470510060000920030000078005502400000079806";
        public string Board { get; set; }
        public int GameID { get; set; }

        private List<string> game;

        public SudokuBoard()
        {
            this.Board = initialboard;
            GameID = 0; 
        }

        public void PrintSudoku()
        {
            Console.WriteLine("    1   2   3   4   5   6   7   8   9");
            Console.WriteLine("  +-----------+-----------+-----------+");
            char[] rows = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
            int rowID = 0;

            for (int i = 1; i < Board.Length; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (j == 0)
                    {
                        Console.Write("{0} | {1} ", rows[rowID], Board[i - 1].ToString());
                        rowID++;
                    }
                    else
                    {
                        Console.Write("| {0} ", Board[i - 1].ToString());
                    }
                    if (j < 8)
                    {
                        i++;
                    }
                }
                Console.WriteLine("|");
                {
                    if (i % 3 == 0) Console.WriteLine("  +-----------+-----------+-----------+");
                }
            }
        }

        public void addEntry(string input)
        {
            int row = rowToInt(input[0]);
            int column = input[1] - '0';
            char entry = input[3];

            int offset = row * 9 + column;

            StringBuilder newBoard = new StringBuilder(Board);
            newBoard[offset - 1] = entry;
            Board = newBoard.ToString();
        }

        private int rowToInt(char header)
        {
            char[] rows = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
            int value = 0;

            for(int i = 0; i < rows.Length; ++i)
            {
                if(header == rows[i])
                {
                    value = i; ;
                }
            }
            return value;
        }

        public int GetID()
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
    }
}
