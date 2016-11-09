CREATE TABLE [dbo].[InventoryLocation] (
    [InvLoc_ID]   INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [InvLoc_Name] VARCHAR (50)  NOT NULL,
    [InvLoc_Desc] VARCHAR (50)  NOT NULL,
    [Store_No]    INT           NOT NULL,
    [SubTeam_No]  INT           NOT NULL,
    [Notes]       VARCHAR (250) NULL,
    CONSTRAINT [PK_InventoryLocation] PRIMARY KEY NONCLUSTERED ([InvLoc_ID] ASC),
    CONSTRAINT [FK_InventoryLocation_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_InventoryLocation_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [idxStoreSubTeamID] UNIQUE CLUSTERED ([InvLoc_ID] ASC, [Store_No] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80)
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryLocation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryLocation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryLocation] TO [IRMAReportsRole]
    AS [dbo];

