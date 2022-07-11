namespace TourneyKeeper.DTO.Web
{
    public class LeaderboardData
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Placement { get; set; }
        public int Seed { get; set; }
        public string Name { get; set; }
        public string Club { get; set; }
        public string GamePath { get; set; }
        public string PrimaryCodex { get; set; }
        public string TeamName { get; set; }
        public int Penalty { get; set; }
        public int Quiz { get; set; }
        public int Painting { get; set; }
        public int BattlePoints { get; set; }
        public int Fairplay { get; set; }
        public int SecondaryPoints { get; set; }
        public int Points { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public bool HasArmylist { get; set; }
    }
}