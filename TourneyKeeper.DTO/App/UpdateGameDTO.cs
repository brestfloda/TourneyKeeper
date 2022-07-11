namespace TourneyKeeper.DTO.App
{
    public class UpdateGameDTO : BaseDTO
    {
        public string Token { get; set; }
        public int GameId { get; set; }
        public int MyScore { get; set; }
        public int OpponentScore { get; set; }
        public int OpponentSecondaryScore { get; set; }
        public int MySecondaryScore { get; set; }

        public override string Describe()
        {
            return $"{Token}, {GameId}, {MyScore}, {OpponentScore}, {OpponentSecondaryScore}, {MySecondaryScore}";
        }
    }
}