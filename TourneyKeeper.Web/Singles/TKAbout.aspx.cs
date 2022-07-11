using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using TourneyKeeper.DTO.Web;
using System.Net;

namespace TourneyKeeper.Web
{
    public partial class TKAbout : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - About";

            var id = General.GetParam<int>("Id");
            BindData(id);
        }

        private void BindData(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var tournament = context.TKTournament.Single(t => t.Id == id);

                descriptionLiteral.Text = WebUtility.HtmlDecode(tournament.Description);
            }
        }
    }
}