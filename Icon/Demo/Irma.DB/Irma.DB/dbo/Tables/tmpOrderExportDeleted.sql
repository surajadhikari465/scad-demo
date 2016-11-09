CREATE TABLE [dbo].[tmpOrderExportDeleted] (
    [OrderHeader_ID] INT NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrderExportDeleted] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrderExportDeleted] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrderExportDeleted] TO [IRMAReportsRole]
    AS [dbo];

