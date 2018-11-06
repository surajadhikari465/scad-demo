CREATE TABLE [dbo].[OrderExportDeletedQueue] (
    [OrderHeader_ID]            INT      NULL,
    [QueueInsertedDate]         DATETIME NULL,
    [DeliveredToStoreOpsDate]   DATETIME NULL,
    [OrderExportDeletedQueueId] INT      IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [pk_OrderExportDeletedQueue_OrderExportDeletedQueueId] PRIMARY KEY CLUSTERED ([OrderExportDeletedQueueId] ASC) WITH (FILLFACTOR = 90)
);


GO
ALTER TABLE [dbo].[OrderExportDeletedQueue] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderExportDeletedQueue] TO [iCONReportingRole]
    AS [dbo];

