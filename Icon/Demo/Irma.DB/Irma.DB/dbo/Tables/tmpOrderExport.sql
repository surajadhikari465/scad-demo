CREATE TABLE [dbo].[tmpOrderExport] (
    [OrderExportQueueID]      INT      NULL,
    [OrderHeader_ID]          INT      NULL,
    [QueueInsertedDate]       DATETIME NULL,
    [DeliveredToStoreOpsDate] DATETIME NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO



GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO



GO
GRANT DELETE
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrderExport] TO [IRMAReportsRole]
    AS [dbo];

