using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Web
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUser"] == null)
            {
                loginHyperLink.NavigateUrl = $@"javascript:OpenModal('#loginModal')";
                loginHyperLink.Text = "Login / Create user";
            }
            else
            {
                TKPlayer player = Session["LoggedInUser"] as TKPlayer;

                loginHyperLink.NavigateUrl = string.Format("/Shared/TKGames.aspx?PlayerId={0}", player.Id);
                loginHyperLink.Text = player.Name;
                logoutHyperLink.NavigateUrl = "/shared/tklogout.aspx";
                logoutHyperLink.Text = "logout";
                logoutHyperLinkPre.Text = " (";
                logoutHyperLinkPost.Text = ")";
            }
        }
    }
}