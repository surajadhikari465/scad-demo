CREATE TABLE [dbo].[ReasonCodeMappings] (
    [ReasonCodeMappingID] INT IDENTITY (1, 1) NOT NULL,
    [ReasonCodeTypeID]    INT NOT NULL,
    [ReasonCodeDetailID]  INT NOT NULL,
    [Disabled]            BIT CONSTRAINT [DF_ReasonCodeMappings_Disabled] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ReasonCodeMapping] PRIMARY KEY CLUSTERED ([ReasonCodeMappingID] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON),
    CONSTRAINT [FK_ReasonCodeMapping_ReasonCodeDetail] FOREIGN KEY ([ReasonCodeDetailID]) REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID]),
    CONSTRAINT [FK_ReasonCodeMapping_ReasonCodeType] FOREIGN KEY ([ReasonCodeTypeID]) REFERENCES [dbo].[ReasonCodeType] ([ReasonCodeTypeID])
);


GO
ALTER TABLE [dbo].[ReasonCodeMappings] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ReasonCodeMappings] TO [iCONReportingRole]
    AS [dbo];

