CREATE TABLE [dbo].[tmpBulkOrderItemFix] (
    [Item_key]       VARCHAR (255) NULL,
    [Identifier]     VARCHAR (255) NULL,
    [Description]    VARCHAR (255) NULL,
    [Cost Unit]      VARCHAR (255) NULL,
    [Cost per Pound] VARCHAR (255) NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpBulkOrderItemFix] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpBulkOrderItemFix] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpBulkOrderItemFix] TO [IRMAReportsRole]
    AS [dbo];

