using FootballTournament.Models;

namespace FootballTournament.Utilities
{

    public struct StandingsDisplay
    {
        private readonly List<TeamStandings> _lowerBracketStandings;
        private readonly List<TeamStandings> _teamStandings;
        private readonly List<TeamStandings> _upperBracketStandings;
        private readonly List<Team> _teams;

        public StandingsDisplay(List<Team> teams, List<TeamStandings> teamStandings,
            List<TeamStandings> upperBracketStandings, List<TeamStandings> lowerBracketStandings)
        {
            _teams = teams;
            _teamStandings = teamStandings;
            _upperBracketStandings = upperBracketStandings;
            _lowerBracketStandings = lowerBracketStandings;
        }

        public void DisplayStandings()
        {
            Console.WriteLine("League Standings:");
            DisplayLeagueStandings();
            DisplayFinalRoundsStandings();
        }


        public void DisplayLeagueStandings()
        {
            if (_teamStandings.Count == 0)
            {
                DisplayTeamsInAlphabeticalOrder();
            }
            else
            {
                var sortedStandings = StandingsProcessor.SortStandings(_teamStandings);
                StandingsProcessor.SetStandingsPositions(sortedStandings);
                DisplayStandingsTable(sortedStandings);
            }
        }


        public void DisplayFinalRoundsStandings()
        {
            if (_upperBracketStandings.Count == 0)
            {
                Console.WriteLine("Final Rounds Not Yet Started");
                DisplayTeamsInAlphabeticalOrder();
            }
            else
            {
                var sortedStandingsUpper = StandingsProcessor.SortStandings(_upperBracketStandings);
                StandingsProcessor.SetStandingsPositions(sortedStandingsUpper);
                Console.WriteLine("\nUpper Bracket Standings:");
                DisplayStandingsTable(sortedStandingsUpper);
                
                var sortedStandingsLower = StandingsProcessor.SortStandings(_lowerBracketStandings);
                StandingsProcessor.SetStandingsPositions(sortedStandingsLower);
                Console.WriteLine("\nLower Bracket Standings:");
                DisplayStandingsTable(sortedStandingsLower);
            }
        }


        private void DisplayTeamsInAlphabeticalOrder()
        {
            var sortedTeams = _teams.OrderBy(t => t.FullName).ToList();

            Console.WriteLine("Pos\tTeam");
            Console.WriteLine("------------------");

            for (var i = 0; i < sortedTeams.Count; i++)
            {
                Console.WriteLine($"{i + 1}\t{sortedTeams[i].FullName}");
            }
        }

        private void DisplayStandingsTable(List<TeamStandings> sortedStandings)
        {
            Console.WriteLine("Pos\tSR\tTeam\t\t\tM\tW\tD\tL\tGF\tGA\tGD\tP\tStreak");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

            foreach (var standings in sortedStandings)
            {
                // CL, EL, EC qualification or promotion qualification
                if (standings.Position <= 3) 
                    Console.ForegroundColor = ConsoleColor.Green;
                // Relegation threat
                else if (standings.Position >= sortedStandings.Count - 2) 
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;

                var position = standings.Position == sortedStandings[standings.Position - 1] // Check for ties
                    .Position
                    ? standings.Position.ToString()
                    : "-";
                // Calculate the number of spaces needed to align the team names
                var maxLength = sortedStandings.Max(s => s.Team.FullName.Length);
                // If the team name is too long, the table will be misaligned so we need to add invisible spaces
                var invisibleRow = new string(' ', Math.Max(maxLength - 23, 0));
                var specialRanking = standings.Team.SpecialRanking == "0" ? "" : "(" + standings.Team.SpecialRanking + ")";
                var teamNameFormat = "{0,-" + (23 + invisibleRow.Length) + "}";

                Console.WriteLine(
                    $"{position}\t" +
                    $"{specialRanking}\t" +
                    $"{string.Format(teamNameFormat, standings.Team.FullName)}" +
                    $"{invisibleRow}\t" +
                    $"{standings.GamesPlayed}\t" +
                    $"{standings.Wins}\t" +
                    $"{standings.Draws}\t" +
                    $"{standings.Losses}\t" +
                    $"{standings.GoalsFor}\t" +
                    $"{standings.GoalsAgainst}\t" +
                    $"{standings.GoalDifference}\t" +
                    $"{standings.Points}\t" +
                    $"{standings.Streak}");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}