using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKAdmin : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    int playerId = int.Parse(Request["PlayerId"]);
                    var loggedInPlayer = Session["LoggedInUser"] as TKPlayer;
                    if (loggedInPlayer == null)
                    {
                        Response.Redirect("/");
                    }

                    if (playerId != loggedInPlayer.Id)
                    {
                        Response.Redirect("/");
                    }
                }
            }
        }

        protected void CalculateRankingClick(object sender, EventArgs e)
        {
            var calculator = new RankingCalculator();
            calculator.DoRankings();
        }

        protected void WLD1Click(object sender, EventArgs e)
        {
            var calculator = new RankingCalculator();
            calculator.WLD1();
        }

        protected void WLD2Click(object sender, EventArgs e)
        {
            var calculator = new RankingCalculator();
            calculator.WLD2                                 ();
        }
    }
}