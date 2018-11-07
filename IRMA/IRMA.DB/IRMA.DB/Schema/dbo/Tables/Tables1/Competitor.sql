CREATE TABLE [dbo].[Competitor] (
    [CompetitorID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Competitor] PRIMARY KEY CLUSTERED ([CompetitorID] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Competitor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Competitor] TO [IRMAReportsRole]
    AS [dbo];

