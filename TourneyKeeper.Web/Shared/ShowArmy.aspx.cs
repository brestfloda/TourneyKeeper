using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class ShowArmy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["id"], out int id))
            {
                return;
            }

            using (var context = new TourneyKeeperEntities())
            {
                var tournamentPlayer = context.TKTournamentPlayer.Single(p => p.Id.Equals(id));
                var tournament = context.TKTournament.Single(t => t.Id == tournamentPlayer.TournamentId);
                var showArmy = tournament.ShowListsDate.HasValue ? tournament.ShowListsDate < DateTime.Now :
                    tournament.TournamentDate < DateTime.Now || General.IsAdminOrOrganizer(Session["LoggedInUser"] as TKPlayer, tournament.Id);
                if (showArmy)
                {
                    var tmp = WebUtility.HtmlEncode(tournamentPlayer.ArmyList == null ? "" : tournamentPlayer.ArmyList.Replace("\r\n", "<br/>").Replace("\n", "<br/>"));
                    ArmylistLabel.Text =  tmp?.Replace("&lt;br/&gt;", "<br/>");
                }
            }
        }
    }
}