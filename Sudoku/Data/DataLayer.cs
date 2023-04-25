using Sudoku.Business;
using System.IO;
using System.Reflection.PortableExecutable;

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
        public static void SaveGame(SudokuBoard sudoku, bool loadedGame)
        {
            // if the game wasn't loaded append the new game to the csv
            if (loadedGame == false)
            {
                string csvOutput = SudokuToCSV(sudoku);

                using StreamWriter writer = new(@"GameHistory.csv", true);
                writer.WriteLine(csvOutput);
            }
            // if it was a loaded game rewrite thhe csv with the updated game
            else
            {
                string[] lines = File.ReadAllLines(@"GameHistory.csv");

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i][..1].Equals(sudoku.GameID))
                    {
                        lines[i] = SudokuToCSV(sudoku);
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

                // find game with gameID matching user input
                if (gameID.Equals(iD))
                {
                    bool isComplete;
                    //check if game is complete or not
                    if (splitLine[1] == "1")
                    {
                        isComplete = true;
                    }
                    else
                    {
                        isComplete = false;
                    }

                    // fill list with moves saved in the csv
                    List<string> lines = new();
                    for (int i = 4; i < splitLine.Length; i++)
                    {
                        lines.Add(splitLine[i]);
                    }

                    // create and run new SudukoBoard
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
                // find last entry in csv file
                // the highest gameID will always be the last added
                var lastLine = File.ReadLines(@"GameHistory.csv").Last();
                var splitLine = lastLine.Split(',');
                int nextID = int.Parse(splitLine[0]) + 1;

                //return new gameID
                return nextID;
            }
            // if the csv file has no entries return 1
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Getsthe incomplete games. 
        /// <returns>a List of strings showing incomplete games</returns>
        public static List<SudokuBoard> GetIncompleteGames()
        {
            List<SudokuBoard> games = new();
            using StreamReader reader = new(@"GameHistory.csv");
            // get a list of all completed games
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitLine = line!.Split(',');
                //if game is complete load the game and add to list
                if (splitLine[1] == "0")
                {
                    SudokuBoard? sudoku = LoadGame(splitLine[0], false);
                    if (sudoku != null)
                    {
                        games.Add(sudoku);
                    }
                }              
            }
            return games;
        }

        /// <summary>
        /// Gets the complete games.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCompleteGames()
        {
            // get a list of all incomplete games
            List<string> games = new();
            using StreamReader reader = new(@"GameHistory.csv");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitLine = line!.Split(',');
                //if game is complete create string represenmting game and add to list
                if (splitLine[1] == "1")
                {
                    string sudoku = $"GameID: {splitLine[0]} Completion time: {splitLine[2]} minutes";
                    if (sudoku != null)
                    {
                        games.Add(sudoku);
                    }
                }
            }
            return games;
        }

        /// <summary>
        /// Converts the sudoku object to csv form
        /// .</summary>
        /// <param name="sudoku">The SudokuBoard.</param>
        /// <returns>a string representing the game in a form compatible with csv</returns>
        private static string SudokuToCSV(SudokuBoard sudoku)
        {
            int isComplete;
            // convert isComplete bool to int 
            if (sudoku.IsComplete == true)
            {
                isComplete = 1;
            }
            else
            {
                isComplete = 0;
            }

            // construct string using properties of the SudokuBoard
            string csvOutput = $"{sudoku.GameID},{isComplete},{Math.Round(sudoku.Time,2)},{sudoku.TimeLimit}";
            // fill the rest of the csv with the moves made
            foreach (string s in sudoku.GetMoves())
            {
                csvOutput += "," + s;
            }
            return csvOutput;
        }

        /// <summary>
        /// Erases the save game data.
        /// </summary>
        public static void EraseData()
        {
            // open file and set the length to 0, wiping it
            using FileStream file = File.Open(@"GameHistory.csv", FileMode.Open);
            file.SetLength(0);
            file.Close();
        }
    }
}
