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
    public interface ITableGenerator
    {
        int? GetTable(List<TKTeamMatch> teamMatches, List<int?> tables, TKTournamentTeam team1, TKTournamentTeam team2);
        int? GetTable(List<int?> tables, TKTournamentPlayer player1, TKTournamentPlayer player2, List<TKGame> games = null);
    }
}
