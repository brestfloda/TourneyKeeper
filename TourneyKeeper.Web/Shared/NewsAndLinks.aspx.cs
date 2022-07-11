using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;

namespace TourneyKeeper.Web.Shared
{
    public partial class NewsAndLinks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void TournamentsDataSource_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            e.ObjectInstance = new TourneyKeeperEntities();
        }
    }
}