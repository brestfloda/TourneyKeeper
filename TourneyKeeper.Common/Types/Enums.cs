namespace TourneyKeeper.Common
{
    public enum DrawTypeEnum
    {
        RandomDraw,
        SwissDraw
    }

    public enum TableGenerator
    {
        Linear,
        Random,
        Unique
    }

    public enum TournamentType
    {
        Singles = 1,
        Team = 2
    }

    public enum RequiredToReportEnum
    {
        PlayerInTournament = 1,
        PlayerInGame = 2
    }

    public enum TeamScoringSystem
    {
        Battlepoints = 1,
        Battlefrontscoring = 4,
        XWing51MAX = 1009,
        Cutoff = 1012,
        Max = 1013
    }

    public enum PairingOption
    {
        DoNotPairCountrymen,
        DoNotPairClubMembers
    }
}
