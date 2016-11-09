CREATE TABLE [dbo].[LabelType] (
    [LabelType_ID]  INT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LabelTypeDesc] VARCHAR (4) NULL,
    CONSTRAINT [PK_LabelType_LabelTypeId] PRIMARY KEY CLUSTERED ([LabelType_ID] ASC)
);





GO
ALTER TABLE [dbo].[LabelType] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT DELETE
    ON OBJECT::[dbo].[LabelType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[LabelType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[LabelType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LabelType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LabelType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LabelType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LabelType] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[LabelType] TO [iCONReportingRole]
    AS [dbo];

