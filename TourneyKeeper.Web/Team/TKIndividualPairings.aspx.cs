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
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web
{
    public partial class IndividualPairings : TKWebPage
    {
        private TKTournament tournament = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Individual Pairings";

            var id = General.GetParam<int>("Id");
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                tournament = context.TKTournament.Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);

                int? round = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();

                if (!round.HasValue)
                {
                    return;
                }

                if (!IsPostBack)
                {
                    RoundDropDown.DataSource = Enumerable.Range(1, round.Value).Reverse();
                    RoundDropDown.DataBind();

                    BindData(id, round.Value, SortDirection.Descending, "Initial");
                }
            }
        }

        private void BindData(int id, int round, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var tournament = context.TKTournament.Single(t => t.Id == id);
                var player = Session["LoggedInUser"] as TKPlayer;
                var tournamentPlayer = player == null ? null : context.TKTournamentPlayer.Include(t => t.TKTournamentTeam).SingleOrDefault(tp => tp.PlayerId == player.Id && tp.TournamentId == id);

                int currentRound = RoundDropDown.Items.Cast<ListItem>().Select(i => int.Parse(i.Value)).Max();
                bool hide = tournament.HideResultsforRound && currentRound == round;
                var allowEdit = tournament.AllowEdit && round == currentRound;

                var data = context.TKGame
                    .Include(t => t.TKTournamentPlayer)
                    .Include(t => t.TKTournamentPlayer1)
                    .Include(t => t.TKTournamentPlayer.TKCodex)
                    .Include(t => t.TKTournamentPlayer1.TKCodex)
                    .Include(t => t.TKTournamentPlayer.TKTournamentTeam)
                    .Include(t => t.TKTournamentPlayer1.TKTournamentTeam)
                    .Include(t => t.TKTeamMatch)
                    .Where(g => g.TournamentId == id && g.Round == round)
                    .ToList()
                    .Select(g => new PairingsData()
                    {
                        TableNumber = g.TableNumber,
                        Player1Name = g.TKTournamentPlayer != null ? g.TKTournamentPlayer.NameTeamAndArmy : g.TKTeamMatch.TKTournamentTeam.Name,
                        Player2Name = g.TKTournamentPlayer1 != null ? g.TKTournamentPlayer1.NameTeamAndArmy : g.TKTeamMatch.TKTournamentTeam1.Name,
                        Team1Name = g.TKTeamMatch.TKTournamentTeam.Name,
                        Team2Name = g.TKTeamMatch.TKTournamentTeam1.Name,
                        Player1Result =  g.Player1Result,
                        Player2Result =  g.Player2Result,
                        Player1SecondaryResult =  g.Player1SecondaryResult,
                        Player2SecondaryResult =  g.Player2SecondaryResult,
                        Player1Id = g.Player1Id.HasValue ? g.TKTournamentPlayer.PlayerId : 0,
                        Player2Id = g.Player2Id.HasValue ? g.TKTournamentPlayer1.PlayerId : 0,
                        Id = g.Id,
                        TournamentId = g.TournamentId,
                        AllowEdit = General.IsAdminOrOrganizer(Session["LoggedInUser"] as TKPlayer, id) ||
                            (allowEdit && tournamentPlayer != null && g.TKTournamentPlayer != null && g.TKTournamentPlayer1 != null && (g.TKTournamentPlayer.TournamentTeamId == tournamentPlayer.TournamentTeamId || g.TKTournamentPlayer1.TournamentTeamId == tournamentPlayer.TournamentTeamId)),
                        UseSecondaryPoints = g.TKTournament.UseSecondaryPoints
                    });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderBy(d => d.Team1Name)
                            .ThenBy(d => d.TableNumber);
                        break;
                    case "Table":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.Team1Name)
                            .ThenBy(d => d.TableNumber) : data
                            .OrderByDescending(d => d.Team1Name)
                            .ThenBy(d => d.TableNumber);
                        break;
                    case "Player1":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.Player1Name) : data
                            .OrderByDescending(d => d.Player1Name);
                        break;
                    case "Player2":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.Player2Name) : data
                            .OrderByDescending(d => d.Player2Name);
                        break;
                }

                PairingsGridView.DataSource = data;
                PairingsGridView.DataBind();
            }
        }

        protected void PairingsGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();
            int round = int.Parse(RoundDropDown.SelectedValue);

            BindData(id, round, sortDirection, e.SortExpression);
        }

        protected void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            int round = int.Parse(RoundDropDown.SelectedValue);

            BindData(id, round, SortDirection.Descending, "Initial");
        }

        protected void SetupPairingsClick(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            Response.Redirect(string.Format("/Team/TKTeamSetupPairings.aspx?id={0}&round={1}", id, RoundDropDown.SelectedValue.ToString()));
        }

        protected void PairingsGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
        }
    }
}