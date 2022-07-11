using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class Pairings : TKWebPage
    {
        private TKTournament tournament = null;
        public bool editResults = false;
        public bool hideResultsforRound = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Pairings";

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
                    editResults = (tournament.AllowEdit && DateTime.Now < tournament.TournamentEndDate.AddDays(1)) 
                        || (Session["LoggedInUser"] != null && ((TKPlayer)Session["LoggedInUser"]).IsAdmin)
                        || (Session["LoggedInUser"] != null && tournament.TKOrganizer.Where(o => o.PlayerId == ((TKPlayer)Session["LoggedInUser"]).Id && o.TournamentId == tournament.Id).Any());

                    RoundDropDown.DataSource = Enumerable.Range(1, round.Value).Reverse();
                    RoundDropDown.DataBind();

                    BindData(id, round.Value, SortDirection.Descending, "Initial");
                }
                else
                {
                    int selectedRound;
                    if (!int.TryParse(RoundDropDown.SelectedValue, out selectedRound))
                    {
                        return;
                    }
                    int? currentRound = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                    if (!currentRound.HasValue)
                    {
                        return;
                    }

                    editResults = (tournament.AllowEdit && DateTime.Now < tournament.TournamentEndDate.AddDays(1) && selectedRound == currentRound.Value) 
                        || (Session["LoggedInUser"] != null && ((TKPlayer)Session["LoggedInUser"]).IsAdmin) 
                        || (Session["LoggedInUser"] != null && tournament.TKOrganizer.Where(o => o.PlayerId == ((TKPlayer)Session["LoggedInUser"]).Id && o.TournamentId == tournament.Id).Any());
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
                int currentRound = RoundDropDown.Items.Cast<ListItem>().Select(i => int.Parse(i.Value)).Max();
                hideResultsforRound = tournament.HideResultsforRound && currentRound == round && DateTime.Now < tournament.TournamentEndDate.AddDays(1);
                var playerId = ((TKPlayer)Session["LoggedInUser"])?.Id??0;

                IEnumerable<PairingsData> data = context.TKGame
                    .Where(g => g.TournamentId == id && g.Round == round)
                    .Select(g => new PairingsData()
                    {
                        TableNumber = g.TableNumber,
                        Player1Name = g.TKTournamentPlayer.PlayerName,
                        Player2Name = g.TKTournamentPlayer1.PlayerName,
                        Player1Result = g.Player1Result,
                        Player2Result = g.Player2Result,
                        Player1SecondaryResult = g.Player1SecondaryResult,
                        Player2SecondaryResult = g.Player2SecondaryResult,
                        Player1Id = g.Player1Id.HasValue ? g.TKTournamentPlayer.PlayerId : 0,
                        Player2Id = g.Player2Id.HasValue ? g.TKTournamentPlayer1.PlayerId : 0,
                        Id = g.Id,
                        TournamentId = g.TournamentId,
                        AllowEdit = editResults,
                        UseSecondaryPoints = g.TKTournament.UseSecondaryPoints,
                        PlayerId = playerId,
                        IsCurrentRound = currentRound == round
                    });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderBy(tp => tp.TableNumber);
                        break;
                    case "TableNumber":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TableNumber) : data
                            .OrderByDescending(tp => tp.TableNumber);
                        break;
                    case "Player1":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Player1Name) : data
                            .OrderByDescending(tp => tp.Player1Name);
                        break;
                    case "Player2":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Player2Name) : data
                            .OrderByDescending(tp => tp.Player2Name);
                        break;
                    case "Result":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Player1Result) : data
                            .OrderByDescending(tp => tp.Player1Result);
                        break;
                }

                PairingsGridView.DataSource = data;
                PairingsGridView.DataBind();
            }
        }

        protected void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            int round = int.Parse(RoundDropDown.SelectedValue);

            BindData(id, round, SortDirection.Descending, "Initial");
        }

        protected void GridViewInit(object sender, EventArgs e)
        {
        }

        protected void PairingsGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
        }

        protected void PairingsGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");
            int round = int.Parse(RoundDropDown.SelectedValue);

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(id, round, sortDirection, e.SortExpression);
        }
    }
}