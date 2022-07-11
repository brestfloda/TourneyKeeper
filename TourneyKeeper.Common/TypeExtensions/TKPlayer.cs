using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Common
{
    public partial class TKPlayer
    {
        public IList<int> TournamentIds { get; set; }
        public IList<int> OrganizerForTournamentIds { get; set; }

        public bool IsPlayerInTournament(int tournamentId)
        {
            return TournamentIds == null ? false : TournamentIds.Any(tp => tp == tournamentId);
        }

        public bool IsPlayerOrganizer(int tournamentId)
        {
            if (OrganizerForTournamentIds == null)
            {
                OrganizerForTournamentIds = new PlayerManager().GetOrganizerTournamentIds(Id);
            }
            return OrganizerForTournamentIds == null ? false : OrganizerForTournamentIds.Any(tp => tp == tournamentId);
        }
    }
}
