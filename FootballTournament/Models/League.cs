namespace FootballTournament.Models;

public class League
{
    public League(string name, int championsLeagueSpots, int europaLeagueSpots, int conferenceLeagueSpots,
        int upperLeaguePromotionSpots, int lowerLeagueRelegationSpots)
    {
        Name = name;
        ChampionsLeagueSpots = championsLeagueSpots;
        EuropaLeagueSpots = europaLeagueSpots;
        ConferenceLeagueSpots = conferenceLeagueSpots;
        UpperLeaguePromotionSpots = upperLeaguePromotionSpots;
        LowerLeagueRelegationSpots = lowerLeagueRelegationSpots;
    }

    private string Name { get; }
    private int ChampionsLeagueSpots { get; }
    private int EuropaLeagueSpots { get; }
    private int ConferenceLeagueSpots { get; }
    private int UpperLeaguePromotionSpots { get; }
    private int LowerLeagueRelegationSpots { get; }

    public override string ToString()
    {
        return
            $"{Name} - CL: {ChampionsLeagueSpots}, EL: {EuropaLeagueSpots}, Conference: {ConferenceLeagueSpots}, Upper: {UpperLeaguePromotionSpots}, Relegation: {LowerLeagueRelegationSpots}";
    }
}