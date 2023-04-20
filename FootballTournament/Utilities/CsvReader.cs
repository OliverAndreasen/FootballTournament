using FootballTournament.Models;
using System.Collections.Generic;
using System.IO;

namespace FootballTournament.Utilities
{
    public class CsvReader<T>
    {
        private readonly TournamentFileManager _fileManager;

        public CsvReader()
        {
            _fileManager = new TournamentFileManager();
        }

        public List<T> ReadData(string fileName)
        {
            var filePath = GetFilePath(fileName);
            var data = new List<T>();

            using var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                int postponedMatchNumber = 1;

                var line = reader.ReadLine() ?? string.Empty;
                var values = line.Split(',');
                // if any of the values is POSTPONED, the match is not processed
                if (values[2] == "POSTPONED") 
                {
                    CsvWriter postponedWriter = new CsvWriter();
                    postponedWriter.WritePostponedMatches(values, postponedMatchNumber, fileName);
                    postponedMatchNumber++;
                    continue;
                }
                var item = _fileManager.CreateItem<T>(values);
                data.Add(item);
            }

            return data;
        }

        private string GetFilePath(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(directory, "Files", fileName);
            return fullPath;
        }
    }
}