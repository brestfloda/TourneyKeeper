using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web.Controls
{
    public partial class GamesOverview : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, Page);

            if (!IsPostBack)
            {
                BindData(id, SortDirection.Descending, "Initial");
            }
        }

        protected void GamesGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(id, sortDirection, e.SortExpression);
        }

        private void BindData(int id, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var tournament = context.TKTournament.Single(t => t.Id == id);

                var data = context.TKGame
                    .Where(g => g.TournamentId == id);

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderByDescending(tp => tp.Round)
                            .ThenBy(tp => tp.TableNumber);
                        break;
                    case "Round":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Round) : data
                            .OrderByDescending(tp => tp.Round);
                        break;
                    case "TableNumber":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TableNumber) : data
                            .OrderByDescending(tp => tp.TableNumber);
                        break;
                    case "Player1":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TKTournamentPlayer.PlayerName) : data
                            .OrderByDescending(tp => tp.TKTournamentPlayer.PlayerName);
                        break;
                    case "Player2":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TKTournamentPlayer1.PlayerName) : data
                            .OrderByDescending(tp => tp.TKTournamentPlayer1.PlayerName);
                        break;
                    case "Result":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Player1Result) : data
                            .OrderByDescending(tp => tp.Player1Result);
                        break;
                    case "LastEdited":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.LastEdited) : data
                            .OrderByDescending(tp => tp.LastEdited);
                        break;
                    case "LastEditedBy":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.LastEditedBy) : data
                            .OrderByDescending(tp => tp.LastEditedBy);
                        break;
                }

                GamesGridView.DataSource = data.ToList();
                GamesGridView.DataBind();
            }
        }
    }
}