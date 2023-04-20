namespace FootballTournament.Models
{
    public enum MatchResult
    {
        Win,
        Draw,
        Loss
    }

    public class TeamStandings
    {
        public TeamStandings(Team team)
        {
            Team = team;
            Position = 0;
            GamesPlayed = 0;
            Wins = 0;
            Draws = 0;
            Losses = 0;
            GoalsFor = 0;
            GoalsAgainst = 0;
            Points = 0;
            Streak = "-";
        }

        public Team Team { get; set; }
        public int Position { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public int Points { get; set; }
        public string Streak { get; set; }

        public void UpdateStreak(MatchResult result)
        {
            Streak = Streak.Length >= 5 ? Streak.Substring(1) : Streak;
            Streak += result switch
            {
                MatchResult.Win => "W",
                MatchResult.Draw => "D",
                MatchResult.Loss => "L",
                _ => "-"
            };
        }
    }
}