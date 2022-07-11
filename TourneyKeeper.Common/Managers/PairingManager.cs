using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using TourneyKeeper.Common.SharedCode;
using System.Data.Entity;
using System.Diagnostics;
using TourneyKeeper.Common.Managers.TableGenerators;

namespace TourneyKeeper.Common.Managers
{
    public class PairingManager
    {
        private ITableGenerator tableGenerator;

        public PairingManager(TableGenerator tableGenerator = TableGenerator.Unique)
        {
            this.tableGenerator = TableGeneratorFactory.GetGenerator(tableGenerator);
        }

        public List<string> TeamDraw(int tournamentId, DrawTypeEnum drawType, string token)
        {
            List<string> warnings = new List<string>();
            TKTournament tournament;
            List<TKTeamMatch> teamMatches;
            int creatingRound;

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                Security.CheckToken(token, tournamentId, context);

                tournament = context.TKTournament.Single(t => t.Id == tournamentId);
                var tournamentTeams = context.TKTournamentTeam
                    .Where(t => t.TournamentId == tournamentId)
                    .OrderByDescending(tp => tp.MatchPoints)
                    .ThenByDescending(tp => tp.BattlePoints)
                    .ThenByDescending(tp2 => tp2.SecondaryPoints).ToList();
                var oldTeamMatches = context.TKTeamMatch.Where(tm => tm.TournamentId == tournamentId).ToList();

                TeamDrawSanityCheck(tournamentId, tournament.TeamSize, tournamentTeams);

                teamMatches = new List<TKTeamMatch>();
                var tables = Enumerable.Range(1, tournamentTeams.Count / 2).Cast<int?>().ToList();

                int? val = context.TKTeamMatch.Where(g => g.TKTournament.Id == tournamentId).Max(g => (int?)g.Round);
                creatingRound = val.HasValue ? val.Value + 1 : 1;

                switch (drawType)
                {
                    case DrawTypeEnum.RandomDraw:
                        TeamRandom(tournamentId, teamMatches, tournamentTeams, oldTeamMatches, tables, creatingRound);
                        break;
                    case DrawTypeEnum.SwissDraw:
                        TeamSwiss(tournamentId, teamMatches, tournamentTeams, oldTeamMatches, tables, creatingRound, warnings);
                        break;
                }

                warnings = warnings.Concat(CheckAndSaveTeamRound(context, tournamentTeams, teamMatches, oldTeamMatches, tournamentId, creatingRound)).ToList();
            }

            NotifyTeamDraw(tournamentId, creatingRound);

            return warnings;
        }

        private void NotifyTeamDraw(int tournamentId, int creatingRound)
        {
            try
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var matches = context.TKTeamMatch
                        .Where(m => m.Round == creatingRound && m.TournamentId == tournamentId);

                    foreach (var match in matches)
                    {
                        var players1 = context.TKTournamentPlayer.Where(tp => tp.TournamentTeamId == match.Team1Id && tp.TKPlayer.AppNotificationToken != null).ToList();
                        foreach (var player in players1)
                        {
                            NotificationManager.NotifyNewTeamMatch(match.TKTournament.Name, match.TKTournamentTeam1?.Name, match.TableNumber?.ToString(), player.TKPlayer.AppNotificationToken);
                        }
                        var players2 = context.TKTournamentPlayer.Where(tp => tp.TournamentTeamId == match.Team2Id && tp.TKPlayer.AppNotificationToken != null).ToList();
                        foreach (var player in players2)
                        {
                            NotificationManager.NotifyNewTeamMatch(match.TKTournament.Name, match.TKTournamentTeam?.Name, match.TableNumber?.ToString(), player.TKPlayer.AppNotificationToken);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
            }
        }

        private void TeamRandom(int tournamentId, List<TKTeamMatch> teamMatches, List<TKTournamentTeam> tournamentTeams, List<TKTeamMatch> oldTeamMatches, List<int?> tables, int creatingRound)
        {
            var random = new Random();

            while (tournamentTeams.Count > 1)
            {
                int r = random.Next(0, tournamentTeams.Count);
                TKTournamentTeam team1 = tournamentTeams[r];
                tournamentTeams.RemoveAt(r);
                r = random.Next(0, tournamentTeams.Count);
                TKTournamentTeam team2 = tournamentTeams[r];
                tournamentTeams.RemoveAt(r);

                TKTeamMatch teamMatch = new TKTeamMatch()
                {
                    TKTournamentTeam = team1,
                    TKTournamentTeam1 = team2,
                    Round = creatingRound,
                    TableNumber = tableGenerator.GetTable(oldTeamMatches, tables, team1, team2),
                    TournamentId = tournamentId
                };

                teamMatches.Add(teamMatch);
            }
        }

        private void TeamSwiss(int tournamentId, List<TKTeamMatch> teamMatches, List<TKTournamentTeam> tournamentTeams, List<TKTeamMatch> oldTeamMatches, List<int?> tables, int creatingRound, List<string> warnings)
        {
            tournamentTeams = RandomizeTeams(tournamentTeams);

            while (tournamentTeams.Count > 1)
            {
                TKTournamentTeam team1 = tournamentTeams[0];
                tournamentTeams.RemoveAt(0);

                List<int> opponents;
                opponents = oldTeamMatches.Where(g => g.Team1Id == team1.Id).Select(g => g.Team2Id).ToList();
                opponents = opponents.Concat(oldTeamMatches.Where(g => g.Team2Id == team1.Id).Select(g => g.Team1Id)).ToList();

                TKTournamentTeam team2 = tournamentTeams.FirstOrDefault(tp => !opponents.Any(o => o == tp.Id));
                if (team2 == null)
                {
                    team2 = tournamentTeams.First();
                }
                tournamentTeams.Remove(team2);

                TKTeamMatch teamMatch = new TKTeamMatch()
                {
                    TKTournamentTeam = team1,
                    TKTournamentTeam1 = team2,
                    Round = creatingRound,
                    TableNumber = tableGenerator.GetTable(oldTeamMatches, tables, team1, team2),
                    TournamentId = tournamentId
                };

                teamMatches.Add(teamMatch);
            }
        }

        private static List<TKTournamentTeam> RandomizeTeams(List<TKTournamentTeam> teamMatches)
        {
            var teamSet = new Dictionary<Tuple<int, int, int>, List<TKTournamentTeam>>();
            foreach (var tm in teamMatches)
            {
                var key = new Tuple<int, int, int>(tm.MatchPoints, tm.BattlePoints, tm.SecondaryPoints);
                if (!teamSet.ContainsKey(key))
                {
                    teamSet.Add(key, new List<TKTournamentTeam>());
                }
                teamSet[key].Add(tm);
            }

            var random = new Random();
            var teamsRandomized = new List<TKTournamentTeam>();
            foreach (var keySet in teamSet.OrderByDescending(p => p.Key.Item1).ThenByDescending(p => p.Key.Item2).ThenByDescending(p => p.Key.Item3))
            {
                while (keySet.Value.Count > 0)
                {
                    var rp = random.Next(0, keySet.Value.Count);
                    var tmpTeam = keySet.Value[rp];
                    keySet.Value.RemoveAt(rp);
                    teamsRandomized.Add(tmpTeam);
                }
            }

            return teamsRandomized;
        }

        private static void TeamDrawSanityCheck(int tournamentId, int? teamSize, List<TKTournamentTeam> teams)
        {
            if (teams.Count == 0)
            {
                throw new ApplicationException("No teams");
            }

            if (teamSize.HasValue)
            {
                string teamSizeProblem = "";
                foreach (TKTournamentTeam t in teams)
                {
                    if (t.TKTournamentPlayer.Where(tp => !tp.NonPlayer).Count() != teamSize)
                    {
                        teamSizeProblem += $"{t.Name},";
                    }
                }
                if (!string.IsNullOrEmpty(teamSizeProblem))
                {
                    throw new ApplicationException($"Not all teams match size criteria. Teams causing troble are: {teamSizeProblem.Substring(0, teamSizeProblem.Length - 1)}");
                }
            }

            var teamBaseline = teams.First(t => t.TournamentId == tournamentId);
            int playerCount = teamBaseline.TKTournamentPlayer.Where(tp => !tp.NonPlayer).Count();
            foreach (TKTournamentTeam t in teams)
            {
                if (t.TKTournamentPlayer.Where(tp => !tp.NonPlayer).Count() != playerCount)
                {
                    throw new ApplicationException($"Not all teams are of even size. Teams causing troble could be: {t.Name} or {teamBaseline.Name}");
                }
            }
        }

        public void AddLateComers(int tournamentId, string token)
        {
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                Security.CheckToken(token, tournamentId, context);

                int? round = context.TKGame.Where(g => g.TKTournament.Id == tournamentId).Max(g => (int?)g.Round);
                if ((round ?? 0) == 1)
                {
                    var playersWithNoGame = context.TKTournamentPlayer.Where(tp => tp.TournamentId == tournamentId && (tp.TKGame.Count + tp.TKGame1.Count) == 0).ToList();

                    if (playersWithNoGame.Count % 2 == 1)
                    {
                        throw new ApplicationException("Uneven amount of players, add a standin player");
                    }

                    var maxTable = (context.TKGame.Where(g => g.TournamentId == tournamentId).Max(g => g.TableNumber)) ?? 0;
                    var random = new Random(DateTime.Now.Second);
                    while (playersWithNoGame.Count > 1)
                    {
                        int r = random.Next(0, playersWithNoGame.Count);
                        var player1 = playersWithNoGame[r];
                        playersWithNoGame.RemoveAt(r);
                        r = random.Next(0, playersWithNoGame.Count);
                        var player2 = playersWithNoGame[r];
                        playersWithNoGame.RemoveAt(r);

                        var game = new TKGame
                        {
                            Player1Id = player1.Id,
                            Player2Id = player2.Id,
                            Round = 1,
                            TournamentId = tournamentId,
                            TableNumber = ++maxTable
                        };

                        context.TKGame.Add(game);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new ApplicationException("You can only add latecomers for the first round. Add the players and redraw for later rounds.");
                }
            }
        }

        public List<string> SinglesDraw(int tournamentId, DrawTypeEnum drawType, IEnumerable<PairingOption> options, string token)
        {
            List<string> messages = null;
            int creatingRound;
            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                Security.CheckToken(token, tournamentId, context);

                var tournament = context.TKTournament
                    .Include(t => t.TKSinglesScoringSystem)
                    .Single(t => t.Id == tournamentId);
                var warnings = new List<string>();
                if (tournament.UseSeed && (options.Contains(PairingOption.DoNotPairClubMembers) || options.Contains(PairingOption.DoNotPairCountrymen)))
                {
                    warnings.Add("You cannot use both Seed, Do Not Pair Club Members or Do Not Pair Countrymen at the same time");
                    return warnings;
                }
                if (drawType != DrawTypeEnum.RandomDraw && (options.Contains(PairingOption.DoNotPairClubMembers) || options.Contains(PairingOption.DoNotPairCountrymen)))
                {
                    warnings.Add("You can only use Do Not Pair Club Members or Do Not Pair Countrymen when doing a random draw");
                    return warnings;
                }
                var tournamentPlayers = context.TKTournamentPlayer
                    .Where(tp => tp.TournamentId == tournamentId && tp.Active)
                    .OrderByDescending(tp => tp.BattlePoints)
                    .ThenByDescending(tp2 => tp2.SecondaryPoints).ToList();
                var oldGames = context.TKGame.Where(g => g.TournamentId == tournamentId).ToList();
                var tables = Enumerable.Range(1, tournamentPlayers.Count / 2).Cast<int?>().ToList();
                var games = new List<TKGame>();

                var val = context.TKGame.Where(g => g.TKTournament.Id == tournamentId).Max(g => (int?)g.Round);
                creatingRound = val.HasValue ? val.Value + 1 : 1;

                switch (drawType)
                {
                    case DrawTypeEnum.RandomDraw:
                        SingleRandom(tournament, tournamentPlayers, oldGames, tables, games, creatingRound, warnings, options);
                        break;
                    case DrawTypeEnum.SwissDraw:
                        SingleSwiss(tournament, tournamentPlayers, oldGames, tables, games, creatingRound, warnings, options);
                        break;
                }

                messages = warnings.Concat(CheckAndSaveSinglesRound(context, tournamentPlayers, games, oldGames)).ToList();
            }

            NotifyPlayers(tournamentId, creatingRound);

            return messages;
        }

        private void NotifyPlayers(int tournamentId, int round)
        {
            try
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var games = context.TKGame
                        .Where(
                            g => g.TournamentId == tournamentId &&
                            g.Round == round &&
                            ((g.TKTournamentPlayer != null && g.TKTournamentPlayer.TKPlayer.AppNotificationToken != null) ||
                            (g.TKTournamentPlayer1 != null && g.TKTournamentPlayer1.TKPlayer.AppNotificationToken != null)))
                        .ToList();

                    NotificationManager.NotifyNewGame(games);
                }
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
            }
        }

        private void SingleRandom(TKTournament tournament, List<TKTournamentPlayer> tmpTournamentPlayers, List<TKGame> oldGames, List<int?> tables, List<TKGame> games, int creatingRound, List<string> warnings, IEnumerable<PairingOption> options)
        {
            var random = new Random(DateTime.Now.Second);

            if (options.Contains(PairingOption.DoNotPairClubMembers))
            {
                SingleRandomWithDoNotPairClubMembers(tournament, tmpTournamentPlayers, oldGames, tables, games, creatingRound, warnings, random);
            }
            else if (options.Contains(PairingOption.DoNotPairCountrymen))
            {
                SingleRandomWithDoNotPairCountrymen(tournament, tmpTournamentPlayers, oldGames, tables, games, creatingRound, warnings, random);
            }
            else if (tournament.UseSeed)
            {
                SingleRandomWithSeed(tournament, tmpTournamentPlayers, oldGames, tables, games, creatingRound, random);
            }
            else
            {
                while (tmpTournamentPlayers.Count > 1)
                {
                    int r = random.Next(0, tmpTournamentPlayers.Count);
                    var player1 = tmpTournamentPlayers[r];
                    tmpTournamentPlayers.RemoveAt(r);
                    r = random.Next(0, tmpTournamentPlayers.Count);
                    var player2 = tmpTournamentPlayers[r];
                    tmpTournamentPlayers.RemoveAt(r);

                    AddGame(tournament.Id, oldGames, tables, games, creatingRound, player1, player2);
                }
            }
        }

        private void SingleRandomWithSeed(TKTournament tournament, List<TKTournamentPlayer> tmpTournamentPlayers, List<TKGame> oldGames, List<int?> tables, List<TKGame> games, int creatingRound, Random random)
        {
            var pairingLayers = tmpTournamentPlayers.GroupBy(tp => tp.Seed).OrderBy(g => g.Key).ToDictionary(g => g.Key, g => g.ToList());
            var keys = pairingLayers.Keys.ToList();
            int j = 0;

            for (int i = keys[j]; keys.Contains(i); i = keys.Count == j + 1 ? int.MaxValue : keys[++j])
            {
                while (pairingLayers[i].Count > 1)
                {
                    int r = random.Next(0, pairingLayers[i].Count);
                    var player1 = pairingLayers[i][r];
                    pairingLayers[i].RemoveAt(r);
                    r = random.Next(0, pairingLayers[i].Count);
                    var player2 = pairingLayers[i][r];
                    pairingLayers[i].RemoveAt(r);

                    AddGame(tournament.Id, oldGames, tables, games, creatingRound, player1, player2);
                }

                if (pairingLayers[i].Count == 1)
                {
                    pairingLayers[keys[j + 1]] = new TKTournamentPlayer[] { pairingLayers[i][0] }.Concat(pairingLayers[keys[j + 1]]).ToList();
                    pairingLayers[i].RemoveAt(0);
                }
            }
        }

        private void SingleRandomWithDoNotPairClubMembers(TKTournament tournament, List<TKTournamentPlayer> tmpTournamentPlayers, List<TKGame> oldGames, List<int?> tables, List<TKGame> games, int creatingRound, List<string> warnings, Random random)
        {
            var pairingLayers = tmpTournamentPlayers
                .GroupBy(tp => tp.Club)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key ?? "None", g => new { Count = g.Count(), List = g.ToList() })
                .OrderByDescending(g => g.Value.Count)
                .ToDictionary(g => g.Key, g => g.Value);
            var keys = pairingLayers.Keys.ToList();

            while (keys.Count > 0 && pairingLayers.Count > 1)
            {
                var player1 = pairingLayers[keys[0]].List[0];
                pairingLayers[keys[0]].List.RemoveAt(0);
                var layerRandom = random.Next(1, pairingLayers.Count);
                var player2Random = random.Next(0, pairingLayers[keys[layerRandom]].List.Count);
                var player2 = pairingLayers[keys[layerRandom]].List[player2Random];
                pairingLayers[keys[layerRandom]].List.RemoveAt(player2Random);

                if (pairingLayers[keys[layerRandom]].List.Count == 0)
                {
                    pairingLayers.Remove(keys[layerRandom]);
                    keys.Remove(keys[layerRandom]);
                }
                if (pairingLayers[keys[0]].List.Count == 0)
                {
                    pairingLayers.Remove(keys[0]);
                    keys.Remove(keys[0]);
                }

                AddGame(tournament.Id, oldGames, tables, games, creatingRound, player1, player2);
            }
            while (keys.Count > 0 && pairingLayers[keys[0]].List.Count > 1)
            {
                var player1 = pairingLayers[keys[0]].List[0];
                pairingLayers[keys[0]].List.RemoveAt(0);
                var player2Random = random.Next(0, pairingLayers[keys[0]].List.Count);
                var player2 = pairingLayers[keys[0]].List[player2Random];
                pairingLayers[keys[0]].List.RemoveAt(player2Random);

                AddGame(tournament.Id, oldGames, tables, games, creatingRound, player1, player2);
                warnings.Add($"Players from same club paired: {player1.PlayerName} and {player2.PlayerName}");
            }
        }

        private void SingleRandomWithDoNotPairCountrymen(TKTournament tournament, List<TKTournamentPlayer> tmpTournamentPlayers, List<TKGame> oldGames, List<int?> tables, List<TKGame> games, int creatingRound, List<string> warnings, Random random)
        {
            var pairingLayers = tmpTournamentPlayers
                .GroupBy(tp => tp.TKPlayer.Country)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => new { Count = g.Count(), List = g.ToList() })
                .OrderByDescending(g => g.Value.Count)
                .ToDictionary(g => g.Key, g => g.Value);
            var keys = pairingLayers.Keys.ToList();

            while (keys.Count > 0 && pairingLayers.Count > 1)
            {
                var player1 = pairingLayers[keys[0]].List[0];
                pairingLayers[keys[0]].List.RemoveAt(0);
                var layerRandom = random.Next(1, pairingLayers.Count);
                var player2Random = random.Next(0, pairingLayers[keys[layerRandom]].List.Count);
                var player2 = pairingLayers[keys[layerRandom]].List[player2Random];
                pairingLayers[keys[layerRandom]].List.RemoveAt(player2Random);

                if (pairingLayers[keys[layerRandom]].List.Count == 0)
                {
                    pairingLayers.Remove(keys[layerRandom]);
                    keys.Remove(keys[layerRandom]);
                }
                if (pairingLayers[keys[0]].List.Count == 0)
                {
                    pairingLayers.Remove(keys[0]);
                    keys.Remove(keys[0]);
                }

                AddGame(tournament.Id, oldGames, tables, games, creatingRound, player1, player2);
            }
            while (keys.Count > 0 && pairingLayers[keys[0]].List.Count > 1)
            {
                var player1 = pairingLayers[keys[0]].List[0];
                pairingLayers[keys[0]].List.RemoveAt(0);
                var player2Random = random.Next(0, pairingLayers[keys[0]].List.Count);
                var player2 = pairingLayers[keys[0]].List[player2Random];
                pairingLayers[keys[0]].List.RemoveAt(player2Random);

                AddGame(tournament.Id, oldGames, tables, games, creatingRound, player1, player2);
                warnings.Add($"Players from same country paired: {player1.PlayerName} and {player2.PlayerName}");
            }
        }

        private void AddGame(int tournamentId, List<TKGame> oldGames, List<int?> tables, List<TKGame> games, int creatingRound, TKTournamentPlayer player1, TKTournamentPlayer player2)
        {
            var game = new TKGame()
            {
                TKTournamentPlayer = player1,
                TKTournamentPlayer1 = player2,
                Round = creatingRound,
                TableNumber = tableGenerator.GetTable(tables, player1, player2, oldGames),
                TournamentId = tournamentId
            };

            games.Add(game);
        }

        private void SingleSwiss(TKTournament tournament, List<TKTournamentPlayer> tournamentPlayers, List<TKGame> oldGames, List<int?> tables, List<TKGame> games, int creatingRound, List<string> warnings, IEnumerable<PairingOption> options)
        {
            var isITC = tournament.TKSinglesScoringSystem.Name.Equals("ITC", StringComparison.InvariantCultureIgnoreCase);

            if (isITC)
            {
                tournamentPlayers = tournamentPlayers.OrderByDescending(p => p.Wins).ThenByDescending(p => p.Draws).ThenByDescending(p => p.BattlePoints).ToList();
            }
            else if (tournament.UseSeed && !isITC)
            {
                tournamentPlayers = tournamentPlayers.OrderByDescending(p => p.BattlePoints).ThenBy(p => p.Seed).ThenByDescending(p => p.SecondaryPoints).ToList();
            }

            tournamentPlayers = RandomizePlayers(tournamentPlayers);

            while (tournamentPlayers.Count > 1)
            {
                var player1 = tournamentPlayers[0];
                tournamentPlayers.RemoveAt(0);

                var opponents = oldGames.Where(g => g.Player1Id == player1.Id).Select(g => g.Player2Id.Value);
                opponents = opponents.Concat(oldGames.Where(g => g.Player2Id == player1.Id).Select(g => g.Player1Id.Value));

                var player2 = tournamentPlayers.FirstOrDefault(tp => !opponents.Any(o => o == tp.Id));
                if (player2 == null)
                {
                    player2 = tournamentPlayers.First();
                    warnings.Add($"Player {player1.PlayerName} and player {player2.PlayerName} have already played, correct pairings manually");
                }
                tournamentPlayers.Remove(player2);

                AddGame(tournament.Id, oldGames, tables, games, creatingRound, player1, player2);
            }
        }

        /// <summary>
        /// Randomize if players are on even points
        /// </summary>
        /// <param name="tournamentPlayers"></param>
        /// <returns></returns>
        private static List<TKTournamentPlayer> RandomizePlayers(List<TKTournamentPlayer> tournamentPlayers)
        {
            var playerSet = new Dictionary<Tuple<int, int>, List<TKTournamentPlayer>>();
            foreach (var tp in tournamentPlayers)
            {
                var key = new Tuple<int, int>(tp.BattlePoints, tp.SecondaryPoints);
                if (!playerSet.ContainsKey(key))
                {
                    playerSet.Add(key, new List<TKTournamentPlayer>());
                }
                playerSet[key].Add(tp);
            }

            var random = new Random();
            var tournamentPlayersRandomized = new List<TKTournamentPlayer>();
            foreach (var keySet in playerSet.OrderByDescending(p => p.Key.Item1).ThenByDescending(p => p.Key.Item2))
            {
                while (keySet.Value.Count > 0)
                {
                    var rp = random.Next(0, keySet.Value.Count);
                    var tmpPlayer = keySet.Value[rp];
                    keySet.Value.RemoveAt(rp);
                    tournamentPlayersRandomized.Add(tmpPlayer);
                }
            }

            return tournamentPlayersRandomized;
        }

        private IEnumerable<string> CheckAndSaveSinglesRound(TourneyKeeperEntities context, List<TKTournamentPlayer> tmpTournamentPlayers, List<TKGame> games, List<TKGame> oldGames)
        {
            if (tmpTournamentPlayers.Count % 2 == 1)
            {
                throw new ApplicationException("Uneven amount of players, add a standin player");
            }

            foreach (TKGame game in games)
            {
                var tmpOldGames = oldGames.Where(g => (g.Player1Id == game.Player1Id && g.Player2Id == game.Player2Id) || (g.Player1Id == game.Player2Id && g.Player2Id == game.Player1Id));
                if (tmpOldGames.Count() > 0)
                {
                    yield return $"Player {game.TKTournamentPlayer.PlayerName} already played player {game.TKTournamentPlayer1.PlayerName} - fix manually";
                }
            }

            context.TKGame.AddRange(games);
            context.SaveChanges();
        }

        private IEnumerable<string> CheckAndSaveTeamRound(TourneyKeeperEntities context, List<TKTournamentTeam> tmpTournamentTeams, List<TKTeamMatch> teamMatches, List<TKTeamMatch> oldTeamMatches, int tournamentId, int round)
        {
            if (tmpTournamentTeams.Count % 2 == 1)
            {
                throw new ApplicationException($"{tmpTournamentTeams[0].Name} not in a team match");
            }

            foreach (TKTeamMatch teamMatch in teamMatches)
            {
                var tmpOldMatches = oldTeamMatches.Where(g => (g.Team1Id == teamMatch.Team1Id && g.Team2Id == teamMatch.Team2Id) || (g.Team1Id == teamMatch.Team2Id && g.Team2Id == teamMatch.Team1Id));
                if (tmpOldMatches.Count() > 0)
                {
                    yield return $"Team {teamMatch.TKTournamentTeam.Name} and team {teamMatch.TKTournamentTeam1.Name} have already played, correct pairings manually";
                }
            }

            context.TKTeamMatch.AddRange(teamMatches);
            context.SaveChanges();

            foreach (TKTeamMatch teamMatch in context.TKTeamMatch.Where(t => t.TournamentId == tournamentId && t.Round == round))
            {
                var players = context.TKTournamentPlayer.Where(tp => tp.TournamentTeamId == teamMatch.TKTournamentTeam.Id && !tp.NonPlayer).ToList();
                for (int i = 0; i < players.Count; i++)
                {
                    var tkGame = new TKGame
                    {
                        Round = teamMatch.Round,
                        TeamMatchId = teamMatch.Id,
                        TournamentId = teamMatch.TournamentId
                    };

                    context.TKGame.Add(tkGame);
                }
            }

            context.SaveChanges();
        }
    }
}
