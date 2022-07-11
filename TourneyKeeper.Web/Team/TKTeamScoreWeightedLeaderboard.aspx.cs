using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;

namespace TourneyKeeper.Web
{
    public partial class TeamScoreWeightedLeaderboard : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Team Weighted Leaderboard";

            var id = General.GetParam<int>("Id");
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                TKTournament tournament = context.TKTournament.Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);
                
                int? round = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                List<TeamScoreWeightedLeaderboardData> leaderboardData = context.TKTournamentTeam
                    .Include(t => t.TKTeamMatch)
                    .Include(t => t.TKTeamMatch1)
                    .Where(tt => tt.TournamentId == id).ToList()
                    .OrderByDescending(tt => tt.BattlePoints)
                    .ThenByDescending(tt => tt.SecondaryPoints)
                    .Select(tt => new TeamScoreWeightedLeaderboardData()
                    {
                        Id = tt.Id,
                        TeamId = tt.Id,
                        Placement = 1,
                        Name = tt.Name,
                        OpponentsPoints1 = tt.TKTeamMatch.Where(tp2 => tp2.Team1Id == tt.Id).Sum(tp2 => (int?)tp2.Team2Points),
                        OpponentsPoints2 = tt.TKTeamMatch1.Where(tp2 => tp2.Team2Id == tt.Id).Sum(tp2 => (int?)tp2.Team1Points),
                        TotalOpponentsPoints1 = tt.TKTeamMatch.Where(tp2 => tp2.Team1Id == tt.Id).ToList().Sum(s => s.TKTournamentTeam1.BattlePoints),
                        TotalOpponentsPoints2 = tt.TKTeamMatch1.Where(tp2 => tp2.Team2Id == tt.Id).ToList().Sum(s => s.TKTournamentTeam.BattlePoints),
                        Points = tt.BattlePoints + tt.Penalty
                    }).OrderByDescending(i => i.WeightedScore).ToList();
                for (int i = 1; i < leaderboardData.Count + 1; i++)
                {
                    leaderboardData[i - 1].Placement = i;
                }
                LeaderboardGridView.DataSource = leaderboardData;
                LeaderboardGridView.DataBind();
            }
        }
    }

    public class TeamScoreWeightedLeaderboardData
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int Placement { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int OpponentsPoints
        {
            get
            {
                return (OpponentsPoints1.HasValue ? OpponentsPoints1.Value : 0) + (OpponentsPoints2.HasValue ? OpponentsPoints2.Value : 0);
            }
        }
        public int TotalOpponentsPoints
        {
            get
            {
                return (TotalOpponentsPoints1.HasValue ? TotalOpponentsPoints1.Value : 0) + (TotalOpponentsPoints2.HasValue ? TotalOpponentsPoints2.Value : 0);
            }
        }
        public int TotalPoints
        {
            get
            {
                return OpponentsPoints + Points;
            }
        }
        public int? OpponentsPoints1 { get; set; }
        public int? OpponentsPoints2 { get; set; }
        public int? TotalOpponentsPoints1 { get; set; }
        public int? TotalOpponentsPoints2 { get; set; }
        public float WeightedScore
        {
            get
            {
                return ((float)TotalOpponentsPoints * (float)Points) / 500.0f;
            }
        }
    }
}