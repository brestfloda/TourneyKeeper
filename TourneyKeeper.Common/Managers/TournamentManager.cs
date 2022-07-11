using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security;
using TourneyKeeper.Common.SharedCode;
using TourneyKeeper.DTO.Web;
using System.Data.Entity;

namespace TourneyKeeper.Common.Managers
{
    public class TournamentManager : IManager<TKTournament>
    {
        public void DeleteLastRound(int tournamentId, string token)
        {
            GameManager gameManager = new GameManager();
            int? roundNumber = gameManager.GetCurrentRoundNumber(tournamentId);
            if (!roundNumber.HasValue)
            {
                return;
            }

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                Security.CheckToken(token, tournamentId, context);

                context.TKTeamMatch.RemoveRange(context.TKTeamMatch.Where(t => t.TournamentId == tournamentId && t.Round == roundNumber.Value));
                context.TKGame.RemoveRange(context.TKGame.Where(g => g.TournamentId == tournamentId && g.Round == roundNumber.Value));
                context.SaveChanges();

                var currentRound = gameManager.GetCurrentRoundNumber(tournamentId);

                if (currentRound.HasValue)
                {
                    foreach (var game in context.TKGame.Where(g => g.Round == currentRound && g.TournamentId == tournamentId))
                    {
                        gameManager.Update(game, token);
                    }
                    context.SaveChanges();
                }
                else
                {
                    var players = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournamentId);
                    foreach (var player in players)
                    {
                        player.BattlePoints = 0;
                        player.SecondaryPoints = 0;
                        player.Draws = 0;
                        player.Wins = 0;
                        player.Losses = 0;
                        player.Draws = 0;
                        player.Wins = 0;
                        player.Losses = 0;
                        player.GamePath = "";
                    }
                    context.SaveChanges();

                    var teams = context.TKTournamentTeam.Where(tt => tt.TournamentId == tournamentId);
                    foreach (var team in teams)
                    {
                        team.BattlePoints = 0;
                        team.MatchPoints = 0;
                        team.SecondaryPoints = 0;
                    }
                    context.SaveChanges();
                }
            }
        }

        public void AddOrganizer(TKOrganizer organizer)
        {
            using (var context = new TourneyKeeperEntities())
            {
                if (context.TKOrganizer.SingleOrDefault(p => p.PlayerId == organizer.PlayerId && p.TournamentId == organizer.TournamentId) == null)
                {
                    var tmpOrganizer = new TKOrganizer();
                    tmpOrganizer.TournamentId = organizer.TournamentId;
                    tmpOrganizer.PlayerId = organizer.PlayerId;

                    context.TKOrganizer.Add(organizer);
                    context.SaveChanges();
                }
            }
        }

        public List<TKTournament> GetTournaments(DateTime from, DateTime to)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTournament
                    .Include(t => t.TKGameSystem)
                    .Where(t => t.TournamentDate >= from && t.TournamentEndDate <= to).ToList();
            }
        }

        public int GetCurrentRound(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKGame.Where(g => g.TKTournament.Id == tournamentId).Max(g => (int?)g.Round) ?? 0;
            }
        }

        public List<TKGameSystem> GetGameSystems()
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKGameSystem.ToList();
            }
        }

        public List<TKSinglesScoringSystem> GetSinglesScoringSystems()
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKSinglesScoringSystem.ToList();
            }
        }

        public List<TKTeamScoringSystem> GetTeamScoringSystems()
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTeamScoringSystem.OrderBy(o => o.Name).ToList();
            }
        }

        public void Update(TKTournament tournament, string token)
        {
            var isAdminOrOrganizer = General.IsAdminOrOrganizer(token, tournament.Id);
            if (!isAdminOrOrganizer)
            {
                throw new SecurityException("Only admin or organizer edit the tournament");
            }

            using (var context = new TourneyKeeperEntities())
            {
                var tkTournament = context.TKTournament.Single(t => t.Id == tournament.Id);

                tkTournament.Active = tournament.Active;
                tkTournament.Name = tournament.Name;
                tkTournament.TournamentDate = tournament.TournamentDate;
                tkTournament.TournamentEndDate = tournament.TournamentEndDate;
                tkTournament.GameSystemId = tournament.GameSystemId;
                tkTournament.TournamentTypeId = tournament.TournamentTypeId;
                tkTournament.TeamSize = tournament.TeamSize;
                tkTournament.SinglesScoringSystemId = tournament.SinglesScoringSystemId;
                tkTournament.TeamScoringSystemId = tournament.TeamScoringSystemId;
                tkTournament.UseSeed = tournament.UseSeed;
                tkTournament.UseSecondaryPoints = tournament.UseSecondaryPoints;
                tkTournament.AllowEdit = tournament.AllowEdit;
                tkTournament.OnlineSignup = tournament.OnlineSignup;
                tkTournament.OnlineSignupStart = tournament.OnlineSignupStart;
                tkTournament.MaxPlayers = tournament.MaxPlayers;
                tkTournament.Country = tournament.Country;
                tkTournament.ShowListsDate = tournament.ShowListsDate;
                tkTournament.PlayersDefaultActive = tournament.PlayersDefaultActive;
                tkTournament.Description = tournament.Description;
                tkTournament.RequireLogin = tournament.RequireLogin;
                tkTournament.ShowSoftScores = tournament.ShowSoftScores;
                tkTournament.NationalTournament = tournament.NationalTournament;
                tkTournament.UseAbout = tournament.UseAbout;
                tkTournament.MinScoreForWin = tournament.MinScoreForWin;
                tkTournament.MaxScoreForLoss = tournament.MaxScoreForLoss;
                tkTournament.OrganizerEmail = tournament.OrganizerEmail;
                tkTournament.HideResultsforRound = tournament.HideResultsforRound;

                context.SaveChanges();

                if (tkTournament.TKGame.Any())
                {
                    var gameManager = new GameManager();
                    foreach (var game in tkTournament.TKGame)
                    {
                        gameManager.Update(game, token);
                    }
                }
            }
        }

        public int AddTournament(TKTournament tournament, int currentUserId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                context.TKTournament.Add(tournament);

                var organizer = new TKOrganizer
                {
                    PlayerId = currentUserId,
                    TKTournament = tournament
                };
                context.TKOrganizer.Add(organizer);

                context.SaveChanges();

                return tournament.Id;
            }
        }

        public TKTournament GetTournament(int id, int currentUserId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                bool isAdmin = context.TKPlayer.Where(p => p.Id == currentUserId).Select(p => p.IsAdmin).Single();

                return context.TKTournament
                        .Join(context.TKOrganizer, t => t.Id, o => o.TournamentId, (t, o) => new { Tournament = t, Organizer = o })
                        .Where(t => t.Tournament.Id == id && (t.Organizer.PlayerId == currentUserId || isAdmin))
                        .Select(t => t.Tournament)
                        .Distinct()
                        .SingleOrDefault();
            }
        }

        public TKTournament Get(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTournament
                        .Where(t => t.Id == id)
                        .SingleOrDefault();
            }
        }

        public IList<YouArePlayingData> GetCurrentTournaments(int playerId, string playerToken)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                //TODO: det er fanme grimt det her...
                var youArePlayingData = context.TKTournamentPlayer
                    .Where(tp => tp.PlayerId == playerId)
                    .Where(tp => tp.TKTournament.Active)
                    .Where(tp => tp.Active)
                    .Where(tp => tp.TKTournament.TournamentEndDate > DbFunctions.AddDays(DateTime.Now, -1))
                    .ToList()
                    .Select(tp => new YouArePlayingData
                    {
                        TournamentId = tp.TournamentId,
                        TournamentName = tp.TKTournament.Name,
                        IsTeamTournament = tp.TKTournament.TournamentTypeId == (int)TournamentType.Team,
                        TournamentPlayerId = tp.Id,
                        TeamId = tp.TournamentTeamId,
                        EndDate = tp.TKTournament.TournamentEndDate,
                        Round = "not drawn yet",
                        Army = tp.TKCodex == null ?
                        $"Enter your army <a id='alink{tp.Id}' href='#'>here</a><script>$('#alink{tp.Id}').click(function(e){{ EnterArmy(e, {tp.Id},{tp.TKTournament.GameSystemId}); return false; }});</script>" :
                        $"Your army is <a id='alink{tp.Id}' href='#'>{(string.IsNullOrEmpty(tp.TKCodex.Name) ? "blank" : tp.TKCodex.Name)}</a><script>$('#alink{tp.Id}').click(function(e){{ EnterArmy(e, {tp.Id},{tp.TKTournament.GameSystemId}); return false; }});</script>",
                        IsTeamCurrentlyMatched = false
                    })
                    .ToList();

                var gameManager = new GameManager();
                var teamMatchManager = new TeamMatchManager();
                for (int i = 0; i < youArePlayingData.Count; i++)
                {
                    var data = youArePlayingData[i];
                    var game = gameManager.GetCurrentGame(data.TournamentPlayerId);
                    var teamMatch = data.IsTeamTournament ? teamMatchManager.GetCurrentMatch(playerToken) : null;

                    if (youArePlayingData[i].IsTeamTournament)
                    {
                        if (teamMatch != null)
                        {
                            var currentlyMatched = game?.Round == teamMatch.Round;
                            data.IsCurrentlyMatched = currentlyMatched;
                            data.IsTeamCurrentlyMatched = true;
                            data.Round = teamMatch.Round.ToString();
                            data.TeamTable = teamMatch.TableNumber.ToString();
                            data.Table = game?.TableNumber.ToString();
                            data.CurrentOpponentName = !currentlyMatched ? "" : game.Player1Id == data.TournamentPlayerId ? game.TKTournamentPlayer1.PlayerName : game.TKTournamentPlayer.PlayerName;
                            data.CurrentOpponentTeamName = teamMatch.Team1Id == data.TeamId ? teamMatch.TKTournamentTeam1.Name : teamMatch.TKTournamentTeam.Name;
                            data.CurrentOpponentTournamentPlayerId = !currentlyMatched ? null : game.Player1Id == data.TournamentPlayerId ? game.Player2Id : game.Player1Id;
                            data.ResultsLink = !currentlyMatched ? "" : $"/Shared/TKEditGame.aspx?TournamentId={data.TournamentId}&GameId={game.Id}";
                            data.SetupPairingsLink = $"/Team/TKTeamSetupPairings.aspx?id={data.TournamentId}&MatchId={teamMatch.Id}";
                        }
                        data.LeaderboardLink = $"/Team/TKTeamLeaderboard.aspx?id={data.TournamentId}";
                        data.PairingsLink = $"/Team/TKTeampairings.aspx?id={data.TournamentId}";
                    }
                    else
                    {
                        if (game != null)
                        {
                            data.Round = game.Round.ToString();
                            data.Table = game.TableNumber.ToString();
                            data.CurrentOpponentName = game.Player1Id == data.TournamentPlayerId ? game.TKTournamentPlayer1.PlayerName : game.TKTournamentPlayer.PlayerName;
                            data.CurrentOpponentTournamentPlayerId = game.Player1Id == data.TournamentPlayerId ? game.Player2Id : game.Player1Id;
                            data.ResultsLink = $"/Shared/TKEditGame.aspx?TournamentId={data.TournamentId}&GameId={game.Id}";
                            data.IsCurrentlyMatched = true;
                        }
                        else
                        {
                            data.IsCurrentlyMatched = false;
                        }
                        data.LeaderboardLink = $"/Singles/TKLeaderboard.aspx?id={data.TournamentId}";
                        data.PairingsLink = $"/Singles/TKPairings.aspx?id={data.TournamentId}";
                    }
                }

                return youArePlayingData.ToList();
            }
        }
    }
}
