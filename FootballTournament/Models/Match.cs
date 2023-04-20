namespace FootballTournament.Models;

public class Match
{
    public Match(string homeTeam, string awayTeam, int? homeGoals, int? awayGoals)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
    }

    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
}