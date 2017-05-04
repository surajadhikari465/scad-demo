CREATE TABLE [dbo].[AvgCostAdjReason] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (75) NOT NULL,
    [Active]      BIT          CONSTRAINT [DF_AvgCostAdjReason_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AvgCostAdjReason] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostAdjReason] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostAdjReason] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostAdjReason] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostAdjReason] TO [IRMAReportsRole]
    AS [dbo];

