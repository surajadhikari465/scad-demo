CREATE TABLE [dbo].[AvgCostQueue] (
    [AvgCostQueueID] INT IDENTITY (1, 1) NOT NULL,
    [Item_Key]       INT NOT NULL,
    [Store_No]       INT NOT NULL,
    [SubTeam_No]     INT NOT NULL,
    CONSTRAINT [PK_AvgCostQueue] PRIMARY KEY CLUSTERED ([AvgCostQueueID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_AvgCostQueue_StoreNo_ItemKey]
    ON [dbo].[AvgCostQueue]([Store_No] ASC, [Item_Key] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostQueue] TO [IRMAReportsRole]
    AS [dbo];

