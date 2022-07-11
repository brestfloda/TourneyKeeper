using System;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class Team : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");

            tournamentIdForSignOut.Value = id.ToString();
            playerIdForSignOut.Value = Session["LoggedInUser"] != null ? ((TKPlayer)Session["LoggedInUser"]).Id.ToString() : "0";

            aboutHyperLink.NavigateUrl = string.Format("/Team/TKTeamAbout.aspx?id={0}", id);
            leaderboardHyperLink.NavigateUrl = string.Format("/Team/TKTeamleaderboard.aspx?id={0}", id);
            individualLeaderboardHyperLink.NavigateUrl = string.Format("/Team/TKIndividualLeaderboard.aspx?id={0}", id);
            teamPairingsHyperLink.NavigateUrl = string.Format("/Team/TKTeampairings.aspx?id={0}", id);
            gamesAndPairingsHyperLink.NavigateUrl = string.Format("/Team/TKIndividualpairings.aspx?id={0}", id);
            weightedLeaderboardHyperLink.NavigateUrl = string.Format("/Team/TKTeamweightedleaderboard.aspx?id={0}", id);
            weightedTeamLeaderboardHyperLink.NavigateUrl = string.Format("/Team/TKTeamScoreWeightedLeaderboard.aspx?id={0}", id);
            codexLeaderboardHyperLink.NavigateUrl = string.Format("/Team/TKTeamcodexleaderboard.aspx?id={0}", id);
            playersHyperLink.NavigateUrl = string.Format("/Team/TKTeamPlayers.aspx?id={0}", id);
            teamsHyperLink.NavigateUrl = string.Format("/Team/TKTeams.aspx?id={0}", id);
            pairingsHyperLink.NavigateUrl = string.Format("/Team/TKTeamCreatePairings.aspx?id={0}", id);
            exportHyperLink.NavigateUrl = string.Format("/Team/TKPairingExport.aspx?id={0}", id);
            exportArmyListsHyperLink.NavigateUrl = $"/Team/TKArmyListsExport.aspx?Id={id}";
            tournamentHyperLink.NavigateUrl = string.Format("/Shared/TKCreateTournament.aspx?Id={0}&PlayerId={1}", id, Session["LoggedInUser"] != null ? ((TKPlayer)Session["LoggedInUser"]).Id.ToString() : "0");
            organizersHyperLink.NavigateUrl = string.Format("/Team/TKTeamOrganizers.aspx?id={0}", id);
            codexMatrixHyperLink.NavigateUrl = string.Format("/Team/TKTeamCodexMatrix.aspx?id={0}", id);
            gamesOverviewHyperLink.NavigateUrl = string.Format("/Team/TKTeamGamesOverview.aspx?id={0}", id);

            signupLink.NavigateUrl = string.Format("/Shared/TKSignup.aspx?id={0}", id);

            General.ShowOrHideContent(id, Session["LoggedInUser"] as TKPlayer, signupPlaceHolder, signoutPlaceHolder, SignupContentPlaceHolder, adminPlaceHolder, aboutPlaceHolder, null, titleLiteral, tournamentNameLiteral, contactLiteral);
        }
    }
}