using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TourneyKeeper.Common;
using System.Configuration;
using TourneyKeeper.Common.Managers;
using System.Diagnostics;
using TourneyKeeper.Common.Managers.TableGenerators;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class MassTest
    {
        private string usernameTemplate = "MassTestUser";
        private string teamTemplate = "MassFoWTestTeam";
        private int numberOf40KTeams = 50;
        private int numberOfFoWTeams = 30;
        private int numberOf40KPlayersOnTeam = 8;
        private int numberOfFoWPlayersOnTeam = 5;
        private int gamesystemId = 17;
        private int tournamentId = 257;

        [TestMethod]
        public void CreateFoWTestUsersAndTeams()
        {
            using (var context = new TourneyKeeperEntities())
            {
                for (int i = 0; i < numberOfFoWTeams; i++)
                {
                    TKTournamentTeam team = new TKTournamentTeam
                    {
                        Name = teamTemplate + i.ToString(),
                        TournamentId = tournamentId
                    };

                    context.TKTournamentTeam.Add(team);
                }
                context.SaveChanges();

                var players = context.TKPlayer.Where(p => p.Name.StartsWith(usernameTemplate)).ToList();
                var teams = context.TKTournamentTeam.Where(t => t.Name.StartsWith(teamTemplate)).ToList();
                var codexes = context.TKCodex.Where(t => t.GameSystemId == gamesystemId).ToList();
                var random = new Random();

                for (int j = 0; j < numberOfFoWTeams * numberOfFoWPlayersOnTeam; j++)
                {
                    var tournamentPlayer = new TKTournamentPlayer
                    {
                        PlayerId = players[j].Id,
                        PlayerName = players[j].Name,
                        PrimaryCodexId = codexes[random.Next(codexes.Count)].Id,
                        TournamentTeamId = teams[j % numberOfFoWTeams].Id,
                        TournamentId = tournamentId
                    };

                    context.TKTournamentPlayer.Add(tournamentPlayer);
                    context.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void Create40KTestUsersAndTeams()
        {
            using (var context = new TourneyKeeperEntities())
            {
                for (int i = 0; i < numberOf40KTeams * numberOf40KPlayersOnTeam; i++)
                {
                    TKPlayer player = new TKPlayer
                    {
                        Country = "Denmark",
                        Email = "x@x.x",
                        Name = usernameTemplate + i.ToString(),
                        Username = usernameTemplate + i.ToString()
                    };

                    context.TKPlayer.Add(player);
                }
                context.SaveChanges();

                for (int i = 0; i < numberOf40KTeams; i++)
                {
                    var team = new TKTournamentTeam
                    {
                        Name = teamTemplate + i.ToString(),
                        TournamentId = tournamentId
                    };

                    context.TKTournamentTeam.Add(team);
                }
                context.SaveChanges();

                var players = context.TKPlayer.Where(p => p.Name.StartsWith(usernameTemplate)).ToList();
                var teams = context.TKTournamentTeam.Where(t => t.Name.StartsWith(teamTemplate)).ToList();

                for (int j = 0; j < numberOf40KTeams * numberOf40KPlayersOnTeam; j++)
                {
                    var tournamentPlayer = new TKTournamentPlayer
                    {
                        PlayerId = players[j].Id,
                        PlayerName = players[j].Name,
                        PrimaryCodexId = 1,
                        TournamentTeamId = teams[j % numberOf40KTeams].Id,
                        TournamentId = tournamentId
                    };

                    context.TKTournamentPlayer.Add(tournamentPlayer);
                    context.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void Setup40KPairings()
        {
            using (var context = new TourneyKeeperEntities())
            {
                GameManager manager = new GameManager();
                Random r = new Random();
                int? round = context.TKGame.Where(g => g.TournamentId == tournamentId).Select(g => (int?)g.Round).Max();
                var teammatches = context.TKTeamMatch.Where(t => t.TournamentId == tournamentId && t.Round == round.Value).ToList();

                for (int i = 0; i < teammatches.Count; i++)
                {
                    var team1 = context.TKTournamentTeam.Single(t1 => t1.Id == teammatches[i].Team1Id);
                    var team2 = context.TKTournamentTeam.Single(t2 => t2.Id == teammatches[i].Team2Id);
                    var team1Players = context.TKTournamentPlayer.Where(p => !p.NonPlayer && p.TournamentTeamId == team1.Id).ToList();
                    var team2Players = context.TKTournamentPlayer.Where(p => !p.NonPlayer && p.TournamentTeamId == team2.Id).ToList();
                    var games = context.TKGame.Where(g => g.TeamMatchId == teammatches[i].Id).ToList();

                    for (int j = 0; j < team1Players.Count; j++)
                    {
                        int score = r.Next(10);
                        games[j].Player1Id = team1Players[j].Id;
                        games[j].Player2Id = team2Players[j].Id;
                        games[j].Player1Result = 10 + score;
                        games[j].Player2Result = 10 - score;

                        TKGame g = games[j];

                        manager.Update(g, "null");
                    }

                    Debug.WriteLine("Teammatch: " + i.ToString());
                }
            }
        }

        [TestMethod]
        public void SetupFoWPairings()
        {
            using (var context = new TourneyKeeperEntities())
            {
                var pairingManager = new PairingManager(TableGenerator.Linear);
                var manager = new GameManager();
                var r = new Random();

                for (int k = 0; k < 6; k++)
                {
                    pairingManager.TeamDraw(tournamentId, DrawTypeEnum.SwissDraw, "328d7bd7-21be-467c-91f3-c6c164584027");

                    int? round = context.TKGame.Where(g => g.TournamentId == tournamentId).Select(g => (int?)g.Round).Max();
                    var teammatches = context.TKTeamMatch.Where(t => t.TournamentId == tournamentId && t.Round == round.Value).ToList();

                    for (int i = 0; i < teammatches.Count; i++)
                    {
                        var team1 = context.TKTournamentTeam.Single(t1 => t1.Id == teammatches[i].Team1Id);
                        var team2 = context.TKTournamentTeam.Single(t2 => t2.Id == teammatches[i].Team2Id);
                        var team1Players = context.TKTournamentPlayer.Where(p => p.TournamentTeamId == team1.Id).ToList();
                        var team2Players = context.TKTournamentPlayer.Where(p => p.TournamentTeamId == team2.Id).ToList();
                        var games = context.TKGame.Where(g => g.TeamMatchId == teammatches[i].Id).ToList();

                        for (int j = 0; j < team1Players.Count; j++)
                        {
                            int p1sec = r.Next(7);
                            int p2sec = r.Next(7);
                            games[j].Player1Id = team1Players[j].Id;
                            games[j].Player2Id = team2Players[j].Id;
                            games[j].Player1Result = p1sec == p2sec ? 0 : p1sec > p2sec ? 1 : 0;
                            games[j].Player2Result = p1sec == p2sec ? 0 : p1sec > p2sec ? 0 : 1;
                            games[j].Player1SecondaryResult = p1sec;
                            games[j].Player2SecondaryResult = p2sec;

                            TKGame g = games[j];

                            manager.Update(g, "null");
                        }

                        Debug.WriteLine("Teammatch: " + i.ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void DeleteUsers()
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tournamentplayers = context.TKTournamentPlayer.Where(p => p.PlayerName.StartsWith(usernameTemplate) && p.TournamentId == tournamentId).ToList();
                //var players = context.TKPlayer.Where(p => p.Name.StartsWith(usernameTemplate) && p.TournamentId == tournamentId).ToList();
                var teams = context.TKTournamentTeam.Where(t => t.Name.StartsWith(teamTemplate) && t.TournamentId == tournamentId).ToList();
                var games = context.TKGame.Where(t => t.TournamentId == tournamentId).ToList();
                var teamMatches = context.TKTeamMatch.Where(t => t.TournamentId == tournamentId).ToList();

                context.TKGame.RemoveRange(games);
                context.TKTeamMatch.RemoveRange(teamMatches);
                context.TKTournamentPlayer.RemoveRange(tournamentplayers);
                //context.TKPlayer.RemoveRange(players);
                context.TKTournamentTeam.RemoveRange(teams);

                //context.SaveChanges();
            }
        }
    }
}
