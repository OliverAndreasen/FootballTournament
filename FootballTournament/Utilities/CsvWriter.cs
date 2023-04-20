using System;
using System.IO;

namespace FootballTournament.Utilities
{
    public class CsvWriter
    {
        public void WritePostponedMatches(string[] values, int postponedMatchNumber, string fileName)
        {
            var roundNumber = GetRoundNumberFromFileName(fileName);
            var postponedFileName = $"round{roundNumber}-{postponedMatchNumber}.csv";
            var postponedFilePath = GetFilePath(Path.Combine("Rounds", postponedFileName));

            using (var writer = new StreamWriter(postponedFilePath, true))
            {
                writer.WriteLine(string.Join(',', values));
                writer.Flush(); // Flush changes to the file
            }
        }


        private int GetRoundNumberFromFileName(string fileName)
        {
            // Extract the round number from the file name
            var roundNumber = Path.GetFileNameWithoutExtension(fileName).Split('-')[0].Replace("round", string.Empty);
            return int.Parse(roundNumber);
        }

        private string GetFilePath(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(directory, "Files", fileName);
            Console.WriteLine(fullPath);
            return fullPath;
        }
    }
}
