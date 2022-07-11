namespace TourneyKeeper.Common.SharedCode
{
    public class EloRating
    {
        public double Point1 { get; set; }
        public double Point2 { get; set; }
        public double eA { get; set; }
        public double eB { get; set; }

        public EloRating(double CurrentRating1, double CurrentRating2, double Score1, double Score2)
        {
            eA = 1 / (1 + 10 * ((CurrentRating2 - CurrentRating1) / 400));
            eB = 1 / (1 + 10 * ((CurrentRating1 - CurrentRating2) / 400));

            Point1 = 40.0 * eA;
            Point2 = 40.0 * eB;
        }
    }
}
