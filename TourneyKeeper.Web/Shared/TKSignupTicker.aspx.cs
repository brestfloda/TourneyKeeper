using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKSignupTicker : System.Web.UI.Page
    {
        public string startDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            signupLink.NavigateUrl = string.Format("/Shared/TKSignup.aspx?id={0}", id);

            using (var context = new TourneyKeeperEntities())
            {
                TKTournament tournament = context.TKTournament.Single(t => t.Id == id);

                startDate = tournament.OnlineSignupStart.HasValue? tournament.OnlineSignupStart.Value.ToString("MM/dd/yyyy HH:mm:ss") :"";
            }
        }
    }
}