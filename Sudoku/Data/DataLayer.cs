using Sudoku.Business;

namespace Sudoku.Data
{
    /**
    * Static class containing functions for interacting with the csv file.
    * Deals with saving and loading games.
    * @author Michael Mackenzie
    */
    internal class DataLayer
    {

        /// <summary>
        /// Saves the game of sudoku to the csv file
        /// </summary>
        /// <param name="sudoku">The board that is being saved</param>
        /// <param name="loadedGame">boolean indicating wether the game had been loaded or is a new game</param>
        public static void SaveGame(SudokuBoard sudoku, bool loadedGame, string gameType)
        {
            if (loadedGame == false)
            {
                string csvOutput = SudokuToCSV(sudoku, gameType);

                using StreamWriter writer = new(@"GameHistory.csv", true);
                writer.WriteLine(csvOutput);
            }
            else
            {
                string[] lines = File.ReadAllLines(@"GameHistory.csv");

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i][..1].Equals(sudoku.GameID))
                    {
                        lines[i] = SudokuToCSV(sudoku, gameType);
                    }
                }

                using StreamWriter writer = new(@"GameHistory.csv", false);
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Loads the game.
        /// </summary>
        /// <param name="iD">The i d.</param>
        /// <param name="replay">if set to <c>true</c> [replay].</param>
        /// <returns>returns the sudokuBoard if it exists else it returns null</returns>
        public static SudokuBoard? LoadGame(string iD, bool replay)
        {
            using StreamReader reader = new(@"GameHistory.csv");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitLine = line!.Split(',');
                string gameID = splitLine[0];


                if (gameID.Equals(iD))
                {
                    bool isComplete;

                    Console.WriteLine(splitLine[1]);

                    if (splitLine[1] == "1")
                    {
                        isComplete = true;
                    }
                    else
                    {
                        isComplete = false;
                    }

                    List<string> lines = new();
                    for (int i = 4; i < splitLine.Length; i++)
                    {
                        lines.Add(splitLine[i]);
                    }

                    SudokuBoard loadedGame = new(gameID, isComplete, lines, replay, Double.Parse(splitLine[2]), Double.Parse(splitLine[3]));
                    return loadedGame;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns>the Id value for the new game</returns>
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
                return 1;
            }
        }

        /// <summary>
        /// Getsthe incomplete games.
        /// </summary>
        /// <returns>a List of strings showing incomplete games</returns>
        public static List<string> GetIncompleteGames()
        {
            List<string> incompleteGames = new();
            using StreamReader reader = new(@"GameHistory.csv");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitLine = line!.Split(',');
                if (splitLine[1].Equals("0"))
                {
                    incompleteGames.Add(splitLine[0]);
                }              
            }
            return incompleteGames;
        }

        /// <summary>
        /// Gets the complete games.
        /// </summary>
        /// <returns>a list of string showing all competed games</returns>
        public static List<string> GetCompleteGames()
        {
            List<string> completeGames = new();
            using StreamReader reader = new(@"GameHistory.csv");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitLine = line!.Split(',');
                if (splitLine[1] == "1")
                {
                    completeGames.Add(splitLine[0]);
                }
            }
            return completeGames;
        }

        /// <summary>
        /// Converts the sudoku object to csv form
        /// .</summary>
        /// <param name="sudoku">The SudokuBoard.</param>
        /// <returns>a string representing the game in a form compatible with csv</returns>
        private static string SudokuToCSV(SudokuBoard sudoku, string gameType)
        {
            int isComplete;

            if (sudoku.IsComplete == true)
            {
                isComplete = 1;
            }
            else
            {
                isComplete = 0;
            }
            string csvOutput = $"{sudoku.GameID},{isComplete},{sudoku.Time},{sudoku.TimeLimit}";

            foreach (string s in sudoku.GetMoves())
            {
                csvOutput += "," + s;
            }
            return csvOutput;
        }
    }
}
