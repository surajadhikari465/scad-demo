CREATE TABLE [dbo].[ShrinkSubType] 
 (
 [ShrinkSubType_ID]			  INT           IDENTITY (1, 1) NOT NULL,
 [InventoryAdjustmentCode_ID] INT           NOT NULL,
 [ReasonCodeDescription]      VARCHAR (255) NOT NULL,
 [LastUpdateUser_Id]          INT           NOT NULL,
 [LastUpdateDateTime]         DATETIME      NOT NULL,
 CONSTRAINT [PK_ShrinkSubType] PRIMARY KEY CLUSTERED ([ShrinkSubType_ID] ASC) WITH (FILLFACTOR = 80),
 CONSTRAINT [FK_ShrinkSubType_InventoryAdjustmentCode_ID] FOREIGN KEY ([InventoryAdjustmentCode_ID]) REFERENCES [dbo].[InventoryAdjustmentCode] ([InventoryAdjustmentCode_ID])
 );
 
GO
ALTER TABLE [dbo].[ShrinkSubType] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ShrinkSubType] TO [IRMASchedJobsRole]
    AS [dbo];

GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ShrinkSubType] TO [IRMA_Teradata]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ShrinkSubType] TO [IRMAReportsRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[ShrinkSubType] TO [IRMASchedJobs]
    AS [dbo]

GO
GRANT SELECT
    ON OBJECT::[dbo].[ShrinkSubType] TO [IRMAClientRole]
    AS [dbo];