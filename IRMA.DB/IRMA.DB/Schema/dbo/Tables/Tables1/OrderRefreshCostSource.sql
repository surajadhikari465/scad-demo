CREATE TABLE [dbo].[OrderRefreshCostSource] (
    [OrderRefreshCostSource_ID] INT          IDENTITY (1, 1) NOT NULL,
    [RefreshSourceName]         VARCHAR (30) NULL,
    CONSTRAINT [PK_OrderRefreshCostSource_OrderRefreshCostSource_ID] PRIMARY KEY CLUSTERED ([OrderRefreshCostSource_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
ALTER TABLE [dbo].[OrderRefreshCostSource] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderRefreshCostSource] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderRefreshCostSource] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderRefreshCostSource] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderRefreshCostSource] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderRefreshCostSource] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderRefreshCostSource] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[OrderRefreshCostSource] TO [iCONReportingRole]
    AS [dbo];

