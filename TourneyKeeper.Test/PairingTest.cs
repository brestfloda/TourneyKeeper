using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.Managers.TableGenerators;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class PairingTest
    {
        [TestMethod]
        public void DoSinglesDrawRandom()
        {
            var manager = new PairingManager(TableGenerator.Linear);
            manager.SinglesDraw(2451, DrawTypeEnum.RandomDraw, new List<PairingOption>(), "328d7bd7-21be-467c-91f3-c6c164584027");
        }

        [TestMethod]
        public void DoTeamDrawRandom()
        {
            var manager = new PairingManager(TableGenerator.Linear);
            manager.TeamDraw(2463, DrawTypeEnum.RandomDraw, "328d7bd7-21be-467c-91f3-c6c164584027");
        }

        [TestMethod]
        public void SwapPlayers()
        {
            var manager = new SwapManager();
            var game1 = new TKGame { Id = 33426, TournamentPlayer1MarkForSwap = true };
            var game2 = new TKGame { Id = 33427, TournamentPlayer1MarkForSwap = true };

            manager.SwapPlayers(game1, game2, "328d7bd7-21be-467c-91f3-c6c164584027");
        }

        [TestMethod]
        public void SwapTeams()
        {
            var manager = new SwapManager();

            manager.SwapTeams(1897, 1900, 3, "328d7bd7-21be-467c-91f3-c6c164584027");
        }

        [TestMethod]
        public void CanGetCurrentMatch()
        {
            var manager = new TeamMatchManager();

            var res = manager.GetCurrentMatch("328d7bd7-21be-467c-91f3-c6c164584027");
        }

        [TestMethod]
        public void CanGetOpponents()
        {
            var manager = new TournamentTeamManager();

            var res = manager.GetOpponents("328d7bd7-21be-467c-91f3-c6c164584027");
        }
    }
}
