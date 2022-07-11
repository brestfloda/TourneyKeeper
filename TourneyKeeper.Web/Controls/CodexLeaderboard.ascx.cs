using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using TourneyKeeper.DTO.Web;
using System.Data.Entity;
using System.Web.UI.HtmlControls;
using TourneyKeeper.Common.Exceptions;

namespace TourneyKeeper.Web.Controls
{
    public partial class CodexLeaderboard : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Codex Leaderboard";

            int id = General.GetParam<int>("Id");
            BindData(id, SortDirection.Descending, "Initial");
        }

        private void BindData(int id, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var tournament = context.TKTournament.Single(t => t.Id == id);
                var hideResultsforRound = tournament.HideResultsforRound && DateTime.Now < tournament.TournamentEndDate.AddDays(1);
                if (hideResultsforRound)
                {
                    if (tournament.TournamentType == TournamentType.Team)
                    {
                        Response.Redirect($"/team/TKTeamLeaderboard.aspx?id={id}");
                        Response.Flush();
                    }
                    else
                    {
                        Response.Redirect($"/singles/TKLeaderboard.aspx?id={id}");
                        Response.Flush();
                    }
                }

                General.RedirectToFrontIfNotReady(tournament);

                int? round = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                var data = context.TKTournamentPlayer
                    .Include(t => t.TKCodex)
                    .Where(tp => tp.TournamentId == id && tp.TKCodex != null && tp.Active).ToList()
                    .GroupBy(tp => tp.TKCodex)
                    .Select(tp => new CodexLeaderboardData
                    {
                        Placement = 1,
                        PrimaryCodex = tp.Key.Name,
                        Count = tp.Count(),
                        Points = (float)tp.Sum(t => t.BattlePoints) / tp.Count(),
                        StdDev = Math.Sqrt(tp.Select(val => (val.BattlePoints - tp.Average(t => t.BattlePoints)) * (val.BattlePoints - tp.Average(t => t.BattlePoints))).Sum() / tp.Count())
                    })
                    .Concat(context.TKTournamentPlayer
                    .Include(t => t.TKCodex)
                    .Where(tp => tp.TournamentId == id && tp.TKCodex == null && tp.Active).ToList()
                    .GroupBy(tp => tp.TKCodex)
                    .Select(tp => new CodexLeaderboardData
                    {
                        Placement = 1,
                        PrimaryCodex = "Blank",
                        Count = tp.Count(),
                        Points = (float)tp.Sum(t => t.BattlePoints) / tp.Count(),
                        StdDev = Math.Sqrt(tp.Select(val => (val.BattlePoints - tp.Average(t => t.BattlePoints)) * (val.BattlePoints - tp.Average(t => t.BattlePoints))).Sum() / tp.Count())
                    }))
                    .OrderByDescending(tp => tp.Points)
                    .ToList();
                for (int i = 1; i < data.Count + 1; i++)
                {
                    data[i - 1].Placement = i;
                }

                var data2 = data.AsEnumerable();

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
                    case "Points":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.Points) : data2
                            .OrderByDescending(tp => tp.Points);
                        break;
                    case "Count":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.Count) : data2
                            .OrderByDescending(tp => tp.Count);
                        break;
                    case "StdDev":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.StdDev) : data2
                            .OrderByDescending(tp => tp.StdDev);
                        break;
                };

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
}