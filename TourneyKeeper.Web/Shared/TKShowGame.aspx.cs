using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;

namespace TourneyKeeper.Web
{
    public partial class TKShowGame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = int.Parse(Request["GameId"]);
            using (var context = new TourneyKeeperEntities())
            {
                var game = context.TKGame.Single(g => g.Id == id);

                Player1Name.Text = game.TKTournamentPlayer.PlayerName;
                Player1Name.NavigateUrl = string.Format("/Shared/TKGames.aspx?PlayerId={0}", game.TKTournamentPlayer.PlayerId);
                Player2Name.Text = game.TKTournamentPlayer1.PlayerName;
                Player2Name.NavigateUrl = string.Format("/Shared/TKGames.aspx?PlayerId={0}", game.TKTournamentPlayer1.PlayerId);
                Player1Points.Text = game.Player1Result.ToString();
                Player2Points.Text = game.Player2Result.ToString();
                Player1SecondaryPoints.Text = game.Player1SecondaryResult.ToString();
                Player2SecondaryPoints.Text = game.Player2SecondaryResult.ToString();
                var tmp1 = WebUtility.HtmlEncode(game.TKTournamentPlayer.ArmyList == null ? "" : game.TKTournamentPlayer.ArmyList.Replace("\r\n", "<br/>").Replace("\n", "<br/>"));
                Player1Armylist.Text = tmp1?.Replace("&lt;br/&gt;", "<br/>");
                var tmp2 = WebUtility.HtmlEncode(game.TKTournamentPlayer1.ArmyList == null ? "" : game.TKTournamentPlayer1.ArmyList.Replace("\r\n", "<br/>").Replace("\n", "<br/>"));
                Player2Armylist.Text = tmp2?.Replace("&lt;br/&gt;", "<br/>");
                Player1PrimaryCodex.Text = game.TKTournamentPlayer.TKCodex == null ? "" : game.TKTournamentPlayer.TKCodex.Name;
                Player2PrimaryCodex.Text = game.TKTournamentPlayer1.TKCodex == null ? "" : game.TKTournamentPlayer1.TKCodex.Name;
                Player1SecondaryCodex.Text = game.TKTournamentPlayer.TKCodex1 == null ? "" : game.TKTournamentPlayer.TKCodex1.Name;
                Player2SecondaryCodex.Text = game.TKTournamentPlayer1.TKCodex1 == null ? "" : game.TKTournamentPlayer1.TKCodex1.Name;
                Player1TertiaryCodex.Text = game.TKTournamentPlayer.TKCodex2 == null ? "" : game.TKTournamentPlayer.TKCodex2.Name;
                Player2TertiaryCodex.Text = game.TKTournamentPlayer1.TKCodex2 == null ? "" : game.TKTournamentPlayer1.TKCodex2.Name;
                Player1QuaternaryCodex.Text = game.TKTournamentPlayer.TKCodex3 == null ? "" : game.TKTournamentPlayer.TKCodex3.Name;
                Player2QuaternaryCodex.Text = game.TKTournamentPlayer1.TKCodex3 == null ? "" : game.TKTournamentPlayer1.TKCodex3.Name;
            }
        }
    }
}