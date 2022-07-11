namespace TourneyKeeper.DTO.Web
{
    public class EditTeamData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Penalty { get; set; }
        public int BattlePointPenalty { get; set; }
        public int Players { get; set; }
        public int NonPlayers { get; set; }
        public string Token { get; set; }
        public bool? Paid { get; set; }
    }
}