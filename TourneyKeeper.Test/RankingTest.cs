using Microsoft.VisualStudio.TestTools.UnitTesting;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class RankingTest
    {
        [TestMethod]
        public void DoRanking()
        {
            RankingCalculator calculator = new RankingCalculator();
            calculator.DoRankings();
            calculator.WLD1();
            calculator.WLD2();
        }
    }
}
