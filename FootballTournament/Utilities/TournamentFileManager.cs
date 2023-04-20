using FootballTournament.Models;

namespace FootballTournament.Utilities
{
    public class TournamentFileManager
    {
        public T CreateItem<T>(string[] values)
        {
            if (typeof(T) == typeof(League))
            {
                var leagueName = values[0];
                //Skips the first value, then converts the rest to integers array
                var settings = values.Skip(1).Select(int.Parse).ToArray();
                return (T)(object)new League(leagueName,
                    settings[0],
                    settings[1],
                    settings[2],
                    settings[3],
                    settings[4]);
            }
            else if (typeof(T) == typeof(Team))
            {
                var abbreviation = values[0];
                var fullName = values[1];
                var specialRanking = values[2];
                Team team = new Team(abbreviation, fullName, specialRanking);
                team.Validate();
                return (T)(object)team;
            }
            else if (typeof(T) == typeof(Match))
            {
                var homeTeam = values[0];
                var awayTeam = values[1];

                //Splits the score into two values, then converts them to integers
                var score = values[2].Split('-').Select(int.Parse).ToArray();
                var homeGoals = score[0];
                var awayGoals = score[1];
                return (T)(object)new Match(homeTeam, awayTeam, homeGoals, awayGoals);
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }
        }
    }
}
