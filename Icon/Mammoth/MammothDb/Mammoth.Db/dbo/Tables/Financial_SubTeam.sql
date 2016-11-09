CREATE TABLE [dbo].[Financial_SubTeam] (
    [FinancialSubTeamID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (255) NOT NULL,
    [PSNumber]           INT            NULL,
    [POSDeptNumber]      INT            NULL,
    [AddedDate]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]       DATETIME       NULL,
    CONSTRAINT [PK_Financial_SubTeam] PRIMARY KEY CLUSTERED ([FinancialSubTeamID] ASC) WITH (FILLFACTOR = 100)
);



