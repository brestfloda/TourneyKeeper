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
    
    public partial class TKGame
    {
        public int Id { get; set; }
        public Nullable<int> Player1Id { get; set; }
        public Nullable<int> Player2Id { get; set; }
        public int Round { get; set; }
        public Nullable<int> TableNumber { get; set; }
        public int Player1Result { get; set; }
        public int Player2Result { get; set; }
        public int TournamentId { get; set; }
        public Nullable<int> TeamMatchId { get; set; }
        public string Ranked { get; set; }
        public int Player1SecondaryResult { get; set; }
        public int Player2SecondaryResult { get; set; }
        public Nullable<double> Player1ELO { get; set; }
        public Nullable<double> Player2ELO { get; set; }
        public Nullable<System.DateTime> LastEdited { get; set; }
        public string LastEditedBy { get; set; }
    
        public virtual TKTeamMatch TKTeamMatch { get; set; }
        public virtual TKTournamentPlayer TKTournamentPlayer { get; set; }
        public virtual TKTournamentPlayer TKTournamentPlayer1 { get; set; }
        public virtual TKTournament TKTournament { get; set; }
    }
}
