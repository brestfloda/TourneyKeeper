using System;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web
{
    public partial class TKITCExport : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            using (var context = new TourneyKeeperEntities())
            {
                int placement = 1;
                var data = context.TKTournamentPlayer
                    .Include(t => t.TKPlayer)
                    .Include(t => t.TKCodex)
                    .Include(t => t.TKCodex1)
                    .Where(tp => tp.TournamentId == id)
                    .ToList()
                    .Select(tp => new ITCExportData()
                    {
                        Id = tp.Id,
                        PlayerId = tp.PlayerId,
                        Placement = placement++,
                        FirstName = GetFirstName(tp.PlayerName),
                        LastName = GetLastName(tp.PlayerName),
                        PrimaryCodex = tp.TKCodex != null ? tp.TKCodex.Name : "",
                        SecondaryCodex = tp.TKCodex1 != null ? tp.TKCodex1.Name : "",
                        BattlePoints = tp.BattlePoints,
                        Club = tp.Club,
                        Email = tp.TKPlayer.Email,
                        PlacingPoints = 0,
                        BonusPoints = 0,
                        Wins = tp.Wins,
                        Draws = tp.Draws,
                        Losses = tp.Losses,
                        GamePath = tp.GamePath,
                        OpponentBattlePoints = context.TKGame.Where(tp2 => tp2.Player1Id == tp.Id && tp2.TournamentId == id).ToList().Count > 0 ? context.TKGame.Where(tp2 => tp2.Player1Id == tp.Id && tp2.TournamentId == id).Sum(tp2 => tp2.TKTournamentPlayer1.BattlePoints) : 0 +
                            context.TKGame.Where(tp2 => tp2.Player2Id == tp.Id && tp2.TournamentId == id).ToList().Count > 0 ? context.TKGame.Where(tp2 => tp2.Player2Id == tp.Id && tp2.TournamentId == id).ToList().Sum(tp2 => tp2.TKTournamentPlayer.BattlePoints) : 0
                    })
                    .OrderByDescending(tp => tp.Wins)
                    .ThenByDescending(tp => tp.Draws)
                    .ThenBy(tp => tp.Losses)
                    .ThenByDescending(tp => tp.BattlePoints)
                    .ThenByDescending(tp => tp.OpponentBattlePoints)
                    .ToList();

                if (data.Count > 0)
                    data[0].PlacingPoints = 100;
                if (data.Count > 1)
                    data[1].PlacingPoints = 90;
                if (data.Count > 2)
                    data[2].PlacingPoints = 85;
                if (data.Count > 3)
                    data[3].PlacingPoints = 80;
                if (data.Count > 4)
                    data[4].PlacingPoints = 75;
                if (data.Count > 5)
                    data[5].PlacingPoints = 70;
                if (data.Count > 6)
                    data[6].PlacingPoints = 65;
                if (data.Count > 7)
                    data[7].PlacingPoints = 60;

                var numPlayers = data.Count;
                var top10pct = (int)(((float)numPlayers - 8) * 0.1f);
                var next20pct = (int)(((float)numPlayers - 8) * 0.2f);
                var next30pct = (int)(((float)numPlayers - 8) * 0.3f);
                var last40pct = numPlayers - top10pct - next20pct - next30pct;

                for (int i = 8; i < 8 + top10pct; i++)
                {
                    data[i].PlacingPoints = 55;
                }

                for (int i = 8 + top10pct; i < 8 + top10pct + next20pct; i++)
                {
                    data[i].PlacingPoints = 40;
                }

                for (int i = 8 + top10pct + next20pct; i < 8 + top10pct + next20pct + next30pct; i++)
                {
                    data[i].PlacingPoints = 30;
                }

                for (int i = 8 + top10pct + next20pct + next30pct; i < data.Count; i++)
                {
                    data[i].PlacingPoints = 20;
                }

                for (int i = 0; i < data.Count; i++)
                {
                    var games = data[i].GamePath.Split('-');
                    for (int j = 0; j < games.Length; j++)
                    {
                        if (games[j].Equals("w"))
                            data[i].BonusPoints += 3;
                        if (games[j].Equals("d"))
                            data[i].BonusPoints += 1;
                        if (games[j].Equals("l"))
                            j = games.Length;
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (var d in data)
                {
                    sb.AppendLine($"{d.Email}\t{d.FirstName}\t{d.LastName}\t{d.Club}\t{d.PrimaryCodex}\t{d.BonusPoints}");
                }

                ExportDataTextBox.Text = sb.ToString();
            }
        }

        private string GetFirstName(string playerName)
        {
            var split = playerName.Split(' ');
            if (split.Length == 0)
            {
                return "";
            }
            if (split.Length == 1)
            {
                return split[0];
            }
            return string.Join(" ", split.Take(split.Length - 1));
        }

        private string GetLastName(string playerName)
        {
            var split = playerName.Split(' ');
            return split[split.Length - 1];
        }
    }
}