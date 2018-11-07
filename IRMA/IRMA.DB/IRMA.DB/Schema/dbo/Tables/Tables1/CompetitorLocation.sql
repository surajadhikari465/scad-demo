CREATE TABLE [dbo].[CompetitorLocation] (
    [CompetitorLocationID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]                 VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CompetitorLocation] PRIMARY KEY CLUSTERED ([CompetitorLocationID] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorLocation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorLocation] TO [IRMAReportsRole]
    AS [dbo];

