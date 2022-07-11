namespace TourneyKeeper.DTO.Web
{
    public class EditPlayerData
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Placement { get; set; }
        public int Seed { get; set; }
        public string PlayerName { get; set; }
        public string PrimaryCodex { get; set; }
        public string SecondaryCodex { get; set; }
        public string TertiaryCodex { get; set; }
        public string QuaternaryCodex { get; set; }
        public string ArmyList { get; set; }
        public string Club { get; set; }
        public int FairPlay { get; set; }
        public int Penalty { get; set; }
        public int Quiz { get; set; }
        public int Painting { get; set; }
        public int BattlePoints { get; set; }
        public int SecondaryPoints { get; set; }
        public int Points { get; set; }
        public bool Paid { get; set; }
        public bool DoNotRank { get; set; }
        public bool Active { get; set; }
        public string TeamName { get; set; }
        public string Token { get; set; }
        public int GameSystemId { get; set; }
        public int? TournamentTeamId { get; set; }
        public bool NonPlayer { get; set; }
    }
}