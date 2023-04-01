using Sudoku.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Data
{
    internal class DataLayer
    {
        public static void SaveGame(SudokuBoard sudoku)
        {
            int csvIsComplete;

            if (sudoku.IsComplete == true)
            {
                csvIsComplete = 1;
            }
            else
            {
                csvIsComplete = 0;
            }

            string csvOutput = $"{sudoku.GameID},{csvIsComplete}";

            foreach(string s in sudoku.GetMoves())
            {
                csvOutput += "," + s;
            }

            using StreamWriter writer = new(@"GameHistory.csv", true);
            writer.WriteLine(csvOutput);
        }

        public static SudokuBoard? LoadGame(int iD)
        {
            using (StreamReader reader = new(@"GameHistory.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var splitLine = line!.Split(',');

                    if (int.Parse(splitLine[0]) == iD)
                    {
                        bool isComplete;

                        if (splitLine[1] == "1")
                        {
                            isComplete = true;
                        }
                        else
                        {
                            isComplete = false;
                        }

                        List<string> lines = new();
                        for(int i = 2; i < splitLine.Length; i++)
                        {
                            lines.Add(splitLine[i]);
                        }
                        SudokuBoard loadedGame = new SudokuBoard(int.Parse(splitLine[0]), isComplete, lines);
                        return loadedGame;
                    }
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
    }
}
