CREATE TABLE [dbo].[VendorDealType] (
    [VendorDealTypeID] INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Code]             CHAR (1)     NOT NULL,
    [Description]      VARCHAR (50) NOT NULL,
    [CaseAmtType]      VARCHAR (10) NULL,
    CONSTRAINT [PK_VendorDealType] PRIMARY KEY CLUSTERED ([VendorDealTypeID] ASC) WITH (FILLFACTOR = 80)
);





GO
ALTER TABLE [dbo].[VendorDealType] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorDealType] TO [IMHARole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealType] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealType] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[VendorDealType] TO [iCONReportingRole]
    AS [dbo];

