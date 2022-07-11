using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web.Controls
{
    public partial class YouArePlaying : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public bool BindData()
        {
            if (Session["LoggedInUser"] == null)
            {
                return false;
            }
            else
            {
                var player = Session["LoggedInUser"] as TKPlayer;
                var tournamentManager = new TournamentManager();
                var currentTournaments = tournamentManager.GetCurrentTournaments(player.Id, player.Token).OrderBy(t => t.EndDate);

                YouArePlayingListView.DataSource = currentTournaments;
                YouArePlayingListView.DataBind();

                return currentTournaments.Any();
            }
        }
    }
}