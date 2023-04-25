using Sudoku.Data;
using System.Text;

namespace Sudoku.Business
{
    internal class SudokuBoard
    {
        /// <summary>
        /// a string of 0s and 1s representing the starting numbers 
        /// </summary>
        private readonly string startingNumbers;

        /// <summary>
        /// Gets the game identifier.
        /// </summary>
        /// <value>
        /// The game identifier.
        /// </value>
        public string GameID { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is complete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets or sets the board.
        /// </summary>
        /// <value>
        /// The board.
        /// </value>
        public string Board { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public double Time { get; set; }

        public double TimeLimit { get; set; }

        /// <summary>
        /// The game history
        /// </summary>
        private readonly List<string> gameHistory;

        /// <summary>
        /// The undone moves
        /// </summary>
        private readonly List<string> undoneMoves;

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuBoard" /> class.
        /// </summary>
        /// <param name="difficulty">
        /// The difficulty.</param>
        public SudokuBoard(string difficulty)
        {
            //generate a new sudoku puzzle from scratch
            SudokuGenerator generator = new();
            Board = generator.GeneratePuzzle(difficulty);
            //get new ID
            GameID = DataLayer.GetID().ToString();
            //set isComplete to false and instatiate the lists
            IsComplete = false;
            Time = 0;
            TimeLimit = 0;
            gameHistory = new List<string>{Board};
            undoneMoves = new List<string>();
            //record the starting numbers
            startingNumbers = GetStartingNumbers(Board);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuBoard" /> class.
        /// For loading games from csv file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isComplete">if set to <c>true</c> [is complete].</param>
        /// <param name="gameHistory">The game history.</param>
        /// <param name="replay">if set to <c>true</c> [replay].</param>
        public SudokuBoard(string id, bool isComplete, List<string> gameHistory, bool replay, double time, double timeLimit)
        {
            // record starting numbers from the first entry in the gameHistory
            startingNumbers = GetStartingNumbers(gameHistory[0]);
            undoneMoves = new List<string>();

            // if the game is not complete
            if (isComplete == false)
            {
                // continue on from last point
                IsComplete = isComplete;
                GameID = id;
                this.gameHistory = gameHistory;
                Board = gameHistory.Last();
                Time = time;
                TimeLimit = timeLimit;
            }
            // if the game is complete
            else
            {
                // reset the game with thge original starting numbers
                this.IsComplete = false;
                GameID = DataLayer.GetID().ToString();
                Board = gameHistory[0];
                time = 0;
                TimeLimit = 0;
                if (replay == true)
                {
                    this.gameHistory = gameHistory;
                }
                else
                {
                    this.gameHistory = new List<string> { Board };
                }
            }
        }

        /// <summary>
        /// Prints the sudoku.
        /// </summary>
        public void PrintSudoku()
        {
            // print headers
            Console.WriteLine("    A   B   C   D   E   F   G   H   I");
            Console.WriteLine("  +===========+===========+===========+");
            int rowID = 1;

            for (int i = 1; i < Board.Length; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    // print starting number differently from others so the user can differentiate
                    // numbers that cannot be changed (starting numbers) will be between underscores e.g. _3_
                    if (startingNumbers[i - 1] == '0')
                    {
                        // if it's the start of a row print the row ID aswell
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

                //print = insted of - every 3rd row so the user can easily see the 3x3 square grids
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

        /// <summary>
        /// Checks the sudoku.
        /// </summary>
        /// <returns>false if puzzle is incomplete true if complete</returns>
        public bool CheckSudoku()
        {
            // check every row contains 9 unique numbers
            for (int i = 0; i < 9; i++)
            {
                var columnCheck = new List<char>();
                for (int j = i; j < Board.Length; j += 9)
                {
                    if (!columnCheck.Contains(Board[j]) && Board[j] != ' ')
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

            //check every column contains 9 unique numbers
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

            //check each 3x3 square contains 9 unique numbers
            int x = 0;
            for (int i = 1; i < 10; i++)
            {
                var squareCheck = new List<char>();
                int y = x;

                for (int j = 1; j < 10; j++)
                {
                    if (!squareCheck.Contains(Board[y]) && Board[y] != ' ')
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

        /// <summary>
        /// Adds the entry into the sudoku.
        /// </summary>
        /// <param name="input">The input.</param>
        public void AddEntry(string input)
        {
            // convert the input string into numbers
            input = input.ToUpper();
            int column = ColumnToInt(input[0]);
            int row = input[1] - '0';
            char entry = input[2];
            // get the index of the number entered
            int index = row * 9 + column - 9;
            // reset undoneMoves
            undoneMoves.Clear();

            // check the number is not a starting number
            if (startingNumbers[index] == '0')
            {
                //check the entered number is a valid input
                if (entry == '1' || entry == '2' || entry == '3' || entry == '4' || entry == '5' || entry == '6' || entry == '7' || entry == '8' || entry == '9' || entry == ' ')
                {
                    //input the number entered into the board
                    StringBuilder newBoard = new(Board);
                    newBoard[index] = entry;
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

        /// <summary>
        /// Gets the moves.
        /// </summary>
        /// <returns>A list of stages of the game</returns>
        public List<string> GetMoves()
        {
            return gameHistory;
        }

        /// <summary>Undoes the last move.</summary>
        public void Undo() 
        { 
            // Only undo move id a move has been made
            if (gameHistory.Count > 1)
            {
                //undo move and add the move to undoneMoves list
                undoneMoves.Add(gameHistory.Last());
                gameHistory.Remove(gameHistory.Last());
                Board = gameHistory.Last();
            }
            else
            {
                Console.WriteLine("Can't undo move");
            }
        }

        /// <summary>Redoes last undone move.</summary>
        public void Redo()
        {
            // only redo if undo has been used
            if (undoneMoves.Count > 0)
            {
                // add undone move back to gameHistory and remove from undoneMoves
                gameHistory.Add(undoneMoves.Last());
                undoneMoves.Remove(undoneMoves.Last());
                Board = gameHistory.Last();
            }
            else
            {
                Console.WriteLine("Can't redo move");
            }
        }

        /// <summary>
        /// Restarts this Sudoku game.
        /// Doesn't wipe progress in the game, instead adds the starting state of the board as a new move so that the restart can be undone by the user
        /// </summary>
        public void Restart()
        {
            Console.WriteLine("are you sure you want to restart puzzle form the beinning? \nthis can be undone if you change your mind. \n1 for yes, 2 for no");
            while (true)
            {
                var input = Console.ReadLine();
                //id input is 1 return game to starting state
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

        /// <summary>
        /// records the numbers that were on the board at the beginning so the game knows they can't be changed
        /// </summary>
        /// <param name="startingBoard">The starting board.</param>
        /// <returns>returns a string of 0's if the square is blank or 1's if the square is filled</returns>
        private static string GetStartingNumbers(String startingBoard)
        {
            StringBuilder str = new();
            foreach (char c in startingBoard)
            {
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

        /// <summary>
        /// Changes column header to an int.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>an integer i.e. A = 0</returns>
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
