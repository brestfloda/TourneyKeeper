using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;

namespace TourneyKeeper.Common.Managers
{
    public class SwapManager
    {
        public string SwapPlayers(int player1Id, int player2Id, int round, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var gamesToSwapFrom = context.TKGame.Where(g => (g.Round == round && (g.Player1Id == player1Id || g.Player1Id == player2Id || g.Player2Id == player1Id || g.Player2Id == player2Id)));
                if (gamesToSwapFrom.Count() != 2)
                {
                    return "You must select exactly 2 players from 2 different games in the same round if you want to swap";
                }

                var tkgame1 = gamesToSwapFrom.First();
                var tkgame2 = gamesToSwapFrom.ToList().Last();

                if (tkgame1 == tkgame2)
                {
                    return "You must select exactly 2 players from 2 different games in the same round if you want to swap";
                }

                tkgame1.TournamentPlayer1MarkForSwap = (tkgame1.Player1Id.HasValue && tkgame1.Player1Id.Value == player1Id) || (tkgame1.Player1Id.HasValue && tkgame1.Player1Id.Value == player2Id);
                tkgame1.TournamentPlayer2MarkForSwap = (tkgame1.Player2Id.HasValue && tkgame1.Player2Id.Value == player1Id) || (tkgame1.Player2Id.HasValue && tkgame1.Player2Id.Value == player2Id);

                tkgame2.TournamentPlayer1MarkForSwap = (tkgame2.Player1Id.HasValue && tkgame2.Player1Id.Value == player1Id) || (tkgame2.Player1Id.HasValue && tkgame2.Player1Id.Value == player2Id);
                tkgame2.TournamentPlayer2MarkForSwap = (tkgame2.Player2Id.HasValue && tkgame2.Player2Id.Value == player1Id) || (tkgame2.Player2Id.HasValue && tkgame2.Player2Id.Value == player2Id);

                SwapPlayers(tkgame1, tkgame2, token);

                return null;
            }
        }

        public string SwapTeamPlayers(int matchId, int player1Id, int player2Id, int round, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var match = context.TKTeamMatch
                    .Include(m => m.TKTournamentTeam)
                    .Include(m => m.TKTournamentTeam1)
                    .Single(m => m.Id == matchId);
                var playersOnTeam1 = match.TKTournamentTeam
                    .TKTournamentPlayer
                    .Where(p => p.Id == player1Id || p.Id == player2Id)
                    .Count();
                var playersOnTeam2 = match.TKTournamentTeam1
                    .TKTournamentPlayer
                    .Where(p => p.Id == player1Id || p.Id == player2Id)
                    .Count();

                if (!((playersOnTeam1 == 2 && playersOnTeam2 == 0) || (playersOnTeam2 == 2 && playersOnTeam1 == 0)))
                {
                    return "You must select exactly 2 players from 1 team in the same match if you want to swap";
                }

                var gamesToSwapFrom = context.TKGame.Where(g => (g.TeamMatchId == matchId && (g.Player1Id == player1Id || g.Player1Id == player2Id || g.Player2Id == player1Id || g.Player2Id == player2Id)));
                if (gamesToSwapFrom.Count() != 2)
                {
                    return "You must select exactly 2 players from 1 team in 2 games in the same match if you want to swap";
                }

                var tkgame1 = gamesToSwapFrom.First();
                var tkgame2 = gamesToSwapFrom.ToList().Last();

                tkgame1.TournamentPlayer1MarkForSwap = (tkgame1.Player1Id.HasValue && tkgame1.Player1Id.Value == player1Id) || (tkgame1.Player1Id.HasValue && tkgame1.Player1Id.Value == player2Id);
                tkgame1.TournamentPlayer2MarkForSwap = (tkgame1.Player2Id.HasValue && tkgame1.Player2Id.Value == player1Id) || (tkgame1.Player2Id.HasValue && tkgame1.Player2Id.Value == player2Id);

                tkgame2.TournamentPlayer1MarkForSwap = (tkgame2.Player1Id.HasValue && tkgame2.Player1Id.Value == player1Id) || (tkgame2.Player1Id.HasValue && tkgame2.Player1Id.Value == player2Id);
                tkgame2.TournamentPlayer2MarkForSwap = (tkgame2.Player2Id.HasValue && tkgame2.Player2Id.Value == player1Id) || (tkgame2.Player2Id.HasValue && tkgame2.Player2Id.Value == player2Id);

                SwapPlayers(tkgame1, tkgame2, token);

                return null;
            }
        }

        public string SwapTeams(int team1Id, int team2Id, int round, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var matchesToSwapFrom = context.TKTeamMatch.Where(g => (g.Round == round && (g.Team1Id == team1Id || g.Team1Id == team2Id || g.Team2Id == team1Id || g.Team2Id == team2Id)));
                if (matchesToSwapFrom.Count() != 2)
                {
                    return "You must select exactly 2 teams from 2 different matches in the same round if you want to swap";
                }

                var tkMatch1 = matchesToSwapFrom.First();
                var tkMatch2 = matchesToSwapFrom.ToList().Last();

                if (tkMatch1 == tkMatch2)
                {
                    return "You must select exactly 2 teams from 2 different matches in the same round if you want to swap";
                }

                tkMatch1.TournamentTeam1MarkForSwap = (tkMatch1.Team1Id == team1Id) || (tkMatch1.Team1Id == team2Id);
                tkMatch1.TournamentTeam2MarkForSwap = (tkMatch1.Team2Id == team1Id) || (tkMatch1.Team2Id == team2Id);

                tkMatch2.TournamentTeam1MarkForSwap = (tkMatch2.Team1Id == team1Id) || (tkMatch2.Team1Id == team2Id);
                tkMatch2.TournamentTeam2MarkForSwap = (tkMatch2.Team2Id == team1Id) || (tkMatch2.Team2Id == team2Id);

                SwapTeamMatches(tkMatch1, tkMatch2, token);

                return null;
            }
        }

        public void SwapPlayers(TKGame game1, TKGame game2, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var dbGame1 = context.TKGame.Single(g => g.Id == game1.Id);
                var dbGame2 = context.TKGame.Single(g => g.Id == game2.Id);

                Security.CheckToken(token, dbGame1.TournamentId, context);

                if (game1.TournamentPlayer1MarkForSwap)
                {
                    if (game2.TournamentPlayer1MarkForSwap)
                    {
                        int tmpId = dbGame2.Player1Id.Value;
                        dbGame2.Player1Id = dbGame1.Player1Id;
                        dbGame1.Player1Id = tmpId;
                    }
                    else
                    {
                        int tmpId = dbGame2.Player2Id.Value;
                        dbGame2.Player2Id = dbGame1.Player1Id;
                        dbGame1.Player1Id = tmpId;
                    }
                }
                else
                {
                    if (game2.TournamentPlayer1MarkForSwap)
                    {
                        int tmpId = dbGame2.Player1Id.Value;
                        dbGame2.Player1Id = dbGame1.Player2Id;
                        dbGame1.Player2Id = tmpId;
                    }
                    else
                    {
                        int tmpId = dbGame2.Player2Id.Value;
                        dbGame2.Player2Id = dbGame1.Player2Id;
                        dbGame1.Player2Id = tmpId;
                    }
                }

                context.SaveChanges();

                NotificationManager.NotifySwap(new TKGame[] { dbGame1, dbGame2 });
            }
        }

        private void SwapTeamMatches(TKTeamMatch teamMatch1, TKTeamMatch teamMatch2, string token)
        {
            TKTournament tournament = null;

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                tournament = context.TKTournament.Single(t => t.Id == teamMatch1.TournamentId);

                TKTeamMatch dbTeamMatch1 = context.TKTeamMatch.Single(g => g.Id == teamMatch1.Id);
                TKTeamMatch dbTeamMatch2 = context.TKTeamMatch.Single(g => g.Id == teamMatch2.Id);

                Security.CheckToken(token, dbTeamMatch1.TournamentId, context);

                if (teamMatch1.TournamentTeam1MarkForSwap)
                {
                    if (teamMatch2.TournamentTeam1MarkForSwap)
                    {
                        int tmpId = dbTeamMatch2.Team1Id;
                        dbTeamMatch2.Team1Id = teamMatch1.Team1Id;
                        dbTeamMatch1.Team1Id = tmpId;
                    }
                    else
                    {
                        int tmpId = dbTeamMatch2.Team2Id;
                        dbTeamMatch2.Team2Id = teamMatch1.Team1Id;
                        dbTeamMatch1.Team1Id = tmpId;
                    }
                }
                else
                {
                    if (teamMatch2.TournamentTeam1MarkForSwap)
                    {
                        int tmpId = dbTeamMatch2.Team1Id;
                        dbTeamMatch2.Team1Id = teamMatch1.Team2Id;
                        dbTeamMatch1.Team2Id = tmpId;
                    }
                    else
                    {
                        int tmpId = dbTeamMatch2.Team2Id;
                        dbTeamMatch2.Team2Id = teamMatch1.Team2Id;
                        dbTeamMatch1.Team2Id = tmpId;
                    }
                }

                context.SaveChanges();

                NotifyTeamSwap(dbTeamMatch1, dbTeamMatch2);
            }
        }

        private void NotifyTeamSwap(TKTeamMatch dbTeamMatch1, TKTeamMatch dbTeamMatch2)
        {
            try
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var players1 = context.TKTournamentPlayer.Where(tp => tp.TournamentTeamId == dbTeamMatch1.Team1Id && tp.TKPlayer.AppNotificationToken != null);
                    foreach (var player in players1)
                    {
                        NotificationManager.NotifyTeamMatchSwap(dbTeamMatch1.TKTournament.Name, dbTeamMatch1.TKTournamentTeam1?.Name, dbTeamMatch1.TableNumber?.ToString(), player.TKPlayer.AppNotificationToken);
                    }
                    var players2 = context.TKTournamentPlayer.Where(tp => tp.TournamentTeamId == dbTeamMatch1.Team2Id && tp.TKPlayer.AppNotificationToken != null);
                    foreach (var player in players2)
                    {
                        NotificationManager.NotifyTeamMatchSwap(dbTeamMatch1.TKTournament.Name, dbTeamMatch1.TKTournamentTeam?.Name, dbTeamMatch1.TableNumber?.ToString(), player.TKPlayer.AppNotificationToken);
                    }
                    var players3 = context.TKTournamentPlayer.Where(tp => tp.TournamentTeamId == dbTeamMatch2.Team1Id && tp.TKPlayer.AppNotificationToken != null);
                    foreach (var player in players3)
                    {
                        NotificationManager.NotifyTeamMatchSwap(dbTeamMatch2.TKTournament.Name, dbTeamMatch2.TKTournamentTeam1?.Name, dbTeamMatch2.TableNumber?.ToString(), player.TKPlayer.AppNotificationToken);
                    }
                    var players4 = context.TKTournamentPlayer.Where(tp => tp.TournamentTeamId == dbTeamMatch2.Team2Id && tp.TKPlayer.AppNotificationToken != null);
                    foreach (var player in players4)
                    {
                        NotificationManager.NotifyTeamMatchSwap(dbTeamMatch2.TKTournament.Name, dbTeamMatch2.TKTournamentTeam?.Name, dbTeamMatch2.TableNumber?.ToString(), player.TKPlayer.AppNotificationToken);
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
            }
        }
    }
}
