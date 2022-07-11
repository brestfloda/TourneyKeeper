using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web
{
    public partial class IndividualLeaderboard : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Individual Leaderboard";
            var keywords = new HtmlMeta { Name = "description", Content = "Individual Leaderboard shows the current ranking of players at a team tournament." };
            Header.Controls.Add(keywords);

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

                int placement = 1;
                IEnumerable<LeaderboardData> data = context.TKTournamentPlayer
                    .Include(t => t.TKCodex)
                    .Include(t => t.TKTournament)
                    .Include(t => t.TKTournamentTeam)
                    .Where(tp => tp.TournamentId == tournamentId && !tp.NonPlayer)
                    .OrderByDescending(tp => tp.BattlePoints + tp.Penalty)
                    .ThenByDescending(tp => tp.SecondaryPoints)
                    .ThenBy(tp => tp.PlayerName)
                    .ToList()
                    .Select(tp => new LeaderboardData()
                    {
                        Id = tp.Id,
                        PlayerId = tp.PlayerId,
                        Placement = placement++,
                        Name = tp.NameAndTeam,
                        GamePath = tp.GamePath,
                        PrimaryCodex = tp.TKCodex != null ? tp.TKCodex.Name : "",
                        Penalty = tp.Penalty,
                        BattlePoints = tp.BattlePoints,
                        SecondaryPoints = tp.SecondaryPoints,
                        Points = tp.TotalPoints,
                        HasArmylist = !string.IsNullOrEmpty(tp.ArmyList),
                        TeamName = tp.TeamName
                    });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderByDescending(tp => tp.Points)
                            .ThenByDescending(tp => tp.BattlePoints)
                            .ThenByDescending(tp => tp.SecondaryPoints)
                            .ThenBy(tp => tp.Seed);
                        break;
                    case "Placement":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Placement) : data
                            .OrderByDescending(tp => tp.Placement);
                        break;
                    case "Player":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TeamName)
                            .ThenBy(tp => tp.Name) : data
                            .OrderByDescending(tp => tp.TeamName)
                            .ThenByDescending(tp => tp.Name);
                        break;
                    case "GamePath":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.GamePath) : data
                            .OrderByDescending(tp => tp.GamePath);
                        break;
                    case "Army":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.PrimaryCodex)
                            .ThenByDescending(tp => tp.Points) : data
                            .OrderByDescending(tp => tp.PrimaryCodex)
                            .ThenByDescending(tp => tp.Points);
                        break;
                    case "Penalty":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Penalty) : data
                            .OrderByDescending(tp => tp.Penalty);
                        break;
                    case "SecondaryPoints":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.SecondaryPoints) : data
                            .OrderByDescending(tp => tp.SecondaryPoints);
                        break;
                    case "Points":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Points) : data
                            .OrderByDescending(tp => tp.Points);
                        break;
                }

                LeaderboardGridView.DataSource = data;
                LeaderboardGridView.DataBind();

                bool penalty = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.Penalty != 0);
                LeaderboardGridView.Columns[3].Visible = tournament.ShowListsDate.HasValue ? tournament.ShowListsDate < DateTime.Now :
                    tournament.TournamentDate < DateTime.Now || General.IsAdminOrOrganizer(Session["LoggedInUser"] as TKPlayer, tournamentId);
                LeaderboardGridView.Columns[4].Visible = penalty;
                LeaderboardGridView.Columns[5].Visible = tournament.UseSecondaryPoints;
            }
        }

        protected void LeaderboardGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(id, sortDirection, e.SortExpression);
        }

        protected string ShowArmylistLink(int playerId, string primaryCodex, bool hasArmylist)
        {
            if (hasArmylist)
            {
                return string.Format("<a href=\"javascript:OpenPopup({0})\" runat=\"server\">{1}</a>", playerId, primaryCodex);
            }
            else
            {
                return string.Format("{0}", primaryCodex);
            }
        }
    }
}