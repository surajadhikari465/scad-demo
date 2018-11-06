CREATE TABLE [dbo].[tmpVendExport] (
    [VendorExportQueueID]     INT          NULL,
    [Vendor_ID]               INT          NULL,
    [QueueInsertedDate]       DATETIME     NULL,
    [DeliveredToStoreOpsDate] DATETIME     NULL,
    [Old_PS_Vendor_ID]        VARCHAR (10) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tmpVendExport] TO [IRMASchedJobs]
    AS [dbo];

