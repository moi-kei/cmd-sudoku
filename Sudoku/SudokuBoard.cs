using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sudoku
{
    internal class SudokuBoard
    {
        private string initialBoardDone = "53467891267219534819834256785976142342685379171392485696153728428741963534528617_";
        private string initialBoard = "_347_6_______5_2__75______497_261_48382_47_51__6____92__3_____78__55_24______798_6";

        private string startingNumbers;
        public string Board { get; set; }
        public int GameID { get; set; }

        private List<string> game;

        public SudokuBoard()
        {
            this.Board = initialBoard;
            GameID = 0; 

            StringBuilder str = new StringBuilder();

            foreach(char c in initialBoard)
            {
                if(c == '_')
                {
                    str.Append('0');
                }
                else
                {
                    str.Append('1');
                }
            }

            startingNumbers = str.ToString();
        }

        public void PrintSudoku()
        {
            Console.WriteLine("    A   B   C   D   E   F   G   H   I");
            Console.WriteLine("  +-----------+-----------+-----------+");
            int rowID = 1;

            for (int i = 1; i < Board.Length; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (startingNumbers[i - 1] == '0')
                    {
                        if (j == 0)
                        {
                            Console.Write($"{rowID} | {Board[i - 1]} ");
                            rowID++;
                        }
                        else
                        {
                            Console.Write($"| {Board[i - 1]} ");
                        }
                        if (j < 8)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            Console.Write($"{rowID} |_{Board[i - 1]}_");
                            rowID++;
                        }
                        else
                        {
                            Console.Write(HttpUtility.HtmlDecode($"|_{Board[i - 1]}_"));
                        }
                        if (j < 8)
                        {
                            i++;
                        }
                    }
                }
                Console.WriteLine("|");
                {
                    if (i % 3 == 0) Console.WriteLine("  +-----------+-----------+-----------+");
                }
            }
        }

        public bool CheckSudoku()
        {

            for (int i = 0; i < 9; i++)
            {
                var columnCheck = new List<char>();
                for (int j = i; j < Board.Length; j+=9)
                {
                    if (!columnCheck.Contains(Board[j]) && Board[j] != '_')
                    {
                        columnCheck.Add(Board[j]);
                    }
                    else
                    {
                        return false;
                    }
                }
                if(columnCheck.Count != 9)
                {
                    return false;
                }
            }

            for (int i = 0; i < Board.Length; i++)
            {
                var rowCheck = new List<char>();
                for (int j = 0; j < 9; j++)
                {
                    if (!rowCheck.Contains(Board[i]) && Board[i] != '_')
                    {
                        rowCheck.Add(Board[i]);
                    }
                    else
                    {
                        return false;
                    }
                    if (j < 8)
                    {
                        i++;
                    }
                }
                if (rowCheck.Count != 9)
                {
                    return false;
                }
            }

            int x = 0;
            for (int i = 1; i < 10; i++)
            {
                var squareCheck = new List<char>();
                int y = x;

                for (int j = 1; j < 10; j++)
                {
                    if (!squareCheck.Contains(Board[y]) && Board[y] != '_')
                    {
                        squareCheck.Add(Board[y]);
                    }
                    else
                    {
                        return false;
                    }

                    if (j % 3 == 0)
                    {
                        y += 7;
                    }
                    else
                    {
                        y += 1;
                    }
                }

                if(i%3 == 0)
                {
                    x = x + 21;
                }
                else
                {
                    x += 3;
                }
            }

            

            return true;
        }

        public void AddEntry(string input)
        {
            input = input.ToUpper();
            int row = RowToInt(input[0]);
            int column = input[1] - '0';
            char entry = input[3];

            int offset = row * 9 + column -1;

            if (startingNumbers[offset] == '0')
            {
                if (entry == '1' || entry == '2' || entry == '3' || entry == '4' || entry == '5' || entry == '6' || entry == '7' || entry == '8' || entry == '9')
                {
                    StringBuilder newBoard = new StringBuilder(Board);
                    newBoard[offset] = entry;
                    Board = newBoard.ToString();
                }
                else
                {
                    Console.WriteLine("invalid input");
                }
            }
            else
            {
                Console.WriteLine("This square was filled initially and cant be changed");
            }
        }

        private static int RowToInt(char header)
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
