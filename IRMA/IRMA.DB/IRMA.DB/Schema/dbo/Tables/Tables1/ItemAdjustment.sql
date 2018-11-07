CREATE TABLE [dbo].[ItemAdjustment] (
    [Adjustment_ID]   INT          NOT NULL,
    [Adjustment_Name] VARCHAR (25) NOT NULL,
    [Adjust_Quantity] BIT          CONSTRAINT [DF__ItemAdjus__Adjus__319832C2] DEFAULT ((0)) NOT NULL,
    [Adjustment_Type] INT          CONSTRAINT [DF__ItemAdjus__Adjus__328C56FB] DEFAULT ((0)) NOT NULL,
    [Reset_Addition]  BIT          CONSTRAINT [DF__ItemAdjus__Reset__33807B34] DEFAULT ((0)) NOT NULL,
    [User_ID]         INT          NULL,
    CONSTRAINT [PK_ItemAdjustment_AdjustmentID] PRIMARY KEY CLUSTERED ([Adjustment_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__ItemAdjus__User___5F492382] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
ALTER TABLE [dbo].[ItemAdjustment] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxItemAdjustmentName]
    ON [dbo].[ItemAdjustment]([Adjustment_Name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemAdjustmentUserID]
    ON [dbo].[ItemAdjustment]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAdjustment] TO [iCONReportingRole]
    AS [dbo];

