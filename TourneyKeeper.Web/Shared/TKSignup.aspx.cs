using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web
{
    public partial class TKSignup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            if (player == null)
            {
                Response.Redirect("/");
            }

            TKTournament tournament;
            using (var context = new TourneyKeeperEntities())
            {
                tournament = context.TKTournament.Single(t => t.Id == id);

                if (!IsPostBack)
                {
                    if (tournament.OnlineSignup && tournament.OnlineSignupStart > DateTime.Now)
                    {
                        Response.Redirect($"/Shared/TKSignupTicker.aspx?id={id}", true);
                        Response.Flush();
                    }

                    if (tournament.TournamentType == TournamentType.Team)
                    {
                        signupRankedButton.Visible = false;
                        signupUnrankedButton.Text = "Sign up";
                        signupUnrankedButton.Enabled = false;
                        signupNonPlayerButton.Visible = true;
                        signupNonPlayerButton.Enabled = false;
                        TeamPlaceHolder.Visible = true;
                        TeamDropDownList.DataSource = (new TKTournamentTeam[] { new TKTournamentTeam { Id = -1, Name = "Select your team" } }.Concat(context.TKTournamentTeam.Where(t => t.TournamentId == id).OrderBy(c => c.Name))).ToList();
                        TeamDropDownList.DataBind();
                    }

                    var pdata = new TKCodex[] { new TKCodex { Name = "Select your primary codex" } }.Concat(context.TKCodex.Where(c => c.Active.Value && c.GameSystemId == tournament.GameSystemId).OrderBy(c => c.Name)).ToList();
                    PrimaryCodexDropDownList.DataSource = pdata;
                    PrimaryCodexDropDownList.DataBind();

                    var sdata = new TKCodex[] { new TKCodex { Name = "Select your secondary codex" } }.Concat(context.TKCodex.Where(c => c.Active.Value && c.GameSystemId == tournament.GameSystemId).OrderBy(c => c.Name)).ToList();
                    SecondaryCodexDropDownList.DataSource = sdata;
                    SecondaryCodexDropDownList.DataBind();

                    var tdata = new TKCodex[] { new TKCodex { Name = "Select your tertiary codex" } }.Concat(context.TKCodex.Where(c => c.Active.Value && c.GameSystemId == tournament.GameSystemId).OrderBy(c => c.Name)).ToList();
                    TertiaryCodexDropDownList.DataSource = tdata;
                    TertiaryCodexDropDownList.DataBind();

                    var qdata = new TKCodex[] { new TKCodex { Name = "Select your quaternary codex" } }.Concat(context.TKCodex.Where(c => c.Active.Value && c.GameSystemId == tournament.GameSystemId).OrderBy(c => c.Name)).ToList();
                    QuaternaryCodexDropDownList.DataSource = qdata;
                    QuaternaryCodexDropDownList.DataBind();

                    tournamentId.Value = id.ToString();

                    if (tournament.MaxPlayers.HasValue && tournament.TKTournamentPlayer.Where(tp => !tp.NonPlayer).Count() >= tournament.MaxPlayers)
                    {
                        tournamentFullLiteral.Text = "<span style=\"color: red;\">Tournament is full!</span><br/>";
                        return;
                    }
                }
            }
        }

        protected void SignUpRankedClick(object sender, EventArgs e)
        {
            SignUp(false, true);
        }

        protected void SignUpUnrankedClick(object sender, EventArgs e)
        {
            SignUp(true, true);
        }

        protected void SignUpNonPlayerClick(object sender, EventArgs e)
        {
            SignUp(false, false);
        }

        private void SignUp(bool doNotRank, bool isPlayer)
        {
            int id = int.Parse(tournamentId.Value.ToString());
            using (var context = new TourneyKeeperEntities())
            {
                var tournament = context.TKTournament.Single(t => t.Id == id);
                var player = Session["LoggedInUser"] as TKPlayer;

                if (isPlayer)
                {
                    if (tournament.MaxPlayers.HasValue && tournament.TKTournamentPlayer.Where(tp => !tp.NonPlayer).Count() >= tournament.MaxPlayers)
                    {
                        tournamentFullLiteral.Text = "<span style=\"color: red;\">Tournament is full!</span><br/>";
                        return;
                    }
                }

                if (tournament.TKTournamentPlayer.Any(p => p.PlayerId == player.Id))
                {
                    var tournamentPlayer = tournament.TKTournamentPlayer.Single(p => p.PlayerId == player.Id);
                    if (tournament.TournamentType == TournamentType.Team)
                    {
                        Response.Redirect($"/Team/TKTeamLeaderboard.aspx?Id={id}");
                    }
                    else
                    {
                        Response.Redirect($"/Singles/TKLeaderboard.aspx?Id={id}");
                    }
                }
                else
                {
                    var tournamentPlayerManager = new TournamentPlayerManager();
                    int.TryParse(TeamDropDownList.SelectedItem?.Value, out int selectedTeamId);
                    TKTournamentTeam team = null;
                    if (tournament.TournamentType == TournamentType.Team)
                    {
                        team = context.TKTournamentTeam.Single(t => t.Id == selectedTeamId);
                    }

                    var newPlayer = isPlayer ?
                        new TKTournamentPlayer()
                        {
                            TournamentId = id,
                            PlayerId = player.Id,
                            PlayerName = player.Name,
                            DoNotRank = doNotRank,
                            PrimaryCodexId = int.Parse(PrimaryCodexDropDownList.SelectedValue) == 0 ? new int?() : int.Parse(PrimaryCodexDropDownList.SelectedValue),
                            SecondaryCodexId = int.Parse(SecondaryCodexDropDownList.SelectedValue) == 0 ? new int?() : int.Parse(SecondaryCodexDropDownList.SelectedValue),
                            TertiaryCodexId = int.Parse(TertiaryCodexDropDownList.SelectedValue) == 0 ? new int?() : int.Parse(TertiaryCodexDropDownList.SelectedValue),
                            QuaternaryCodexId = int.Parse(QuaternaryCodexDropDownList.SelectedValue) == 0 ? new int?() : int.Parse(QuaternaryCodexDropDownList.SelectedValue),
                            ArmyList = WebUtility.HtmlEncode(ArmyListTextBox.Text),
                            Active = tournament.TournamentType == TournamentType.Team ? true : tournament.PlayersDefaultActive,
                            Paid = tournament.TournamentType == TournamentType.Team && (team.Paid ?? false),
                            TournamentTeamId = tournament.TournamentType == TournamentType.Team ? (int?)selectedTeamId : null
                        } :
                        new TKTournamentPlayer()
                        {
                            TournamentId = id,
                            PlayerId = player.Id,
                            PlayerName = player.Name,
                            DoNotRank = true,
                            NonPlayer = true,
                            Active = tournament.PlayersDefaultActive,
                            Paid = tournament.TournamentType == TournamentType.Team && (team.Paid ?? false),
                            TournamentTeamId = tournament.TournamentType == TournamentType.Team ? (int?)selectedTeamId : null
                        };
                    newPlayer = tournamentPlayerManager.AddTournamentPlayer(newPlayer);

                    if (tournament.TournamentType == TournamentType.Team)
                    {
                        Response.Redirect($"/Team/TKTeamLeaderboard.aspx?Id={id}");
                    }
                    else
                    {
                        Response.Redirect($"/Singles/TKLeaderboard.aspx?Id={id}");
                    }
                }
            }
        }

        protected void TeamDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");

            using (var context = new TourneyKeeperEntities())
            {
                TKTournament tournament = context.TKTournament.Single(t => t.Id == id);

                var selectedTeamId = int.Parse(TeamDropDownList.SelectedItem.Value);
                if (selectedTeamId == -1)
                {
                    signupNonPlayerButton.Enabled = false;
                    signupUnrankedButton.Enabled = false;
                }
                else
                {
                    signupNonPlayerButton.Enabled = true;
                    signupUnrankedButton.Enabled = true;
                }
            }
        }

        protected void OptionsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var optionsData = e.Row.DataItem as OptionsData;
                if (optionsData.PlayerTicket)
                {
                    ((TextBox)e.Row.FindControl("Amount")).Enabled = false;
                }
            }
        }

        protected void CreateTeamClick(object sender, EventArgs e)
        {
            var player = Session["LoggedInUser"] as TKPlayer;
            var id = General.GetParam<int>("Id");
            var teamManager = new TournamentTeamManager();
            var team = new TKTournamentTeam
            {
                Name = NewTeamTextBox.Text,
                TournamentId = id
            };
            teamManager.AddTournamentTeam(team, player.Token);

            Response.Redirect($"/Shared/TKsignup.aspx?Id={id}");
        }
    }
}