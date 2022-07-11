using System.Collections.Generic;
using System.Linq;

namespace TourneyKeeper.Common.Managers.TableGenerators
{
    public class TableGeneratorUnique : ITableGenerator
    {
        public int? GetTable(List<TKTeamMatch> teamMatches, List<int?> tables, TKTournamentTeam team1, TKTournamentTeam team2)
        {
            var tablesPlayed = teamMatches
                .Where(tm => tm.Team1Id == team1.Id || tm.Team1Id == team2.Id || tm.Team2Id == team1.Id || tm.Team2Id == team2.Id)
                .Select(g => g.TableNumber).ToList();
            var table = tables.FirstOrDefault(t => !tablesPlayed.Any(o => o == t));
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
            var tablesPlayed = games == null ? new List<int?>() : games.Where(g => g.Player1Id == player1.Id || g.Player1Id == player2.Id || g.Player2Id == player1.Id || g.Player2Id == player2.Id).Select(g => g.TableNumber).ToList();
            var table = tables.FirstOrDefault(t => !tablesPlayed.Any(o => o == t));
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
