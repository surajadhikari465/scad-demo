CREATE TABLE [dbo].[ReasonCodeType] (
    [ReasonCodeTypeID]   INT          IDENTITY (1, 1) NOT NULL,
    [ReasonCodeTypeAbbr] CHAR (2)     NOT NULL,
    [ReasonCodeTypeDesc] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ReasonCodeType] PRIMARY KEY CLUSTERED ([ReasonCodeTypeID] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON)
);


GO
ALTER TABLE [dbo].[ReasonCodeType] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReasonCodeType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReasonCodeType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeType] TO [IRMA_Teradata]
    AS [dbo];





GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeType] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeType] TO [iCONReportingRole]
    AS [dbo];

