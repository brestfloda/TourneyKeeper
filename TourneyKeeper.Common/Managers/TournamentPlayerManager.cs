using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity;
using System.Security;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Common.Managers
{
    public class TournamentPlayerManager : IManager<TKTournamentPlayer>
    {
        public TKTournamentPlayer AddTournamentPlayer(TKTournamentPlayer tournamentPlayer)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tournament = context.TKTournament.Single(t => t.Id == tournamentPlayer.TournamentId);

                tournamentPlayer.Active = tournament.PlayersDefaultActive;

                tournamentPlayer = context.TKTournamentPlayer.Add(tournamentPlayer);
                context.SaveChanges();

                return tournamentPlayer;
            }
        }

        public int NumberOfActivePlayers(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournamentId && tp.Active).Count();
            }
        }

        public void Update(TKTournamentPlayer tournamentPlayer, string token)
        {
            var isAdminOrOrganizer = General.IsAdminOrOrganizer(token, tournamentPlayer.TournamentId);
            if (!isAdminOrOrganizer)
            {
                throw new SecurityException("Only admin or organizer can edit");
            }

            using (var context = new TourneyKeeperEntities())
            {
                TKTournamentPlayer tkTournamentPlayer = context.TKTournamentPlayer.Single(t => t.Id == tournamentPlayer.Id);

                tkTournamentPlayer.Seed = tournamentPlayer.Seed;
                tkTournamentPlayer.Paid = tournamentPlayer.Paid;
                tkTournamentPlayer.FairPlay = tournamentPlayer.FairPlay;
                tkTournamentPlayer.Painting = tournamentPlayer.Painting;
                tkTournamentPlayer.Quiz = tournamentPlayer.Quiz;
                tkTournamentPlayer.Penalty = tournamentPlayer.Penalty;
                tkTournamentPlayer.TournamentTeamId = tournamentPlayer.TournamentTeamId;
                tkTournamentPlayer.DoNotRank = tournamentPlayer.DoNotRank;
                tkTournamentPlayer.Club = tournamentPlayer.Club;
                tkTournamentPlayer.PlayerName = tournamentPlayer.PlayerName;
                tkTournamentPlayer.NonPlayer = tournamentPlayer.NonPlayer;
                tkTournamentPlayer.Active = tournamentPlayer.Active;

                context.SaveChanges();
            }
        }

        public object GetPlayerDetails(int tournamentPlayerId)
        {
            throw new NotImplementedException();
        }

        public void SetPlayerArmyDetails(int tournamentPlayerId, string army, int? primaryId, int? secondaryId, int? tertiaryId, int? quaternaryId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tkTournamentPlayer = context.TKTournamentPlayer.Single(t => t.Id == tournamentPlayerId);

                tkTournamentPlayer.ArmyList = army;
                tkTournamentPlayer.PrimaryCodexId = primaryId;
                tkTournamentPlayer.SecondaryCodexId = secondaryId;
                tkTournamentPlayer.TertiaryCodexId = tertiaryId;
                tkTournamentPlayer.QuaternaryCodexId = quaternaryId;

                context.SaveChanges();
            }
        }

        public TKTournamentPlayer GetPlayerArmyDetails(int tournamentPlayerId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tkTournamentPlayer = context.TKTournamentPlayer
                    .Include(t => t.TKCodex)
                    .Include(t => t.TKTournamentTeam)
                    .Single(t => t.Id == tournamentPlayerId);

                return tkTournamentPlayer;
            }
        }

        public TKTournamentPlayer Get(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tkTournamentPlayer = context.TKTournamentPlayer.Single(t => t.Id == id);
                return tkTournamentPlayer;
            }
        }

        /// <summary>
        /// Use this over DeletePlayer(tournamentid, playerId)
        /// </summary>
        /// <param name="tournamentPlayerId"></param>
        /// <returns>True if a deletion occurred</returns>
        public bool DeletePlayer(int tournamentPlayerId)
        {
            try
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var tp = context.TKTournamentPlayer.SingleOrDefault(t => t.Id == tournamentPlayerId);

                    if (tp.TKGame.Count > 0 ||
                        tp.TKGame1.Count > 0 ||
                        (tp.TKTournamentTeam != null && (tp.TKTournamentTeam.TKTeamMatch.Any() || tp.TKTournamentTeam.TKTeamMatch1.Any())))
                    {
                        LogManager.LogEvent($"Could not delete player: {tournamentPlayerId}");
                        return false;
                    }

                    //TODO: slet for team
                    if (tp.TKOptionPlayer != null && tp.TKOptionPlayer.Any())
                    {
                        context.TKOptionPlayer.RemoveRange(tp.TKOptionPlayer);
                    }
                    context.TKTournamentPlayer.Remove(tp);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError($"Could not delete player: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="playerId"></param>
        /// <returns>True if a deletion occurred</returns>
        public bool DeletePlayer(int tournamentId, int playerId)
        {
            try
            {
                int tournamentPlayerId = 0;
                using (var context = new TourneyKeeperEntities())
                {
                    tournamentPlayerId = context.TKTournamentPlayer.SingleOrDefault(t => t.TournamentId == tournamentId && t.PlayerId == playerId).Id;
                }

                return DeletePlayer(tournamentPlayerId);
            }
            catch (Exception ex)
            {
                LogManager.LogError($"Could not delete player: {ex.Message}");
                throw;
            }
        }

        public void UpdateTournamentPlayerArmyData(TKTournamentPlayer tournamentPlayerArmyData)
        {
            using (var context = new TourneyKeeperEntities())
            {
                TKTournamentPlayer tkTournamentPlayer = context.TKTournamentPlayer.Single(t => t.Id == tournamentPlayerArmyData.Id);

                tkTournamentPlayer.PrimaryCodexId = tournamentPlayerArmyData.PrimaryCodexId;
                tkTournamentPlayer.SecondaryCodexId = tournamentPlayerArmyData.SecondaryCodexId;
                tkTournamentPlayer.TertiaryCodexId = tournamentPlayerArmyData.TertiaryCodexId;
                tkTournamentPlayer.QuaternaryCodexId = tournamentPlayerArmyData.QuaternaryCodexId;
                tkTournamentPlayer.ArmyList = tournamentPlayerArmyData.ArmyList;

                context.SaveChanges();
            }
        }
    }
}
