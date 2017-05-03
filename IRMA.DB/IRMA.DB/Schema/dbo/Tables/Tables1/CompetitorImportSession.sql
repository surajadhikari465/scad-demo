CREATE TABLE [dbo].[CompetitorImportSession] (
    [CompetitorImportSessionID] INT           IDENTITY (1, 1) NOT NULL,
    [User_ID]                   INT           NOT NULL,
    [StartDateTime]             SMALLDATETIME NOT NULL,
    CONSTRAINT [PK_CompetitorImportSession] PRIMARY KEY CLUSTERED ([CompetitorImportSessionID] ASC),
    CONSTRAINT [FK_CompetitorImportSession_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorImportSession] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorImportSession] TO [IRMAReportsRole]
    AS [dbo];

