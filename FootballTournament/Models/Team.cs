namespace FootballTournament.Models;

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
    
    // Convert Team class to tuple
    public (string abbreviation, string fullName, string specialRanking) ToTuple()
    {
        return (Abbreviation, FullName, SpecialRanking);
    }
}