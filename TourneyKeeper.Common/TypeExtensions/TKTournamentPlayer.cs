using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourneyKeeper.Common
{
    public partial class TKTournamentPlayer
    {
        public int TotalPoints
        {
            get
            {
                return BattlePoints + Penalty + (TKTournament.ShowSoftScores ? FairPlay + Painting + Quiz : 0);
            }
        }

        public string TeamName
        {
            get
            {
                return TKTournamentTeam == null ? "" : TKTournamentTeam.Name;
            }
        }

        public string NameAndCodex
        {
            get
            {
                if (TKCodex != null)
                {
                    return string.Format("{0} ({1})", PlayerName, TKCodex.Name);
                }
                else
                {
                    return PlayerName;
                }
            }
        }

        public string NameAndTeam
        {
            get
            {
                if (TKTournamentTeam != null)
                {
                    return string.Format("{0} ({1})", PlayerName, TKTournamentTeam.Name);
                }
                else
                {
                    return PlayerName;
                }
            }
        }

        public string NameTeamAndArmy
        {
            get
            {
                if (TKTournamentTeam != null)
                {
                    string tmp = $"{PlayerName} ({TKTournamentTeam.Name}";
                    tmp += TKCodex != null ? $" - {TKCodex.Name})" : ")";
                    return tmp;
                }
                else
                {
                    return PlayerName;
                }
            }
        }

        public string NameNonPlayerStatus
        {
            get
            {
                if (TKTournamentTeam != null)
                {
                    return string.Format("{0}{1}", PlayerName, NonPlayer ? " (NP)" : "");
                }
                else
                {
                    return PlayerName;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TKTournamentPlayer))
            {
                return false;
            }

            return ((TKTournamentPlayer)obj).Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Seed, BattlePoints, PlayerName);
        }
    }
}