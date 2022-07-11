using System;
using System.Collections.Generic;

namespace TourneyKeeper.Web.WebAPI.DTO
{
    public class GameDto
    {
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public string Player1PrimaryCodex { get; set; }
        public string Player2PrimaryCodex { get; set; }
        public string Player1SecondaryCodex { get; set; }
        public string Player2SecondaryCodex { get; set; }
        public string Player1TertiaryCodex { get; set; }
        public string Player2TertiaryCodex { get; set; }
        public string Player1Armylist { get; set; }
        public string Player2Armylist { get; set; }
        public int Player1Result { get; set; }
        public int Player2Result { get; set; }
        public int Player1SecondaryResult { get; set; }
        public int Player2SecondaryResult { get; set; }
        public int Round { get; set; }
    }
}