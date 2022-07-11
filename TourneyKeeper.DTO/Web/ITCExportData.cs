namespace TourneyKeeper.DTO.Web
{
    public class ITCExportData
    {
        public int BattlePoints { get; set; }
        public int BonusPoints { get; set; }
        public string Club { get; set; }
        public string Email { get; set; }
        public string GamePath { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Placement { get; set; }
        public int PlacingPoints { get; set; }
        public int PlayerId { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public string PrimaryCodex { get; set; }
        public string SecondaryCodex { get; set; }
        public int OpponentBattlePoints { get; set; }
    }
}