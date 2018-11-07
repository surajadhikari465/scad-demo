CREATE TABLE [dbo].[tmpCostList] (
    [Identifier] VARCHAR (13) NULL,
    [Pound Cost] MONEY        NULL,
    [Item_key]   INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpCostList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpCostList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpCostList] TO [IRMAReportsRole]
    AS [dbo];

