CREATE TABLE [dbo].[PMPriceChangeLoad] (
    [Item_Key]  VARCHAR (255) NULL,
    [Price]     VARCHAR (255) NULL,
    [Org_Level] VARCHAR (255) NULL,
    [Level_ID]  VARCHAR (255) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMPriceChangeLoad] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMPriceChangeLoad] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMPriceChangeLoad] TO [IRMAReportsRole]
    AS [dbo];

