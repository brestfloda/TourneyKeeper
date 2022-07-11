using System;

namespace TourneyKeeper.DTO.Web
{
    public class PlayerArmyDetailsDTO
    {
        public int TournamentPlayerId { get; set; }
        public int? PrimaryId { get; set; }
        public int? SecondaryId { get; set; }
        public int? TertiaryId { get; set; }
        public int? QuaternaryId { get; set; }
        public string Army { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int? TeamId { get; set; }
        public string TeamName { get; set; }
        public bool Active { get; set; }
    }
}