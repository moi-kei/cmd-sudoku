using Sudoku.Data;
using System.Text;

namespace Sudoku.Business
{
    internal class SudokuBoard
    {
        private readonly string startingNumbers;
        public string GameID { get; }
        public bool IsComplete { get; set; }
        public string Board { get; set; }
        public string? UndoneMove { get; set; }
        private readonly List<string> gameHistory;

        public SudokuBoard(string difficulty)
        {
            SudokuGenerator generator = new();
            Board = generator.GeneratePuzzle(difficulty);
            GameID = DataLayer.GetID().ToString();
            IsComplete = false;
            gameHistory = new List<string>{Board};
            startingNumbers = GetStartingNumbers(Board);
        }

        public SudokuBoard(string id, bool isComplete, List<string> gameHistory)
        {
            IsComplete = isComplete;
            startingNumbers = GetStartingNumbers(gameHistory[0]);

            if(IsComplete == false)
            {
                GameID = id;
                this.gameHistory = gameHistory;
                Board = gameHistory.Last();
            }
            else
            {
                GameID = DataLayer.GetID().ToString();
                Board = gameHistory[0];
                this.gameHistory = new List<string> {Board};
            }
        }

        public void PrintSudoku()
        {
            Console.WriteLine($" Game ID: {GameID}");
            Console.WriteLine("    A   B   C   D   E   F   G   H   I");
            Console.WriteLine("  +===========+===========+===========+");
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
                            Console.Write($"|_{Board[i - 1]}_");
                        }
                        if (j < 8)
                        {
                            i++;
                        }
                    }
                }

                Console.WriteLine("|");

                if (i % 27 == 0)
                {
                    Console.WriteLine("  +===========+===========+===========+");
                }
                else if (i % 3 == 0)
                {
                    Console.WriteLine("  +-----------+-----------+-----------+");
                }

            }
        }

        public bool CheckSudoku()
        {
            for (int i = 0; i < 9; i++)
            {
                var columnCheck = new List<char>();
                for (int j = i; j < Board.Length; j += 9)
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
                if (columnCheck.Count != 9)
                {
                    return false;
                }
            }

            for (int i = 0; i < Board.Length; i++)
            {
                var rowCheck = new List<char>();
                for (int j = 0; j < 9; j++)
                {
                    if (!rowCheck.Contains(Board[i]) && Board[i] != ' ')
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

                if (i % 3 == 0)
                {
                    x += 21;
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
            int column = ColumnToInt(input[0]);
            int row = input[1] - '0';
            char entry = input[3];

            int offset = row * 9 + column - 9;

            if (startingNumbers[offset] == '0')
            {
                if (entry == '1' || entry == '2' || entry == '3' || entry == '4' || entry == '5' || entry == '6' || entry == '7' || entry == '8' || entry == '9' || entry == ' ')
                {
                    StringBuilder newBoard = new(Board);
                    newBoard[offset] = entry;
                    Board = newBoard.ToString();
                    gameHistory.Add(Board);
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

        public List<string> GetMoves()
        {
            return gameHistory;
        }

        public void Undo()
        {
            if (gameHistory.Count > 1)
            {
                UndoneMove = gameHistory.Last();
                gameHistory.Remove(gameHistory.Last());
                Board = gameHistory.Last();
            }
            else
            {
                Console.WriteLine("Can't undo move");
            }
        }

        public void Redo()
        {
            if (UndoneMove != null)
            {
                gameHistory.Add(UndoneMove);
                Board = gameHistory.Last();
            }
            else
            {
                Console.WriteLine("Can't redo move");
            }
        }

        public void Restart()
        {
            Console.WriteLine("are you sure you want to restart puzzle form the beinning? \nthis can be undone if you change your mind. \n1 for yes, 2 for no");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "1")
                {
                    gameHistory.Add(Board);
                    Board = gameHistory[0];
                    break;
                }
                else if(input =="2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("invalid input");
                }
            }
        }

        private static string GetStartingNumbers(String startingBoard)
        {
            StringBuilder str = new();
            foreach (char c in startingBoard)
            {
                Console.WriteLine(c);
                if (c == ' ')
                {
                    str.Append('0');
                }
                else
                {
                    str.Append('1');
                }
            }
            return str.ToString();
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
    }
}
