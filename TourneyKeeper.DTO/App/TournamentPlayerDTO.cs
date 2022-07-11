using System;

namespace TourneyKeeper.DTO.App
{
    public class TournamentPlayerDTO : BaseDTO
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }

        public override string Describe()
        {
            return $"{Id}, {PlayerName}";
        }

        public override string ToString()
        {
            return PlayerName;
        }
    }
}