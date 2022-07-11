using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Web
{
    public class TKWebPage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (HttpContext.Current.Request.Cookies["TKUser"] != null && HttpContext.Current.Session["LoggedInUser"] == null)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var playerManager = new PlayerManager();
                    int pid = int.Parse(HttpContext.Current.Request.Cookies["TKUser"]["Id"]);
                    var player = context.TKPlayer.Single(p => p.Id == pid);
                    player.TournamentIds = playerManager.GetTournamentIds(player.Id);
                    player.OrganizerForTournamentIds = playerManager.GetOrganizerTournamentIds(player.Id);

                    HttpContext.Current.Session["LoggedInUser"] = player;
                }
            }
        }
    }
}