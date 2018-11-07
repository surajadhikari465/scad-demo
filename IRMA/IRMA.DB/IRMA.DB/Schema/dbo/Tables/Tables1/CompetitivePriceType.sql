CREATE TABLE [dbo].[CompetitivePriceType] (
    [CompetitivePriceTypeID] INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Description]            VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CompetitivePriceType] PRIMARY KEY CLUSTERED ([CompetitivePriceTypeID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitivePriceType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitivePriceType] TO [IRMAReportsRole]
    AS [dbo];

