using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.Managers.TableGenerators;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class CreateTeamPairings : TKWebPage
    {
        private TKTournament tournament = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");

            swapErrorLabel.Visible = false;

            using (var context = new TourneyKeeperEntities())
            {
                tournament = context.TKTournament.Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);

                var player = Session["LoggedInUser"] as TKPlayer;
                if (player != null)
                {
                    if (!(player != null && (player.IsAdmin || player.IsPlayerOrganizer(tournament.Id))))
                    {
                        Response.Redirect("/");
                    }
                }
                else
                {
                    Response.Redirect("/");
                }

                if(string.IsNullOrEmpty(RoundHiddenField?.Value))
                {
                    int? round = context.TKTeamMatch.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                    if (!round.HasValue)
                    {
                        return;
                    }

                    BindData(SortDirection.Descending, "Initial", round.Value);
                }
                else
                {
                    int? round = int.Parse(RoundHiddenField?.Value);
                    if (!round.HasValue)
                    {
                        return;
                    }

                    BindData(SortDirection.Descending, "Initial", round.Value);
                }
            }
        }

        protected void PairingsGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(sortDirection, e.SortExpression, int.Parse(RoundHiddenField.Value));
        }

        protected void SwapClick(object sender, EventArgs e)
        {
            int team1Id = int.Parse(swapTeam1DropDownList.SelectedItem.Value);
            int team2Id = int.Parse(swapTeam2DropDownList.SelectedItem.Value);

            if (team1Id == team2Id)
            {
                swapErrorLabel.Visible = true;
            }

            var swapManager = new SwapManager();
            TKPlayer player = Session["LoggedInUser"] as TKPlayer;

            string result = swapManager.SwapTeams(team1Id, team2Id, int.Parse(RoundHiddenField.Value), player.Token);

            swapErrorLabel.Visible = !string.IsNullOrEmpty(result);
            swapErrorLabel.Text = result;

            BindData(SortDirection.Descending, "Initial", int.Parse(RoundHiddenField.Value));
        }

        private void BindData(SortDirection direction, string expression, int roundVal)
        {
            var tournamentId = General.GetParam<int>("Id");

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var data = context.TKTeamMatch.Where(g => g.TournamentId == tournamentId && g.Round == roundVal);

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderBy(d => d.TableNumber == null);
                        break;
                    case "TableNumber":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.TableNumber) : data
                            .OrderByDescending(d => d.TableNumber);
                        break;
                    case "Team1Penalty":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.Team1Penalty) : data
                            .OrderByDescending(d => d.Team1Penalty);
                        break;
                    case "Team2Penalty":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.Team2Penalty) : data
                            .OrderByDescending(d => d.Team2Penalty);
                        break;
                }

                TKPlayer player = Session["LoggedInUser"] as TKPlayer;
                var token = player == null ? null : player.Token;

                var dataList = data.ToList();
                dataList.ForEach(d => d.Token = token);

                PairingsGridView.DataSource = dataList;
                PairingsGridView.DataBind();

                swapTeam1DropDownList.DataSource = context.TKTournamentTeam.Where(p => p.TournamentId == tournamentId).OrderBy(p2 => p2.Name).ToList();
                swapTeam1DropDownList.DataBind();

                swapTeam2DropDownList.DataSource = context.TKTournamentTeam.Where(p => p.TournamentId == tournamentId).OrderBy(p2 => p2.Name).ToList();
                swapTeam2DropDownList.DataBind();
            }

            RoundHiddenField.Value = roundVal.ToString();
        }

        protected void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var id = General.GetParam<int>("Id");
                var tournament = context.TKTournament.Single(t => t.Id == id);
                var round = int.Parse(RoundHiddenField.Value);

                BindData(SortDirection.Descending, "Initial", int.Parse(RoundHiddenField.Value));
            }
        }

        protected void CreateRandomPairingsClick(object sender, EventArgs e)
        {
            var tableGenerator = (TableGenerator)Enum.Parse(typeof(TableGenerator), TableGenerationHiddenField.Value);
            Draw(DrawTypeEnum.RandomDraw, tableGenerator);
        }

        protected void CreateSwissPairingsClick(object sender, EventArgs e)
        {
            var tableGenerator = (TableGenerator)Enum.Parse(typeof(TableGenerator), TableGenerationHiddenField.Value);
            Draw(DrawTypeEnum.SwissDraw, tableGenerator);
        }

        private void Draw(DrawTypeEnum drawType, TableGenerator tableGenerator)
        {
            var id = General.GetParam<int>("Id");
            var pairingManager = new PairingManager(tableGenerator);
            try
            {
                IEnumerable<string> warnings = pairingManager.TeamDraw(id, drawType, ((TKPlayer)Session["LoggedInUser"]).Token);
                if (warnings.Any())
                {
                    warnings = new string[] { "<br>" }.Concat(warnings);
                }
                warningsLabel.Text = string.Join("<br>", warnings);
            }
            catch (Exception e)
            {
                warningsLabel.Text = e.Message;
            }

            using (var context = new TourneyKeeperEntities())
            {
                var round = context.TKTeamMatch.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                if (!round.HasValue)
                {
                    return;
                }

                BindData(SortDirection.Descending, "Initial", round.Value);
            }
        }

        protected void PairingsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var id = General.GetParam<int>("Id");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var teamMatch = e.Row.DataItem as TKTeamMatch;

                    var teams = context.TKTournamentTeam.Where(tp => tp.TournamentId == id);

                    var teams1DropDownList = (DropDownList)e.Row.FindControl("team1DropDownList");
                    teams1DropDownList.DataSource = teams.ToList();
                    teams1DropDownList.DataBind();
                    var teams2DropDownList = (DropDownList)e.Row.FindControl("team2DropDownList");
                    teams2DropDownList.DataSource = teams.ToList();
                    teams2DropDownList.DataBind();

                    if (teamMatch.TKTournamentTeam != null)
                    {
                        teams1DropDownList.SelectedValue = teamMatch.TKTournamentTeam.Id.ToString();
                    }
                    if (teamMatch.TKTournamentTeam1 != null)
                    {
                        teams2DropDownList.SelectedValue = teamMatch.TKTournamentTeam1.Id.ToString();
                    }
                }
            }
        }

        protected void DeleteLastRoundClick(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;

            TournamentManager tournamentManager = new TournamentManager();
            tournamentManager.DeleteLastRound(id, player.Token);

            Response.Redirect(string.Format("/team/TKTeamCreatePairings.aspx?id={0}", id));
        }

        protected void Team1DropDownListDataBound(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player == null ? null : player.Token;
            string matchId = ((HiddenField)((DropDownList)sender).Parent.Parent.FindControl("MatchId")).Value;
            ((DropDownList)sender).Attributes.Add("onchange", $"javascript: UpdateField('/WebAPI/TeamMatch/Update', 'Team1Id', {matchId}, this.value, this, '{token}');");
        }

        protected void Team2DropDownListDataBound(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player == null ? null : player.Token;
            string matchId = ((HiddenField)((DropDownList)sender).Parent.Parent.FindControl("MatchId")).Value;
            ((DropDownList)sender).Attributes.Add("onchange", $"javascript: UpdateField('/WebAPI/TeamMatch/Update', 'Team2Id', {matchId}, this.value, this, '{token}');");
        }
    }
}