using System;
using System.Diagnostics;
using System.Linq;
using TourneyKeeper.Common;
using System.Net;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKTeamAbout : TKWebPage
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