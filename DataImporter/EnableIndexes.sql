USE voresmail_dk_db
GO

ALTER INDEX "IX_MOArmy_TeamId_TournamentId" ON MOArmy REBUILD
ALTER INDEX "IX_MOArmy_TournamentId" ON MOArmy REBUILD
ALTER INDEX "IX_MOArmy_TeamId" ON MOArmy REBUILD
ALTER INDEX "PK_TKTeamMatch" ON TKTeamMatch REBUILD
ALTER INDEX "NonClusteredIndex-TournamentId" ON TKTeamMatch REBUILD
ALTER INDEX "NonClusteredIndex-20151115-175309" ON TKPlayer REBUILD
ALTER INDEX "ncix_quiz_tournamentid" ON TKTournamentPlayer REBUILD
ALTER INDEX "ncix_painting_tournamentid" ON TKTournamentPlayer REBUILD
ALTER INDEX "ncix_penalty_tournamentid" ON TKTournamentPlayer REBUILD
ALTER INDEX "IX_TKTournamentPlayer_TournamentTeamId" ON TKTournamentPlayer REBUILD
ALTER INDEX "ncix_tournamentplayer" ON TKTournamentPlayer REBUILD
ALTER INDEX "ncix_playerid_tournamentid" ON TKTournamentPlayer REBUILD
ALTER INDEX "PK_TKGame" ON TKGame REBUILD
ALTER INDEX "NonClusteredIndex-Player1Id" ON TKGame REBUILD
ALTER INDEX "NonClusteredIndex-Player2Id" ON TKGame REBUILD
ALTER INDEX "NonClusteredIndex-Player1IdRound" ON TKGame REBUILD
ALTER INDEX "NonClusteredIndex-Player2IdRound" ON TKGame REBUILD
ALTER INDEX "NonClusteredIndex-TournamentId" ON TKGame REBUILD
ALTER INDEX "ncix_tkgame" ON TKGame REBUILD
ALTER INDEX "IX_TKGame_TeamMatchId" ON TKGame REBUILD
ALTER INDEX "IX_TKPlayer_Token" ON TKPlayer REBUILD
ALTER INDEX "ncix_tournamentid" ON TKTournamentTeam REBUILD
ALTER INDEX "NonClusteredIndex-20160702-105517" ON TKTournamentPlayer REBUILD
GO

DECLARE @sql NVARCHAR(MAX) = N'';

;WITH x AS 
(
  SELECT DISTINCT obj = 
      QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' 
    + QUOTENAME(OBJECT_NAME(parent_object_id)) 
  FROM sys.foreign_keys
)
SELECT @sql += N'ALTER TABLE ' + obj + ' WITH CHECK CHECK CONSTRAINT ALL;
' FROM x;

EXEC sp_executesql @sql;
GO
