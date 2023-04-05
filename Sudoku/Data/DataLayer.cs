using Sudoku.Business;

namespace Sudoku.Data
{
    internal class DataLayer
    {
        public static void SaveGame(SudokuBoard sudoku, bool loadedGame)
        {
            if (loadedGame == false)
            {
                string csvOutput = SudokuToCSV(sudoku);

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
                    for (int i = 2; i < splitLine.Length; i++)
                    {
                        lines.Add(splitLine[i]);
                    }

                    SudokuBoard loadedGame = new(gameID, isComplete, lines, replay);
                    return loadedGame;
                }
            }
            return null;
        }

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

        public static List<string> GetCompleteGames()
        {
            List<string> completeGames = new();
            using StreamReader reader = new(@"GameHistory.csv");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitLine = line!.Split(',');
                if (splitLine[1].Equals("1"))
                {
                    completeGames.Add(splitLine[0]);
                }
            }
            return completeGames;
        }

        private static string SudokuToCSV(SudokuBoard sudoku)
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
            string csvOutput = $"{sudoku.GameID},{isComplete}";

            foreach (string s in sudoku.GetMoves())
            {
                csvOutput += "," + s;
            }
            return csvOutput;
        }
    }
}
