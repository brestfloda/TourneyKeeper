using System;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;

namespace TourneyKeeper.Web
{
    public partial class TKMyTournaments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = int.Parse(Request["PlayerId"]);

            if (!IsPostBack)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    TKPlayer player = Session["LoggedInUser"] as TKPlayer;
                    if (player == null)
                    {
                        Response.Redirect("/");
                    }

                    if (player.IsAdmin)
                    {
                        var tournaments = context.TKTournament
                            .OrderByDescending(t => t.TournamentDate)
                            .ToList()
                            .Select(d => new
                            {
                                Id = d.Id,
                                Name = d.Name,
                                Country = d.Country,
                                TournamentDate = d.TournamentDate,
                                TournamentTypeName = d.TournamentType.ToString(),
                                TKGameSystemName = d.TKGameSystem.Name,
                                PlayerId = id,
                            });

                        myTournamentsGridView.DataSource = tournaments;
                        myTournamentsGridView.DataBind();
                    }
                    else
                    {
                        if (id != player.Id)
                        {
                            Response.Redirect("/");
                        }

                        var tournaments = context.TKTournament
                            .Join(context.TKOrganizer, t => t.Id, o => o.TournamentId, (t, o) => new { Tournament = t, Organizer = o })
                            .Where(d => d.Organizer.PlayerId == id)
                            .OrderByDescending(t => t.Tournament.TournamentDate)
                            .ToList()
                            .Select(d => new
                            {
                                Id = d.Tournament.Id,
                                Name = d.Tournament.Name,
                                Country = d.Tournament.Country,
                                TournamentDate = d.Tournament.TournamentDate,
                                TournamentTypeName = d.Tournament.TournamentType.ToString(),
                                TKGameSystemName = d.Tournament.TKGameSystem.Name,
                                PlayerId = id,
                            });

                        myTournamentsGridView.DataSource = tournaments;
                        myTournamentsGridView.DataBind();
                    }
                }
            }
        }
    }
}