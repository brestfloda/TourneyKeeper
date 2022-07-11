using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using TourneyKeeper.Common;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class ParseArmies
    {
        int tournamentId = 2471;

        [TestMethod]
        public void DoParse()
        {
            var file = File.ReadAllLines(@"c:\tmp\final.txt");
            var armies = ParseAllArmies(file);
            var g = armies.GroupBy(a => a.Team)
                .Select(b => new
                {
                    Team = b.Key,
                    Count = b.Count()
                });
            File.WriteAllLines(@"c:\tmp\Armies.csv", new string[] { "Team;Player;Codex" }.Concat(armies.OrderBy(a => a.Team).Select(a => $"{a.Team};{a.Player};{a.Codex?.Name}")));

            using (var context = new TourneyKeeperEntities())
            {
                foreach (var army in armies)
                {
                    if(army.Codex == null)
                    {
                        continue;
                    }

                    var tp = context.TKTournamentPlayer.Single(t => t.Id == army.Player.Id);
                    tp.PrimaryCodexId = army.Codex.Id;
                    tp.ArmyList = army.Armylist;
                    context.SaveChanges();
                }
            }
        }

        public List<Army> ParseAllArmies(string[] file)
        {
            List<Army> allArmies = new List<Army>();

            for (int i = 0; i < file.Length; i++)
            {
                var army = FindNextArmy(file, ref i);
                if (army != null)
                {
                    allArmies.Add(army);
                }
            }

            return allArmies;
        }

        private Army FindNextArmy(string[] allText, ref int start)
        {
            var startAndEnd = FindArmyStartAndEnd(allText, start);
            if (startAndEnd.Item1 == int.MaxValue)
            {
                return null;
            }
            var team = GetTeam(allText, startAndEnd.Item1 + 1);
            var army = new StringBuilder();
            for (int i = startAndEnd.Item1; i < startAndEnd.Item2; i++)
            {
                army.AppendLine(allText[i]);
            }
            var player = GetPlayer(allText, startAndEnd.Item1, startAndEnd.Item2);
            var primaryCodex = GetPrimaryCodex(allText, startAndEnd.Item1, startAndEnd.Item2);

            start = startAndEnd.Item2;

            return new Army { Team = team, Armylist = army.ToString(), Codex = primaryCodex, Player = player };
        }

        private string GetTeam(string[] allText, int v)
        {
            var split = allText[v].Split(':');
            return split[1].Trim().ToUpper();
        }

        private TKCodex GetPrimaryCodex(string[] allText, int start, int end)
        {
            try
            {
                for (int i = start; i < end; i++)
                {
                    if (allText[i].ToUpper().Contains("ARMY FACTION"))
                    {
                        var split = allText[i].Split(':');
                        if (split.Length == 2)
                        {
                            using (var context = new TourneyKeeperEntities())
                            {
                                var name = split[1].Trim();
                                var codex = context.TKCodex.SingleOrDefault(c => c.GameSystemId == 1 && c.Active == true && c.Name.Equals(name));
                                return codex;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private TKTournamentPlayer GetPlayer(string[] allText, int start, int end)
        {
            try
            {
                for (int i = start; i < end; i++)
                {
                    if (allText[i].ToUpper().Contains("PLAYER TOURNEYKEEPER PROFILE"))
                    {
                        var split = allText[i].Split(':');
                        var split2 = split[1].ToUpper().Replace("PLAYERID=", "").Replace("(", "").Replace(")", "").Split(',');
                        if (split2.Length == 2)
                        {
                            if (int.TryParse(split2[1], out int id))
                            {
                                using (var context = new TourneyKeeperEntities())
                                {
                                    var player = context.TKTournamentPlayer.SingleOrDefault(tp => tp.TournamentId == tournamentId && tp.TKPlayer.Id == id);
                                    return player;
                                }
                            }
                        }
                        else
                        {
                            using (var context = new TourneyKeeperEntities())
                            {
                                int.TryParse(split2[0].Trim(), out int playerId);
                                var player = context.TKTournamentPlayer.SingleOrDefault(tp => tp.TournamentId == tournamentId && (tp.PlayerName == split2[0].Trim() || tp.PlayerId == playerId || tp.TKPlayer.Username == split2[0].Trim()));
                                return player;
                            }
                        }
                    }
                }

                return null;
            }
            catch //(Exception e)
            {
                return null;
            }
        }

        private Tuple<int, int> FindArmyStartAndEnd(string[] allText, int start)
        {
            bool startFound = false;
            bool endFound = false;
            int startArmy = 0;
            int endArmy = 0;

            while (!endFound && start < allText.Length)
            {
                if (allText[start].ToUpper().StartsWith("+") && allText[start].ToUpper().Contains("TEAM") && !startFound)
                {
                    startArmy = start - 1;
                    startFound = true;
                }
                else if (startFound && allText[start].ToUpper().StartsWith("+") && allText[start].ToUpper().Contains("TEAM"))
                {
                    endArmy = start - 2;
                    endFound = true;
                }

                if (allText[start].ToUpper().Contains("ARMY REINFORCEMENT FACTION"))
                {
                    endArmy = start + 1;
                    endFound = true;
                }

                start++;
            }

            if (startFound && start >= allText.Length)
            {
                return new Tuple<int, int>(startArmy, start);
            }

            if (!startFound && !endFound)
            {
                return new Tuple<int, int>(int.MaxValue, int.MaxValue);
            }

            return new Tuple<int, int>(startArmy, endArmy);
        }
    }

    public class Army
    {
        public override string ToString()
        {
            return $"{Team} - {Player} - {Codex?.Name}";
        }

        public string Armylist { get; set; }
        public TKCodex Codex { get; set; }
        public string Team { get; set; }
        public TKTournamentPlayer Player { get; set; }
    }
}
