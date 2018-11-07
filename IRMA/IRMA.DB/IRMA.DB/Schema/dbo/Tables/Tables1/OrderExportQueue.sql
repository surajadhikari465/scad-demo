CREATE TABLE [dbo].[OrderExportQueue] (
    [OrderExportQueueID]      INT      IDENTITY (1, 1) NOT NULL,
    [OrderHeader_ID]          INT      NOT NULL,
    [QueueInsertedDate]       DATETIME NULL,
    [DeliveredToStoreOpsDate] DATETIME NULL,
    CONSTRAINT [PK_OrderExportQueue] PRIMARY KEY CLUSTERED ([OrderExportQueueID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [idxOrderExportQueueOrderHeaderID]
    ON [dbo].[OrderExportQueue]([OrderHeader_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportQueue] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderExportQueue] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[OrderExportQueue] TO [IRMASchedJobs]
    AS [dbo];

