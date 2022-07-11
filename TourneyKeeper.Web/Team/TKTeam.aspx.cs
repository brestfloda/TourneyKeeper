using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;

namespace TourneyKeeper.Web
{
    public partial class TKTeam : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Team Overview";

            int id = int.Parse(Request["TeamId"]);

            BindData(id);
        }

        protected void TeamMatchesGridViewRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var teamData = e.Row.DataItem as TeamData;
                int id = int.Parse(Request["TeamId"]);

                using (var context = new TourneyKeeperEntities())
                {
#if DEBUG
                    context.Database.Log = x => Debug.WriteLine(x);
#endif
                    var team = context.TKTournamentTeam.Single(tt => tt.Id == id);
                    var tournament = context.TKTournament.Single(t => t.Id == team.TournamentId);
                    var currentRound = context.TKTeamMatch.Where(tm => tm.TournamentId == team.TournamentId).Max(x => x.Round);
                    var grid = (GridView)e.Row.FindControl("GamesGridView");
                    bool hide = tournament.HideResultsforRound && currentRound == teamData.Round;

                    var data = context.TKGame
                        .Include(g => g.TKTournament)
                        .Include(g => g.TKTournament.TKTeamScoringSystem)
                        .Include(g => g.TKTournamentPlayer)
                        .Include(g => g.TKTournamentPlayer.TKCodex)
                        .Include(g => g.TKTournamentPlayer1)
                        .Include(g => g.TKTournamentPlayer1.TKCodex)
                        .Include(g => g.TKTeamMatch)
                        .Include(g => g.TKTeamMatch.TKTournamentTeam)
                        .Include(g => g.TKTeamMatch.TKTournamentTeam1)
                        .Where(g => g.Round == teamData.Round && (g.TKTournamentPlayer.TournamentTeamId == id || g.TKTournamentPlayer1.TournamentTeamId == id))
                        .ToList()
                        .Select(g => new PairingsData()
                        {
                            TableNumber = g.TableNumber,
                            Player1Name = g.TKTournamentPlayer != null ? g.TKTournamentPlayer.NameTeamAndArmy : g.TKTeamMatch.TKTournamentTeam.Name,
                            Player2Name = g.TKTournamentPlayer1 != null ? g.TKTournamentPlayer1.NameTeamAndArmy : g.TKTeamMatch.TKTournamentTeam1.Name,
                            Team1Name = g.TKTeamMatch.TKTournamentTeam.Name,
                            Team2Name = g.TKTeamMatch.TKTournamentTeam1.Name,
                            Player1Result = g.Player1Result,
                            Player2Result = g.Player2Result,
                            Player1SecondaryResult = g.Player1SecondaryResult,
                            Player2SecondaryResult = g.Player2SecondaryResult,
                            Player1Id = g.Player1Id.HasValue ? g.TKTournamentPlayer.PlayerId : 0,
                            Player2Id = g.Player2Id.HasValue ? g.TKTournamentPlayer1.PlayerId : 0,
                            Id = g.Id,
                            TournamentId = g.TournamentId,
                            AllowEdit = false,
                            UseSecondaryPoints = g.TKTournament.UseSecondaryPoints,
                            Tournament = g.TKTournament,
                            TeamMatch = g.TKTeamMatch
                        });

                    grid.DataSource = data;
                    grid.DataBind();

                    if (data.FirstOrDefault() == null)
                    {
                        return;
                    }

                    bool isBattlefront = tournament.TKTeamScoringSystem.Name.Equals("battlefront scoring", StringComparison.InvariantCultureIgnoreCase);
                    bool isBattlepoints = tournament.TKTeamScoringSystem.Name.Equals("battle points", StringComparison.InvariantCultureIgnoreCase);
                    var match = data.First().TeamMatch;
                    var formattedPoints = match.Team1Points == 0 && match.Team2Points == 0 ? "" :
                        isBattlepoints ? $"{match.Team1Points} - {match.Team2Points}" :
                        isBattlefront ? $"({match.Team1SecondaryPoints}) {match.Team1Points} - {match.Team2Points} ({match.Team2SecondaryPoints})" :
                        $"({match.Team1Points}) {match.Team1MatchPoints} - {match.Team2MatchPoints} ({match.Team2Points})";

                    grid.HeaderRow.Cells[1].Text = $@"<div style=""text-align: center;"">{formattedPoints}</div>";
                }
            }
        }

        private void BindData(int tournamentTeamId)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                List<TeamData> teamData = context.TKTeamMatch
                    .Where(t => t.Team1Id == tournamentTeamId || t.Team2Id == tournamentTeamId).ToList()
                    .Select(t => new TeamData()
                    {
                        Round = t.Round
                    })
                    .Distinct()
                    .OrderBy(t => t.Round)
                    .ToList();

                TeamMatchesGridView.DataSource = teamData;
                TeamMatchesGridView.DataBind();
            }
        }
    }

    public class PairingsData
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int TournamentId { get; set; }
        public int? TableNumber { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public int Player1Result { get; set; }
        public int Player1SecondaryResult { get; set; }
        public int Player2Result { get; set; }
        public int Player2SecondaryResult { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public bool AllowEdit { get; set; }
        public bool UseSecondaryPoints { get; set; }
        public TKTournament Tournament { get; set; }
        public TKTeamMatch TeamMatch { get; set; }
        public bool IsCurrentRound { get; internal set; }
    }
}