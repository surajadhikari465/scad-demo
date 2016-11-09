CREATE TABLE [dbo].[CycleCountHistory] (
    [CycleCountItemID] INT             NOT NULL,
    [ScanDateTime]     DATETIME        NOT NULL,
    [Count]            DECIMAL (18, 4) NULL,
    [Weight]           DECIMAL (18, 4) NULL,
    [PackSize]         DECIMAL (9, 4)  NULL,
    [IsCaseCnt]        BIT             CONSTRAINT [DF_CycleCountHistory_IsCaseCnt] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [FK_CycleCountHistory_CycleCountItems1] FOREIGN KEY ([CycleCountItemID]) REFERENCES [dbo].[CycleCountItems] ([CycleCountItemID])
);


GO
CREATE CLUSTERED INDEX [idxCycleCountItemID]
    ON [dbo].[CycleCountHistory]([CycleCountItemID] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountHistory] TO [IRMAReportsRole]
    AS [dbo];

