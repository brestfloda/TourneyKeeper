using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using Excel = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab

namespace TourneyKeeper.Test
{
    [TestClass]
    public class ImportPlayers
    {
        public class PlayerImport
        {
            public int TeamTournamentId { get; set; }
            public int SinglesTournamentId { get; set; }
            public object PlayerIdObj { get; set; }
            public int PlayerId { get; set; }
            public string Role { get; set; }
            public bool ESC { get; set; }
            public int TeamId { get; set; }
        }

        [TestMethod]
        public void ImportT9A()
        {
            var players = new List<PlayerImport>();

            //GetPlayers(players, 2473, 2482, @"C:\Users\mbn\OneDrive\Documents\ETC TK\T9A");
            //GetPlayers(players, 2471, 2481, @"C:\Users\mbn\OneDrive\Documents\ETC TK\40K");
            //GetPlayers(players, 2476, 2485, @"C:\Users\mbn\OneDrive\Documents\ETC TK\AoS");
            GetPlayers(players, 2474, 2483, @"C:\Users\mbn\OneDrive\Documents\ETC TK\FoW");
            //GetPlayers(players, 2475, 2484, @"C:\Users\mbn\OneDrive\Documents\ETC TK\XWing");

            var tpManager = new TournamentPlayerManager();
            using (var context = new TourneyKeeperEntities())
            {
                foreach (var player in players)
                {
                    if (player.Role != null && player.Role.Trim().Equals("Player", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var px = context.TKTournamentPlayer.SingleOrDefault(p => p.PlayerId == player.PlayerId && p.TournamentId == player.TeamTournamentId);
                        if (px == null)
                        {
                            var tkp = context.TKPlayer.SingleOrDefault(p => p.Id == player.PlayerId);
                            if (tkp != null)
                            {
                                var tournament = context.TKTournament.SingleOrDefault(t => t.Id == player.TeamTournamentId);
                                var newPlayer = new TKTournamentPlayer()
                                {
                                    TournamentId = player.TeamTournamentId,
                                    PlayerId = player.PlayerId,
                                    PlayerName = tkp.Name,
                                    DoNotRank = true,
                                    Active = true,
                                    TournamentTeamId = player.TeamId
                                };

                                tpManager.AddTournamentPlayer(newPlayer);
                            }
                        }
                    }
                    else if (player.Role != null && player.Role.Trim().Equals("Coach", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var px = context.TKTournamentPlayer.SingleOrDefault(p => p.PlayerId == player.PlayerId && p.TournamentId == player.TeamTournamentId);
                        if (px == null)
                        {
                            var tkp = context.TKPlayer.SingleOrDefault(p => p.Id == player.PlayerId);
                            if (tkp != null)
                            {
                                var tournament = context.TKTournament.SingleOrDefault(t => t.Id == player.TeamTournamentId);
                                var newPlayer = new TKTournamentPlayer()
                                {
                                    TournamentId = player.TeamTournamentId,
                                    PlayerId = player.PlayerId,
                                    PlayerName = tkp.Name,
                                    DoNotRank = true,
                                    Active = true,
                                    TournamentTeamId = player.TeamId,
                                    NonPlayer = true,
                                };

                                tpManager.AddTournamentPlayer(newPlayer);
                            }
                        }
                    }

                    if (player.ESC)
                    {
                        var px = context.TKTournamentPlayer.SingleOrDefault(p => p.PlayerId == player.PlayerId && p.TournamentId == player.SinglesTournamentId);
                        if (px != null && !px.Paid)
                        {
                            px.Paid = true;
                            tpManager.Update(px, "328d7bd7-21be-467c-91f3-c6c164584027");
                        }
                        else if (px == null)
                        {
                            var tkp = context.TKPlayer.SingleOrDefault(p => p.Id == player.PlayerId);
                            if (tkp != null)
                            {
                                var newPlayer = new TKTournamentPlayer()
                                {
                                    TournamentId = player.SinglesTournamentId,
                                    PlayerId = player.PlayerId,
                                    PlayerName = tkp.Name,
                                    DoNotRank = true,
                                    Active = true,
                                };

                                tpManager.AddTournamentPlayer(newPlayer);
                            }
                        }
                    }
                }
            }
        }

        private void GetPlayers(List<PlayerImport> players, int teamId, int singlesId, string folder)
        {
            foreach (var file in Directory.EnumerateFiles(folder))
            {
                players.AddRange(ReadFile(teamId, singlesId, file));
            }
        }

        public List<PlayerImport> ReadFile(int teamTournamentId, int singlesTournamentId, string file)
        {
            Excel.Application xlApp = null;
            Excel.Workbook xlWorkbook = null;
            Excel._Worksheet xlWorksheet = null;
            Excel.Range xlRange = null;
            try
            {
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(file);
                xlWorksheet = xlWorkbook.Sheets[1];
                xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                var players = new List<PlayerImport>();
                int teamId = int.Parse(Path.GetFileNameWithoutExtension(file));

                for (int i = 4; i <= rowCount; i++)
                {
                    if (xlRange.Cells[i, 1].Value == null)
                    {
                        break;
                    }

                    var tmp = new PlayerImport
                    {
                        ESC = xlRange.Cells[i, 6].Value == "Yes",
                        PlayerIdObj = (object)xlRange.Cells[i, 3].Value,
                        Role = xlRange.Cells[i, 4].Value,
                        TeamTournamentId = teamTournamentId,
                        SinglesTournamentId = singlesTournamentId,
                        TeamId = teamId
                    };

                    if (tmp.PlayerIdObj != null)
                    {
                        if (int.TryParse(tmp.PlayerIdObj.ToString(), out var p))
                        {
                            tmp.PlayerId = p;
                        }

                        players.Add(tmp);
                    }
                }

                return players;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
        }
    }
}