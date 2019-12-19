CREATE TABLE [dbo].[TEAM_Interim] (
    [idTeam]    INT           IDENTITY (1, 1) NOT NULL,
    [teamName]  NVARCHAR (50) NOT NULL,
    [teamVIMId] INT           NULL,
    CONSTRAINT [PK_TEAM] PRIMARY KEY CLUSTERED ([idTeam] ASC)
);

