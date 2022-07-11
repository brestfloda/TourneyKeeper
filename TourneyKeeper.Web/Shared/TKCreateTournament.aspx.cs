using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.EnumTranslator;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKCreateTournament : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var playerId = General.GetParam<int>("PlayerId");

            using (var context = new TourneyKeeperEntities())
            {
                var loggedInPlayer = Session["LoggedInUser"] as TKPlayer;
                if (loggedInPlayer == null)
                {
                    Response.Redirect("/");
                }

                if (playerId != loggedInPlayer.Id)
                {
                    Response.Redirect("/");
                }
            }

            int.TryParse(Request["id"], out int tournamentId);

            if (playerId < 1)
            {
                Response.Redirect("/");
                return;
            }

            if (!IsPostBack)
            {
                BindData(playerId, tournamentId);
            }
        }

        private void BindData(int playerId, int tournamentId)
        {
            var tournamentManager = new TournamentManager();

            if (tournamentId > 0)
            {
                TKTournament tournament = tournamentManager.GetTournament(tournamentId, playerId);

                tournamentIdHidden.Value = tournamentId.ToString();
                tournamentNameTextbox.Text = tournament.Name;
                ActiveHiddenField.Value = tournament.Active.ToString();
                tournamentDateTextbox.Text = tournament.TournamentDate.ToShortDateString();
                tournamentEndDateTextbox.Text = tournament.TournamentEndDate.ToShortDateString();
                GameSystemHiddenField.Value = tournament.GameSystemId.ToString();
                TournamentTypeHiddenField.Value = tournament.TournamentTypeId.ToString();
                SinglesScoringHiddenField.Value = tournament.SinglesScoringSystemId.ToString();
                TeamScoringHiddenField.Value = tournament.TeamScoringSystemId.ToString();
                RequiredToReportHiddenField.Value = tournament.RequireLogin.ToString();
                CountryHiddenField.Value = tournament.Country;
                PlayersDefaultActiveHiddenField.Value = tournament.PlayersDefaultActive.ToString();
                maxPlayersTextbox.Text = tournament.MaxPlayers.ToString();
                teamSizeTextbox.Text = tournament.TeamSize.ToString();
                UseSecondaryHiddenField.Value = tournament.UseSecondaryPoints.ToString();
                UseSeedHiddenField.Value = tournament.UseSeed.ToString();
                UseAboutHiddenField.Value = tournament.UseAbout.ToString();
                AllowEditHiddenField.Value = tournament.AllowEdit.ToString();
                NationalTournamentHiddenField.Value = tournament.NationalTournament.ToString();
                ShowSoftScoresHiddenField.Value = tournament.ShowSoftScores.ToString();
                OnlineSignupHiddenField.Value = tournament.OnlineSignup.ToString();
                HideResultsForRoundHiddenField.Value = tournament.HideResultsforRound.ToString();
                onlineSignupStartTextbox.Text = tournament.OnlineSignupStart.HasValue ? tournament.OnlineSignupStart.Value.Date.ToShortDateString() : "";
                onlineSignupStartTimeTextbox.Text = tournament.OnlineSignupStart.HasValue ? tournament.OnlineSignupStart.Value.TimeOfDay.ToString() : "";
                if (tournament.ShowListsDate.HasValue)
                {
                    showListsDateTextBox.Text = tournament.ShowListsDate.HasValue ? tournament.ShowListsDate.Value.Date.ToShortDateString() : "";
                    showListsDateTimeTextBox.Text = tournament.ShowListsDate.HasValue ? tournament.ShowListsDate.Value.TimeOfDay.ToString() : "";
                }
                maxLossTextBox.Text = tournament.MaxScoreForLoss?.ToString();
                minWinTextBox.Text = tournament.MinScoreForWin?.ToString();
                organizerEmailTextBox.Text = tournament.OrganizerEmail;

                descriptionTextbox.Text = WebUtility.HtmlDecode(tournament.Description);

                okButton.Text = "Update Tournament";
                var createTournamentHyperLink = General.FindControl<HyperLink>(Page.Controls, "createTournamentHyperLink");
                createTournamentHyperLink.Text = "Update Tournament";
                activeRowPlaceHolder.Visible = true;

            }
            else
            {
                PlayersDefaultActiveHiddenField.Value = "True";
                GameSystemHiddenField.Value = "1";
                TournamentTypeHiddenField.Value = "1";
                SinglesScoringHiddenField.Value = "1";
                TeamScoringHiddenField.Value = "1";
                RequiredToReportHiddenField.Value = "1";
                CountryHiddenField.Value = "Denmark";
                OnlineSignupHiddenField.Value = "True";
                AllowEditHiddenField.Value = "True";
                ActiveHiddenField.Value = "True";
                ShowSoftScoresHiddenField.Value = "True";
                NationalTournamentHiddenField.Value = "False";
                HideResultsForRoundHiddenField.Value = "False";
                UseSeedHiddenField.Value = "False";
                UseSecondaryHiddenField.Value = "False";
                UseAboutHiddenField.Value = "False";
                ShowSoftScoresHiddenField.Value = "False";
            }

            var singlesSelected = TournamentTypeHiddenField.Value == ((int)TournamentType.Singles).ToString() ? "selected" : "";
            var teamSelected = TournamentTypeHiddenField.Value == ((int)TournamentType.Team).ToString() ? "selected" : "";
            var tournamentTypeContent = $"<option value='1' {singlesSelected}>Singles</option>";
            tournamentTypeContent += $"<option value='2' {teamSelected}>Team</option>";
            tournamentTypeDropDownLiteral.Text = tournamentTypeContent;

            var teamScoringSystems = tournamentManager.GetTeamScoringSystems();
            var teamScoringSystemContent = "";
            foreach (var teamScoringSystem in teamScoringSystems)
            {
                var isSelected = TeamScoringHiddenField.Value == teamScoringSystem.Id.ToString() ? "selected" : "";
                teamScoringSystemContent += $"<option value='{teamScoringSystem.Id}' {isSelected}>{teamScoringSystem.Name}</option>";
            }
            teamScoringDropDownLiteral.Text = teamScoringSystemContent;

            var singlesScoringSystems = tournamentManager.GetSinglesScoringSystems();
            var singlesScoringSystemContent = "";
            foreach (var singlesScoringSystem in singlesScoringSystems)
            {
                var isSelected = SinglesScoringHiddenField.Value == singlesScoringSystem.Id.ToString() ? "selected" : "";
                singlesScoringSystemContent += $"<option value='{singlesScoringSystem.Id}' {isSelected}>{singlesScoringSystem.Name}</option>";
            }
            singlesScoringDropDownLiteral.Text = singlesScoringSystemContent;

            var countryContent = "";
            foreach (var country in Country.Countries)
            {
                var isSelected = CountryHiddenField.Value == country ? "selected" : "";
                countryContent += $"<option value='{country}' {isSelected}>{country}</option>";
            }
            countryDropDownLiteral.Text = countryContent;

            var gameSystems = tournamentManager.GetGameSystems();
            var gameSystemContent = "";
            foreach (var gameSystem in gameSystems)
            {
                var isSelected = GameSystemHiddenField.Value == gameSystem.Id.ToString() ? "selected" : "";
                gameSystemContent += $"<option value='{gameSystem.Id}' {isSelected}>{gameSystem.Name}</option>";
            }
            gameSystemDropDownLiteral.Text = gameSystemContent;

            var requiredToReportEnums = Enum.GetValues(typeof(RequiredToReportEnum)).Cast<RequiredToReportEnum>().Select(r => new { Value = (int)r, Text = EnumTranslator.Translate(r) });
            var requiredToReportContent = "";
            foreach (var requiredToReportEnum in requiredToReportEnums)
            {
                var isSelected = RequiredToReportHiddenField.Value == requiredToReportEnum.Value.ToString() ? "selected" : "";
                requiredToReportContent += $"<option value='{requiredToReportEnum.Value}' {isSelected}>{requiredToReportEnum.Text}</option>";
            }
            requiredToReportLiteral.Text = requiredToReportContent;
        }

        protected void UpdateData()
        {
            var player = Session["LoggedInUser"] as TKPlayer;

            Page.Validate();

            if (Page.IsValid && player != null)
            {
                var tournamentManager = new TournamentManager();

                TKTournament tournamentTmp = null;
                using (var context = new TourneyKeeperEntities())
                {
                    var id = !string.IsNullOrEmpty(tournamentIdHidden.Value) ? int.Parse(tournamentIdHidden.Value) : 0;
                    tournamentTmp = context.TKTournament.SingleOrDefault(t => t.Id == id);
                }

                int.TryParse(maxLossTextBox.Text, out int maxLoss);
                int.TryParse(minWinTextBox.Text, out int minWin);

                if (maxLoss > minWin)
                {
                    int tmp = maxLoss;
                    maxLoss = minWin;
                    minWin = tmp;
                }

                var tournament = new TKTournament
                {
                    Id = !string.IsNullOrEmpty(tournamentIdHidden.Value) ? int.Parse(tournamentIdHidden.Value) : 0,
                    Active = bool.Parse(ActiveHiddenField.Value),
                    AllowEdit = bool.Parse(AllowEditHiddenField.Value),
                    NationalTournament = bool.Parse(NationalTournamentHiddenField.Value),
                    HideResultsforRound = bool.Parse(HideResultsForRoundHiddenField.Value),
                    Country = CountryHiddenField.Value,
                    GameSystemId = int.Parse(GameSystemHiddenField.Value),
                    MaxPlayers = !string.IsNullOrEmpty(maxPlayersTextbox.Text) ? (int?)int.Parse(maxPlayersTextbox.Text) : null,
                    Name = tournamentNameTextbox.Text,
                    OnlineSignup = bool.Parse(OnlineSignupHiddenField.Value),
                    OnlineSignupStart = !string.IsNullOrEmpty(onlineSignupStartTextbox.Text) ? (DateTime?)DateTime.Parse(string.Format("{0} {1}", onlineSignupStartTextbox.Text, onlineSignupStartTimeTextbox.Text)) : null,
                    SinglesScoringSystemId = int.Parse(SinglesScoringHiddenField.Value),
                    TeamScoringSystemId = int.Parse(TeamScoringHiddenField.Value),
                    TeamSize = !string.IsNullOrEmpty(teamSizeTextbox.Text) ? (int?)int.Parse(teamSizeTextbox.Text) : null,
                    TournamentDate = DateTime.Parse(tournamentDateTextbox.Text),
                    TournamentEndDate = !string.IsNullOrEmpty(tournamentEndDateTextbox.Text) ? DateTime.Parse(tournamentEndDateTextbox.Text) : DateTime.Parse(tournamentDateTextbox.Text),
                    TournamentTypeId = int.Parse(TournamentTypeHiddenField.Value),
                    UseSeed = bool.Parse(UseSeedHiddenField.Value),
                    UseSecondaryPoints = bool.Parse(UseSecondaryHiddenField.Value),
                    UseAbout = bool.Parse(UseAboutHiddenField.Value),
                    ShowListsDate = !string.IsNullOrEmpty(showListsDateTextBox.Text) ? (DateTime?)DateTime.Parse(string.Format("{0} {1}", showListsDateTextBox.Text, showListsDateTimeTextBox.Text)) : null,
                    PlayersDefaultActive = bool.Parse(PlayersDefaultActiveHiddenField.Value),
                    ShowSoftScores = bool.Parse(ShowSoftScoresHiddenField.Value),
                    Description = WebUtility.HtmlEncode(descriptionTextbox.Text),
                    RequireLogin = int.Parse(RequiredToReportHiddenField.Value),
                    MaxScoreForLoss = maxLoss,
                    MinScoreForWin = minWin,
                    OrganizerEmail = organizerEmailTextBox.Text
                };

                if (tournament.Id > 0)
                {
                    var token = player?.Token;

                    tournamentManager.Update(tournament, token);
                }
                else
                {
                    tournament.Id = tournamentManager.AddTournament(tournament, player.Id);
                    player.OrganizerForTournamentIds.Add(tournament.Id);
                }

                if (tournament != null)
                {
                    Response.Redirect($"/Shared/TKCreateTournament.aspx?Id={tournament.Id}&PlayerId={player.Id}");
                }
                else
                {
                    if (tournament.TournamentType == TournamentType.Singles)
                    {
                        Response.Redirect($"/Singles/TKLeaderboard.aspx?id={tournament.Id}");
                    }
                    else
                    {
                        Response.Redirect($"/Team/TKTeamLeaderboard.aspx?Id={tournament.Id}");
                    }
                }
            }
        }

        protected void ShowListsDateTimeServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(showListsDateTextBox.Text))
            {
                args.IsValid = true;
                return;
            }

            args.IsValid = DateTime.TryParse(string.Format("{0} {1}", showListsDateTextBox.Text, showListsDateTimeTextBox.Text), out DateTime date);
        }

        protected void OnlineSignupStartTimeServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(onlineSignupStartTextbox.Text))
            {
                args.IsValid = true;
                return;
            }

            args.IsValid = DateTime.TryParse(string.Format("{0} {1}", onlineSignupStartTextbox.Text, onlineSignupStartTimeTextbox.Text), out DateTime date);
        }

        protected void okButton_Click(object sender, EventArgs e)
        {
            UpdateData();
        }
    }
}