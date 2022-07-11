namespace TourneyKeeper.DTO.App
{
    public class GetOpponentsResponseDTO : BaseDTO
    {
        public int? GameId { get; set; }
        public TournamentPlayerDTO[] Opponents { get; set; }
        public TournamentPlayerDTO CurrentOpponent { get; set; }
        public int Tables { get; set; }
        public int? CurrentTable { get; set; }
        public bool PlayerTeam1 { get; set; }
        public int TournamentPlayerId { get; set; }

        public override string Describe()
        {
            return $"{GameId}, {Tables}, {PlayerTeam1}, {TournamentPlayerId}";
        }
    }
}