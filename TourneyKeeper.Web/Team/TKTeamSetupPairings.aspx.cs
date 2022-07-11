using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Exceptions;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;

namespace TourneyKeeper.Web
{
    public partial class SetupTeamPairings : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Team Setup Pairings";

            warningsLabel.Text = "";
            swapErrorLabel.Visible = false;

            using (var context = new TourneyKeeperEntities())
            {
                var id = General.GetParam<int>("Id");
                var tournament = context.TKTournament.Single(t => t.Id == id);
                var player = Session["LoggedInUser"] as TKPlayer;

                if (General.IsAdminOrOrganizer(player, id))
                {
                    //ikke en skid...
                }
                else
                {
                    var tp = context.TKTournamentPlayer.SingleOrDefault(t => t.PlayerId == player.Id && t.TournamentId == id);
                    if (tp == null)
                    {
                        Response.Redirect($"/Team/TKTeamPairings.aspx?id={id}");
                        return;
                    }

                    int matchId = int.Parse(Request["MatchId"]);
                    var isPlayerInMatch = context.TKTeamMatch
                        .Where(tm => (tm.Team1Id == tp.TKTournamentTeam.Id || tm.Team2Id == tp.TKTournamentTeam.Id) &&
                        tm.Id == matchId).Any();
                    if (!(isPlayerInMatch || General.IsAdminOrOrganizer(player, id)))
                    {
                        Response.Redirect($"/Team/TKTeamPairings.aspx?id={id}");
                        return;
                    }
                }

                penaltyPlaceHolder.Visible = player != null && (player.IsAdmin || player.IsPlayerOrganizer(tournament.Id));
            }

            if (!IsPostBack)
            {
                BindData(SortDirection.Descending, "Initial");
            }
        }

        protected void PairingsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                using (var context = new TourneyKeeperEntities())
                {
#if DEBUG
                    context.Database.Log = x => Debug.WriteLine(x);
#endif
                    var game = e.Row.DataItem as TKGame;

                    var team1Players = game.TKTeamMatch
                        .TKTournamentTeam
                        .TKTournamentPlayer
                        .Where(tp => !tp.NonPlayer)
                        .Select(tp => new { Id = tp.Id, Name = tp.PlayerName, NameAndCodex = tp.NameAndCodex })
                        .OrderBy(tp => tp.NameAndCodex);
                    var team2Players = game.TKTeamMatch
                        .TKTournamentTeam1
                        .TKTournamentPlayer
                        .Where(tp => !tp.NonPlayer)
                        .Select(tp => new { Id = tp.Id, Name = tp.PlayerName, NameAndCodex = tp.NameAndCodex })
                        .OrderBy(tp => tp.NameAndCodex);

                    var team1PlayersDropDownList = (DropDownList)e.Row.FindControl("team1PlayersDropDownList");
                    team1PlayersDropDownList.DataSource = team1Players;
                    team1PlayersDropDownList.DataBind();
                    var team2PlayersDropDownList = (DropDownList)e.Row.FindControl("team2PlayersDropDownList");
                    team2PlayersDropDownList.DataSource = team2Players;
                    team2PlayersDropDownList.DataBind();

                    if (game.TKTournamentPlayer != null)
                    {
                        team1PlayersDropDownList.SelectedValue = game.TKTournamentPlayer.Id.ToString();
                    }
                    if (game.TKTournamentPlayer1 != null)
                    {
                        team2PlayersDropDownList.SelectedValue = game.TKTournamentPlayer1.Id.ToString();
                    }
                }
            }
        }

        private void BindData(SortDirection direction, string expression)
        {
            var id = General.GetParam<int>("Id");
            var matchId = General.GetParam<int>("MatchId");

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                TKTournament tournament = context.TKTournament.Single(t => t.Id == id);
                TKPlayer player = Session["LoggedInUser"] as TKPlayer;
                var token = player?.Token;
                TKTeamMatch teamMatch = context.TKTeamMatch.SingleOrDefault(tm => tm.Id == matchId);
                GridView gridView;

                if (player != null && (player.IsAdmin || player.IsPlayerOrganizer(tournament.Id)))
                {
                    gridView = PairingsGridView;
                }
                else if (player == null || tournament.TournamentEndDate.AddDays(1) < DateTime.Now)
                {
                    gridView = readOnlyPairingsGridView;
                }
                else
                {
                    TKTournamentPlayer tournamentPlayer = context.TKTournamentPlayer
                        .Include(t => t.TKCodex)
                        .SingleOrDefault(tp => tp.PlayerId == player.Id && tp.TournamentId == tournament.Id);
                    var teamMatches = context.TKTeamMatch.Where(tm => tm.Team1Id == tournamentPlayer.TournamentTeamId || tm.Team2Id == tournamentPlayer.TournamentTeamId).OrderByDescending(tm2 => tm2.Round);
                    teamMatch = teamMatches.FirstOrDefault();

                    gridView = (teamMatch.Team1Id == tournamentPlayer.TournamentTeamId || teamMatch.Team2Id == tournamentPlayer.TournamentTeamId) ? PairingsGridView : readOnlyPairingsGridView;
                }

                PairingsGridView.Visible = PairingsGridView == gridView;
                readOnlyPairingsGridView.Visible = readOnlyPairingsGridView == gridView;

                var data = context.TKGame
                    .Include(t => t.TKTournamentPlayer)
                    .Include(t => t.TKTournamentPlayer1)
                    .Where(g => g.TeamMatchId == teamMatch.Id);

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderBy(d => d.TKTournamentPlayer == null ? "" : d.TKTournamentPlayer.PlayerName);
                        break;
                    case "TableNumber":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.TableNumber) : data
                            .OrderByDescending(d => d.TableNumber);
                        break;
                    case "Player1":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.TKTournamentPlayer == null ? "" : d.TKTournamentPlayer.PlayerName) : data
                            .OrderByDescending(d => d.TKTournamentPlayer == null ? "" : d.TKTournamentPlayer.PlayerName);
                        break;
                    case "Player2":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.TKTournamentPlayer1 == null ? "" : d.TKTournamentPlayer1.PlayerName) : data
                            .OrderByDescending(d => d.TKTournamentPlayer1 == null ? "" : d.TKTournamentPlayer1.PlayerName);
                        break;
                }

                var dataList = data.ToList();
                dataList.ForEach(d => d.Token = token);

                gridView.DataSource = dataList;
                gridView.DataBind();

                team1PenaltyTextBox.Text = teamMatch.Team1Penalty.ToString();
                team2PenaltyTextBox.Text = teamMatch.Team2Penalty.ToString();
                teamMatchIdHidden.Value = teamMatch.Id.ToString();

                gridView.Columns[4].Visible = tournament.UseSecondaryPoints;
                gridView.Columns[7].Visible = tournament.UseSecondaryPoints;

                var t1Players = context.TKTournamentPlayer.Where(p => p.TournamentTeamId == teamMatch.Team1Id).ToList();
                var t2Players = context.TKTournamentPlayer.Where(p => p.TournamentTeamId == teamMatch.Team2Id).ToList();

                swapPlayer1DropDownList.DataSource = t1Players.Concat(t2Players).OrderBy(p => p.PlayerName);
                swapPlayer1DropDownList.DataBind();

                swapPlayer2DropDownList.DataSource = t1Players.Concat(t2Players).OrderBy(p => p.PlayerName);
                swapPlayer2DropDownList.DataBind();
            }
        }

        protected void PairingsGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(sortDirection, e.SortExpression);
        }

        protected void PenaltyButtonClick(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                if (!int.TryParse(teamMatchIdHidden.Value, out int id))
                {
                    return;
                }

                TKTeamMatch teamMatch = context.TKTeamMatch.SingleOrDefault(m => m.Id == id);
                teamMatch.Team1Penalty = string.IsNullOrEmpty(team1PenaltyTextBox.Text) ? null : (int?)int.Parse(team1PenaltyTextBox.Text);
                teamMatch.Team2Penalty = string.IsNullOrEmpty(team2PenaltyTextBox.Text) ? null : (int?)int.Parse(team2PenaltyTextBox.Text);

                context.SaveChanges();

                TKGame game = context.TKGame.FirstOrDefault(g => g.TeamMatchId == teamMatch.Id);

                var player = Session["LoggedInUser"] as TKPlayer;
                var token = player?.Token;

                GameManager gameManager = new GameManager();
                gameManager.Update(game, token);
            }
        }

        protected void SwapClick(object sender, EventArgs e)
        {
            int player1Id = int.Parse(swapPlayer1DropDownList.SelectedItem.Value);
            int player2Id = int.Parse(swapPlayer2DropDownList.SelectedItem.Value);
            int matchId = int.Parse(Request["MatchId"]);

            if (player1Id == player2Id)
            {
                swapErrorLabel.Visible = true;
            }

            var swapManager = new SwapManager();
            var teamMatchManager = new TeamMatchManager();
            TKPlayer player = Session["LoggedInUser"] as TKPlayer;
            var teamMatch = teamMatchManager.Get(matchId);

            string result = swapManager.SwapTeamPlayers(matchId, player1Id, player2Id, teamMatch.Round, player.Token);

            swapErrorLabel.Visible = !string.IsNullOrEmpty(result);
            swapErrorLabel.Text = result;

            BindData(SortDirection.Descending, "Initial");
        }

        protected void Team1PlayersDropDownListDataBound(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player?.Token;
            string gameId = ((HiddenField)((DropDownList)sender).Parent.Parent.FindControl("GameId")).Value;
            ((DropDownList)sender).Attributes.Add("onchange", $"javascript: UpdateFieldCallBackFieldAndResult('/WebAPI/Game/Update', 'Player1Id', {gameId}, this.value, this, '{token}', CheckResultCallBack);");
        }

        protected void Team2PlayersDropDownListDataBound(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player?.Token;
            string gameId = ((HiddenField)((DropDownList)sender).Parent.Parent.FindControl("GameId")).Value;
            ((DropDownList)sender).Attributes.Add("onchange", $"javascript: UpdateFieldCallBackFieldAndResult('/WebAPI/Game/Update', 'Player2Id', {gameId}, this.value, this, '{token}', CheckResultCallBack);");
        }
    }
}