using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Exceptions;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.Managers.TableGenerators;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class CreatePairings : TKWebPage
    {
        private TKTournament tournament = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Create Pairings";

            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            swapErrorLabel.Visible = false;
            warningsLabel.Text = "";

            using (var context = new TourneyKeeperEntities())
            {
                tournament = context.TKTournament.Single(t => t.Id == id);

                var isSelected = int.TryParse(RoundHiddenField.Value, out int selectedRound);
                int? round = isSelected ? selectedRound : context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                if (!round.HasValue)
                {
                    return;
                }

                PairingsGridView.EditIndex = -1;
                BindData(id, context, round.Value);
            }
        }

        private void BindData(int tournamentId, TourneyKeeperEntities context, int roundVal)
        {
            var tournament = context.TKTournament.Single(t => t.Id == tournamentId);

            swapPlayer1DropDownList.DataSource = context.TKTournamentPlayer.Where(p => p.TournamentId == tournamentId && p.Active).OrderBy(p2 => p2.PlayerName).ToList();
            swapPlayer1DropDownList.DataBind();

            swapPlayer2DropDownList.DataSource = context.TKTournamentPlayer.Where(p => p.TournamentId == tournamentId && p.Active).OrderBy(p2 => p2.PlayerName).ToList();
            swapPlayer2DropDownList.DataBind();

            var pairingsData = context.TKGame.Where(g => g.TournamentId == tournamentId && g.Round == roundVal).ToList();

            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player?.Token;
            var dataList = pairingsData.ToList();
            dataList.ForEach(d => d.Token = token);

            PairingsGridView.DataSource = dataList;
            PairingsGridView.DataBind();

            PairingsGridView.Columns[4].Visible = tournament.UseSecondaryPoints;
            PairingsGridView.Columns[7].Visible = tournament.UseSecondaryPoints;

            RoundHiddenField.Value = roundVal.ToString();
        }

        protected void SwapClick(object sender, EventArgs e)
        {
            try
            {
                int player1Id = int.Parse(swapPlayer1DropDownList.SelectedItem.Value);
                int player2Id = int.Parse(swapPlayer2DropDownList.SelectedItem.Value);

                if (player1Id == player2Id)
                {
                    swapErrorLabel.Visible = true;
                }

                var swapManager = new SwapManager();
                var player = Session["LoggedInUser"] as TKPlayer;

                string result = swapManager.SwapPlayers(player1Id, player2Id, int.Parse(RoundHiddenField.Value), player.Token);

                swapErrorLabel.Visible = !string.IsNullOrEmpty(result);
                swapErrorLabel.Text = result;

                using (var context = new TourneyKeeperEntities())
                {
                    var id = General.GetParam<int>("Id");
                    BindData(id, context, int.Parse(RoundHiddenField.Value));
                }
            }
            catch
            {
                swapErrorLabel.Visible = true;
                swapErrorLabel.Text = "Not possible to swap at this time";
            }
        }

        protected void CreateRandomPairingsClick(object sender, EventArgs e)
        {
            var options = OptionsHiddenField.Value.Split(',').Where(o => o != "None").Select(o => (PairingOption)Enum.Parse(typeof(PairingOption), o));
            var tableGenerator = (TableGenerator)Enum.Parse(typeof(TableGenerator), TableGenerationHiddenField.Value);
            Draw(DrawTypeEnum.RandomDraw, tableGenerator, options);
        }

        protected void CreateSwissPairingsClick(object sender, EventArgs e)
        {
            var options = OptionsHiddenField.Value.Split(',').Where(o => o != "None").Select(o => (PairingOption)Enum.Parse(typeof(PairingOption), o));
            var tableGenerator = (TableGenerator)Enum.Parse(typeof(TableGenerator), TableGenerationHiddenField.Value);
            Draw(DrawTypeEnum.SwissDraw, tableGenerator, options);
        }

        protected void DeleteLastRoundClick(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;

            var tournamentManager = new TournamentManager();
            tournamentManager.DeleteLastRound(id, player.Token);

            Response.Redirect(string.Format("/singles/tkcreatepairings.aspx?id={0}", id));
        }

        private void Draw(DrawTypeEnum drawType, TableGenerator tableGenerator, IEnumerable<PairingOption> options)
        {
            var id = General.GetParam<int>("Id");
            var pairingManager = new PairingManager(tableGenerator);

            try
            {
                IEnumerable<string> warnings = pairingManager.SinglesDraw(id, drawType, options, ((TKPlayer)Session["LoggedInUser"]).Token);
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
                int? round = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                if (!round.HasValue)
                {
                    return;
                }

                BindData(id, context, round.Value);
            }
        }

        protected void PairingsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var id = General.GetParam<int>("Id");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var game = e.Row.DataItem as TKGame;

                    var players = context.TKTournamentPlayer.Where(tp => tp.TournamentId == id && tp.Active).OrderBy(tp => tp.PlayerName).ToList();

                    var players1DropDownList = (DropDownList)e.Row.FindControl("players1DropDownList");
                    players1DropDownList.DataSource = players;
                    players1DropDownList.DataBind();
                    var players2DropDownList = (DropDownList)e.Row.FindControl("players2DropDownList");
                    players2DropDownList.DataSource = players;
                    players2DropDownList.DataBind();

                    if (game.TKTournamentPlayer != null)
                    {
                        players1DropDownList.SelectedValue = game.TKTournamentPlayer.Id.ToString();
                    }
                    if (game.TKTournamentPlayer1 != null)
                    {
                        players2DropDownList.SelectedValue = game.TKTournamentPlayer1.Id.ToString();
                    }
                }
            }
        }

        protected void Player1DropDownListDataBound(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player?.Token;
            var gameId = ((HiddenField)((DropDownList)sender).Parent.Parent.FindControl("GameId")).Value;
            ((DropDownList)sender).Attributes.Add("onchange", $"javascript: UpdateField('/WebAPI/Game/Update', 'Player1Id', {gameId}, this.value, this, '{token}');");
        }

        protected void Player2DropDownListDataBound(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player?.Token;
            var gameId = ((HiddenField)((DropDownList)sender).Parent.Parent.FindControl("GameId")).Value;
            ((DropDownList)sender).Attributes.Add("onchange", $"javascript: UpdateField('/WebAPI/Game/Update', 'Player2Id', {gameId}, this.value, this, '{token}');");
        }

        protected void AddLatecomersLinkButtonClick(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            var pairingManager = new PairingManager();

            try
            {
                warningsLabel.Text = "";
                pairingManager.AddLateComers(id, player.Token);
                using (var context = new TourneyKeeperEntities())
                {
                    BindData(id, context, int.Parse(RoundHiddenField.Value));
                }
            }
            catch (Exception ex)
            {
                warningsLabel.Text = ex.Message;
            }
        }
    }
}