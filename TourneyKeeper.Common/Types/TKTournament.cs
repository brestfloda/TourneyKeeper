using System;

namespace TourneyKeeper.Common
{
    public partial class TKTournament
    {
        public TournamentType TournamentType
        {
            get
            {
                switch (this.TournamentTypeId)
                {
                    case 1:
                        return TournamentType.Singles;
                    case 2:
                        return TournamentType.Team;
                    default:
                        throw new Exception("TournamentType not mapped");
                }
            }
        }

        public string TournamentTypeName
        {
            get
            {
                switch (this.TournamentTypeId)
                {
                    case 1:
                        return "Singles";
                    case 2:
                        return "Team";
                    default:
                        throw new Exception("TournamentTypeName not mapped");
                }
            }
        }
    }
}
