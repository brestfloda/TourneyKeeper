namespace TourneyKeeper.DTO.App
{
    public class SelectOpponentDTO : BaseDTO
    {
        public int TournamentPlayerId { get; set; }
        public int GameId { get; set; }
        public int OpponentId { get; set; }
        public int Table { get; set; }
        public bool PlayerTeam1 { get; set; }

        public override string Describe()
        {
            return $"{TournamentPlayerId}, {GameId}, {OpponentId}, {Table}";
        }
    }
}