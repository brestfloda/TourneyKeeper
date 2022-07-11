using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;
using System.Net;

namespace TourneyKeeper.Web
{
    public partial class TeamWeightedLeaderboard : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Team Weighted Leaderboard";

            var id = General.GetParam<int>("Id");
            BindData(id, SortDirection.Descending, "Initial");
        }

        private void BindData(int tournamentId, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var tournament = context.TKTournament.Single(t => t.Id == tournamentId);

                General.RedirectToFrontIfNotReady(tournament);

                int? round = context.TKGame.Where(g => g.TournamentId == tournamentId).Select(g => (int?)g.Round).Max();
                if (!round.HasValue)
                {
                    return;
                }
                var weight = round.Value * 100.0f;
                var data = context.TKTournamentPlayer
                    .Include(t => t.TKTournament)
                    .Include(t => t.TKCodex)
                    .Include(t => t.TKGame)
                    .Include(t => t.TKGame1)
                    .Where(tp => tp.TournamentId == tournamentId && !tp.NonPlayer && tp.Active)
                    .ToList()
                    .OrderByDescending(tp => tp.TotalPoints)
                    .ThenByDescending(tp => tp.SecondaryPoints)
                    .Select(tp => new WeightedLeaderboardData()
                    {
                        Weight = weight,
                        Id = tp.Id,
                        PlayerId = tp.PlayerId,
                        Placement = 1,
                        Name = tp.PlayerName,
                        PrimaryCodex = tp.TKCodex != null ? tp.TKCodex.Name : "",
                        ArmyList = WebUtility.HtmlEncode(tp.ArmyList),
                        OpponentsPoints1 = tp.TKGame.Where(g => g.TournamentId == tournamentId).Sum(tp2 => (int?)tp2.Player2Result),
                        OpponentsPoints2 = tp.TKGame1.Where(g => g.TournamentId == tournamentId).Sum(tp2 => (int?)tp2.Player1Result),
                        TotalOpponentsPoints1 = tp.TKGame.Where(g => g.TournamentId == tournamentId).ToList().Sum(s => s.TKTournamentPlayer1.BattlePoints),
                        TotalOpponentsPoints2 = tp.TKGame1.Where(g => g.TournamentId == tournamentId).ToList().Sum(s => s.TKTournamentPlayer.BattlePoints),
                        Points = tp.BattlePoints
                    })
                    .OrderByDescending(i => i.WeightedScore)
                    .ToList();
                for (int i = 1; i < data.Count + 1; i++)
                {
                    data[i - 1].Placement = i;
                }

                IEnumerable<WeightedLeaderboardData> data2 = data.AsEnumerable();

                switch (expression)
                {
                    case "Initial":
                        data2 = data2
                            .OrderBy(tp => tp.Placement);
                        break;
                    case "Placement":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.Placement) : data2
                            .OrderByDescending(tp => tp.Placement);
                        break;
                    case "PrimaryCodex":
                        data2 = direction == SortDirection.Ascending ? data2
                           .OrderBy(tp => tp.PrimaryCodex) : data2
                           .OrderByDescending(tp => tp.PrimaryCodex);
                        break;
                    case "Player":
                        data2 = direction == SortDirection.Ascending ? data2
                           .OrderBy(tp => tp.Name) : data2
                           .OrderByDescending(tp => tp.Name);
                        break;
                    case "TotalPoints":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.TotalPoints) : data2
                            .OrderByDescending(tp => tp.TotalPoints);
                        break;
                    case "TotalOpponentsPoints":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.TotalOpponentsPoints) : data2
                            .OrderByDescending(tp => tp.TotalOpponentsPoints);
                        break;
                    case "WeightedScore":
                        data2 = direction == SortDirection.Ascending ? data2
                           .OrderBy(tp => tp.WeightedScore) : data2
                           .OrderByDescending(tp => tp.WeightedScore);
                        break;
                }

                LeaderboardGridView.DataSource = data2;
                LeaderboardGridView.DataBind();
            }
        }

        protected void LeaderboardGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(id, sortDirection, e.SortExpression);
        }
    }

    public class WeightedLeaderboardData
    {
        public float Weight { get; set; }
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Placement { get; set; }
        public string Name { get; set; }
        public string PrimaryCodex { get; set; }
        public string ArmyList { get; set; }
        public int Points { get; set; }
        public int OpponentsPoints
        {
            get
            {
                return (OpponentsPoints1 ?? 0) + (OpponentsPoints2 ?? 0);
            }
        }
        public int TotalOpponentsPoints
        {
            get
            {
                return (TotalOpponentsPoints1 ?? 0) + (TotalOpponentsPoints2 ?? 0);
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
                return ((float)TotalOpponentsPoints * (float)Points) / Weight;
            }
        }
    }
}