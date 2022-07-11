using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using TourneyKeeper.Common.SharedCode;
using TourneyKeeper.DTO.App;
using System.Data.Entity;

namespace TourneyKeeper.Common.Managers
{
    public class TournamentTeamManager : IManager<TKTournamentTeam>
    {
        public void Update(TKTournamentTeam tournamentTeam, string token)
        {
            var isAdminOrOrganizer = General.IsAdminOrOrganizer(token, tournamentTeam.TournamentId);
            if (!isAdminOrOrganizer)
            {
                throw new SecurityException("Only admin or organizer can edit");
            }

            using (var context = new TourneyKeeperEntities())
            {
                TKTournamentTeam tkTournamentTeam = context.TKTournamentTeam.Single(t => t.Id == tournamentTeam.Id);

                tkTournamentTeam.Name = tournamentTeam.Name;
                tkTournamentTeam.Penalty = tournamentTeam.Penalty;
                tkTournamentTeam.BattlePointPenalty = tournamentTeam.BattlePointPenalty;
                tkTournamentTeam.Paid = tournamentTeam.Paid;

                context.SaveChanges();
            }
        }

        public GetOpponentsResponseDTO GetOpponents(string playerToken)
        {
            var teamMatchManager = new TeamMatchManager();
            var match = teamMatchManager.GetCurrentMatch(playerToken);
            if (match != null)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var tournamentPlayer = context.TKTournamentPlayer.FirstOrDefault(tp => tp.TKPlayer.Token == playerToken && (tp.TournamentTeamId == match.Team1Id || tp.TournamentTeamId == match.Team2Id));
                    if (tournamentPlayer != null && tournamentPlayer.TournamentTeamId == match.Team1Id)
                    {
                        var opponents = context.TKTournamentPlayer
                            .Include(tp => tp.TKCodex)
                            .Where(tp => tp.TournamentTeamId == match.Team2Id).ToList();
                        var playerGame = context.TKGame.SingleOrDefault(g => g.TeamMatchId == match.Id && g.Round == match.Round && g.Player1Id == tournamentPlayer.Id);
                        if (playerGame == null)
                        {
                            playerGame = context.TKGame.FirstOrDefault(g => g.TeamMatchId == match.Id && g.Round == match.Round && g.Player1Id == null && g.Player2Id == null);
                        }
                        var dto = new GetOpponentsResponseDTO
                        {
                            GameId = playerGame?.Id,
                            Opponents = opponents.Select(tp => new TournamentPlayerDTO { Id = tp.Id, PlayerName = tp.NameAndCodex }).ToArray(),
                            CurrentOpponent = playerGame != null ? new TournamentPlayerDTO { Id = playerGame?.TKTournamentPlayer1?.Id ?? -1, PlayerName = playerGame?.TKTournamentPlayer1?.NameAndCodex } : null,
                            Tables = opponents.Count,
                            CurrentTable = playerGame?.TableNumber,
                            PlayerTeam1 = true,
                            TournamentPlayerId = tournamentPlayer.Id
                        };
                        return dto;
                    }
                    else if (tournamentPlayer != null && tournamentPlayer.TournamentTeamId == match.Team2Id)
                    {
                        var opponents = context.TKTournamentPlayer
                            .Include(tp => tp.TKCodex)
                            .Where(tp => tp.TournamentTeamId == match.Team1Id).ToList();
                        var playerGame = context.TKGame.SingleOrDefault(g => g.TeamMatchId == match.Id && g.Round == match.Round && g.Player2Id == tournamentPlayer.Id);
                        if (playerGame == null)
                        {
                            playerGame = context.TKGame.FirstOrDefault(g => g.TeamMatchId == match.Id && g.Round == match.Round && g.Player1Id == null && g.Player2Id == null);
                        }
                        var dto = new GetOpponentsResponseDTO
                        {
                            GameId = playerGame?.Id,
                            Opponents = opponents.Select(tp => new TournamentPlayerDTO { Id = tp.Id, PlayerName = tp.PlayerName }).ToArray(),
                            CurrentOpponent = playerGame != null ? new TournamentPlayerDTO { Id = playerGame?.TKTournamentPlayer?.Id ?? -1, PlayerName = playerGame?.TKTournamentPlayer?.NameAndCodex } : null,
                            Tables = opponents.Count,
                            CurrentTable = playerGame?.TableNumber,
                            PlayerTeam1 = false,
                            TournamentPlayerId = tournamentPlayer.Id
                        };
                        return dto;
                    }
                    else
                    {
                        return new GetOpponentsResponseDTO { GameId = null };
                    }
                }
            }
            else
            {
                return new GetOpponentsResponseDTO { GameId = null };
            }
        }

        public int AddTournamentTeam(TKTournamentTeam tournamentTeam, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
                tournamentTeam.TournamentId = tournamentTeam.TournamentId;
                tournamentTeam.Name = tournamentTeam.Name;

                context.TKTournamentTeam.Add(tournamentTeam);
                context.SaveChanges();

                return tournamentTeam.Id;
            }
        }

        public void DeleteTournamentTeams(List<int> tournamentTeamIds, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
                foreach (int tournamentTeamId in tournamentTeamIds)
                {
                    if (!context.TKTeamMatch.Any(m => m.Team1Id == tournamentTeamId) && !context.TKTeamMatch.Any(m => m.Team2Id == tournamentTeamId))
                    {
                        Security.CheckToken(token, context.TKTournamentTeam.Single(t => t.Id == tournamentTeamId).TournamentId, context);

                        var tmpTournamentTeam = context.TKTournamentTeam.Single(p => p.Id == tournamentTeamId);

                        var tmpPlayers = context.TKTournamentPlayer.Where(p => p.TournamentTeamId == tournamentTeamId);
                        foreach (var player in tmpPlayers)
                        {
                            player.TournamentTeamId = null;
                        }

                        context.TKOptionTeam.RemoveRange(context.TKOptionTeam.Where(o => o.TeamId == tournamentTeamId));
                        context.TKTournamentTeam.Remove(tmpTournamentTeam);
                    }
                }
                context.SaveChanges();
            }
        }

        public TKTournamentTeam Get(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTournamentTeam.Single(t => t.Id == id);
            }
        }
    }
}
