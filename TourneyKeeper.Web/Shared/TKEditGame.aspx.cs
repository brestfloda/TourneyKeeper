using System;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Exceptions;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;

namespace TourneyKeeper.Web
{
    public partial class TKEditGame : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int tournamentId = int.Parse(Request["TournamentId"]);
            int gameId = int.Parse(Request["GameId"]);
            warningsLabel.Text = "";

            if (IsPostBack)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var tournament = context.TKTournament
                        .Include(t => t.TKGameSystem)
                        .Single(t => t.Id == tournamentId);

                    if (!int.TryParse(this.player1Score.Text, out int player1Score))
                    {
                        return;
                    }
                    if (!int.TryParse(this.player2Score.Text, out int player2Score))
                    {
                        return;
                    }
                    if (!int.TryParse(this.player1SecondaryScore.Text, out int player1SecondaryScore))
                    {
                        player1SecondaryScore = 0;
                    }
                    if (!int.TryParse(this.player2SecondaryScore.Text, out int player2SecondaryScore))
                    {
                        player2SecondaryScore = 0;
                    }

                    var game = context.TKGame.Single(g => g.Id == gameId);
                    game.Player1Result = player1Score;
                    game.Player2Result = player2Score;
                    game.Player1SecondaryResult = player1SecondaryScore;
                    game.Player2SecondaryResult = player2SecondaryScore;

                    var player = (TKPlayer)Session["LoggedInUser"];
                    var isAdminOrOrganizer = General.IsAdminOrOrganizer(player, tournamentId);
                    var tournamentPlayer = context.TKTournamentPlayer.SingleOrDefault(tp => tp.TournamentId == tournamentId && tp.PlayerId == player.Id);

                    string ret = CheckForLogin(tournament, player, tournamentPlayer, game, isAdminOrOrganizer);
                    if (!string.IsNullOrEmpty(ret))
                    {
                        warningsLabel.Text = ret;
                        return;
                    }

                    var results = $"P1: {player1Score} P2: {player2Score}";
                    LogManager.LogEvent("EditGame", $"Player: {player?.Id} - {player?.Name} Game: {game.Id} Results: {results}");

                    var gameManager = new GameManager();
                    if ((DateTime.Now < tournament.TournamentEndDate.AddDays(1)) || (Session["LoggedInUser"] != null && isAdminOrOrganizer))
                    {
                        gameManager.Update(game, player);
                        if (tournament.TournamentType == TournamentType.Team)
                        {
                            Response.Redirect(string.Format("/Team/TKTeamleaderboard.aspx?id={0}", tournamentId), false);
                        }
                        else
                        {
                            Response.Redirect(string.Format("/Singles/TKPairings.aspx?id={0}", tournamentId), false);
                        }
                    }
                    else
                    {
                        if (tournament.TournamentType == TournamentType.Team)
                        {
                            Response.Redirect(string.Format("/Team/TKTeamleaderboard.aspx?id={0}", tournamentId), false);
                        }
                        else
                        {
                            Response.Redirect(string.Format("/Singles/TKPairings.aspx?id={0}", tournamentId), false);
                        }
                    }
                }
            }
            else
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var tournament = context.TKTournament
                        .Include(t => t.TKGameSystem)
                        .Single(t => t.Id == tournamentId);
                    var game = context.TKGame.Single(g => g.Id == gameId);

                    var isAdminOrOrganizer = General.IsAdminOrOrganizer(((TKPlayer)Session["LoggedInUser"]), tournamentId);
                    var player = (TKPlayer)Session["LoggedInUser"];
                    var tournamentPlayer = context.TKTournamentPlayer.SingleOrDefault(tp => tp.TournamentId == tournamentId && tp.PlayerId == player.Id);

                    string ret = CheckForLogin(tournament, player, tournamentPlayer, game, isAdminOrOrganizer);
                    if (!string.IsNullOrEmpty(ret))
                    {
                        warningsLabel.Text = ret;
                        return;
                    }

                    if (tournament.UseSecondaryPoints)
                    {
                        player2PointsPlaceHolder.Visible = player1PointsPlaceHolder.Visible = true;
                        player2SecondaryPlaceholder.Visible = player1SecondaryPlaceholder.Visible = true;
                        SetLabel(tournament, game);
                        player1Score.Text = game.Player1Result.ToString();
                        player2Score.Text = game.Player2Result.ToString();
                        player1SecondaryScore.Text = game.Player1SecondaryResult.ToString();
                        player2SecondaryScore.Text = game.Player2SecondaryResult.ToString();
                        secPoints1Label.Text = tournament.TKGameSystem.Name.Equals("Flames Of War", StringComparison.InvariantCultureIgnoreCase) ? "Small points" : "Sec. points";
                        secPoints2Label.Text = tournament.TKGameSystem.Name.Equals("Flames Of War", StringComparison.InvariantCultureIgnoreCase) ? "Small points" : "Sec. points";
                    }
                    else
                    {
                        player2PointsPlaceHolder.Visible = player1PointsPlaceHolder.Visible = true;
                        player2SecondaryPlaceholder.Visible = player1SecondaryPlaceholder.Visible = false;
                        SetLabel(tournament, game);
                        player1Score.Text = game.Player1Result.ToString();
                        player2Score.Text = game.Player2Result.ToString();
                    }

                    GameId.Value = gameId.ToString();
                    TournamentId.Value = tournamentId.ToString();
                }
            }
        }

        private string CheckForLogin(TKTournament tournament, TKPlayer player, TKTournamentPlayer tournamentPlayer, TKGame game, bool isAdminOrOrganizer)
        {
            if (isAdminOrOrganizer)
            {
                return null;
            }

            if (tournament.RequireLogin == (int)RequiredToReportEnum.PlayerInTournament)
            {
                if (player == null || tournamentPlayer == null)
                {
                    return "This tournament requires you to be logged in and a player in the tournament before you can enter results";
                }
            }
            else if (tournament.RequireLogin == (int)RequiredToReportEnum.PlayerInGame)
            {
                if (player == null || tournamentPlayer == null)
                {
                    return "This tournament requires you to be logged in and a player in the game before you can enter results";
                }
                var isPlayingGame = tournamentPlayer == null ? false : game.Player1Id == tournamentPlayer.Id || game.Player2Id == tournamentPlayer.Id;
                if (!isPlayingGame)
                {
                    return "This tournament requires you to be logged in and a player in the game before you can enter results";
                }
            }
            else
            {
                throw new Exception("System error - missing configuration");
            }

            return null;
        }

        private void SetLabel(TKTournament tournament, TKGame game)
        {
            var p1tmp = game.TKTournamentPlayer == null ? game.TKTeamMatch.TKTournamentTeam.Name : game.TKTournamentPlayer.NameAndCodex;
            var p2tmp = game.TKTournamentPlayer1 == null ? game.TKTeamMatch.TKTournamentTeam1.Name : game.TKTournamentPlayer1.NameAndCodex;
            if (tournament.TKGameSystem.Name.Equals("Flames Of War", StringComparison.InvariantCultureIgnoreCase))
            {
                player1Label.Text = $"Big Points ({p1tmp})";
                player2Label.Text = $"Big Points ({p2tmp})";
            }
            else
            {
                player1Label.Text = p1tmp;
                player2Label.Text = p2tmp;
            }
        }
    }
}