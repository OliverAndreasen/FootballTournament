using FootballTournament.Models;
using FootballTournament.Utilities;
// Add this using statement

// Add this using statement

namespace FootballTournament;

internal class Program
{
    public static void Main(string[] args)
    {
        var csvReaderLeague = new CsvReader<League>();
        var leagues = csvReaderLeague.ReadData("setup.csv");
        
        var csvReaderTeams = new CsvReader<Team>();
        var teams = csvReaderTeams.ReadData("teams.csv");

        //league to process, using the first one for example
        var league = leagues[0];

        // Set the path to the Rounds folder
        var roundsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Rounds");


        //Test cases for the program
        //Test1 -> Team plays twice in the same round
        //var roundsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Test", "TeamPlaysTwiceInSameRound");
        
        //Test2 -> Team plays twice as home team against the same team in the initial 22 rounds
        //var roundsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "Test", "TeamPlaysTwiceInitialRounds");
        
        //Test3 -> Unknown team in the round file its not processed
        //var roundsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "test", "UnknownTeamNotProcessed");
        
        //Test4 -> A team is not allowed to play against itself
        //var roundsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "test", "TeamPlayingAgainstItself");
        //-------------------------------------------------------------------------------->


        // Process the standings and display the results
        var standings = new Standings(teams, league, roundsFolderPath);
        standings.ProcessAndDisplayStandings();

        // Display league and team information
        Console.WriteLine(league.ToString());
        
        
    }
}