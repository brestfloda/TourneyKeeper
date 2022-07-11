using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Common.Managers
{
    public class CodexManager : IManager<TKCodex>
    {
        public TKCodex Get(int id)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKCodex.Single(g => g.Id == id);
            }
        }

        public void Update(TKCodex entity, string token)
        {
            throw new NotImplementedException();
        }

        public IList<TKCodex> GetAllCodices(int gamesystemId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKCodex.Where(c => c.GameSystemId == gamesystemId && (c.Active ?? false)).ToList();
            }
        }

        public IList<Tuple<string, int>> GetCodexCount(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                return context.TKTournamentPlayer
                    .Where(c => c.TournamentId == tournamentId && c.PrimaryCodexId != null && c.Active)
                    .GroupBy(c => c.PrimaryCodexId)
                    .Select(c => new { Name = c.FirstOrDefault().TKCodex.Name, Count = c.Count() })
                    .AsEnumerable()
                    .Select(c => new Tuple<string, int>(c.Name, c.Count))
                    .Concat(context.TKTournamentPlayer
                    .Where(c => c.TournamentId == tournamentId && c.PrimaryCodexId == null && c.Active)
                    .GroupBy(c => c.PrimaryCodexId)
                    .Select(c => new { Name = c.FirstOrDefault().TKCodex.Name, Count = c.Count() })
                    .AsEnumerable()
                    .Select(c => new Tuple<string, int>("Blank", c.Count)))
                    .ToList();
            }
        }
    }
}
