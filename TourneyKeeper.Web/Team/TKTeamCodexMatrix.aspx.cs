using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;

namespace TourneyKeeper.Web
{
    public partial class TKTeamCodexMatrix : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TourneyKeeper - Codex Matrix";

            var id = General.GetParam<int>("Id");
            BindData(id);
        }

        private void BindData(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                TKTournament tournament = context.TKTournament.Single(t => t.Id == id);

                General.RedirectToFrontIfNotReady(tournament);

                var result = new Dictionary<TKCodex, Dictionary<TKCodex, string>>();

                var games = context.TKGame
                    .Include(t => t.TKTournamentPlayer.TKCodex)
                    .Include(t => t.TKTournamentPlayer1.TKCodex)
                    .Where(g => g.TournamentId == id && g.TKTournamentPlayer.TKCodex != null && g.TKTournamentPlayer1.TKCodex != null).ToList();

                var allCodex = (games
                    .Where(g => g.TournamentId == id)
                    .Select(g => g.TKTournamentPlayer.TKCodex)
                    .Concat(games
                    .Where(g => g.TournamentId == id)
                    .Select(g => g.TKTournamentPlayer1.TKCodex))).Distinct().OrderBy(c => c.Name).ToList();

                for (int i = 0; i < allCodex.Count; i++)
                {
                    result.Add(allCodex[i], new Dictionary<TKCodex, string>());

                    for (int k = 0; k < allCodex.Count; k++)
                    {
                        var allWin = games
                            .Where(g => g.TournamentId == id && g.TKTournamentPlayer.TKCodex == allCodex[i] && g.TKTournamentPlayer1.TKCodex == allCodex[k] && g.Player1Result > g.Player2Result)
                            .Concat(games
                            .Where(g => g.TournamentId == id && g.TKTournamentPlayer1.TKCodex == allCodex[i] && g.TKTournamentPlayer.TKCodex == allCodex[k] && g.Player2Result > g.Player1Result))
                            .Count();
                        var allGames = games
                            .Where(g => g.TournamentId == id && g.TKTournamentPlayer.TKCodex == allCodex[i] && g.TKTournamentPlayer1.TKCodex == allCodex[k])
                            .Concat(games
                            .Where(g => g.TournamentId == id && g.TKTournamentPlayer1.TKCodex == allCodex[i] && g.TKTournamentPlayer.TKCodex == allCodex[k]))
                            .Count();

                        var perc = (100.0f * (float)allWin) / ((float)allGames);
                        if (float.IsNaN(perc))
                            result[allCodex[i]].Add(allCodex[k], "-");
                        else
                            result[allCodex[i]].Add(allCodex[k], $"{((int)perc)}% ({allGames})");
                    }
                }

                var cNames = new string[] { " " }.Concat(result.Keys.Select(k => string.IsNullOrEmpty(k.Name) ? "Blank" : k.Name)).ToList();
                DataTable table = new DataTable();
                foreach (var name in cNames)
                {
                    DataColumn column;
                    column = new DataColumn();
                    column.DataType = typeof(string);
                    column.ColumnName = name;
                    table.Columns.Add(column);
                }

                int j = 0;
                foreach (var rowData in result)
                {
                    DataRow row2 = table.NewRow();
                    var tmp = rowData.Value.ToList();
                    row2[0] = string.IsNullOrEmpty(rowData.Key.Name) ? "Blank" : rowData.Key.Name;
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        row2[i + 1] = i == j ? "-" : tmp[i].Value;
                    }
                    table.Rows.Add(row2);
                    j++;
                }

                LeaderboardGridView.DataSource = table;
                LeaderboardGridView.DataBind();
            }
        }

        protected void LeaderboardGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[i].VerticalAlign = VerticalAlign.Middle;
                }
            }
        }
    }
}