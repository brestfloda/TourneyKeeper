using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TourneyKeeper.Common;
using System.Collections.Generic;
using TourneyKeeper.Common.SharedCode;
using static TourneyKeeper.Common.SharedCode.General;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class RollingPowersTest
    {
        [TestMethod]
        public void Start2()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var charge = 0;

            for (int i = 0; i < 10000000; i++)
            {
                ChargeIt(random, ref charge);
            }
        }

        private void ChargeIt(Random random, ref int charge)
        {
            var dice = new List<int>();
            dice.Add(random.Next(1, 7));
            dice.Add(random.Next(1, 7));
            dice.Add(random.Next(1, 7));

            if ((dice[0] + dice[1] + dice[2]) > 8)
            {
                charge++;
            }
            else
            {
                dice.Sort();
                dice[0] = random.Next(1, 7);
                if ((dice[0] + dice[1] + dice[2]) > 8)
                {
                    charge++;
                }
            }
        }

        [TestMethod]
        public void Start()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var lowDropStart = 0;

            for (int i = 0; i < 10000000; i++)
            {
                RollIt(random, ref lowDropStart);
            }
        }

        private static void RollIt(Random random, ref int lowDropStart)
        {
            var lowDropRoll = random.Next(2, 8);
            var highDropRoll = random.Next(1, 7);

            if (lowDropRoll == highDropRoll)
            {
                RollIt(random, ref lowDropStart);
            }
            else if (lowDropRoll > highDropRoll)
            {
                var seize = random.Next(1, 7);
                if (seize != 6)
                {
                    lowDropStart++;
                }
            }
            else
            {
                var seize = random.Next(1, 7);
                if (seize == 6)
                {
                    lowDropStart++;
                }
            }
        }

        [TestMethod]
        public void DoMassRoll()
        {
            General.MassRoll(100, 3, 2, RerollTypes.FullReroll, -1, 4, RerollTypes.Reroll1s, 5, RerollTypes.Reroll1s, 5);
        }
    }
}