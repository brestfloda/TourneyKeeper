using System.Collections.Generic;
using System.Linq;

namespace TourneyKeeper.Common.Managers.TableGenerators
{
    public class TableGeneratorLinear : ITableGenerator
    {
        public int? GetTable(List<TKTeamMatch> teamMatches, List<int?> tables, TKTournamentTeam team1, TKTournamentTeam team2)
        {
            var table = tables.FirstOrDefault();
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
            var table = tables.FirstOrDefault();
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
