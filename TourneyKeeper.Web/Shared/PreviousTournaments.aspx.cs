using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;

namespace TourneyKeeper.Web.Shared
{
    public partial class PreviousTournaments : System.Web.UI.Page
    {
        private string selectedCountry = null;
        private int? selectedGameSystem = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                selectedCountry = Request.Cookies["TKCountrySelectionPrevious"]?["Country"];
                selectedGameSystem = int.TryParse(Request.Cookies["TKGameSystemSelectionPrevious"]?["GameSystem"], out int tmpGS) ? new int?(tmpGS) : 0;
                BindData(SortDirection.Descending, "Initial");
            }
            else
            {
                string selectedCookieCountry = Request.Cookies["TKCountrySelectionPrevious"]?["Country"];
                var ddlCountry = FinishedTournamentsGridView.HeaderRow.Cells[1].FindControl("ddCountry") as DropDownList;
                selectedCountry = ddlCountry.SelectedValue;

                string selectedCookieGameSystem = Request.Cookies["TKGameSystemSelectionPrevious"]?["GameSystem"];
                var ddlGameSystem = FinishedTournamentsGridView.HeaderRow.Cells[4].FindControl("ddGameSystem") as DropDownList;
                int tmpGS;
                selectedGameSystem = int.TryParse(ddlGameSystem.SelectedValue, out tmpGS) ? new int?(tmpGS) : null;

                if (!ddlCountry.SelectedValue.Equals(selectedCookieCountry, StringComparison.InvariantCultureIgnoreCase))
                {
                    HttpCookie loginCookie = new HttpCookie("TKCountrySelectionPrevious");
                    loginCookie["Country"] = ddlCountry.SelectedValue;
                    loginCookie.Expires = DateTime.Now.AddDays(1000);
                    Response.Cookies.Add(loginCookie);
                    BindData(SortDirection.Descending, "Initial");
                }
                else if (!ddlGameSystem.SelectedValue.Equals(selectedCookieGameSystem, StringComparison.InvariantCultureIgnoreCase))
                {
                    HttpCookie loginCookie = new HttpCookie("TKGameSystemSelectionPrevious");
                    loginCookie["GameSystem"] = ddlGameSystem.SelectedValue;
                    loginCookie.Expires = DateTime.Now.AddDays(1000);
                    Response.Cookies.Add(loginCookie);
                    BindData(SortDirection.Descending, "Initial");
                }
            }
        }

        private void BindData(SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var data = context.TKTournament
                    .Include(t => t.TKGameSystem)
                    .Where(t => t.Active && t.TournamentEndDate < DateTime.Now &&
                    (selectedCountry == null || selectedCountry.Equals("All") || t.Country.Equals(selectedCountry)) &&
                    (!selectedGameSystem.HasValue || selectedGameSystem == 0 || t.GameSystemId == selectedGameSystem))
                    .ToList();

                if (!data.Any())
                {
                    data = context.TKTournament
                    .Where(t => t.Active && t.TournamentEndDate < DateTime.Now)
                    .ToList();
                }

                var player = Session["LoggedInUser"] as TKPlayer;
                if (player != null)
                {
                    data = data.Where(d => !d.NationalTournament || (d.NationalTournament && player.Country == d.Country)).ToList();
                }

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderByDescending(t => t.TournamentDate)
                            .ToList();
                        break;
                    case "Name":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(t => t.Name)
                            .ToList() : data
                            .OrderByDescending(t => t.Name)
                            .ToList();
                        break;
                    case "Country":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(t => t.Country)
                            .ToList() : data
                            .OrderByDescending(t => t.Country)
                            .ToList();
                        break;
                    case "TournamentDate":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(t => t.TournamentDate)
                            .ToList() : data
                            .OrderByDescending(t => t.TournamentDate).ToList();
                        break;
                    case "TournamentType":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(t => t.TournamentType)
                            .ToList() : data
                            .OrderByDescending(t => t.TournamentType)
                            .ToList();
                        break;
                    case "GameSystem":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(t => t.TKGameSystem.Name)
                            .ToList() : data
                            .OrderByDescending(t => t.TKGameSystem.Name)
                            .ToList();
                        break;
                };

                FinishedTournamentsGridView.DataSource = data;
                FinishedTournamentsGridView.DataBind();
            }
        }

        protected void TournamentsDataSource_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            e.ObjectInstance = new TourneyKeeperEntities();
        }

        protected void FinishedTournamentsGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(sortDirection, e.SortExpression);
        }

        protected void CurrentTournamentsGridView_DataBound(object sender, EventArgs e)
        {
            if (FinishedTournamentsGridView.HeaderRow != null)
            {
                TableCellCollection cells = FinishedTournamentsGridView.HeaderRow.Cells;

                var ddlCountry = cells[1].FindControl("ddCountry") as DropDownList;
                ddlCountry.DataSource = new List<string> { "All" }.Concat(Country.Countries);
                ddlCountry.SelectedValue = selectedCountry;
                ddlCountry.DataBind();

                using (var context = new TourneyKeeperEntities())
                {
                    var gameSystems = new List<TKGameSystem> { new TKGameSystem { Id = 0, Name = "All" } }.Concat(context.TKGameSystem.OrderBy(g => g.Name)).ToList();
                    var ddlGameSystem = cells[4].FindControl("ddGameSystem") as DropDownList;
                    ddlGameSystem.DataSource = gameSystems;
                    ddlGameSystem.SelectedValue = selectedGameSystem.ToString();
                    ddlGameSystem.DataBind();
                }
            }
        }
    }
}