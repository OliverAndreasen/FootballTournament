using FootballTournament.Models;

namespace FootballTournament.Utilities
{
    public class StandingsProcessor
    {
        public static List<TeamStandings> SortStandings(List<TeamStandings> standings)
        {
            if (standings == null){ 
                return new List<TeamStandings>();
            }
            return standings
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenByDescending(s => s.GoalsFor)
                .ThenBy(s => s.GoalsAgainst)
                .ThenBy(s => s.Team.FullName)
                .ToList();
        }

        public static void SetStandingsPositions(List<TeamStandings> sortedStandings)
        {
            var currentPosition = 1;
            TeamStandings previousStandings = null;

            foreach (var currentStandings in sortedStandings)
            {
                if (previousStandings != null &&
                    currentStandings.Points == previousStandings.Points &&
                    currentStandings.GoalDifference == previousStandings.GoalDifference &&
                    currentStandings.GoalsFor == previousStandings.GoalsFor &&
                    currentStandings.GoalsAgainst == previousStandings.GoalsAgainst)
                {
                    currentStandings.Position = previousStandings.Position;

                }
                else
                {
                    currentStandings.Position = currentPosition;
                }

                currentPosition++;
                previousStandings = currentStandings;
            }
        }
    }
}