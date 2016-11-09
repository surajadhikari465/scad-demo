CREATE TABLE [dbo].[tmpPurchases] (
    [DC]            INT          NULL,
    [SubTeam]       VARCHAR (50) NOT NULL,
    [OrderHeaderID] INT          NOT NULL,
    [Amount]        MONEY        NOT NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpPurchases] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpPurchases] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpPurchases] TO [IRMAReportsRole]
    AS [dbo];

