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
    
    public partial class TKCodex
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TKCodex()
        {
            this.TKTournamentPlayer = new HashSet<TKTournamentPlayer>();
            this.TKTournamentPlayer1 = new HashSet<TKTournamentPlayer>();
            this.TKTournamentPlayer2 = new HashSet<TKTournamentPlayer>();
            this.TKTournamentPlayer3 = new HashSet<TKTournamentPlayer>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameSystemId { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual TKGameSystem TKGameSystem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKTournamentPlayer> TKTournamentPlayer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKTournamentPlayer> TKTournamentPlayer1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKTournamentPlayer> TKTournamentPlayer2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKTournamentPlayer> TKTournamentPlayer3 { get; set; }
    }
}
