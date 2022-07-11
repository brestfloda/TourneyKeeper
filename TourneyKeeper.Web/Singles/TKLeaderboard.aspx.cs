using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web
{
    public partial class TKLeaderboard : TKWebPage
    {
        private TKTournament tournament = null;
        public string FromSubmit = "";
        private bool showClub = false;
        private bool showQuiz = false;
        private bool showPainting = false;
        private bool showPenalty = false;
        private bool showFairplay = false;
        private bool showArmy = false;
        public bool hideResultsforRound = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Leaderboard";
            var keywords = new HtmlMeta { Name = "description", Content = "The Leaderboard shows the current ranking of players at a singles tournament." };
            Header.Controls.Add(keywords);

            int id = General.GetParam<int>("Id");
            BindData(id, SortDirection.Descending, "Initial");
        }

        private void BindData(int id, SortDirection direction, string expression)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                tournament = context.TKTournament
                    .Include(t => t.TKSinglesScoringSystem)
                    .Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);

                hideResultsforRound = tournament.HideResultsforRound && DateTime.Now < tournament.TournamentEndDate.AddDays(1);
                LeaderboardGridView.Columns[0].SortExpression = hideResultsforRound ? "Player" : "Placement";

                int placement = 1;
                IEnumerable<LeaderboardData> data;
                if (tournament.TKSinglesScoringSystem.Name.Equals("ITC", StringComparison.InvariantCultureIgnoreCase))
                {
                    data = context.TKTournamentPlayer
                        .Include(t => t.TKCodex)
                        .Include(t => t.TKTournament)
                        .Where(tp => tp.TournamentId == id && tp.Active)
                        .ToList()
                        .Select(tp => new LeaderboardData()
                        {
                            Id = tp.Id,
                            Seed = tp.Seed,
                            PlayerId = tp.PlayerId,
                            GamePath = tp.GamePath,
                            PrimaryCodex = tp.TKCodex != null ? tp.TKCodex.Name : "",
                            Penalty = tp.Penalty,
                            Quiz = tp.Quiz,
                            Painting = tp.Painting,
                            Fairplay = tp.FairPlay,
                            BattlePoints = tp.BattlePoints,
                            SecondaryPoints = tp.SecondaryPoints,
                            Points = tp.TotalPoints,
                            Name = tp.PlayerName,
                            HasArmylist = !string.IsNullOrEmpty(tp.ArmyList),
                            Club = tp.Club,
                            Wins = tp.Wins,
                            Draws = tp.Draws,
                            Losses = tp.Losses
                        })
                        .OrderByDescending(tp => tp.Wins)
                        .ThenByDescending(tp => tp.Draws)
                        .ThenBy(tp => tp.Losses)
                        .ThenByDescending(tp => tp.BattlePoints)
                        .ThenBy(tp => tp.Name);
                }
                else
                {
                    data = context.TKTournamentPlayer
                        .Include(t => t.TKCodex)
                        .Include(t => t.TKTournament)
                        .Where(tp => tp.TournamentId == id && tp.Active)
                        .ToList()
                        .Select(tp => new LeaderboardData()
                        {
                            Id = tp.Id,
                            Seed = tp.Seed,
                            PlayerId = tp.PlayerId,
                            GamePath = tp.GamePath,
                            PrimaryCodex = tp.TKCodex != null ? tp.TKCodex.Name : "",
                            Penalty = tp.Penalty,
                            Quiz = tp.Quiz,
                            Painting = tp.Painting,
                            Fairplay = tp.FairPlay,
                            BattlePoints = tp.BattlePoints,
                            SecondaryPoints = tp.SecondaryPoints,
                            Points = tp.TotalPoints,
                            Name = tp.PlayerName,
                            HasArmylist = !string.IsNullOrEmpty(tp.ArmyList),
                            Club = tp.Club,
                            Wins = tp.Wins,
                            Draws = tp.Draws,
                            Losses = tp.Losses
                        });

                    if (hideResultsforRound)
                    {
                        data = data.OrderBy(tp => tp.Name);
                    }
                    else if (tournament.ShowSoftScores)
                    {
                        data = data
                            .OrderByDescending(tp => tp.BattlePoints + tp.Fairplay + tp.Penalty + tp.Painting + tp.Quiz)
                            .ThenByDescending(tp => tp.BattlePoints)
                            .ThenByDescending(tp => tp.SecondaryPoints)
                            .ThenByDescending(tp => tp.Wins)
                            .ThenByDescending(tp => tp.Draws)
                            .ThenBy(tp => tp.Losses)
                            .ThenBy(tp => tp.Seed)
                            .ThenBy(tp => tp.Name);
                    }
                    else
                    {
                        data = data
                            .OrderByDescending(tp => tp.BattlePoints + tp.Penalty)
                            .ThenByDescending(tp => tp.BattlePoints)
                            .ThenByDescending(tp => tp.SecondaryPoints)
                            .ThenByDescending(tp => tp.Wins)
                            .ThenByDescending(tp => tp.Draws)
                            .ThenBy(tp => tp.Losses)
                            .ThenBy(tp => tp.Seed)
                            .ThenBy(tp => tp.Name);
                    }
                }

                data = data.Select(tp => new LeaderboardData()
                {
                    Id = tp.Id,
                    Seed = tp.Seed,
                    PlayerId = tp.PlayerId,
                    GamePath = tp.GamePath,
                    PrimaryCodex = tp.PrimaryCodex,
                    Penalty = tp.Penalty,
                    Quiz = tp.Quiz,
                    Painting = tp.Painting,
                    Fairplay = tp.Fairplay,
                    BattlePoints = tp.BattlePoints,
                    SecondaryPoints = tp.SecondaryPoints,
                    Name = tp.Name,
                    HasArmylist = tp.HasArmylist,
                    Points = tp.Points,
                    Club = tp.Club,
                    Wins = tp.Wins,
                    Draws = tp.Draws,
                    Losses = tp.Losses,
                    Placement = placement++
                });

                switch (expression)
                {
                    case "Initial":
                        data = data
                            .OrderBy(tp => tp.Placement);
                        break;
                    case "Seed":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Seed) : data
                            .OrderByDescending(tp => tp.Seed);
                        break;
                    case "Club":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Club) : data
                            .OrderByDescending(tp => tp.Club);
                        break;
                    case "Placement":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Placement) : data
                            .OrderByDescending(tp => tp.Placement);
                        break;
                    case "Player":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Name) : data
                            .OrderByDescending(tp => tp.Name);
                        break;
                    case "GamePath":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.GamePath) : data
                            .OrderByDescending(tp => tp.GamePath);
                        break;
                    case "Army":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.PrimaryCodex) : data
                            .OrderByDescending(tp => tp.PrimaryCodex);
                        break;
                    case "Penalty":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Penalty) : data
                            .OrderByDescending(tp => tp.Penalty);
                        break;
                    case "Quiz":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Quiz) : data
                            .OrderByDescending(tp => tp.Quiz);
                        break;
                    case "Painting":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Painting) : data
                            .OrderByDescending(tp => tp.Painting);
                        break;
                    case "Fairplay":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Fairplay) : data
                            .OrderByDescending(tp => tp.Fairplay);
                        break;
                    case "BattlePoints":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.BattlePoints) : data
                            .OrderByDescending(tp => tp.BattlePoints);
                        break;
                    case "SecondaryPoints":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.SecondaryPoints) : data
                            .OrderByDescending(tp => tp.SecondaryPoints);
                        break;
                    case "Points":
                        data = direction == SortDirection.Ascending ? data
                            .OrderBy(tp => tp.Points) : data
                            .OrderByDescending(tp => tp.Points);
                        break;
                };

                LeaderboardGridView.DataSource = data;
                LeaderboardGridView.DataBind();
            }
        }

        protected void LeaderboardGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = showClub;
            e.Row.Cells[3].Visible = tournament.UseSeed;
            e.Row.Cells[5].Visible = showArmy;
            e.Row.Cells[6].Visible = showQuiz && tournament.ShowSoftScores;
            e.Row.Cells[7].Visible = showPainting && tournament.ShowSoftScores;
            e.Row.Cells[8].Visible = showFairplay && tournament.ShowSoftScores;
            e.Row.Cells[9].Visible = showPenalty;
            e.Row.Cells[10].Visible = ((showQuiz || showPainting || showPenalty) && tournament.ShowSoftScores) || showFairplay;
            e.Row.Cells[11].Visible = tournament.UseSecondaryPoints;
        }

        protected void LeaderboardGridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var id = General.GetParam<int>("Id");

            var sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionHidden.Value) == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
            sortDirectionHidden.Value = sortDirection.ToString();

            BindData(id, sortDirection, e.SortExpression);
        }

        protected void LeaderboardGridViewInit(object sender, EventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                int id = General.GetParam<int>("Id");
                tournament = context.TKTournament
                    .Include(t => t.TKGameSystem)
                    .Single(t => t.Id == id);
                TKPlayer player = Session["LoggedInUser"] as TKPlayer;

                showClub = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.Club != null && t.Club != "");
                showQuiz = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.Quiz != 0);
                showPainting = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.Painting != 0);
                showFairplay = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.FairPlay != 0);
                showPenalty = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournament.Id).Any(t => t.Penalty != 0);
                showArmy = tournament.ShowListsDate.HasValue ? tournament.ShowListsDate < DateTime.Now :
                    tournament.TournamentDate < DateTime.Now || General.IsAdminOrOrganizer(Session["LoggedInUser"] as TKPlayer, id);
            }
        }

        protected string ShowArmylistLink(int playerId, string primaryCodex, bool hasArmylist)
        {
            if (hasArmylist)
            {
                return string.Format("<a href=\"javascript:OpenPopup({0})\" runat=\"server\">{1}</a>", playerId, primaryCodex);
            }
            else
            {
                return string.Format("{0}", primaryCodex);
            }
        }
    }
}