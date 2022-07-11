using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using TourneyKeeper.Common.Exceptions;
using TourneyKeeper.Common.SharedCode;
using System.Diagnostics;
using TourneyKeeper.DTO.App;

namespace TourneyKeeper.Common.Managers
{
    public class GameManager : IManager<TKGame>
    {
        public int? GetCurrentRoundNumber(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                return context.TKGame.Where(g => g.TKTournament.Id == tournamentId).Max(g => (int?)g.Round);
            }
        }

        public void SelectOpponent(int gameId, int tournamentPlayerId, int opponentId, int table, bool playerTeam1)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var game = context.TKGame.SingleOrDefault(g => g.Id == gameId);
                game.Player1Id = playerTeam1 ? tournamentPlayerId : opponentId;
                game.Player2Id = playerTeam1 ? opponentId : tournamentPlayerId;
                game.TableNumber = table;

                context.SaveChanges();
            }
        }

        public List<TKGame> GetGames(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var games = context.TKGame
                    .Include(g => g.TKTournamentPlayer)
                    .Include(g => g.TKTournamentPlayer1)
                    .Include(g => g.TKTournamentPlayer.TKCodex)
                    .Include(g => g.TKTournamentPlayer.TKCodex1)
                    .Include(g => g.TKTournamentPlayer.TKCodex2)
                    .Include(g => g.TKTournamentPlayer1.TKCodex)
                    .Include(g => g.TKTournamentPlayer1.TKCodex1)
                    .Include(g => g.TKTournamentPlayer1.TKCodex2)
                    .Include(g => g.TKTeamMatch)
                    .Where(g => g.TournamentId == tournamentId)
                    .ToList();

                return games;
            }
        }

        public GamesResponseDTO GetCurrentGames(int playerId)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var games = context.TKGame
                    .Include(g => g.TKTournament)
                    .Include(g => g.TKTournamentPlayer)
                    .Include(g => g.TKTournamentPlayer1)
                    .Include(g => g.TKTeamMatch)
                    .Where(g => (g.TKTournamentPlayer.PlayerId == playerId || g.TKTournamentPlayer1.PlayerId == playerId))
                    .Where(g => g.TKTournament.Active)
                    .Where(g => g.TKTournament.TournamentDate <= DateTime.Now && g.TKTournament.TournamentEndDate > DbFunctions.AddDays(DateTime.Now, -1))
                    .GroupBy(g => g.TournamentId)
                    .Select(group => new
                    {
                        group.Key,
                        LatestGame = group.OrderByDescending(x => x.Round).FirstOrDefault()
                    })
                    .ToList();

                foreach (var game in games)
                {
                    if (game.LatestGame.TKTournament.TournamentType == TournamentType.Team)
                    {
                        var round = game.LatestGame.TKTeamMatch.Round;
                        var latestRound = context.TKTeamMatch.Where(tm => tm.TournamentId == game.LatestGame.TournamentId).OrderByDescending(x => x.Round).FirstOrDefault();
                        if (round != (latestRound?.Round ?? 0))
                        {
                            game.LatestGame.Round = -1;
                        }
                    }
                }

                return new GamesResponseDTO
                {
                    Games = games.Where(g => g.LatestGame.Round != -1).Select(g => g.LatestGame).ToList().Select(game => new GameDTO
                    {
                        GameId = game.Id,
                        Opponent = game.TKTournamentPlayer?.PlayerId == playerId ? game.TKTournamentPlayer1?.PlayerName : game.TKTournamentPlayer?.PlayerName,
                        Table = game.TableNumber,
                        Tournament = game.TKTournament.Name,
                        Round = game.Round,
                        MyScore = playerId == game.TKTournamentPlayer?.PlayerId ? game.Player1Result : game.Player2Result,
                        OpponentScore = playerId != game.TKTournamentPlayer?.PlayerId ? game.Player1Result : game.Player2Result,
                        MySecondaryScore = playerId == game.TKTournamentPlayer?.PlayerId ? game.Player1SecondaryResult : game.Player2SecondaryResult,
                        OpponentSecondaryScore = playerId != game.TKTournamentPlayer?.PlayerId ? game.Player1SecondaryResult : game.Player2SecondaryResult,
                        UseSecondaryPoints = game.TKTournament.UseSecondaryPoints
                    }).ToArray()
                };
            }
        }

        public TKGame GetCurrentGame(int tournamentPlayerId)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var game = context.TKGame
                    .Include(g => g.TKTournamentPlayer)
                    .Include(g => g.TKTournamentPlayer1)
                    .Where(g => (g.TKTournamentPlayer.Id == tournamentPlayerId || g.TKTournamentPlayer1.Id == tournamentPlayerId))
                    .Where(g => g.TKTournament.Active)
                    .Where(g => g.TKTournament.TournamentEndDate > DbFunctions.AddDays(DateTime.Now, -1))
                    .OrderByDescending(g => g.Round)
                    .FirstOrDefault();

                return game;
            }
        }

        public void Update(string token, int gameId, int myScore, int mySecondaryScore, int opponentScore, int opponentSecondaryScore)
        {
            TKGame game = null;

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                game = context.TKGame.SingleOrDefault(g => g.Id == gameId);
                var player = context.TKPlayer.SingleOrDefault(p => p.Token == token);

                game.Player1Result = player.Id == game.TKTournamentPlayer?.PlayerId ? myScore : opponentScore;
                game.Player2Result = player.Id != game.TKTournamentPlayer?.PlayerId ? myScore : opponentScore;
                game.Player1SecondaryResult = player.Id == game.TKTournamentPlayer?.PlayerId ? mySecondaryScore : opponentSecondaryScore;
                game.Player2SecondaryResult = player.Id != game.TKTournamentPlayer?.PlayerId ? mySecondaryScore : opponentSecondaryScore;
            }

            Update(game, token);
        }

        public void Update(TKGame game, string token)
        {
            TKPlayer player = null;
            using (var context = new TourneyKeeperEntities())
            {
                player = context.TKPlayer.SingleOrDefault(p => p.Token == token);
            }
            Update(game, player);
        }

        public void Update(TKGame game, TKPlayer player)
        {
            var isAdmin = General.IsAdminOrOrganizer(player, game.TournamentId);
            var round = GetCurrentRoundNumber(game.TournamentId);
            if ((round.Value != game.Round) && !isAdmin)
            {
                throw new SecurityException("Only admin or organizer can edit results for previous rounds");
            }

            if (game.TeamMatchId.HasValue)
            {
                UpdateTeamGame(game, player);
            }
            else
            {
                using (var context = new TourneyKeeperEntities())
                {
#if DEBUG
                    context.Database.Log = x => Debug.WriteLine(x);
#endif
                    var tkGame = context.TKGame
                        .Include(g => g.TKTournamentPlayer.TKTournament)
                        .Include(g => g.TKTournamentPlayer1.TKTournament)
                        .Single(t => t.Id == game.Id);

                    var tournament = context.TKTournament
                        .Include(g => g.TKGameSystem)
                        .Single(t => t.Id == tkGame.TournamentId);

                    tkGame.Player1Result = game.Player1Result;
                    tkGame.Player2Result = game.Player2Result;
                    tkGame.Player1SecondaryResult = game.Player1SecondaryResult;
                    tkGame.Player2SecondaryResult = game.Player2SecondaryResult;

                    tkGame.TableNumber = game.TableNumber;
                    tkGame.Player1Id = game.Player1Id;
                    tkGame.Player2Id = game.Player2Id;
                    tkGame.Round = game.Round;
                    tkGame.LastEdited = DateTime.Now;
                    tkGame.LastEditedBy = player.Name;

                    context.SaveChanges();

                    UpdatePlayerInfo(game.Player1Id.Value, context);
                    UpdatePlayerInfo(game.Player2Id.Value, context);

                    context.SaveChanges();
                }
            }
        }

        private void UpdateTeamGame(TKGame game, TKPlayer player)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var tournament = context.TKTournament
                    .Single(t => t.Id == game.TournamentId);

                var games = context.TKGame
                    .Include(g => g.TKTournamentPlayer)
                    .Include(g => g.TKTournamentPlayer1)
                    .Where(g => g.TeamMatchId == game.TeamMatchId).ToList();
                if (game.Player1Id != null)
                {
                    var p1 = games.FirstOrDefault(g => (g.Player1Id == game.Player1Id || g.Player2Id == game.Player1Id) && g.Id != game.Id);
                    if (p1 != null)
                    {
                        throw new DuplicatePairingException($"{p1.TKTournamentPlayer.PlayerName} has already been paired");
                    }
                }
                if (game.Player2Id != null)
                {
                    var p2 = games.FirstOrDefault(g => (g.Player1Id == game.Player2Id || g.Player2Id == game.Player2Id) && g.Id != game.Id);
                    if (p2 != null)
                    {
                        throw new DuplicatePairingException($"{p2.TKTournamentPlayer1.PlayerName} has already been paired");
                    }
                }

                TKTournamentPlayer player1 = null;
                TKTournamentPlayer player2 = null;

                var tkGame = context.TKGame
                    .Include(g => g.TKTournamentPlayer)
                    .Include(g => g.TKTournamentPlayer1)
                    .Single(t => t.Id == game.Id);

                player1 = tkGame.TKTournamentPlayer;
                player2 = tkGame.TKTournamentPlayer1;

                tkGame.Player1SecondaryResult = game.Player1SecondaryResult;
                tkGame.Player2SecondaryResult = game.Player2SecondaryResult;
                tkGame.Player1Result = game.Player1Result;
                tkGame.Player2Result = game.Player2Result;
                tkGame.TableNumber = game.TableNumber;
                tkGame.TKTournamentPlayer = context.TKTournamentPlayer.SingleOrDefault(tp => tp.Id == game.Player1Id);
                tkGame.TKTournamentPlayer1 = context.TKTournamentPlayer.SingleOrDefault(tp => tp.Id == game.Player2Id);
                tkGame.Round = game.Round;
                tkGame.LastEdited = DateTime.Now;
                tkGame.LastEditedBy = player.Name;

                context.SaveChanges();

                if (player1 != null && player1.Id != game.Player1Id)
                {
                    UpdatePlayerInfo(player1.Id, context);
                }

                if (player2 != null && player2.Id != game.Player2Id)
                {
                    UpdatePlayerInfo(player2.Id, context);
                }

                if (game.Player1Id.HasValue && game.Player2Id.HasValue)
                {
                    UpdatePlayerInfo(game.Player1Id.Value, context);
                    UpdatePlayerInfo(game.Player2Id.Value, context);
                }

                if (tkGame.TKTournament.TKTeamScoringSystem != null)
                {
                    TKTeamMatch tkTeamMatch = context.TKTeamMatch.Single(t => t.Id == tkGame.TeamMatchId);
                    UpdateTeamMatchPoints(game.Round, tkTeamMatch);

                    if (tkGame.TKTournament.TeamScoringSystemId == (int)TeamScoringSystem.Max)
                    {
                        if (!context.TKGame.Where(g => g.TeamMatchId == tkTeamMatch.Id && g.Player1Result == 0 && g.Player2Result == 0).Any())
                        {
                            tkTeamMatch.Team1MatchPoints = (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0)) > (tournament.MinScoreForWin ?? 0) ? (tournament.MinScoreForWin ?? 0) : (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0)) < (tournament.MaxScoreForLoss ?? 0) ? (tournament.MaxScoreForLoss ?? 0)
                                : (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0));
                            tkTeamMatch.Team2MatchPoints = (tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0)) > (tournament.MinScoreForWin ?? 0) ? (tournament.MinScoreForWin ?? 0) : (tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0)) < (tournament.MaxScoreForLoss ?? 0) ? (tournament.MaxScoreForLoss ?? 0)
                                : (tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0));
                        }
                    }
                    else if (tkGame.TKTournament.TeamScoringSystemId == (int)TeamScoringSystem.Cutoff)
                    {
                        if (!context.TKGame.Where(g => g.TeamMatchId == tkTeamMatch.Id && g.Player1Result == 0 && g.Player2Result == 0).Any())
                        {
                            tkTeamMatch.Team1MatchPoints = (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0)) >= (tournament.MinScoreForWin ?? 0) ? 2 : (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0)) <= (tournament.MaxScoreForLoss ?? 0) ? 0 : 1;
                            tkTeamMatch.Team2MatchPoints = (tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0)) >= (tournament.MinScoreForWin ?? 0) ? 2 : (tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0)) <= (tournament.MaxScoreForLoss ?? 0) ? 0 : 1;
                        }
                    }
                    else if (tkGame.TKTournament.TKTeamScoringSystem.Name.Equals("Battle points", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tkTeamMatch.Team1MatchPoints = tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0);
                        tkTeamMatch.Team2MatchPoints = tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0);
                    }
                    else if (tkGame.TKTournament.TKTeamScoringSystem.Name.Equals("Battlefront scoring", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tkTeamMatch.Team1MatchPoints = tkTeamMatch.Team1Points;
                        tkTeamMatch.Team2MatchPoints = tkTeamMatch.Team2Points;
                    }
                    else if (tkGame.TKTournament.TKTeamScoringSystem.Name.Equals("X-Wing 5-1 MAX", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!context.TKGame.Where(g => g.TeamMatchId == tkTeamMatch.Id && (g.Player1Result == 0 && g.Player2Result == 0)).Any())
                        {
                            tkTeamMatch.Team1MatchPoints = (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0)) > 5 ? 5 : (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0)) < 1 ? 1
                                : (tkTeamMatch.Team1Points + (tkTeamMatch.Team1Penalty ?? 0));
                            tkTeamMatch.Team2MatchPoints = (tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0)) > 5 ? 5 : (tkTeamMatch.Team2Points + (tkTeamMatch.Team1Penalty ?? 0)) < 1 ? 1
                                : (tkTeamMatch.Team2Points + (tkTeamMatch.Team2Penalty ?? 0));
                        }
                    }

                    context.SaveChanges();
                }

                if (game.Player1Id.HasValue && game.Player2Id.HasValue)
                {
                    UpdateTeamPoints(tkGame.TKTeamMatch.TKTournamentTeam, context);
                    UpdateTeamPoints(tkGame.TKTeamMatch.TKTournamentTeam1, context);
                }

                context.SaveChanges();
            }
        }

        public TKGame Get(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKGame.Single(g => g.Id == id);
            }
        }

        public static void UpdatePlayerInfo(int playerId, TourneyKeeperEntities context)
        {
            TKTournamentPlayer player = context.TKTournamentPlayer.Single(tp => tp.Id == playerId);
            int? result1 = context.TKGame.Where(g => g.Player1Id == playerId).Sum(g => (int?)g.Player1Result);
            int? result2 = context.TKGame.Where(g => g.Player2Id == playerId).Sum(g => (int?)g.Player2Result);
            player.BattlePoints = (result1 ?? 0) + (result2 ?? 0);

            int? secondaryResult1 = context.TKGame.Where(g => g.Player1Id == playerId).Sum(g => (int?)g.Player1SecondaryResult);
            int? secondaryResult2 = context.TKGame.Where(g => g.Player2Id == playerId).Sum(g => (int?)g.Player2SecondaryResult);
            player.SecondaryPoints = (secondaryResult1 ?? 0) + (secondaryResult2 ?? 0);

            var games = context.TKGame
               .Where(g => (g.Player1Id == playerId || g.Player2Id == playerId) && (g.Player1Result > 0 || g.Player2Result > 0 || g.Player1SecondaryResult > 0 || g.Player2SecondaryResult > 0))
               .OrderBy(g => g.Round)
               .Select(g => g.Player1Id == playerId ? g.Player1Result > g.Player2Result ? "w" : g.Player1Result == g.Player2Result ? "d" : "l" : g.Player2Result > g.Player1Result ? "w" : g.Player2Result == g.Player1Result ? "d" : "l")
               .ToList();

            player.GamePath = games.Count > 0 ? games.Aggregate((i, j) => i + "-" + j) : "";
            player.Wins = games.Count > 0 ? games.Where(g => g == "w").Count() : 0;
            player.Losses = games.Count > 0 ? games.Where(g => g == "l").Count() : 0;
            player.Draws = games.Count > 0 ? games.Where(g => g == "d").Count() : 0;

            var tournament = GetTournament(player.TournamentId);

            if (tournament.TKSinglesScoringSystem.Name.Equals("ITC", StringComparison.InvariantCultureIgnoreCase))
            {
                player.TournamentPoints = player.BattlePoints;
            }
            else
            {
                player.TournamentPoints = player.TotalPoints;
            }
        }

        private static TKTournament GetTournament(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTournament
                    .Include(g => g.TKSinglesScoringSystem)
                    .Include(g => g.TKGameSystem)
                    .Single(t => t.Id == tournamentId);
            }
        }

        private static void UpdateTeamMatchPoints(int round, TKTeamMatch tkTeamMatch)
        {
            tkTeamMatch.Team1Points = 0;
            tkTeamMatch.Team1SecondaryPoints = 0;
            foreach (TKTournamentPlayer player in tkTeamMatch.TKTournamentTeam.TKTournamentPlayer)
            {
                tkTeamMatch.Team1Points += player.TKGame.Where(p => p.Player1Id == player.Id && p.Round == round).Sum(p2 => p2.Player1Result);
                tkTeamMatch.Team1Points += player.TKGame1.Where(p => p.Player2Id == player.Id && p.Round == round).Sum(p2 => p2.Player2Result);
                tkTeamMatch.Team1SecondaryPoints += player.TKGame.Where(p => p.Player1Id == player.Id && p.Round == round).Sum(p2 => p2.Player1SecondaryResult);
                tkTeamMatch.Team1SecondaryPoints += player.TKGame1.Where(p => p.Player2Id == player.Id && p.Round == round).Sum(p2 => p2.Player2SecondaryResult);
            }
            tkTeamMatch.Team2Points = 0;
            tkTeamMatch.Team2SecondaryPoints = 0;
            foreach (TKTournamentPlayer player2 in tkTeamMatch.TKTournamentTeam1.TKTournamentPlayer)
            {
                tkTeamMatch.Team2Points += player2.TKGame.Where(p => p.Player1Id == player2.Id && p.Round == round).Sum(p2 => p2.Player1Result);
                tkTeamMatch.Team2Points += player2.TKGame1.Where(p => p.Player2Id == player2.Id && p.Round == round).Sum(p2 => p2.Player2Result);
                tkTeamMatch.Team2SecondaryPoints += player2.TKGame.Where(p => p.Player1Id == player2.Id && p.Round == round).Sum(p2 => p2.Player1SecondaryResult);
                tkTeamMatch.Team2SecondaryPoints += player2.TKGame1.Where(p => p.Player2Id == player2.Id && p.Round == round).Sum(p2 => p2.Player2SecondaryResult);
            }
        }

        private static void UpdateTeamPoints(TKTournamentTeam team, TourneyKeeperEntities context)
        {
            team.MatchPoints = 0;
            team.MatchPoints += context.TKTeamMatch.Where(tm => tm.Team1Id == team.Id).Sum(s => (int?)s.Team1MatchPoints) ?? 0;
            team.MatchPoints += context.TKTeamMatch.Where(tm => tm.Team2Id == team.Id).Sum(s => (int?)s.Team2MatchPoints) ?? 0;
            team.BattlePoints = 0;
            team.SecondaryPoints = 0;
            foreach (TKTournamentPlayer player in team.TKTournamentPlayer)
            {
                team.BattlePoints += player.BattlePoints;
                team.SecondaryPoints += player.SecondaryPoints;
            }
        }
    }
}
