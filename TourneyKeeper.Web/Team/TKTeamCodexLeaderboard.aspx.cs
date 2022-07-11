using System;
using System.Web.UI.HtmlControls;

namespace TourneyKeeper.Web
{
    public partial class TKTeamCodexLeaderboard : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var keywords = new HtmlMeta { Name = "description", Content = "The Team Codex Leaderboard shows you the best scoring codices" };
            Header.Controls.Add(keywords);
        }
    }
}