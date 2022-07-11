using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;

namespace TourneyKeeper.Common.Managers
{
    public class TeamMatchManager : IManager<TKTeamMatch>
    {
        public void Update(TKTeamMatch teamMatch, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
                TKTeamMatch tkTeamMatch = context.TKTeamMatch.Single(t => t.Id == teamMatch.Id);

                var round = context.TKGame.Where(g => g.TKTournament.Id == teamMatch.TournamentId).Max(g => (int?)g.Round);
                if (round.Value != tkTeamMatch.Round)
                {
                    var isAdminOrOrganizer = General.IsAdminOrOrganizer(token, tkTeamMatch.TournamentId);
                    if (!isAdminOrOrganizer)
                    {
                        throw new SecurityException("Only admin or organizer can edit results for previous rounds");
                    }
                }

                tkTeamMatch.Round = teamMatch.Round;
                tkTeamMatch.TableNumber = teamMatch.TableNumber;
                tkTeamMatch.Team1Id = teamMatch.Team1Id;
                tkTeamMatch.Team2Id = teamMatch.Team2Id;
                tkTeamMatch.Team1Penalty = teamMatch.Team1Penalty;
                tkTeamMatch.Team2Penalty = teamMatch.Team2Penalty;

                context.SaveChanges();
            }
        }

        public TKTeamMatch GetCurrentMatch(string playerToken)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var match1 = context.TKTournamentPlayer
                    .Join(context.TKTeamMatch, tp => tp.TKTournamentTeam.Id, tm => tm.Team1Id, (tp, tm) => new { TP = tp, TM = tm })
                    .Where(v => v.TP.TKPlayer.Token == playerToken &&
                        v.TP.TKTournament.Active &&
                        v.TP.TKTournament.TournamentEndDate > DbFunctions.AddDays(DateTime.Now, -1))
                    .GroupBy(tp => tp.TP.TournamentId)
                    .Select(group => group.OrderByDescending(x => x.TM.Round).FirstOrDefault().TM)
                    .Include(tm => tm.TKTournamentTeam)
                    .Include(tm => tm.TKTournamentTeam1)
                    .FirstOrDefault();

                var match2 = context.TKTournamentPlayer
                    .Join(context.TKTeamMatch, tp => tp.TKTournamentTeam.Id, tm => tm.Team2Id, (tp, tm) => new { TP = tp, TM = tm })
                    .Where(v => v.TP.TKPlayer.Token == playerToken &&
                        v.TP.TKTournament.Active &&
                        v.TP.TKTournament.TournamentEndDate > DbFunctions.AddDays(DateTime.Now, -1))
                    .GroupBy(tp => tp.TP.TournamentId)
                    .Select(group => group.OrderByDescending(x => x.TM.Round).FirstOrDefault().TM)
                    .Include(tm => tm.TKTournamentTeam)
                    .Include(tm => tm.TKTournamentTeam1)
                    .FirstOrDefault();

                if ((match1?.Round ?? 0) > (match2?.Round ?? 0))
                {
                    return match1;
                }

                return match2;
            }
        }

        public TKTeamMatch Get(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTeamMatch.Single(g => g.Id == id);
            }
        }
    }
}
