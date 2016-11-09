CREATE TABLE [dbo].[DSDVendorStore] (
    [DSDVendorStoreID] INT           IDENTITY (1, 1) NOT NULL,
    [Vendor_ID]        INT           NOT NULL,
    [Store_No]         INT           NOT NULL,
    [BeginDate]        SMALLDATETIME NOT NULL,
    CONSTRAINT [PK_DSDVendorStore] PRIMARY KEY CLUSTERED ([DSDVendorStoreID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_DSDVendorStore_Vendor] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);


GO
ALTER TABLE [dbo].[DSDVendorStore] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[DSDVendorStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[DSDVendorStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DSDVendorStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[DSDVendorStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DSDVendorStore] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[DSDVendorStore] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[DSDVendorStore] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[DSDVendorStore] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[DSDVendorStore] TO [iCONReportingRole]
    AS [dbo];

