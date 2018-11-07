CREATE TABLE [dbo].[CycleCountItems] (
    [CycleCountItemID] INT        IDENTITY (1, 1) NOT NULL,
    [CycleCountID]     INT        NOT NULL,
    [Item_Key]         INT        NOT NULL,
    [AvgCost]          SMALLMONEY NULL,
    CONSTRAINT [PK_CycleCountItems_CycleCountItemID] PRIMARY KEY CLUSTERED ([CycleCountItemID] ASC),
    CONSTRAINT [FK_CycleCountItems_CycleCountHeader1] FOREIGN KEY ([CycleCountID]) REFERENCES [dbo].[CycleCountHeader] ([CycleCountID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CycleCountItems_CountItem]
    ON [dbo].[CycleCountItems]([CycleCountID] ASC, [Item_Key] ASC) WITH (FILLFACTOR = 80);


GO
GRANT INSERT
    ON OBJECT::[dbo].[CycleCountItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[CycleCountItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[CycleCountItems] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountItems] TO [IRMAReportsRole]
    AS [dbo];

