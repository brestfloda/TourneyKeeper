using System;
using TourneyKeeper.Common;

namespace TourneyKeeper.Web
{
    public partial class SharedMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var playerId = Request["PlayerId"];
            var player = Session["LoggedInUser"] as TKPlayer;

            gamesHyperLink.NavigateUrl = string.Format("/Shared/TKGames.aspx?PlayerId={0}", playerId);
            armylistsHyperLink.NavigateUrl = string.Format("/Shared/TKArmylists.aspx?PlayerId={0}", playerId);
            if (player != null && playerId != null && player.Id == int.Parse(playerId))
            {
                settingsHyperLink.NavigateUrl = string.Format("/Shared/TKSettings.aspx?PlayerId={0}", playerId);
                myTournamentsHyperLink.NavigateUrl = string.Format("/Shared/TKMyTournaments.aspx?PlayerId={0}", playerId);
                createTournamentHyperLink.NavigateUrl = string.Format("/Shared/TKCreateTournament.aspx?PlayerId={0}", playerId);
                if (player.IsAdmin)
                {
                    adminHyperLink.NavigateUrl = string.Format("/Shared/TKAdmin.aspx?PlayerId={0}", playerId);
                }
                else
                {
                    adminHyperLink.Visible = false;
                }
            }
            else
            {
                settingsHyperLink.Visible = false;
                myTournamentsHyperLink.Visible = false;
                createTournamentHyperLink.Visible = false;
                adminHyperLink.Visible = false;
            }
        }
    }
}