CREATE TABLE [dbo].[VendorExportQueue] (
    [VendorExportQueueID]     INT          IDENTITY (1, 1) NOT NULL,
    [Vendor_ID]               INT          NOT NULL,
    [QueueInsertedDate]       DATETIME     NULL,
    [DeliveredToStoreOpsDate] DATETIME     NULL,
    [Old_PS_Vendor_ID]        VARCHAR (10) NULL,
    CONSTRAINT [PK_VendorExportQueue] PRIMARY KEY CLUSTERED ([VendorExportQueueID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorExportQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorExportQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorExportQueue] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorExportQueue] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[VendorExportQueue] TO [IRMASchedJobs]
    AS [dbo];

