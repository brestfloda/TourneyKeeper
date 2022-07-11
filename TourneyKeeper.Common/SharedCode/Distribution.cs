using System;

namespace TourneyKeeper.Common.SharedCode
{
    public static class Distribution
    {
        public static double BinomialProbability(
            long trials, double successProbability, long successes)
        {
            long possibilities = trials.Factorial() /
                                ((trials - successes).Factorial() * successes.Factorial());

            double result = possibilities *
                            Math.Pow(successProbability, successes) *
                            Math.Pow(1 - successProbability, (trials - successes));

            return result;
        }

        public static long Factorial(this long x)
        {
            return x <= 1 ? 1 : x * Factorial(x - 1);
        }
    }
}