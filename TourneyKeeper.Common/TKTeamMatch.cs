//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourneyKeeper.Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class TKTeamMatch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TKTeamMatch()
        {
            this.TKGame = new HashSet<TKGame>();
        }
    
        public int Id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int Round { get; set; }
        public Nullable<int> TableNumber { get; set; }
        public int TournamentId { get; set; }
        public int Team1MatchPoints { get; set; }
        public int Team2MatchPoints { get; set; }
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public int Team1SecondaryPoints { get; set; }
        public int Team2SecondaryPoints { get; set; }
        public Nullable<int> Team1Penalty { get; set; }
        public Nullable<int> Team2Penalty { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKGame> TKGame { get; set; }
        public virtual TKTournamentTeam TKTournamentTeam { get; set; }
        public virtual TKTournamentTeam TKTournamentTeam1 { get; set; }
        public virtual TKTournament TKTournament { get; set; }
    }
}