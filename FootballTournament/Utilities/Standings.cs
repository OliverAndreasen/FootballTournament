using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FootballTournament.Models;
namespace FootballTournament.Utilities
{
    public class Standings
    {
        private List<Team> Teams { get; }
        private League League {get; }
        private string RoundsFolder { get; }
        
        private List<Match> _leagueMatches = new List<Match>();
        private List<TeamStandings> _teamStandings = new List<TeamStandings>();

        private List<Match> _lowerBracketMatches = new List<Match>();
        private List<TeamStandings> _lowerBracketStandings = new List<TeamStandings>();

        private List<Match> _upperBracketMatches = new List<Match>();
        private List<TeamStandings> _upperBracketStandings = new List<TeamStandings>();

        public Standings(List<Team> teams, League league, string roundsFolder)
        {
            Teams = teams;
            League = league;
            RoundsFolder = roundsFolder;
        }

        private void ProcessStandings()
        {
            string[] roundFiles = Directory.GetFiles(RoundsFolder, "round*.csv")
                .Where(file => !Path.GetFileNameWithoutExtension(file).Contains("-"))
                .ToArray();
            var numberOfRounds = roundFiles.Length;
            Console.WriteLine("Number of rounds: " + numberOfRounds + "");

            for (var i = 1; i <= numberOfRounds; i++)
            {
                var roundFileName = Path.Combine(RoundsFolder, $"round{i}.csv");

                if (i <= 22)
                {
                    ProcessInitialRound(roundFileName);
                }
                else
                {
                    if (i == 23)
                    {
                        SetUpperAndLowerBracketTeams();
                    }

                    ProcessFinalRound(roundFileName);
                }
            }
        }
        private void ProcessInitialRound(string roundFileName)
        {
            var round = new Round(roundFileName, Teams);
            var validMatches = round.GetMatchesInRound();

            foreach (var match in validMatches)
            {
                if (_leagueMatches.Any(m => m.HomeTeam == match.HomeTeam && m.AwayTeam == match.AwayTeam))
                {
                    throw new InvalidOperationException(
                        $"Duplicate match found: {match.HomeTeam} vs {match.AwayTeam}");
                }
                else
                {
                    _leagueMatches.Add(match);
                    UpdateStandings();
                }
            }
        }
        private void ProcessFinalRound(string roundFileName)
        {
            var round = new Round(roundFileName, Teams);

            var validMatchesUpperBracket = round.GetMatchesInFinalRound(
                _upperBracketStandings.Select(s => s.Team).ToList(), true);
            
            AddValidMatchesToBracket(validMatchesUpperBracket, _upperBracketMatches);

            var validMatchesLowerBracket = round.GetMatchesInFinalRound(
                _lowerBracketStandings.Select(s => s.Team).ToList());
            
            AddValidMatchesToBracket(validMatchesLowerBracket, _lowerBracketMatches);
        }
        private void AddValidMatchesToBracket(List<Match> validMatches, List<Match> bracketMatches)
        {
            foreach (var match in validMatches)
            {
                if (bracketMatches.Any(m => m.HomeTeam == match.HomeTeam && m.AwayTeam == match.AwayTeam))
                {
                    throw new InvalidOperationException($"Duplicate match found: {match.HomeTeam} vs {match.AwayTeam}");
                }
                bracketMatches.Add(match);
            }
        }
        private void UpdateGoalsForAndAgainst(TeamStandings homeTeamStandings, TeamStandings awayTeamStandings, Match match)
        {
            homeTeamStandings.GoalsFor += match.HomeGoals.GetValueOrDefault();
            homeTeamStandings.GoalsAgainst += match.AwayGoals.GetValueOrDefault();
            awayTeamStandings.GoalsFor += match.AwayGoals.GetValueOrDefault();
            awayTeamStandings.GoalsAgainst += match.HomeGoals.GetValueOrDefault();
        }
        private void UpdatePointsAndStreaks(TeamStandings homeTeamStandings, TeamStandings awayTeamStandings, Match match)
        {
            if (match.HomeGoals > match.AwayGoals)
            {
                homeTeamStandings.Wins++;
                homeTeamStandings.Points += 3;
                awayTeamStandings.Losses++;

                homeTeamStandings.UpdateStreak(MatchResult.Win);
                awayTeamStandings.UpdateStreak(MatchResult.Loss);
            }
            else if (match.HomeGoals < match.AwayGoals)
            {
                homeTeamStandings.Losses++;
                awayTeamStandings.Wins++;
                awayTeamStandings.Points += 3;

                homeTeamStandings.UpdateStreak(MatchResult.Loss);
                awayTeamStandings.UpdateStreak(MatchResult.Win);
            }
            else
            {
                homeTeamStandings.Draws++;
                homeTeamStandings.Points++;
                awayTeamStandings.Draws++;
                awayTeamStandings.Points++;

                homeTeamStandings.UpdateStreak(MatchResult.Draw);
                awayTeamStandings.UpdateStreak(MatchResult.Draw);
            }
        }
        private void UpdateLeagueStandings()
        {
            var teamStandings = Teams.Select(t => new TeamStandings(t)).ToList();

            foreach (var match in _leagueMatches)
            {
                var homeTeamStandings = teamStandings.Find(s => s.Team.Abbreviation == match.HomeTeam);
                var awayTeamStandings = teamStandings.Find(s => s.Team.Abbreviation == match.AwayTeam);

                if (homeTeamStandings == null || awayTeamStandings == null)
                {
                    throw new InvalidOperationException($"Team not found: {match.HomeTeam} vs {match.AwayTeam}");
                }

                homeTeamStandings.GamesPlayed++;
                awayTeamStandings.GamesPlayed++;

                UpdateGoalsForAndAgainst(homeTeamStandings, awayTeamStandings, match);

                UpdatePointsAndStreaks(homeTeamStandings, awayTeamStandings, match);
            }

            _teamStandings = teamStandings;
        }
        private void UpdateBracketStandings(List<Match> matches, List<TeamStandings> standings)
        {
            var teamStandings = standings.ToList();

            foreach (var match in matches)
            {
                var homeTeamStandings = teamStandings.Find(s => s.Team.Abbreviation == match.HomeTeam);
                var awayTeamStandings = teamStandings.Find(s => s.Team.Abbreviation == match.AwayTeam);
                
                if (homeTeamStandings == null || awayTeamStandings == null)
                {
                    throw new InvalidOperationException($"Team not found: {match.HomeTeam} vs {match.AwayTeam}");
                }

                homeTeamStandings.GamesPlayed++;
                awayTeamStandings.GamesPlayed++;

                UpdateGoalsForAndAgainst(homeTeamStandings, awayTeamStandings, match);

                UpdatePointsAndStreaks(homeTeamStandings, awayTeamStandings, match);
            }

            StandingsProcessor.SortStandings(teamStandings);
        }
        private void UpdateStandings()
        {
            UpdateLeagueStandings();
            
            if (_upperBracketStandings.Count > 0 && _lowerBracketStandings.Count > 0)
            {
                UpdateBracketStandings(_upperBracketMatches, _upperBracketStandings);
                UpdateBracketStandings(_lowerBracketMatches, _lowerBracketStandings);
            }
        }
        private void SetUpperAndLowerBracketTeams()
        {
            var sortedStandings = StandingsProcessor.SortStandings(_teamStandings);
            // Ranges are used to get the top 6 and bottom 6 teams 
            var upperBracketTeams = sortedStandings.Take(6).Select(s => s.Team).ToList();
            var lowerBracketTeams = sortedStandings.Skip(6).Select(s => s.Team).ToList();

            foreach (var t in upperBracketTeams)
            {
                _upperBracketStandings.Add(new TeamStandings(t));
            }

            foreach (var t in lowerBracketTeams)
            {
                _lowerBracketStandings.Add(new TeamStandings(t));
            }
        }
        
        public void ProcessAndDisplayStandings()
        {
            ProcessStandings();
            UpdateStandings();

            // Create a new instance of StandingsDisplay and pass the required data
            var standingsDisplay = new StandingsDisplay(Teams, _teamStandings, _upperBracketStandings, _lowerBracketStandings);
            standingsDisplay.DisplayStandings();
        }
    }
}