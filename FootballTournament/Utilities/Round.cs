using System;
using System.Collections.Generic;
using System.Linq;
using FootballTournament.Models;

namespace FootballTournament.Utilities;

public class Round
{
    public Round(string roundFile, List<Team> teams)
    {
        RoundFile = roundFile;
        Teams = teams;
        var csvReader = new CsvReader<Match>();
        Matches = csvReader.ReadData(roundFile);
    }

    private string RoundFile { get; }
    private List<Match> Matches { get; }
    private List<Team> Teams { get; }

    public List<Match> GetMatchesInRound()
    {
        return GetValidMatchesForTeams(Matches);
    }

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


    private List<Match> GetValidMatchesInRound(List<Match> matches, List<Team> allowedOpponents)
    {
        var validMatches = new List<Match>();
        foreach (var match in matches)
        {
            if (match.HomeTeam != match.AwayTeam)
            {
                var homeTeam = Teams.FirstOrDefault(t => t.Abbreviation == match.HomeTeam);
                var awayTeam = Teams.FirstOrDefault(t => t.Abbreviation == match.AwayTeam);
                if (homeTeam == null || awayTeam == null)
                {
                    Console.WriteLine($"Match {match.HomeTeam} vs {match.AwayTeam} is not processed.");
                    continue;
                }

                if (allowedOpponents.Contains(homeTeam) && allowedOpponents.Contains(awayTeam))
                {
                    validMatches.Add(match);
                }
                else
                {
                    throw new Exception(
                        $"Match {match.HomeTeam} vs {match.AwayTeam} is not allowed in the final round.");
                }
            }
            else
            {
                throw new Exception($"{match.HomeTeam} is not allowed to play against itself.");
            }
        }

        return validMatches;
    }

    private List<Match> GetValidMatchesForTeams(List<Match> matches)
    {
        var validMatches = new List<Match>();
        var playedTeams = new List<string>();

        foreach (var match in matches)
        {
            // Check if the teams are the same
            if (match.HomeTeam != match.AwayTeam)
            {
                var homeTeam = Teams.FirstOrDefault(t => t.Abbreviation == match.HomeTeam);
                var awayTeam = Teams.FirstOrDefault(t => t.Abbreviation == match.AwayTeam);
                if (homeTeam == null || awayTeam == null)
                {
                    Console.WriteLine($"Match {match.HomeTeam} vs {match.AwayTeam} is not processed.");
                    continue;
                }

                // Check if both homeTeam and awayTeam are in the allowed teams list
                if (Teams.Contains(homeTeam) && Teams.Contains(awayTeam))
                {
                    // Check if the teams have already played in the round
                    if (playedTeams.Contains(match.HomeTeam) || playedTeams.Contains(match.AwayTeam))
                    {
                        throw new Exception($"Match {match.HomeTeam} vs {match.AwayTeam} is not allowed in the round.");
                    }

                    playedTeams.Add(match.HomeTeam);
                    playedTeams.Add(match.AwayTeam);
                    validMatches.Add(match);
                }
            }
            else
            {
                throw new Exception($"{match.HomeTeam} is not allowed to play against itself.");
            }
        }

        return validMatches;
    }
}