using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using System.Data.Entity;
using System.Diagnostics;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Web
{
    public partial class Default : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var keywords = new HtmlMeta { Name = "description", Content = "TourneyKeeper is a free online tool for organizing tabletop tournaments and supports games such as Warhammer 40K, Age of Sigmar, 9th Age and Flames of War. TourneyKeeper is rock solid and free to use." };
            Header.Controls.Add(keywords);

            if(YouArePlaying.BindData())
            {
                youArePlayingPlaceHolder.Visible = true;
                playingNowLiteral.Text = @"<div class=""col-lg-4"">";
                upcomingTournamentsLiteral.Text = @"<div class=""col-lg-4"">";
            }
            else
            {
                youArePlayingPlaceHolder.Visible = false;
                playingNowLiteral.Text = @"<div class=""col-lg-6"">";
                upcomingTournamentsLiteral.Text = @"<div class=""col-lg-6"">";
            }
        }
    }
}