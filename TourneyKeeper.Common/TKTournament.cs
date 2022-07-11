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
    
    public partial class TKTournament
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TKTournament()
        {
            this.TKGame = new HashSet<TKGame>();
            this.TKOption = new HashSet<TKOption>();
            this.TKOrganizer = new HashSet<TKOrganizer>();
            this.TKTeamMatch = new HashSet<TKTeamMatch>();
            this.TKTournamentPlayer = new HashSet<TKTournamentPlayer>();
            this.TKTournamentTeam = new HashSet<TKTournamentTeam>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime TournamentDate { get; set; }
        public bool Active { get; set; }
        public int GameSystemId { get; set; }
        public int TournamentTypeId { get; set; }
        public Nullable<int> TeamSize { get; set; }
        public string TimeTable { get; set; }
        public Nullable<int> TeamScoringSystemId { get; set; }
        public string Missions { get; set; }
        public string ArmySelection { get; set; }
        public bool UseSecondaryPoints { get; set; }
        public bool AllowEdit { get; set; }
        public bool OnlineSignup { get; set; }
        public Nullable<System.DateTime> OnlineSignupStart { get; set; }
        public Nullable<int> MaxPlayers { get; set; }
        public string Country { get; set; }
        public System.DateTime TournamentEndDate { get; set; }
        public bool UseSeed { get; set; }
        public Nullable<int> SinglesScoringSystemId { get; set; }
        public bool UseRandomTeamPairings { get; set; }
        public Nullable<System.DateTime> ShowListsDate { get; set; }
        public bool PlayersDefaultActive { get; set; }
        public bool ShowSoftScores { get; set; }
        public int RequireLogin { get; set; }
        public bool RequirePayment { get; set; }
        public string StripeKey { get; set; }
        public bool NationalTournament { get; set; }
        public string Description { get; set; }
        public bool UseAbout { get; set; }
        public Nullable<int> MaxScoreForLoss { get; set; }
        public Nullable<int> MinScoreForWin { get; set; }
        public string OrganizerEmail { get; set; }
        public bool HideResultsforRound { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKGame> TKGame { get; set; }
        public virtual TKGameSystem TKGameSystem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKOption> TKOption { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKOrganizer> TKOrganizer { get; set; }
        public virtual TKSinglesScoringSystem TKSinglesScoringSystem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKTeamMatch> TKTeamMatch { get; set; }
        public virtual TKTeamScoringSystem TKTeamScoringSystem { get; set; }
        public virtual TKTournamentType TKTournamentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKTournamentPlayer> TKTournamentPlayer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TKTournamentTeam> TKTournamentTeam { get; set; }
    }
}
