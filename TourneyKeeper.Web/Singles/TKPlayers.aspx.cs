using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;

namespace TourneyKeeper.Web
{
    public partial class TKPlayers : TKWebPage
    {
        public string FromSubmit = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Pairings";

            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            if (!IsPostBack)
            {
                LeaderboardGridView.EditIndex = -1;
                BindData(id, SortDirection.Descending, "Initial");
            }
        }

        protected void TournamentsDataSource_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            e.ObjectInstance = new TourneyKeeperEntities();
        }

        protected void LeaderboardGridViewRowCommand(object sender, GridViewCommandEventArgs e)
        {
            FromSubmit = "submit";

            if (e.CommandName == "RemovePlayer")
            {
                var pid = int.Parse(e.CommandArgument.ToString());
                var tpManager = new TournamentPlayerManager();
                tpManager.DeletePlayer(pid);

                var id = General.GetParam<int>("Id");
                Response.Redirect(string.Format("/Singles/tkplayers.aspx?Id={0}", id));
            }
        }

        private void BindData(int tournamentId, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var player = Session["LoggedInUser"] as TKPlayer;
                var token = player?.Token;

                var data = context.TKTournamentPlayer
                    .Include(tp => tp.TKTournament)
                    .Where(tp => tp.TournamentId == tournamentId).ToList()
                    .Select(tp => new EditPlayerData()
                    {
                        Id = tp.Id,
                        PlayerId = tp.PlayerId,
                        Seed = tp.Seed,
                        PlayerName = tp.PlayerName,
                        ArmyList = tp.TKCodex == null ? "Blank" : tp.TKCodex.Name.Replace("<", "").Replace(">", ""),
                        Penalty = tp.Penalty,
                        FairPlay = tp.FairPlay,
                        Quiz = tp.Quiz,
                        Painting = tp.Painting,
                        Paid = tp.Paid,
                        DoNotRank = tp.DoNotRank,
                        Club = tp.Club,
                        Active = tp.Active,
                        Token = token,
                        GameSystemId = tp.TKTournament.GameSystemId
                    });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderBy(tp => tp.PlayerName);
                        break;
                    case "PlayerName":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.PlayerName) : data
                            .OrderByDescending(tp => tp.PlayerName);
                        break;
                    case "Seed":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Seed) : data
                            .OrderByDescending(tp => tp.Seed);
                        break;
                    case "Paid":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Paid) : data
                            .OrderByDescending(tp => tp.Paid);
                        break;
                    case "DoNotRank":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.DoNotRank) : data
                            .OrderByDescending(tp => tp.DoNotRank);
                        break;
                    case "Club":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Club) : data
                            .OrderByDescending(tp => tp.Club);
                        break;
                    case "Fairplay":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.FairPlay) : data
                            .OrderByDescending(tp => tp.FairPlay);
                        break;
                    case "Quiz":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Quiz) : data
                            .OrderByDescending(tp => tp.Quiz);
                        break;
                    case "Painting":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Painting) : data
                            .OrderByDescending(tp => tp.Painting);
                        break;
                    case "Penalty":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Penalty) : data
                            .OrderByDescending(tp => tp.Penalty);
                        break;
                    case "Active":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Active) : data
                            .OrderByDescending(tp => tp.Active);
                        break;
                }

                LeaderboardGridView.DataSource = data;
                LeaderboardGridView.DataBind();

                var activePlayers = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournamentId && tp.Active).Count();
                SearchPlayer.SetNumPlayers(activePlayers.ToString());
            }
        }

        protected void LeaderboardGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();
            sortExpressionHidden.Value = e.SortExpression;

            BindData(id, sortDirection, e.SortExpression);
        }
    }
}