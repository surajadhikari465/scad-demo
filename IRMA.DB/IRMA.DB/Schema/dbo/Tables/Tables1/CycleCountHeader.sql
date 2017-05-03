CREATE TABLE [dbo].[CycleCountHeader] (
    [CycleCountID]  INT      IDENTITY (1, 1) NOT NULL,
    [MasterCountID] INT      NOT NULL,
    [InvLocID]      INT      NULL,
    [StartScan]     DATETIME NULL,
    [External]      BIT      CONSTRAINT [DF_CycleCountItems_External] DEFAULT ((0)) NOT NULL,
    [ClosedDate]    DATETIME CONSTRAINT [DF_CycleCountHeader_Closed] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CycleCountItems] PRIMARY KEY NONCLUSTERED ([CycleCountID] ASC),
    CONSTRAINT [FK_CycleCountHeader_CycleCountMaster] FOREIGN KEY ([MasterCountID]) REFERENCES [dbo].[CycleCountMaster] ([MasterCountID])
);


GO
CREATE NONCLUSTERED INDEX [idxCycleCountName]
    ON [dbo].[CycleCountHeader]([CycleCountID] ASC);


GO
CREATE NONCLUSTERED INDEX [idxInvLocID]
    ON [dbo].[CycleCountHeader]([InvLocID] ASC);


GO
CREATE NONCLUSTERED INDEX [idxStartScan]
    ON [dbo].[CycleCountHeader]([StartScan] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountHeader] TO [IRMAReportsRole]
    AS [dbo];

