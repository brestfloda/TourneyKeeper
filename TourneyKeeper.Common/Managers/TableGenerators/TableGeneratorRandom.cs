using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;

namespace TourneyKeeper.Common.Managers.TableGenerators
{
    public class TableGeneratorRandom : ITableGenerator
    {
        public int? GetTable(List<TKTeamMatch> teamMatches, List<int?> tables, TKTournamentTeam team1, TKTournamentTeam team2)
        {
            var random = new Random();
            var table = tables[random.Next(0, tables.Count)];
            if (table.HasValue)
            {
                tables.Remove(table.Value);
            }
            else
            {
                table = tables.FirstOrDefault();
                if (table.HasValue)
                {
                    tables.Remove(table.Value);
                }
            }
            return table;
        }

        public int? GetTable(List<int?> tables, TKTournamentPlayer player1, TKTournamentPlayer player2, List<TKGame> games = null)
        {
            var random = new Random();
            var table = tables[random.Next(0, tables.Count)];
            if (table.HasValue)
            {
                tables.Remove(table.Value);
            }
            else
            {
                table = tables.FirstOrDefault();
                if (table.HasValue)
                {
                    tables.Remove(table.Value);
                }
            }

            return table;
        }
    }
}
