**Null handling**

The following code shows how to use of null handling in the class Round
Checks if a team has already played in the round

    if (playedTeams.Contains(match.HomeTeam) || playedTeams.Contains(match.AwayTeam))
    {
    throw new Exception($"Match {match.HomeTeam} vs {match.AwayTeam} is not allowed in the round.");
    }

**String interpolation**

    public override string ToString()
    {
        return
            $"{Name} - CL: {ChampionsLeagueSpots}, EL: {EuropaLeagueSpots}, Conference: {ConferenceLeagueSpots}, Upper: {UpperLeaguePromotionSpots}, Relegation: {LowerLeagueRelegationSpots}";
    }


**Pattern Matching**
Here is an example of how to use pattern matching checking if a team is in the allowed opponents list

    if (allowedOpponents.Contains(homeTeam) && allowedOpponents.Contains(awayTeam))
    {
    validMatches.Add(match);
    }


**Classes, structs and enums**

There is a lot of classes in this projcet, but here is an example of a class Team

    public class Team
    {
    public Team(string abbreviation, string fullName, string specialRanking)
    {
    Abbreviation = abbreviation;
    FullName = fullName;
    SpecialRanking = specialRanking;
    }
    
        public string Abbreviation { get; set; }
        public string FullName { get; set; }
        public string SpecialRanking { get; set; }
        
        public void Validate()
        {
            if (string.IsNullOrEmpty(Abbreviation) || Abbreviation.Length >= 4 || !Abbreviation.Equals(Abbreviation.ToUpper()))
            {
                throw new ArgumentException("Abbreviation must be less than 4 characters and in uppercase.");
            }
    
            if (string.IsNullOrEmpty(FullName) || FullName.Length > 30)
            {
                throw new ArgumentException("FullName must be less than 30 characters.");
            }
    
            if (string.IsNullOrEmpty(SpecialRanking) || SpecialRanking.Length != 1)
            {
                throw new ArgumentException("SpecialRanking must be exactly 1 character.");
            }
        }
    }

Here is a struct that is used to display the TeamStandings, might not be the most optimal but since the values are readonly it can be used as an struct

    TeamStandings.cs



Enum example to determine if a team won,lost or drew a match

    public enum MatchResult
    {
        Win,
        Draw,
        Loss
    }


**Named & optional parameters**

Here is an example of the use of named parameters and an optional parameter that checks if its upperBracket league in the class Round

    public List<Match> GetMatchesInFinalRound(List<Team> allowedOpponents, bool isUpperBracket = false)
    {
    var validMatches = new List<Match>();

        if (isUpperBracket)
        {
            validMatches = GetValidMatchesInRound(Matches.Take(3).ToList(), allowedOpponents);
        }
        else if (!isUpperBracket)
        {
            validMatches = GetValidMatchesInRound(Matches.Skip(3).ToList(), allowedOpponents);
        }

        return validMatches;
    }


**Exception, Attributes and DataValidation**


The following code shows how to use of datavalidation in the class Team

        public void Validate(){
        if (string.IsNullOrEmpty(Abbreviation) || Abbreviation.Length >= 4 || !Abbreviation.Equals(Abbreviation.ToUpper()))
        {
        throw new ArgumentException("Abbreviation must be less than 4 characters and in uppercase.");
        }
        if (string.IsNullOrEmpty(FullName) || FullName.Length > 30)
        {
        throw new ArgumentException("FullName must be less than 30 characters.");
        }
        
        if (string.IsNullOrEmpty(SpecialRanking) || SpecialRanking.Length != 1)
            {
                    throw new ArgumentException("SpecialRanking must be exactly 1 character.");
            }
        }

**Arrays / Collections**

In the CvsReader class i convert the csv file to a array of strings

    var values = line.Split(',');

There is a lot of collections in this project, but here is an example of a list of matches.
if the round file is named something over 22 rounds it will split the matches into 2 lists, upper and lower bracket

    public List<Match> GetMatchesInFinalRound(List<Team> allowedOpponents, bool isUpperBracket = false)
    {
        var validMatches = new List<Match>();

        if (isUpperBracket)
        {
            validMatches = GetValidMatchesInRound(Matches.Take(3).ToList(), allowedOpponents);
        }
        else if (!isUpperBracket)
        {
            validMatches = GetValidMatchesInRound(Matches.Skip(3).ToList(), allowedOpponents);
        }

        return validMatches;
    }


**Ranges**
Here is an example of how to use ranges in the class Standings used to get the top 6 and bottom 6 teams


`    
    var upperBracketTeams = sortedStandings.Take(6).Select(s => s.Team).ToList();
`

`    
var lowerBracketTeams = sortedStandings.Skip(6).Select(s => s.Team).ToList();
`

**Generics**

The cvs reader class is generic and can be used to read the data of Team,League And Match
The TournamentFileManager has a method that creates the object of the type that is passed to the method

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
                var line = reader.ReadLine() ?? string.Empty;
                var values = line.Split(',');
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

