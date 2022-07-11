using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;
using System.Net;

namespace TourneyKeeper.Web
{
    public partial class WeightedLeaderboard : TKWebPage
    {
        private bool showArmy = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Weighted Leaderboard";

            var id = General.GetParam<int>("Id");
            BindData(id, SortDirection.Descending, "Initial");
        }

        protected void LeaderboardGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = showArmy;
            e.Row.Cells[3].Visible = showArmy;
        }

        protected void LeaderboardGridViewInit(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var id = General.GetParam<int>("Id");
                var tournament = context.TKTournament.Single(t => t.Id == id);
                var hideResultsforRound = tournament.HideResultsforRound && DateTime.Now < tournament.TournamentEndDate.AddDays(1);
                if(hideResultsforRound)
                {
                    Response.Redirect($"/singles/tkleaderboard.aspx?id={id}");
                }

                TKPlayer player = Session["LoggedInUser"] as TKPlayer;

                showArmy = tournament.ShowListsDate.HasValue ? tournament.ShowListsDate < DateTime.Now :
                               tournament.TournamentDate < DateTime.Now || General.IsAdminOrOrganizer(Session["LoggedInUser"] as TKPlayer, id);
            }
        }

        private void BindData(int id, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                TKTournament tournament = context.TKTournament.Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);

                int? round = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                if (!round.HasValue)
                {
                    return;
                }
                var weight = round.Value * 100.0f;
                var data = context.TKTournamentPlayer
                    .Include(t => t.TKTournament)
                    .Include(t => t.TKGame)
                    .Include(t => t.TKGame1)
                    .Include(t => t.TKGame.Select(g => g.TKTournamentPlayer1))
                    .Include(t => t.TKGame1.Select(g => g.TKTournamentPlayer))
                    .Include(t => t.TKCodex)
                    .Where(tp => tp.TournamentId == id && tp.Active).ToList()
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
                        TotalOpponentsPoints1 = tp.TKGame.Where(tp2 => tp2.Player1Id == tp.Id).Sum(s => s.TKTournamentPlayer1.BattlePoints),
                        TotalOpponentsPoints2 = tp.TKGame1.Where(tp2 => tp2.Player2Id == tp.Id).Sum(s => s.TKTournamentPlayer.BattlePoints),
                        Points = tp.TotalPoints
                    }).OrderByDescending(i => i.WeightedScore).ToList();
                for (int i = 1; i < data.Count + 1; i++)
                {
                    data[i - 1].Placement = i;
                }
                LeaderboardGridView.DataSource = data;
                LeaderboardGridView.DataBind();

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
                    case "Name":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.Name) : data2
                            .OrderByDescending(tp => tp.Name);
                        break;
                    case "PrimaryCodex":
                        data2 = direction == SortDirection.Ascending ? data2
                            .OrderBy(tp => tp.PrimaryCodex) : data2
                            .OrderByDescending(tp => tp.PrimaryCodex);
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