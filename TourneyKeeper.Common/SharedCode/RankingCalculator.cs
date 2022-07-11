using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Common.SharedCode
{
    public class RankingCalculator
    {
        public void DoRankings()
        {
            try
            {
                LogManager.LogEvent($"Ranking Starting.");

                DateTime startDateTime = DateTime.Now;
                DateTime dateTime = DateTime.Now;
                Debug.WriteLine($"Sletter gammel data - {DateTime.Now}");

                using (TourneyKeeperEntities context0 = new TourneyKeeperEntities())
                {
                    context0.Database.ExecuteSqlCommand("DELETE FROM tkranking");
                    context0.Database.ExecuteSqlCommand("UPDATE tkgame SET Ranked = null, Player1ELO = null, Player2ELO = null");
                    context0.Database.ExecuteSqlCommand("UPDATE tkplayer SET wins = 0, losses = 0, draws = 0, totalwins = 0, totallosses = 0, totaldraws = 0");
                }

                Debug.WriteLine($"Sætter ikke ranked - {DateTime.Now.Subtract(dateTime)}");
                dateTime = DateTime.Now;

                using (TourneyKeeperEntities context1 = new TourneyKeeperEntities())
                {
                    context1.Database.ExecuteSqlCommand(@"update tkgame set ranked = 'Not ranked' from tkgame g join tktournament t on t.Id = g.TournamentId where ranked is null and TeamMatchId is not null and t.Active = 1");
                }

                Debug.WriteLine($"Indsætter ranks - {DateTime.Now.Subtract(dateTime)}");
                dateTime = DateTime.Now;
                var rankings = new List<TKRanking>();

                LogManager.LogEvent($"Ranking - Insert");

                using (TourneyKeeperEntities context2 = new TourneyKeeperEntities())
                {
                    context2.Configuration.AutoDetectChangesEnabled = false;
                    context2.Configuration.ValidateOnSaveEnabled = false;
                    Dictionary<int, Dictionary<int, bool>> inserted = new Dictionary<int, Dictionary<int, bool>>();

                    foreach (TKRankingType rankingType in context2.TKRankingType.ToList())
                    {
                        foreach (TKGame game in context2.TKGame
                            .Include(g => g.TKTournament)
                            .Include(g => g.TKTournamentPlayer)
                            .Include(g => g.TKTournamentPlayer.TKPlayer)
                            .Include(g => g.TKTournamentPlayer1)
                            .Include(g => g.TKTournamentPlayer1.TKPlayer)
                            .Where(g =>
                            g.Ranked == null &&
                            g.TeamMatchId == null &&
                            g.TKTournament.GameSystemId == rankingType.GameSystemId &&
                            g.TKTournamentPlayer.TKPlayer.Country.Equals(rankingType.Name) &&
                            g.TKTournamentPlayer1.TKPlayer.Country.Equals(rankingType.Name) &&
                            g.TKTournament.Active &&
                            g.TKTournament.TournamentEndDate < DateTime.Now &&
                            g.TKTournament.TournamentDate > rankingType.RunFrom))
                        {
                            if (game.TKTournamentPlayer.DoNotRank || game.TKTournamentPlayer1.DoNotRank)
                            {
                                game.Ranked = "Not ranked";
                            }
                            else
                            {
                                if (!inserted.ContainsKey(game.TKTournamentPlayer.TKPlayer.Id))
                                {
                                    inserted.Add(game.TKTournamentPlayer.TKPlayer.Id, new Dictionary<int, bool>());
                                }
                                if (!inserted.ContainsKey(game.TKTournamentPlayer1.TKPlayer.Id))
                                {
                                    inserted.Add(game.TKTournamentPlayer1.TKPlayer.Id, new Dictionary<int, bool>());
                                }
                                if (!inserted[game.TKTournamentPlayer.TKPlayer.Id].ContainsKey(rankingType.Id))
                                {
                                    rankings.Add(new TKRanking
                                    {
                                        LastUpdated = DateTime.Now,
                                        PlayerId = game.TKTournamentPlayer.TKPlayer.Id,
                                        PlayerName = game.TKTournamentPlayer.TKPlayer.Name,
                                        Points = 1400,
                                        DateAdded = DateTime.Now,
                                        RankingTypeId = rankingType.Id
                                    });
                                    inserted[game.TKTournamentPlayer.TKPlayer.Id].Add(rankingType.Id, true);
                                }
                                if (!inserted[game.TKTournamentPlayer1.TKPlayer.Id].ContainsKey(rankingType.Id))
                                {
                                    rankings.Add(new TKRanking
                                    {
                                        LastUpdated = DateTime.Now,
                                        PlayerId = game.TKTournamentPlayer1.TKPlayer.Id,
                                        PlayerName = game.TKTournamentPlayer1.TKPlayer.Name,
                                        Points = 1400,
                                        DateAdded = DateTime.Now,
                                        RankingTypeId = rankingType.Id
                                    });
                                    inserted[game.TKTournamentPlayer1.TKPlayer.Id].Add(rankingType.Id, true);
                                }
                            }
                        }
                    }
                }

                using (TourneyKeeperEntities context = new TourneyKeeperEntities())
                {
                    context.BulkInsert(rankings, options => options.AutoMapOutputDirection = false);
                }

                Debug.WriteLine($"Regner point for spil - {DateTime.Now.Subtract(dateTime)}");
                dateTime = DateTime.Now;

                var ranked = new Dictionary<int, TKGame>();
                var rankingsPoint = new List<TKRanking>();
                LogManager.LogEvent($"Ranking - Point");

                using (TourneyKeeperEntities context4 = new TourneyKeeperEntities())
                {
                    var rankingsPointsList = context4.TKRanking.ToList();

                    foreach (TKRankingType rankingType in context4.TKRankingType.ToList())
                    {
                        var list = context4.TKGame
                            .Include(g => g.TKTournament)
                            .Include(g => g.TKTournamentPlayer)
                            .Include(g => g.TKTournamentPlayer.TKPlayer)
                            .Include(g => g.TKTournamentPlayer1)
                            .Include(g => g.TKTournamentPlayer1.TKPlayer)
                            .Where(g =>
                           g.Ranked == null &&
                           g.TeamMatchId == null &&
                           g.TKTournament.TournamentDate > rankingType.RunFrom &&
                           g.TKTournament.GameSystemId == rankingType.GameSystemId &&
                           (g.TKTournamentPlayer.TKPlayer.Country.Equals(rankingType.Name) && g.TKTournamentPlayer1.TKPlayer.Country.Equals(rankingType.Name)) &&
                           !(g.TKTournamentPlayer.DoNotRank || g.TKTournamentPlayer1.DoNotRank) &&
                           g.TKTournament.Active &&
                           g.TKTournament.TournamentEndDate < DateTime.Now
                           ).OrderBy(g => g.Id);
                        foreach (TKGame game in list)
                        {
                            var p1Rank = rankingsPointsList.Single(r => r.PlayerId.Equals(game.TKTournamentPlayer.TKPlayer.Id) && r.TKRankingType.Id == rankingType.Id && r.TKRankingType.GameSystemId == game.TKTournamentPlayer.TKTournament.GameSystemId);
                            var p2Rank = rankingsPointsList.Single(r => r.PlayerId.Equals(game.TKTournamentPlayer1.TKPlayer.Id) && r.TKRankingType.Id == rankingType.Id && r.TKRankingType.GameSystemId == game.TKTournamentPlayer1.TKTournament.GameSystemId);

                            DoPoints(ranked, game, p1Rank, p2Rank);
                        }
                    }

                    rankingsPoint = rankingsPointsList.ToList();
                }

                using (TourneyKeeperEntities context = new TourneyKeeperEntities())
                {
                    context.BulkUpdate(rankingsPoint);
                }

                Debug.WriteLine($"Sætter ranked - {DateTime.Now.Subtract(dateTime)}");
                dateTime = DateTime.Now;
                LogManager.LogEvent($"Ranking - Ranked");

                using (TourneyKeeperEntities context = new TourneyKeeperEntities())
                {
                    var games = ranked.Values.ToList();
                    games.ForEach(g => g.Ranked = "Ranked");

                    context.BulkUpdate(games);
                }

                Debug.WriteLine($"Sætter rank - {DateTime.Now.Subtract(dateTime)}");
                dateTime = DateTime.Now;
                var rankingsRank = new List<TKRanking>();
                LogManager.LogEvent($"Ranking - Ranking");

                using (TourneyKeeperEntities context6 = new TourneyKeeperEntities())
                {
                    var tempRankings = new List<TKRanking>();

                    foreach (TKRankingType type in context6.TKRankingType)
                    {
                        int rank = 1;
                        foreach (var ranking in context6.TKRanking.Where(r => r.RankingTypeId == type.Id).OrderByDescending(r => r.Points))
                        {
                            ranking.Rank = rank++;
                            tempRankings.Add(ranking);
                        }
                    }

                    rankingsRank = tempRankings.ToList();
                }

                using (TourneyKeeperEntities context = new TourneyKeeperEntities())
                {
                    context.BulkUpdate(rankingsRank);
                }

                Debug.WriteLine($"Done - {DateTime.Now.Subtract(startDateTime)}");
                LogManager.LogEvent($"Ranking finished. It took {DateTime.Now.Subtract(startDateTime)}");
            }
            catch (Exception e)
            {
                LogManager.LogError($"Ranking Error. {e.Message} {e.StackTrace}");
            }
        }

        public void WLD2()
        {
            DateTime dateTime = DateTime.Now;
            List<TKPlayer> playersUpdate;
            Debug.WriteLine($"Opdater WLD 2 - {DateTime.Now.Subtract(dateTime)}");
            dateTime = DateTime.Now;
            playersUpdate = new List<TKPlayer>();
            LogManager.LogEvent($"Ranking - WLD 2");

            using (TourneyKeeperEntities context7 = new TourneyKeeperEntities())
            {
                var players = context7.TKPlayer.ToList();

                foreach (var ra in context7.TKRankingType.GroupBy(t => t.GameSystemId).Select(t => t.FirstOrDefault()))
                {
                    foreach (var game in context7.TKGame
                        .Include(g => g.TKTournament)
                        .Include(g => g.TKTournamentPlayer)
                        .Include(g => g.TKTournamentPlayer.TKPlayer)
                        .Include(g => g.TKTournamentPlayer1)
                        .Include(g => g.TKTournamentPlayer1.TKPlayer)
                        .Where(g =>
                        g.Ranked == null &&
                        g.TeamMatchId == null &&
                        g.TKTournament.GameSystemId == ra.GameSystemId &&
                        g.TKTournament.Active &&
                        g.TKTournament.TournamentEndDate < DateTime.Now).OrderBy(g => g.Id))
                    {
                        var player1 = players.SingleOrDefault(p => p.Id == game.TKTournamentPlayer.TKPlayer.Id);
                        var player2 = players.SingleOrDefault(p => p.Id == game.TKTournamentPlayer1.TKPlayer.Id);
                        if (player1 != null && player2 != null)
                        {
                            if (game.Player1Result > game.Player2Result)
                            {
                                player1.TotalWins++;
                                player2.TotalLosses++;
                            }
                            else if (game.Player2Result > game.Player1Result)
                            {
                                player1.TotalLosses++;
                                player2.TotalWins++;
                            }
                            else
                            {
                                player1.TotalDraws++;
                                player2.TotalDraws++;
                            }
                        }
                    }
                }

                playersUpdate = players.ToList();
            }

            using (TourneyKeeperEntities context = new TourneyKeeperEntities())
            {
                context.BulkUpdate(playersUpdate);
            }
        }

        public void WLD1()
        {
            DateTime dateTime = DateTime.Now;
            Debug.WriteLine($"Opdater WLD 1 - {DateTime.Now.Subtract(dateTime)}");
            dateTime = DateTime.Now;
            var playersUpdate = new List<TKPlayer>();
            LogManager.LogEvent($"Ranking - WLD 1");

            using (TourneyKeeperEntities context3 = new TourneyKeeperEntities())
            {
                var players = context3.TKPlayer.ToList();

                foreach (var ra in context3.TKRankingType.GroupBy(t => t.GameSystemId).Select(t => t.FirstOrDefault()))
                {
                    foreach (var game in context3.TKGame
                        .Include(g => g.TKTournament)
                        .Include(g => g.TKTournamentPlayer)
                        .Include(g => g.TKTournamentPlayer.TKPlayer)
                        .Include(g => g.TKTournamentPlayer1)
                        .Include(g => g.TKTournamentPlayer1.TKPlayer)
                        .Where(g =>
                        g.Ranked == null &&
                        g.TeamMatchId == null &&
                        g.TKTournament.GameSystemId == ra.GameSystemId &&
                        g.TKTournament.TournamentDate > ra.RunFrom &&
                        g.TKTournament.Active &&
                        g.Player1ELO != 0 &&
                        g.TKTournament.TournamentEndDate < DateTime.Now).OrderBy(g => g.Id))
                    {
                        var player1 = players.SingleOrDefault(p => p.Id == game.TKTournamentPlayer.TKPlayer.Id);
                        var player2 = players.SingleOrDefault(p => p.Id == game.TKTournamentPlayer1.TKPlayer.Id);
                        if (player1 != null && player2 != null)
                        {
                            if (game.Player1Result > game.Player2Result)
                            {
                                player1.Wins++;
                                player2.Losses++;
                            }
                            else if (game.Player2Result > game.Player1Result)
                            {
                                player1.Losses++;
                                player2.Wins++;
                            }
                            else
                            {
                                player1.Draws++;
                                player2.Draws++;
                            }
                        }
                    }
                }

                playersUpdate = players.ToList();
            }

            using (TourneyKeeperEntities context = new TourneyKeeperEntities())
            {
                context.BulkUpdate(playersUpdate);
            }
        }

        private static void DoPoints(Dictionary<int, TKGame> ranked, TKGame game, TKRanking p1Rank, TKRanking p2Rank)
        {
            float eA = 1.0f / (1.0f + (float)Math.Pow(10.0, ((p2Rank.Points - p1Rank.Points) / 400.0)));

            float rate = game.Player1Result + game.Player2Result;
            if (rate == 0)
            {
                game.Ranked = "Not ranked";
            }
            else
            {
                float point1;
                float point2;
                if (game.Player1Result == game.Player2Result)
                {
                    if (p1Rank.Points == p2Rank.Points)
                    {
                        point1 = 0;
                        point2 = 0;
                    }
                    else
                    {
                        point1 = (1.0f - eA) * 8.0f;
                        if (p1Rank.Points > p2Rank.Points)
                        {
                            point1 *= -1;
                        }
                        point2 = point1 * -1;
                    }
                }
                else
                {
                    point1 = 32.0f *
                        ((game.Player1Result > game.Player2Result ? 1.0f : 0) - eA);
                    point2 = point1 * -1.0f;
                }

                if (!ranked.ContainsKey(game.Id))
                {
                    ranked.Add(game.Id, game);
                }

                game.Player1ELO = point1;
                game.Player2ELO = point2;

                p1Rank.Points += point1;
                p1Rank.LastUpdated = DateTime.Now;
                p1Rank.LatestGame = !p1Rank.LatestGame.HasValue || p1Rank.LatestGame.Value < game.TKTournament.TournamentDate ? game.TKTournament.TournamentDate : p1Rank.LatestGame;

                p2Rank.Points += point2;
                p2Rank.LastUpdated = DateTime.Now;
                p2Rank.LatestGame = !p2Rank.LatestGame.HasValue || p2Rank.LatestGame.Value < game.TKTournament.TournamentDate ? game.TKTournament.TournamentDate : p2Rank.LatestGame;
            }
        }
    }
}
