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
    public partial class TeamLeaderboard : TKWebPage
    {
        private TKTournament tournament = null;
        bool showPenalty = false;
        bool showBattlePointPenalty = false;
        bool showBattlePoints = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Team Leaderboard";
            var keywords = new HtmlMeta { Name = "description", Content = "Team Leaderboard shows the current ranking of teams at the tournament." };
            Header.Controls.Add(keywords);

            if (!IsPostBack)
            {
                var id = General.GetParam<int>("Id");
                using (var context = new TourneyKeeperEntities())
                {
#if DEBUG
                    context.Database.Log = x => Debug.WriteLine(x);
#endif
                    TKTournament tournament = context.TKTournament.Single(t => t.Id == id);

                    General.RedirectToFrontIfNotReady(tournament);

                    BindData(id, SortDirection.Ascending, "Initial");

                    LeaderboardGridView.Columns[2].Visible = tournament.UseSecondaryPoints;
                }
            }
        }


        protected string ShowArmylistImage(int playerId, string primaryCodex, bool hasArmylist)
        {
            if (hasArmylist)
            {
                return string.Format("<a href=\"javascript:OpenPopup({0})\" runat=\"server\"><img src='/Images/list.png' width='18' height='18'/></a>", playerId);
            }
            else
            {
                return string.Format("{0}", primaryCodex);
            }
        }

        private void BindData(int id, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                int placement = 1;
                var data = context.TKTournamentTeam
                    .Include(t => t.TKTournamentPlayer)
                    .Include(t => t.TKTournamentPlayer.Select(tp => tp.TKCodex))
                    .Where(tp => tp.TournamentId == id)
                    .OrderByDescending(tp => tp.MatchPoints + tp.Penalty)
                    .ThenByDescending(tp => tp.BattlePoints + tp.BattlePointPenalty)
                    .ThenByDescending(tp => tp.SecondaryPoints)
                    .ThenBy(tp => tp.Name)
                    .ToList()
                    .Select(tp => new TeamLeaderboardData()
                    {
                        TournamentId = tp.TournamentId,
                        Id = tp.Id,
                        Placement = placement++,
                        Name = tp.Name,
                        BattlePointPenalty = tp.BattlePointPenalty,
                        Penalty = tp.Penalty,
                        SecondaryPoints = tp.SecondaryPoints,
                        Points = tp.MatchPoints + tp.Penalty,
                        BattlePoints = tp.BattlePoints + tp.BattlePointPenalty,
                        Players = tp.TKTournamentPlayer.ToList().Count > 0 ? tp.TKTournamentPlayer
                            .Select(p => string.Format("<a href=\"/Shared/TKGames.aspx?PlayerId={0}\">{1}</a>&nbsp;" + ShowArmylistImage(p.Id, p.TKCodex?.Name, !string.IsNullOrEmpty(p.ArmyList)), p.PlayerId, p.NameNonPlayerStatus))
                            .Aggregate((i, j) => i + ", " + j) : ""
                    });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderByDescending(tp => tp.Points)
                            .ThenByDescending(tp => tp.BattlePoints)
                            .ThenByDescending(tp => tp.SecondaryPoints);
                        break;
                    case "Placement":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Placement) : data
                            .OrderByDescending(tp => tp.Placement);
                        break;
                    case "Name":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Name) : data
                            .OrderByDescending(tp => tp.Name);
                        break;
                    case "SecondaryPoints":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.SecondaryPoints) : data
                            .OrderByDescending(tp => tp.SecondaryPoints);
                        break;
                    case "Penalty":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Penalty) : data
                            .OrderByDescending(tp => tp.Penalty);
                        break;
                    case "BattlePointPenalty":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.BattlePointPenalty) : data
                            .OrderByDescending(tp => tp.BattlePointPenalty);
                        break;
                    case "Points":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Points) : data
                            .OrderByDescending(tp => tp.Points);
                        break;
                    case "BattlePoints":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.BattlePoints) : data
                            .OrderByDescending(tp => tp.BattlePoints);
                        break;
                }

                LeaderboardGridView.DataSource = data;
                LeaderboardGridView.DataBind();
            }
        }

        protected void LeaderboardGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = tournament.UseSecondaryPoints;
            e.Row.Cells[3].Visible = showBattlePointPenalty;
            e.Row.Cells[4].Visible = showPenalty;
            e.Row.Cells[5].Visible = showBattlePoints;
        }

        protected void LeaderboardGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(id, sortDirection, e.SortExpression);
        }

        protected void LeaderboardGridViewInit(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var id = General.GetParam<int>("Id");
                tournament = context.TKTournament.Single(t => t.Id == id);
                TKPlayer player = Session["LoggedInUser"] as TKPlayer;

                showPenalty = context.TKTournamentTeam.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.Penalty != 0);
                showBattlePointPenalty = context.TKTournamentTeam.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.BattlePointPenalty != 0);
                showBattlePoints = !tournament.TKTeamScoringSystem.Name.Equals("battle points", StringComparison.InvariantCultureIgnoreCase) || showPenalty;
                if (tournament.TKTeamScoringSystem.Name.Equals("battlefront scoring", StringComparison.InvariantCultureIgnoreCase))
                {
                    showBattlePoints = false;
                }
            }
        }
    }
}