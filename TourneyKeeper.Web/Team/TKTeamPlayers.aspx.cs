using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web
{
    public partial class TKTeamPlayers : TKWebPage
    {
        public string FromSubmit = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Team Players";

            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            if (!IsPostBack)
            {
                TournamentPlayersGridView.EditIndex = -1;
                BindData(id);
            }
        }

        protected void TournamentsDataSource_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            e.ObjectInstance = new TourneyKeeperEntities();
        }

        protected void TournamentPlayersGridViewRowCommand(object sender, GridViewCommandEventArgs e)
        {
            FromSubmit = "submit";

            if (e.CommandName == "RemovePlayer")
            {
                var pid = int.Parse(e.CommandArgument.ToString());
                var tournamentPlayerManager = new TournamentPlayerManager();
                tournamentPlayerManager.DeletePlayer(pid);
                //TODO: besked hvis spilleren ikke kunne slettes

                var id = General.GetParam<int>("Id");
                Response.Redirect(string.Format("/team/tkteamplayers.aspx?Id={0}", id));
            }
        }

        private void BindData(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var data = context.TKTournamentPlayer
                    .Include(t => t.TKCodex)
                    .Include(tp => tp.TKTournament)
                    .Where(tp => tp.TournamentId == tournamentId).ToList()
                    .Select(tp => new EditPlayerData()
                    {
                        Id = tp.Id,
                        PlayerId = tp.PlayerId,
                        PlayerName = tp.PlayerName,
                        ArmyList = tp.TKCodex == null ? "Blank" : tp.TKCodex.Name.Replace("<", "").Replace(">", ""),
                        Penalty = tp.Penalty,
                        FairPlay = tp.FairPlay,
                        Quiz = tp.Quiz,
                        Painting = tp.Painting,
                        Paid = tp.Paid,
                        DoNotRank = tp.DoNotRank,
                        Club = tp.Club,
                        TournamentTeamId = tp.TKTournamentTeam?.Id,
                        NonPlayer = tp.NonPlayer,
                        GameSystemId = tp.TKTournament.GameSystemId,
                        TeamName = tp.TeamName
                    }).OrderBy(tp => tp.TeamName)
                    .ThenBy(tp => tp.PlayerName);

                var player = Session["LoggedInUser"] as TKPlayer;
                var token = player?.Token;

                var dataList = data.ToList();
                dataList.ForEach(d => d.Token = token);

                TournamentPlayersGridView.DataSource = dataList;
                TournamentPlayersGridView.DataBind();

                var activePlayers = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournamentId && tp.Active).Count();
                SearchPlayer.SetNumPlayers(activePlayers.ToString());
            }
        }

        protected void TournamentPlayersGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var tournamentPlayer = e.Row.DataItem as EditPlayerData;

                    var teams = context.TKTournamentTeam.Where(tp => tp.TournamentId == id).OrderBy(t => t.Name);

                    var teamsDropDownList = e.Row.FindControl("teamDropDownList") as DropDownList;
                    if (teamsDropDownList == null)
                    {
                        return;
                    }
                    teamsDropDownList.DataSource = teams.ToList();
                    teamsDropDownList.DataBind();

                    if (tournamentPlayer.TournamentTeamId != null)
                    {
                        teamsDropDownList.SelectedValue = tournamentPlayer.TournamentTeamId.ToString();
                    }
                }
            }
        }

        protected void TeamDropDownListDataBound(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var token = player?.Token;
            string playerId = ((HiddenField)((DropDownList)sender).Parent.Parent.FindControl("PlayerId")).Value;
            ((DropDownList)sender).Attributes.Add("onchange", $"javascript: UpdateField('/WebAPI/TournamentPlayer/Update', 'TournamentTeamId', {playerId}, this.value, this, '{token}');");
        }
    }
}