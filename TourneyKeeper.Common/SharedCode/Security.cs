using System.Linq;
using TourneyKeeper.Common.Exceptions;

namespace TourneyKeeper.Common.SharedCode
{
    public static class Security
    {
        public static void CheckToken(string token, int tournamentId, TourneyKeeperEntities context)
        {
            var tmp = context.TKPlayer.SingleOrDefault(t => t.Token.Equals(token));
            if (tmp.IsAdmin)
            {
                return;
            }
            var tmp3 = context.TKTournamentPlayer.SingleOrDefault(tp => tp.TKPlayer.Token.Equals(token) && tp.TournamentId == tournamentId);
            if (tmp3 != null)
            {
                return;
            }
            var tmp2 = context.TKOrganizer.Any(o => o.PlayerId == tmp.Id && o.TournamentId == tournamentId);
            if (!tmp2)
            {
                throw new SecurityException("You have tried to access a protected area of TourneyKeeper, ask an organizer for help");
            }
        }
    }
}
