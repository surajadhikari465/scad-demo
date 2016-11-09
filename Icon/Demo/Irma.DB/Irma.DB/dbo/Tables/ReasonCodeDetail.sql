CREATE TABLE [dbo].[ReasonCodeDetail] (
    [ReasonCodeDetailID] INT           IDENTITY (1, 1) NOT NULL,
    [ReasonCode]         NCHAR (3)     NOT NULL,
    [ReasonCodeDesc]     VARCHAR (50)  NOT NULL,
    [ReasonCodeExtDesc]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ReasonCodeDetail] PRIMARY KEY CLUSTERED ([ReasonCodeDetailID] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON)
);





GO
ALTER TABLE [dbo].[ReasonCodeDetail] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeDetail] TO [iCONReportingRole]
    AS [dbo];

