USE voresmail_dk_db
GO

ALTER INDEX "IX_MOArmy_TeamId_TournamentId" ON MOArmy DISABLE
ALTER INDEX "IX_MOArmy_TournamentId" ON MOArmy DISABLE
ALTER INDEX "IX_MOArmy_TeamId" ON MOArmy DISABLE
ALTER INDEX "PK_TKTeamMatch" ON TKTeamMatch DISABLE
ALTER INDEX "NonClusteredIndex-TournamentId" ON TKTeamMatch DISABLE
ALTER INDEX "NonClusteredIndex-20151115-175309" ON TKPlayer DISABLE
ALTER INDEX "ncix_quiz_tournamentid" ON TKTournamentPlayer DISABLE
ALTER INDEX "ncix_painting_tournamentid" ON TKTournamentPlayer DISABLE
ALTER INDEX "ncix_penalty_tournamentid" ON TKTournamentPlayer DISABLE
ALTER INDEX "IX_TKTournamentPlayer_TournamentTeamId" ON TKTournamentPlayer DISABLE
ALTER INDEX "ncix_tournamentplayer" ON TKTournamentPlayer DISABLE
ALTER INDEX "ncix_playerid_tournamentid" ON TKTournamentPlayer DISABLE
ALTER INDEX "PK_TKGame" ON TKGame DISABLE
ALTER INDEX "NonClusteredIndex-Player1Id" ON TKGame DISABLE
ALTER INDEX "NonClusteredIndex-Player2Id" ON TKGame DISABLE
ALTER INDEX "NonClusteredIndex-Player1IdRound" ON TKGame DISABLE
ALTER INDEX "NonClusteredIndex-Player2IdRound" ON TKGame DISABLE
ALTER INDEX "NonClusteredIndex-TournamentId" ON TKGame DISABLE
ALTER INDEX "ncix_tkgame" ON TKGame DISABLE
ALTER INDEX "IX_TKGame_TeamMatchId" ON TKGame DISABLE
ALTER INDEX "IX_TKPlayer_Token" ON TKPlayer DISABLE
ALTER INDEX "ncix_tournamentid" ON TKTournamentTeam DISABLE
ALTER INDEX "NonClusteredIndex-20160702-105517" ON TKTournamentPlayer DISABLE
GO

DECLARE @sql NVARCHAR(MAX) = N'';

;WITH x AS 
(
  SELECT DISTINCT obj = 
      QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' 
    + QUOTENAME(OBJECT_NAME(parent_object_id)) 
  FROM sys.foreign_keys
)
SELECT @sql += N'ALTER TABLE ' + obj + ' NOCHECK CONSTRAINT ALL;
' FROM x;

EXEC sp_executesql @sql;
GO