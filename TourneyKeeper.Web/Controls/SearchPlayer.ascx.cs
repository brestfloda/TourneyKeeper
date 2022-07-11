using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TourneyKeeper.Web.Controls
{
    public partial class SearchPlayer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetNumPlayers(string text)
        {
            numPlayersLabel.Text = text;
        }
    }
}