using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web
{
    public partial class TeamPairings : TKWebPage
    {
        private bool showPenalty;
        private int? currentRound;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Team Pairings";

            var id = General.GetParam<int>("Id");
            using (var context = new TourneyKeeperEntities())
            {
                var tournament = context.TKTournament.Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);

                currentRound = context.TKGame.Where(g => g.TournamentId == id).Select(g => (int?)g.Round).Max();
                if (!currentRound.HasValue)
                {
                    return;
                }
            }

            if (!IsPostBack)
            {
                RoundDropDown.DataSource = Enumerable.Range(1, currentRound.Value).Reverse();
                RoundDropDown.DataBind();

                BindData(id, currentRound, SortDirection.Descending, "Initial");
            }
        }

        protected void PairingsGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();
            int round = int.Parse(RoundDropDown.SelectedValue);

            BindData(id, round, sortDirection, e.SortExpression);
        }

        protected void PairingsGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = showPenalty;
            e.Row.Cells[4].Visible = showPenalty;
        }

        protected void PairingsGridViewInit(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var id = General.GetParam<int>("Id");
                var user = Session["LoggedInUser"] as TKPlayer;

                TKTournament tournament = context.TKTournament.Single(t => t.Id == id);

                showPenalty = context.TKTeamMatch.Where(tp => tp.TournamentId == tournament.Id).Any(t => (t.Team1Penalty != null || t.Team2Penalty != null) && (t.Team1Penalty != 0 || t.Team2Penalty != 0));
            }
        }

        protected void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            int round = int.Parse(RoundDropDown.SelectedValue);

            BindData(id, round, SortDirection.Descending, "Initial");
        }

        private void BindData(int tournamentId, int? round, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var tournament = context.TKTournament.Include(t => t.TKTeamScoringSystem).Single(t => t.Id == tournamentId);
                var isBattlefront = tournament.TKTeamScoringSystem.Name.Equals("battlefront scoring", StringComparison.InvariantCultureIgnoreCase);
                var isBattlepoints = tournament.TKTeamScoringSystem.Name.Equals("battle points", StringComparison.InvariantCultureIgnoreCase);
                var user = Session["LoggedInUser"] as TKPlayer;
                var userId = user?.Id ?? 0;
                var isAdmin = General.IsAdminOrOrganizer(Session["LoggedInUser"] as TKPlayer, tournament.Id);
                var tournamentTeamId = isAdmin ? null : context.TKTournamentPlayer.SingleOrDefault(tp => tp.PlayerId == userId && tp.TournamentId == tournament.Id)?.TournamentTeamId;

                var data = context.TKTeamMatch
                    .Include(t => t.TKTournamentTeam)
                    .Include(t => t.TKTournamentTeam1)
                    .Where(g => g.TournamentId == tournamentId && g.Round == (round.HasValue ? round.Value : 1))
                    .ToList()
                    .Select(g => new TeamPairingsData()
                    {
                        TournamentId = g.TournamentId,
                        Team1Id = g.Team1Id,
                        Team2Id = g.Team2Id,
                        MatchId = g.Id,
                        TableNumber = g.TableNumber,
                        Team1Name = g.TKTournamentTeam.Name,
                        Team2Name = g.TKTournamentTeam1.Name,
                        Team1Points = g.Team1MatchPoints,
                        Team2Points = g.Team2MatchPoints,
                        Team1Penalty = g.Team1Penalty,
                        Team2Penalty = g.Team2Penalty,
                        Team1BattlePoints = isBattlefront ? g.Team1SecondaryPoints : g.Team1Points,
                        Team2BattlePoints = isBattlefront ? g.Team2SecondaryPoints : g.Team2Points,
                        AllowSetup = isAdmin ||
                            (round == currentRound && (tournamentTeamId == g.Team1Id || tournamentTeamId == g.Team2Id)),
                        FormattedPoints = g.Team1Points == 0 && g.Team2Points == 0 ? "" :
                            isBattlepoints ? $"{g.Team1Points} - {g.Team2Points}" :
                            isBattlefront ? $"({g.Team1SecondaryPoints}) {g.Team1Points} - {g.Team2Points} ({g.Team2SecondaryPoints})" :
                            $"({g.Team1Points}) {g.Team1MatchPoints} - {g.Team2MatchPoints} ({g.Team2Points})"
                    });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderBy(d => d.TableNumber);
                        break;
                    case "Table":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.TableNumber) : data
                            .OrderByDescending(d => d.TableNumber);
                        break;
                    case "Team1":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.Team1Name) : data
                            .OrderByDescending(d => d.Team1Name);
                        break;
                    case "Team2":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(d => d.Team2Name) : data
                            .OrderByDescending(d => d.Team2Name);
                        break;
                }

                PairingsGridView.DataSource = data;
                PairingsGridView.DataBind();
            }
        }

        protected void CreatePairingsClick(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            Response.Redirect("/Team/TKTeamCreatePairings.aspx?id=" + id);
        }
    }
}