using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKGames : System.Web.UI.Page
    {
        private bool showResults = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int playerId = int.Parse(Request["PlayerId"]);

            if (!IsPostBack)
            {
                BindData(playerId, SortDirection.Descending, "Initial");
            }
        }

        private void BindData(int id, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var data = context.TKPlayer
                .Where(g => g.Id == id)
                .Join(context.TKTournamentPlayer, pl => pl.Id, tp => tp.PlayerId, (pl, tp) => new { Player = pl, TournamentPlayer = tp })
                .Join(context.TKTournament, d => d.TournamentPlayer.TournamentId, t => t.Id, (d, t) => new { Tournament = t, Player = d.Player, TournamentPlayer = d.TournamentPlayer })
                .Join(context.TKGame, d2 => d2.Tournament.Id, g => g.TournamentId, (t, g) => new { Game = g, Tournament = t.Tournament, Player = t.Player, TournamentPlayer = t.TournamentPlayer })
                .GroupJoin(context.TKRanking, r => r.Player.Id, p => p.PlayerId, (r, p) => new { Game = r.Game, Tournament = r.Tournament, Player = r.Player, TournamentPlayer = r.TournamentPlayer, Ranking = p.FirstOrDefault() })
                .Where(d3 => d3.Game.TKTournamentPlayer != null && d3.Game.TKTournamentPlayer1 != null)
                .Where(d4 => d4.Game.Player1Id == d4.TournamentPlayer.Id || d4.Game.Player2Id == d4.TournamentPlayer.Id)
                .Where(d5 => d5.Tournament.Active)
                .Select(tp => new PlayerData()
                {
                    OpponentPlayerId = tp.Game.Player1Id.Value == tp.TournamentPlayer.Id ? tp.Game.TKTournamentPlayer1.PlayerId : tp.Game.TKTournamentPlayer.PlayerId,
                    PlayerName = tp.TournamentPlayer.PlayerName,
                    IsTeamTournament = tp.Tournament.TournamentTypeId == (int)TournamentType.Team,
                    TournamentId = tp.Tournament.Id,
                    TournamentName = tp.Tournament.Name,
                    TournamentDate = tp.Tournament.TournamentDate,
                    OpponentName = tp.Game.Player1Id.Value == tp.TournamentPlayer.Id ? tp.Game.TKTournamentPlayer1.PlayerName : tp.Game.TKTournamentPlayer.PlayerName,
                    Points = tp.Game.Player1Id.Value == tp.TournamentPlayer.Id ? tp.Game.Player1Result : tp.Game.Player2Result,
                    Ranked = tp.Game.Ranked,
                    ELO = tp.Game.Player1Id.Value == tp.TournamentPlayer.Id ? tp.Game.Player1ELO : tp.Game.Player2ELO,
                    GameId = tp.Game.Id,
                    Wins = tp.Player.TotalWins,
                    Losses = tp.Player.TotalLosses,
                    Draws = tp.Player.TotalDraws,
                    Round = tp.Game.Round,
                    Rank = tp.Ranking == null ? "none" : tp.Ranking.Rank.Value.ToString(),
                    Email = tp.Player.Email,
                    TableNumber = tp.Game.TableNumber
                });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderByDescending(tp => tp.TournamentDate)
                            .ThenByDescending(tp2 => tp2.Round);
                        break;
                    case "TournamentName":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TournamentName) : data
                            .OrderByDescending(tp => tp.TournamentName);
                        break;
                    case "TournamentDate":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TournamentDate) : data
                            .OrderByDescending(tp => tp.TournamentDate);
                        break;
                    case "Opponent":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.OpponentName) : data
                            .OrderByDescending(tp => tp.OpponentName);
                        break;
                    case "Points":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Points) : data
                            .OrderByDescending(tp => tp.Points);
                        break;
                    case "Ranked":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Ranked) : data
                            .OrderByDescending(tp => tp.Ranked);
                        break;
                    case "ELO":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.ELO) : data
                            .OrderByDescending(tp => tp.ELO);
                        break;
                    case "TableNumber":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.TableNumber) : data
                            .OrderByDescending(tp => tp.TableNumber);
                        break;
                };

                var tmp = data.FirstOrDefault();
                PlayerNameLabel.Text = tmp == null ? "" : string.Format("{0} ({1}/{2}/{3} - Rank: {4}) - <a href='mailto:{5}'>Send email</a>", tmp.PlayerName, tmp.Wins, tmp.Losses, tmp.Draws, tmp.Rank, tmp.Email);

                GamesGridView.DataSource = data.ToList();
                GamesGridView.DataBind();
            }
        }

        protected void GamesGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[8].Visible = showResults;
        }

        protected void GamesGridViewInit(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                TKPlayer player = Session["LoggedInUser"] as TKPlayer;
                int playerId = int.Parse(Request["PlayerId"]);

                showResults = player != null ? player.IsAdmin || player.Id == playerId : false;
            }
        }

        protected void GamesGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            int id = int.Parse(Request["PlayerId"]);

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(id, sortDirection, e.SortExpression);
        }
    }
}