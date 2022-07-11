using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKSinglesOrganizers : TKWebPage
    {
        public string FromSubmit = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            if (!IsPostBack)
            {
                OrganizerGridView.EditIndex = -1;
                BindData(id);
            }
        }

        protected void TournamentsDataSource_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            e.ObjectInstance = new TourneyKeeperEntities();
        }

        protected void OrganizerGridViewRowCommand(object sender, GridViewCommandEventArgs e)
        {
            FromSubmit = "submit";

            if (e.CommandName == "RemoveOrganizer")
            {
                var id = General.GetParam<int>("Id");
                using (var context = new TourneyKeeperEntities())
                {
                    var pid = int.Parse(e.CommandArgument.ToString());

                    var organizer = context.TKOrganizer.SingleOrDefault(t => t.PlayerId == pid && t.TournamentId == id);

                    context.TKOrganizer.Remove(organizer);
                    context.SaveChanges();
                }

                Response.Redirect(string.Format("/Singles/TKSinglesOrganizers.aspx?Id={0}", id));
            }
        }

        private void BindData(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                IEnumerable<OrganizerData> data = context.TKOrganizer
                    .Where(o => o.TournamentId == tournamentId).ToList()
                    .Select(o => new OrganizerData()
                    {
                        Id = o.PlayerId,
                        OrganizerName = o.TKPlayer.Name
                    })
                    .OrderBy(tp => tp.OrganizerName);

                OrganizerGridView.DataSource = data;
                OrganizerGridView.DataBind();
            }
        }
    }
}