using System;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class Singles : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");

            tournamentIdForSignOut.Value = id.ToString();
            playerIdForSignOut.Value = Session["LoggedInUser"] != null ? ((TKPlayer)Session["LoggedInUser"]).Id.ToString() : "0";

            aboutHyperLink.NavigateUrl = string.Format("/Singles/TKAbout.aspx?id={0}", id);
            leaderboardHyperLink.NavigateUrl = string.Format("/Singles/TKLeaderboard.aspx?id={0}", id);
            gamesAndPairingsHyperLink.NavigateUrl = string.Format("/Singles/TKPairings.aspx?id={0}", id);
            weightedLeaderboardHyperLink.NavigateUrl = string.Format("/Singles/TKWeightedleaderboard.aspx?id={0}", id);
            codexLeaderboardHyperLink.NavigateUrl = string.Format("/Singles/TKCodexleaderboard.aspx?id={0}", id);
            playersHyperLink.NavigateUrl = string.Format("/Singles/TKPlayers.aspx?id={0}", id);
            pairingsHyperLink.NavigateUrl = string.Format("/Singles/TKCreatePairings.aspx?id={0}", id);
            tournamentHyperLink.NavigateUrl = string.Format("/Shared/TKCreateTournament.aspx?Id={0}&PlayerId={1}", id, Session["LoggedInUser"] != null ? ((TKPlayer)Session["LoggedInUser"]).Id.ToString() : "0");
            ITCExportHyperLink.NavigateUrl = $"/Singles/TKITCExport.aspx?Id={id}";
            ExportPairingsHyperLink.NavigateUrl = $"/Singles/TKSinglesPairingExport.aspx?Id={id}";
            exportArmyListsHyperLink.NavigateUrl = $"/Singles/TKSinglesArmyListsExport.aspx?Id={id}";
            organizersHyperLink.NavigateUrl = string.Format("/Singles/TKSinglesOrganizers.aspx?id={0}", id);
            importHyperLink.NavigateUrl = string.Format("/Singles/TKImport.aspx?id={0}", id);
            codexMatrixHyperLink.NavigateUrl = string.Format("/Singles/TKCodexMatrix.aspx?id={0}", id);
            gamesOverviewHyperLink.NavigateUrl = string.Format("/Singles/TKGamesOverview.aspx?id={0}", id);

            signupLink.NavigateUrl = string.Format("/Shared/TKSignup.aspx?id={0}", id);

            General.ShowOrHideContent(id, Session["LoggedInUser"] as TKPlayer, signupPlaceHolder, signoutPlaceHolder, SignupContentPlaceHolder, adminPlaceHolder, aboutPlaceHolder, ITCExportHyperLink, titleLiteral, tournamentNameLiteral, contactLiteral);
        }
    }
}