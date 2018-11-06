CREATE TABLE [dbo].[InventoryAdjustmentCode] (
    [InventoryAdjustmentCode_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Abbreviation]               CHAR (2)      NOT NULL,
    [AdjustmentDescription]      VARCHAR (255) NULL,
    [AllowsInventoryAdd]         BIT           NOT NULL,
    [AllowsInventoryDelete]      BIT           NOT NULL,
    [AllowsInventoryReset]       BIT           NOT NULL,
    [GLAccount]                  INT           NOT NULL,
    [ActiveForWarehouse]         BIT           NOT NULL,
    [ActiveForStore]             BIT           NOT NULL,
    [Adjustment_Id]              INT           NOT NULL,
    [LastUpdateUser_Id]          INT           NOT NULL,
    [LastUpdateDateTime]         DATETIME      NOT NULL,
    CONSTRAINT [PK_InventoryAdjustmentCode] PRIMARY KEY CLUSTERED ([InventoryAdjustmentCode_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_InvAdjCode_Adjustment_Id] FOREIGN KEY ([Adjustment_Id]) REFERENCES [dbo].[ItemAdjustment] ([Adjustment_ID]),
    CONSTRAINT [FK_LastUpdate_User_Id] FOREIGN KEY ([LastUpdateUser_Id]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryAdjustmentCode] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryAdjustmentCode] TO [IRMASchedJobs]
    AS [dbo];

