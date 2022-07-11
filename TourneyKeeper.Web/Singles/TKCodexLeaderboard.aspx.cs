using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using TourneyKeeper.Common.SharedCode;
using TourneyKeeper.Web.Controls;

namespace TourneyKeeper.Web
{
    public partial class TKCodexLeaderboard : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var keywords = new HtmlMeta { Name = "description", Content = "The Team Codex Leaderboard shows you the best scoring codices" };
            Header.Controls.Add(keywords);
        }
    }
}