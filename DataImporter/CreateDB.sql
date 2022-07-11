USE [master]
GO
/****** Object:  Database [voresmail_dk_db]    Script Date: 02-07-2016 08:15:40 ******/
CREATE DATABASE [voresmail_dk_db]
 CONTAINMENT = NONE
GO
ALTER DATABASE [voresmail_dk_db] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [voresmail_dk_db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [voresmail_dk_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [voresmail_dk_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [voresmail_dk_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [voresmail_dk_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [voresmail_dk_db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [voresmail_dk_db] SET  MULTI_USER 
GO
ALTER DATABASE [voresmail_dk_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [voresmail_dk_db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [voresmail_dk_db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [voresmail_dk_db] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [voresmail_dk_db] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [voresmail_dk_db] SET QUERY_STORE = OFF
GO
USE [voresmail_dk_db]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [voresmail_dk_db]
GO
/****** Object:  Table [dbo].[MOArmy]    Script Date: 02-07-2016 08:15:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOArmy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArmyList] [nvarchar](max) NULL,
	[CodexId] [int] NOT NULL,
	[TeamId] [int] NULL,
	[TournamentId] [int] NOT NULL,
	[SecondaryCodexId] [int] NULL,
	[ArmyTypeId] [int] NULL,
 CONSTRAINT [PK_Army] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOCodex]    Script Date: 02-07-2016 08:15:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOCodex](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[GameSystemId] [int] NOT NULL,
 CONSTRAINT [PK_Codex] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOConsolidatedPairingData]    Script Date: 02-07-2016 08:15:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOConsolidatedPairingData](
	[Id] [int] NOT NULL,
	[CalculatedResult] [int] NULL,
	[CalculatedWeight] [float] NULL,
	[ExpectedResult] [int] NULL,
 CONSTRAINT [PK_MOConsolidatedPairingData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOMissionType]    Script Date: 02-07-2016 08:15:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOMissionType](
	[Id] [int] NOT NULL,
	[Mission] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MOMissionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOPairing]    Script Date: 02-07-2016 08:15:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOPairing](
	[FriendlyArmyId] [int] NOT NULL,
	[EnemyArmyId] [int] NOT NULL,
	[PairingEvaluationId] [int] NOT NULL,
	[TournamentId] [int] NOT NULL,
 CONSTRAINT [PK_Pairing] PRIMARY KEY CLUSTERED 
(
	[FriendlyArmyId] ASC,
	[EnemyArmyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOPairingData]    Script Date: 02-07-2016 08:15:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOPairingData](
	[Id] [int] NOT NULL,
	[FriendlyArmyId] [int] NOT NULL,
	[EnemyArmyId] [int] NOT NULL,
	[ListStrength] [int] NOT NULL,
	[PlayerStrength] [int] NOT NULL,
	[MissionTypeId] [int] NOT NULL,
	[MOConsolidatedPairingDataId] [int] NULL,
 CONSTRAINT [PK_MOPairingData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOPairingEvaluation]    Script Date: 02-07-2016 08:15:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOPairingEvaluation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PairingColor] [nvarchar](50) NOT NULL,
	[Points] [int] NOT NULL,
	[IsVolatile] [nchar](10) NULL,
 CONSTRAINT [PK_PairingEvaluation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOTeam]    Script Date: 02-07-2016 08:15:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOTeam](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsFriendly] [bit] NOT NULL,
	[TournamentId] [int] NOT NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MOTournament]    Script Date: 02-07-2016 08:15:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MOTournament](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[StartDate] [date] NOT NULL,
	[Players] [int] NOT NULL,
 CONSTRAINT [PK_MOTournament] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKFrontPageLinks]    Script Date: 02-07-2016 08:15:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKFrontPageLinks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Headline] [nvarchar](100) NOT NULL,
	[Link] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_TKFrontPageLinks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKGame]    Script Date: 02-07-2016 08:15:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKGame](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Player1Id] [int] NULL,
	[Player2Id] [int] NULL,
	[Round] [int] NOT NULL,
	[TableNumber] [int] NULL,
	[Player1Result] [int] NOT NULL,
	[Player2Result] [int] NOT NULL,
	[TournamentId] [int] NOT NULL,
	[TeamMatchId] [int] NULL,
	[Ranked] [nvarchar](20) NULL,
	[Player1SecondaryResult] [int] NOT NULL,
	[Player2SecondaryResult] [int] NOT NULL,
	[Player1ELO] [float] NULL,
	[Player2ELO] [float] NULL,
 CONSTRAINT [PK_TKGame] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [ClusteredIndex-20140317-070240]    Script Date: 02-07-2016 08:15:44 ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20140317-070240] ON [dbo].[TKGame]
(
	[TournamentId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TKGameSystem]    Script Date: 02-07-2016 08:15:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKGameSystem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GameSystem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKOrganizer]    Script Date: 02-07-2016 08:15:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKOrganizer](
	[PlayerId] [int] NOT NULL,
	[TournamentId] [int] NOT NULL,
 CONSTRAINT [PK_TKOrganizer] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[TournamentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKPlayer]    Script Date: 02-07-2016 08:15:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKPlayer](
	[Id] [int] IDENTITY(31,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NULL,
	[Zipcode] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Comments] [nvarchar](max) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](max) NULL,
	[IsAdmin] [bit] NOT NULL,
	[Username] [nvarchar](50) NULL,
	[LastLoggedIn] [datetime] NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[Draws] [int] NOT NULL,
	[Token] [nvarchar](50) NULL,
 CONSTRAINT [PK_TKPlayer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKRanking]    Script Date: 02-07-2016 08:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKRanking](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Points] [float] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[PlayerName] [nvarchar](100) NOT NULL,
	[RankingTypeId] [int] NOT NULL,
	[LatestGame] [date] NULL,
	[Rank] [int] NULL,
 CONSTRAINT [PK_Ranking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKRankingType]    Script Date: 02-07-2016 08:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKRankingType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[GameSystemId] [int] NOT NULL,
 CONSTRAINT [PK_TKRankingType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKSinglesScoringSystem]    Script Date: 02-07-2016 08:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKSinglesScoringSystem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TKSinglesTournamentScoringSystem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKTeamMatch]    Script Date: 02-07-2016 08:15:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKTeamMatch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Team1Id] [int] NOT NULL,
	[Team2Id] [int] NOT NULL,
	[Round] [int] NOT NULL,
	[TableNumber] [int] NULL,
	[TournamentId] [int] NOT NULL,
	[Team1MatchPoints] [int] NOT NULL,
	[Team2MatchPoints] [int] NOT NULL,
	[Team1Points] [int] NOT NULL,
	[Team2Points] [int] NOT NULL,
	[Team1SecondaryPoints] [int] NOT NULL,
	[Team2SecondaryPoints] [int] NOT NULL,
	[Team1Penalty] [int] NULL,
	[Team2Penalty] [int] NULL,
 CONSTRAINT [PK_TKTeamMatch] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [ClusteredIndex-20140317-071326]    Script Date: 02-07-2016 08:15:46 ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20140317-071326] ON [dbo].[TKTeamMatch]
(
	[TournamentId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TKTeamScoringSystem]    Script Date: 02-07-2016 08:15:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKTeamScoringSystem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TKTeamTournamentScoringSystem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKTournament]    Script Date: 02-07-2016 08:15:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKTournament](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[TournamentDate] [date] NOT NULL,
	[Active] [bit] NOT NULL,
	[GameSystemId] [int] NOT NULL,
	[TableNumberStart] [int] NULL,
	[TableNumberEnd] [int] NULL,
	[TournamentTypeId] [int] NOT NULL,
	[TeamSize] [int] NULL,
	[TimeTable] [nvarchar](max) NULL,
	[TeamScoringSystemId] [int] NULL,
	[Missions] [nvarchar](max) NULL,
	[ArmySelection] [nvarchar](max) NULL,
	[UseSecondaryPoints] [bit] NOT NULL,
	[AllowEdit] [bit] NOT NULL,
	[OnlineSignup] [bit] NOT NULL,
	[OnlineSignupStart] [datetime] NULL,
	[MaxPlayers] [int] NULL,
	[Country] [nvarchar](50) NULL,
	[TournamentEndDate] [date] NOT NULL,
	[UseSeed] [bit] NOT NULL,
	[SinglesScoringSystemId] [int] NULL,
	[UseRandomTeamPairings] [bit] NOT NULL,
	[CheckForTotalPoints] [int] NOT NULL,
	[ShowListsDate] [datetime] NULL,
	[PlayersDefaultActive] [bit] NOT NULL,
 CONSTRAINT [PK_TKTournament] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKTournamentPlayer]    Script Date: 02-07-2016 08:15:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKTournamentPlayer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[TournamentId] [int] NOT NULL,
	[Seed] [int] NOT NULL,
	[Paid] [bit] NOT NULL,
	[CustomPlayerNumber] [nvarchar](50) NULL,
	[FairPlay] [int] NOT NULL,
	[Painting] [int] NOT NULL,
	[Penalty] [int] NOT NULL,
	[TournamentTeamId] [int] NULL,
	[PrimaryCodexId] [int] NULL,
	[SecondaryCodexId] [int] NULL,
	[TertiaryCodexId] [int] NULL,
	[ArmyList] [nvarchar](max) NULL,
	[DoNotRank] [bit] NOT NULL,
	[BattlePoints] [int] NOT NULL,
	[SecondaryPoints] [int] NOT NULL,
	[Club] [nvarchar](50) NULL,
	[Quiz] [int] NOT NULL,
	[PlayerName] [nvarchar](50) NULL,
	[GamePath] [nvarchar](50) NULL,
	[Wins] [int] NOT NULL,
	[Draws] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[TournamentPoints] [int] NULL,
	[NonPlayer] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[QuaternaryCodexId] [int] NULL,
 CONSTRAINT [PK_TKTournamentPlayer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKTournamentTeam]    Script Date: 02-07-2016 08:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKTournamentTeam](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[TournamentId] [int] NOT NULL,
	[Penalty] [int] NOT NULL,
	[BattlePoints] [int] NOT NULL,
	[MatchPoints] [int] NOT NULL,
	[SecondaryPoints] [int] NOT NULL,
	[BattlePointPenalty] [int] NOT NULL,
 CONSTRAINT [PK_TKTournamentTeam] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TKTournamentType]    Script Date: 02-07-2016 08:15:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TKTournamentType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TKTournamentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MOArmy_TeamId]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [IX_MOArmy_TeamId] ON [dbo].[MOArmy]
(
	[TeamId] ASC
)
INCLUDE ( 	[Id],
	[ArmyList],
	[CodexId],
	[TournamentId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MOArmy_TeamId_TournamentId]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [IX_MOArmy_TeamId_TournamentId] ON [dbo].[MOArmy]
(
	[TeamId] ASC,
	[TournamentId] ASC
)
INCLUDE ( 	[Id],
	[ArmyList],
	[CodexId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MOArmy_TournamentId]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [IX_MOArmy_TournamentId] ON [dbo].[MOArmy]
(
	[TournamentId] ASC
)
INCLUDE ( 	[Id],
	[ArmyList],
	[CodexId],
	[TeamId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TKGame_TeamMatchId]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [IX_TKGame_TeamMatchId] ON [dbo].[TKGame]
(
	[TeamMatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ncix_tkgame]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [ncix_tkgame] ON [dbo].[TKGame]
(
	[Round] ASC,
	[TeamMatchId] ASC
)
INCLUDE ( 	[Id],
	[Player1Id],
	[Player2Id],
	[TableNumber],
	[Player1Result],
	[Player2Result],
	[TournamentId],
	[Ranked],
	[Player1SecondaryResult],
	[Player2SecondaryResult],
	[Player1ELO],
	[Player2ELO]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-Player1Id]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Player1Id] ON [dbo].[TKGame]
(
	[Player1Id] ASC
)
INCLUDE ( 	[Player1Result]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-Player1IdRound]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Player1IdRound] ON [dbo].[TKGame]
(
	[Player1Id] ASC,
	[Round] ASC
)
INCLUDE ( 	[Player1Result]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-Player2Id]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Player2Id] ON [dbo].[TKGame]
(
	[Player2Id] ASC
)
INCLUDE ( 	[Player2Result]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-Player2IdRound]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Player2IdRound] ON [dbo].[TKGame]
(
	[Player2Id] ASC,
	[Round] ASC
)
INCLUDE ( 	[Player2Result]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-TournamentId]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-TournamentId] ON [dbo].[TKGame]
(
	[TournamentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-20151115-175309]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20151115-175309] ON [dbo].[TKPlayer]
(
	[IsAdmin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-TournamentId]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-TournamentId] ON [dbo].[TKTeamMatch]
(
	[TournamentId] ASC
)
INCLUDE ( 	[Round]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TKTournamentPlayer_TournamentTeamId]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [IX_TKTournamentPlayer_TournamentTeamId] ON [dbo].[TKTournamentPlayer]
(
	[TournamentTeamId] ASC
)
INCLUDE ( 	[Id],
	[PlayerId],
	[TournamentId],
	[Seed],
	[Paid],
	[CustomPlayerNumber],
	[FairPlay],
	[Painting],
	[Penalty],
	[PrimaryCodexId],
	[SecondaryCodexId],
	[TertiaryCodexId],
	[ArmyList],
	[DoNotRank],
	[BattlePoints],
	[SecondaryPoints],
	[Club],
	[Quiz],
	[PlayerName],
	[GamePath],
	[Wins],
	[Draws],
	[Losses],
	[TournamentPoints],
	[NonPlayer],
	[Active],
	[QuaternaryCodexId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ncix_painting_tournamentid]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [ncix_painting_tournamentid] ON [dbo].[TKTournamentPlayer]
(
	[TournamentId] ASC,
	[Painting] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ncix_penalty_tournamentid]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [ncix_penalty_tournamentid] ON [dbo].[TKTournamentPlayer]
(
	[TournamentId] ASC,
	[Penalty] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ncix_playerid_tournamentid]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [ncix_playerid_tournamentid] ON [dbo].[TKTournamentPlayer]
(
	[PlayerId] ASC,
	[TournamentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ncix_quiz_tournamentid]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [ncix_quiz_tournamentid] ON [dbo].[TKTournamentPlayer]
(
	[TournamentId] ASC,
	[Quiz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ncix_tournamentplayer]    Script Date: 02-07-2016 08:15:50 ******/
CREATE NONCLUSTERED INDEX [ncix_tournamentplayer] ON [dbo].[TKTournamentPlayer]
(
	[PlayerId] ASC
)
INCLUDE ( 	[TournamentId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TKGame] ADD  CONSTRAINT [DF_TKGame_Ranked]  DEFAULT ((0)) FOR [Ranked]
GO
ALTER TABLE [dbo].[TKGame] ADD  CONSTRAINT [DF_TKGame_Player1SecondaryResult]  DEFAULT ((0)) FOR [Player1SecondaryResult]
GO
ALTER TABLE [dbo].[TKGame] ADD  CONSTRAINT [DF_TKGame_Player2SecondaryResult]  DEFAULT ((0)) FOR [Player2SecondaryResult]
GO
ALTER TABLE [dbo].[TKPlayer] ADD  CONSTRAINT [DF_TKPlayer_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[TKPlayer] ADD  CONSTRAINT [DF_TKPlayer_Wins]  DEFAULT ((0)) FOR [Wins]
GO
ALTER TABLE [dbo].[TKPlayer] ADD  CONSTRAINT [DF_TKPlayer_Losses]  DEFAULT ((0)) FOR [Losses]
GO
ALTER TABLE [dbo].[TKPlayer] ADD  CONSTRAINT [DF_TKPlayer_Draws]  DEFAULT ((0)) FOR [Draws]
GO
ALTER TABLE [dbo].[TKTeamMatch] ADD  CONSTRAINT [DF_TKTeamMatch_MatchPoints]  DEFAULT ((0)) FOR [Team1MatchPoints]
GO
ALTER TABLE [dbo].[TKTeamMatch] ADD  CONSTRAINT [DF_TKTeamMatch_Team2MatchPoints]  DEFAULT ((0)) FOR [Team2MatchPoints]
GO
ALTER TABLE [dbo].[TKTeamMatch] ADD  CONSTRAINT [DF_TKTeamMatch_Team1Points]  DEFAULT ((0)) FOR [Team1Points]
GO
ALTER TABLE [dbo].[TKTeamMatch] ADD  CONSTRAINT [DF_TKTeamMatch_Team2Points]  DEFAULT ((0)) FOR [Team2Points]
GO
ALTER TABLE [dbo].[TKTeamMatch] ADD  CONSTRAINT [DF_TKTeamMatch_Team1SecondaryPoints]  DEFAULT ((0)) FOR [Team1SecondaryPoints]
GO
ALTER TABLE [dbo].[TKTeamMatch] ADD  CONSTRAINT [DF_TKTeamMatch_Team2SecondaryPoints]  DEFAULT ((0)) FOR [Team2SecondaryPoints]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_TournamentDate]  DEFAULT (getdate()) FOR [TournamentDate]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_UseSecondaryPoints]  DEFAULT ((0)) FOR [UseSecondaryPoints]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_LockForEdit]  DEFAULT ((1)) FOR [AllowEdit]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_OnlineSignup]  DEFAULT ((0)) FOR [OnlineSignup]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_TournamentEndDate]  DEFAULT (getdate()) FOR [TournamentEndDate]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_UseSeed]  DEFAULT ((0)) FOR [UseSeed]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_UseRandomTeamPairings]  DEFAULT ((0)) FOR [UseRandomTeamPairings]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_CheckForTotalPoints]  DEFAULT ((0)) FOR [CheckForTotalPoints]
GO
ALTER TABLE [dbo].[TKTournament] ADD  CONSTRAINT [DF_TKTournament_PlayersDefaultActive]  DEFAULT ((1)) FOR [PlayersDefaultActive]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Seed]  DEFAULT ((1)) FOR [Seed]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_FairPlay]  DEFAULT ((0)) FOR [FairPlay]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Painting]  DEFAULT ((0)) FOR [Painting]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Penalty]  DEFAULT ((0)) FOR [Penalty]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_DoNotRank]  DEFAULT ((0)) FOR [DoNotRank]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_BattlePoints]  DEFAULT ((0)) FOR [BattlePoints]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_SecondaryPoints]  DEFAULT ((0)) FOR [SecondaryPoints]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Quiz]  DEFAULT ((0)) FOR [Quiz]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Wins]  DEFAULT ((0)) FOR [Wins]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Draws]  DEFAULT ((0)) FOR [Draws]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Losses]  DEFAULT ((0)) FOR [Losses]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_TournamentPoints]  DEFAULT ((0)) FOR [TournamentPoints]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_NonPlayer]  DEFAULT ((0)) FOR [NonPlayer]
GO
ALTER TABLE [dbo].[TKTournamentPlayer] ADD  CONSTRAINT [DF_TKTournamentPlayer_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[TKTournamentTeam] ADD  CONSTRAINT [DF_TKTournamentTeam_Penalty]  DEFAULT ((0)) FOR [Penalty]
GO
ALTER TABLE [dbo].[TKTournamentTeam] ADD  CONSTRAINT [DF_TKTournamentTeam_BattlePoints]  DEFAULT ((0)) FOR [BattlePoints]
GO
ALTER TABLE [dbo].[TKTournamentTeam] ADD  CONSTRAINT [DF_TKTournamentTeam_GamePoints]  DEFAULT ((0)) FOR [MatchPoints]
GO
ALTER TABLE [dbo].[TKTournamentTeam] ADD  CONSTRAINT [DF_TKTournamentTeam_SecondaryPoints]  DEFAULT ((0)) FOR [SecondaryPoints]
GO
ALTER TABLE [dbo].[MOArmy]  WITH CHECK ADD  CONSTRAINT [FK_Army_Codex] FOREIGN KEY([CodexId])
REFERENCES [dbo].[MOCodex] ([Id])
GO
ALTER TABLE [dbo].[MOArmy] CHECK CONSTRAINT [FK_Army_Codex]
GO
ALTER TABLE [dbo].[MOArmy]  WITH CHECK ADD  CONSTRAINT [FK_Army_Team] FOREIGN KEY([TeamId])
REFERENCES [dbo].[MOTeam] ([Id])
GO
ALTER TABLE [dbo].[MOArmy] CHECK CONSTRAINT [FK_Army_Team]
GO
ALTER TABLE [dbo].[MOArmy]  WITH CHECK ADD  CONSTRAINT [FK_MOArmy_MOArmy] FOREIGN KEY([ArmyTypeId])
REFERENCES [dbo].[MOArmy] ([Id])
GO
ALTER TABLE [dbo].[MOArmy] CHECK CONSTRAINT [FK_MOArmy_MOArmy]
GO
ALTER TABLE [dbo].[MOArmy]  WITH CHECK ADD  CONSTRAINT [FK_MOArmy_MOTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[MOTournament] ([Id])
GO
ALTER TABLE [dbo].[MOArmy] CHECK CONSTRAINT [FK_MOArmy_MOTournament]
GO
ALTER TABLE [dbo].[MOCodex]  WITH CHECK ADD  CONSTRAINT [FK_MOCodex_TKGameSystem] FOREIGN KEY([GameSystemId])
REFERENCES [dbo].[TKGameSystem] ([Id])
GO
ALTER TABLE [dbo].[MOCodex] CHECK CONSTRAINT [FK_MOCodex_TKGameSystem]
GO
ALTER TABLE [dbo].[MOPairing]  WITH CHECK ADD  CONSTRAINT [FK_MOPairing_MOTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[MOTournament] ([Id])
GO
ALTER TABLE [dbo].[MOPairing] CHECK CONSTRAINT [FK_MOPairing_MOTournament]
GO
ALTER TABLE [dbo].[MOPairing]  WITH CHECK ADD  CONSTRAINT [FK_Pairing_Army] FOREIGN KEY([FriendlyArmyId])
REFERENCES [dbo].[MOArmy] ([Id])
GO
ALTER TABLE [dbo].[MOPairing] CHECK CONSTRAINT [FK_Pairing_Army]
GO
ALTER TABLE [dbo].[MOPairing]  WITH CHECK ADD  CONSTRAINT [FK_Pairing_Army1] FOREIGN KEY([EnemyArmyId])
REFERENCES [dbo].[MOArmy] ([Id])
GO
ALTER TABLE [dbo].[MOPairing] CHECK CONSTRAINT [FK_Pairing_Army1]
GO
ALTER TABLE [dbo].[MOPairing]  WITH CHECK ADD  CONSTRAINT [FK_Pairing_PairingEvaluation] FOREIGN KEY([PairingEvaluationId])
REFERENCES [dbo].[MOPairingEvaluation] ([Id])
GO
ALTER TABLE [dbo].[MOPairing] CHECK CONSTRAINT [FK_Pairing_PairingEvaluation]
GO
ALTER TABLE [dbo].[MOPairingData]  WITH CHECK ADD  CONSTRAINT [FK_MOPairingData_MOArmy] FOREIGN KEY([FriendlyArmyId])
REFERENCES [dbo].[MOArmy] ([Id])
GO
ALTER TABLE [dbo].[MOPairingData] CHECK CONSTRAINT [FK_MOPairingData_MOArmy]
GO
ALTER TABLE [dbo].[MOPairingData]  WITH CHECK ADD  CONSTRAINT [FK_MOPairingData_MOArmy1] FOREIGN KEY([EnemyArmyId])
REFERENCES [dbo].[MOArmy] ([Id])
GO
ALTER TABLE [dbo].[MOPairingData] CHECK CONSTRAINT [FK_MOPairingData_MOArmy1]
GO
ALTER TABLE [dbo].[MOPairingData]  WITH CHECK ADD  CONSTRAINT [FK_MOPairingData_MOConsolidatedPairingData] FOREIGN KEY([MOConsolidatedPairingDataId])
REFERENCES [dbo].[MOConsolidatedPairingData] ([Id])
GO
ALTER TABLE [dbo].[MOPairingData] CHECK CONSTRAINT [FK_MOPairingData_MOConsolidatedPairingData]
GO
ALTER TABLE [dbo].[MOPairingData]  WITH CHECK ADD  CONSTRAINT [FK_MOPairingData_MOMissionType] FOREIGN KEY([MissionTypeId])
REFERENCES [dbo].[MOMissionType] ([Id])
GO
ALTER TABLE [dbo].[MOPairingData] CHECK CONSTRAINT [FK_MOPairingData_MOMissionType]
GO
ALTER TABLE [dbo].[MOTeam]  WITH CHECK ADD  CONSTRAINT [FK_MOTeam_MOTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[MOTournament] ([Id])
GO
ALTER TABLE [dbo].[MOTeam] CHECK CONSTRAINT [FK_MOTeam_MOTournament]
GO
ALTER TABLE [dbo].[TKGame]  WITH CHECK ADD  CONSTRAINT [FK_Game_TKTournamentPlayer] FOREIGN KEY([Player1Id])
REFERENCES [dbo].[TKTournamentPlayer] ([Id])
GO
ALTER TABLE [dbo].[TKGame] CHECK CONSTRAINT [FK_Game_TKTournamentPlayer]
GO
ALTER TABLE [dbo].[TKGame]  WITH CHECK ADD  CONSTRAINT [FK_Game_TKTournamentPlayer1] FOREIGN KEY([Player2Id])
REFERENCES [dbo].[TKTournamentPlayer] ([Id])
GO
ALTER TABLE [dbo].[TKGame] CHECK CONSTRAINT [FK_Game_TKTournamentPlayer1]
GO
ALTER TABLE [dbo].[TKGame]  WITH CHECK ADD  CONSTRAINT [FK_TKGame_TKTeamMatch] FOREIGN KEY([TeamMatchId])
REFERENCES [dbo].[TKTeamMatch] ([Id])
GO
ALTER TABLE [dbo].[TKGame] CHECK CONSTRAINT [FK_TKGame_TKTeamMatch]
GO
ALTER TABLE [dbo].[TKGame]  WITH CHECK ADD  CONSTRAINT [FK_TKGame_TKTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[TKTournament] ([Id])
GO
ALTER TABLE [dbo].[TKGame] CHECK CONSTRAINT [FK_TKGame_TKTournament]
GO
ALTER TABLE [dbo].[TKOrganizer]  WITH CHECK ADD  CONSTRAINT [FK_TKOrganizer_TKPlayer] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[TKPlayer] ([Id])
GO
ALTER TABLE [dbo].[TKOrganizer] CHECK CONSTRAINT [FK_TKOrganizer_TKPlayer]
GO
ALTER TABLE [dbo].[TKOrganizer]  WITH CHECK ADD  CONSTRAINT [FK_TKOrganizer_TKTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[TKTournament] ([Id])
GO
ALTER TABLE [dbo].[TKOrganizer] CHECK CONSTRAINT [FK_TKOrganizer_TKTournament]
GO
ALTER TABLE [dbo].[TKRanking]  WITH CHECK ADD  CONSTRAINT [FK_Ranking_TKPlayer] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[TKPlayer] ([Id])
GO
ALTER TABLE [dbo].[TKRanking] CHECK CONSTRAINT [FK_Ranking_TKPlayer]
GO
ALTER TABLE [dbo].[TKRanking]  WITH CHECK ADD  CONSTRAINT [FK_TKRanking_TKRankingType] FOREIGN KEY([RankingTypeId])
REFERENCES [dbo].[TKRankingType] ([Id])
GO
ALTER TABLE [dbo].[TKRanking] CHECK CONSTRAINT [FK_TKRanking_TKRankingType]
GO
ALTER TABLE [dbo].[TKRankingType]  WITH CHECK ADD  CONSTRAINT [FK_TKRankingType_TKGameSystem] FOREIGN KEY([GameSystemId])
REFERENCES [dbo].[TKGameSystem] ([Id])
GO
ALTER TABLE [dbo].[TKRankingType] CHECK CONSTRAINT [FK_TKRankingType_TKGameSystem]
GO
ALTER TABLE [dbo].[TKTeamMatch]  WITH CHECK ADD  CONSTRAINT [FK_TKTeamMatch_TKTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[TKTournament] ([Id])
GO
ALTER TABLE [dbo].[TKTeamMatch] CHECK CONSTRAINT [FK_TKTeamMatch_TKTournament]
GO
ALTER TABLE [dbo].[TKTeamMatch]  WITH CHECK ADD  CONSTRAINT [FK_TKTeamMatch_TKTournamentTeam] FOREIGN KEY([Team1Id])
REFERENCES [dbo].[TKTournamentTeam] ([Id])
GO
ALTER TABLE [dbo].[TKTeamMatch] CHECK CONSTRAINT [FK_TKTeamMatch_TKTournamentTeam]
GO
ALTER TABLE [dbo].[TKTeamMatch]  WITH CHECK ADD  CONSTRAINT [FK_TKTeamMatch_TKTournamentTeam1] FOREIGN KEY([Team2Id])
REFERENCES [dbo].[TKTournamentTeam] ([Id])
GO
ALTER TABLE [dbo].[TKTeamMatch] CHECK CONSTRAINT [FK_TKTeamMatch_TKTournamentTeam1]
GO
ALTER TABLE [dbo].[TKTournament]  WITH CHECK ADD  CONSTRAINT [FK_TKTournament_GameSystem] FOREIGN KEY([GameSystemId])
REFERENCES [dbo].[TKGameSystem] ([Id])
GO
ALTER TABLE [dbo].[TKTournament] CHECK CONSTRAINT [FK_TKTournament_GameSystem]
GO
ALTER TABLE [dbo].[TKTournament]  WITH CHECK ADD  CONSTRAINT [FK_TKTournament_TKSinglesScoringSystem] FOREIGN KEY([SinglesScoringSystemId])
REFERENCES [dbo].[TKSinglesScoringSystem] ([Id])
GO
ALTER TABLE [dbo].[TKTournament] CHECK CONSTRAINT [FK_TKTournament_TKSinglesScoringSystem]
GO
ALTER TABLE [dbo].[TKTournament]  WITH CHECK ADD  CONSTRAINT [FK_TKTournament_TKTeamScoringSystem] FOREIGN KEY([TeamScoringSystemId])
REFERENCES [dbo].[TKTeamScoringSystem] ([Id])
GO
ALTER TABLE [dbo].[TKTournament] CHECK CONSTRAINT [FK_TKTournament_TKTeamScoringSystem]
GO
ALTER TABLE [dbo].[TKTournament]  WITH CHECK ADD  CONSTRAINT [FK_TKTournament_TKTournamentType] FOREIGN KEY([TournamentTypeId])
REFERENCES [dbo].[TKTournamentType] ([Id])
GO
ALTER TABLE [dbo].[TKTournament] CHECK CONSTRAINT [FK_TKTournament_TKTournamentType]
GO
ALTER TABLE [dbo].[TKTournamentPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentPlayer_MOCodex] FOREIGN KEY([PrimaryCodexId])
REFERENCES [dbo].[MOCodex] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentPlayer] CHECK CONSTRAINT [FK_TKTournamentPlayer_MOCodex]
GO
ALTER TABLE [dbo].[TKTournamentPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentPlayer_MOCodex1] FOREIGN KEY([SecondaryCodexId])
REFERENCES [dbo].[MOCodex] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentPlayer] CHECK CONSTRAINT [FK_TKTournamentPlayer_MOCodex1]
GO
ALTER TABLE [dbo].[TKTournamentPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentPlayer_MOCodex2] FOREIGN KEY([TertiaryCodexId])
REFERENCES [dbo].[MOCodex] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentPlayer] CHECK CONSTRAINT [FK_TKTournamentPlayer_MOCodex2]
GO
ALTER TABLE [dbo].[TKTournamentPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentPlayer_MOCodex3] FOREIGN KEY([QuaternaryCodexId])
REFERENCES [dbo].[MOCodex] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentPlayer] CHECK CONSTRAINT [FK_TKTournamentPlayer_MOCodex3]
GO
ALTER TABLE [dbo].[TKTournamentPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentPlayer_TKPlayer] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[TKPlayer] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentPlayer] CHECK CONSTRAINT [FK_TKTournamentPlayer_TKPlayer]
GO
ALTER TABLE [dbo].[TKTournamentPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentPlayer_TKTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[TKTournament] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentPlayer] CHECK CONSTRAINT [FK_TKTournamentPlayer_TKTournament]
GO
ALTER TABLE [dbo].[TKTournamentPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentPlayer_TKTournamentTeam] FOREIGN KEY([TournamentTeamId])
REFERENCES [dbo].[TKTournamentTeam] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentPlayer] CHECK CONSTRAINT [FK_TKTournamentPlayer_TKTournamentTeam]
GO
ALTER TABLE [dbo].[TKTournamentTeam]  WITH CHECK ADD  CONSTRAINT [FK_TKTournamentTeam_TKTournament] FOREIGN KEY([TournamentId])
REFERENCES [dbo].[TKTournament] ([Id])
GO
ALTER TABLE [dbo].[TKTournamentTeam] CHECK CONSTRAINT [FK_TKTournamentTeam_TKTournament]
GO
USE [master]
GO
ALTER DATABASE [voresmail_dk_db] SET  READ_WRITE 
GO
