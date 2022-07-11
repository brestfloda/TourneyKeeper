namespace TourneyKeeper.DTO.App
{
    public class GameDTO : BaseDTO
    {
        public int GameId { get; set; }
        public string Opponent { get; set; }
        public int? Table { get; set; }
        public string Tournament { get; set; }
        public int Round { get; set; }
        public int MyScore { get; set; }
        public int OpponentScore { get; set; }
        public int OpponentSecondaryScore { get; set; }
        public int MySecondaryScore { get; set; }
        public bool UseSecondaryPoints { get; set; }

        public override string Describe()
        {
            return $"{GameId}, {Opponent}, {Table}, {Tournament}, {Round}, {MyScore}, {OpponentScore}, {OpponentSecondaryScore}, {MySecondaryScore}, {UseSecondaryPoints}";
        }
    }
}