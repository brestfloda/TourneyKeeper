using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKTeams : TKWebPage
    {
        private TKTournament tournament = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            using (var context = new TourneyKeeperEntities())
            {
                tournament = context.TKTournament.Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);

                TeamSize.Value = tournament.TeamSize.HasValue ? tournament.TeamSize.Value.ToString() : "";

                if (!IsPostBack)
                {
                    LeaderboardGridView.EditIndex = -1;
                    BindData(id);
                }
            }
        }

        protected void LeaderboardGridViewRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveTeam")
            {
                TKPlayer player = Session["LoggedInUser"] as TKPlayer;
                TournamentTeamManager manager = new TournamentTeamManager();
                manager.DeleteTournamentTeams(new List<int>() { int.Parse(e.CommandArgument.ToString()) }, player.Token);

                var id = General.GetParam<int>("Id");
                Response.Redirect(string.Format("/Team/tkteamleaderboard.aspx?Id={0}", id));
            }
        }

        private void BindData(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var player = Session["LoggedInUser"] as TKPlayer;
                var token = player == null ? null : player.Token;

                var editTeamData = context.TKTournamentTeam
                .Where(t => t.TournamentId == tournamentId).ToList()
                .Select(t => new EditTeamData()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Penalty = t.Penalty,
                    BattlePointPenalty = t.BattlePointPenalty,
                    Players = t.TKTournamentPlayer.Where(tp => !tp.NonPlayer).Count(),
                    NonPlayers = t.TKTournamentPlayer.Where(tp => tp.NonPlayer).Count(),
                    Token = token,
                    Paid = t.Paid ?? false
                }).OrderBy(t => t.Name)
                .ThenBy(t => t.Name).ToList();
                LeaderboardGridView.DataSource = editTeamData;
                LeaderboardGridView.DataBind();
            }
        }

        protected void AddTeamButtonClick(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            TKTournamentTeam team = new TKTournamentTeam();
            team.TournamentId = id;
            team.Name = string.IsNullOrEmpty(teamNameTextBox.Text) ? "Team" : teamNameTextBox.Text;

            TKPlayer player = Session["LoggedInUser"] as TKPlayer;
            TournamentTeamManager manager = new TournamentTeamManager();

            manager.AddTournamentTeam(team, player.Token);

            BindData(id);
        }
    }
}