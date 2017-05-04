CREATE TABLE [dbo].[CycleCountMaster] (
    [MasterCountID]       INT      IDENTITY (1, 1) NOT NULL,
    [EndofPeriod]         BIT      CONSTRAINT [DF_MasterCycleCount_EndofPeriod] DEFAULT ((0)) NOT NULL,
    [Store_No]            INT      NOT NULL,
    [SubTeam_No]          INT      NOT NULL,
    [EndScan]             DATETIME NOT NULL,
    [ClosedDate]          DATETIME CONSTRAINT [DF_MasterCycleCount_Closed] DEFAULT ((0)) NULL,
    [UpdateIH]            BIT      CONSTRAINT [UpdateIHDefault] DEFAULT ((0)) NOT NULL,
    [SetNonCountedToZero] BIT      CONSTRAINT [SetNonCountedToZeroDefault] DEFAULT ((0)) NOT NULL,
    [IHUpdated]           BIT      CONSTRAINT [IHUpdatedDefault] DEFAULT ((0)) NOT NULL,
    [BOHFileDate]         DATETIME NULL,
    CONSTRAINT [PK_MasterCycleCount] PRIMARY KEY NONCLUSTERED ([MasterCountID] ASC),
    CONSTRAINT [FK_MasterCycleCount_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_MasterCycleCount_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
CREATE NONCLUSTERED INDEX [idxStoreNo]
    ON [dbo].[CycleCountMaster]([Store_No] ASC);


GO
CREATE NONCLUSTERED INDEX [idxSubTeamNo]
    ON [dbo].[CycleCountMaster]([SubTeam_No] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountMaster] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountMaster] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountMaster] TO [IRMAReportsRole]
    AS [dbo];

