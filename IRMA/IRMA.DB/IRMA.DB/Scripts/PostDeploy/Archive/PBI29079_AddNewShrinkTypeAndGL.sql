--PBI 29079 : As an Accounting TM, I want a new Shrink Type and GL Added to the IRMA Database and Client

GO
IF NOT EXISTS(SELECT 1 FROM dbo.InventoryAdjustmentCode WHERE Abbreviation= 'IL')
INSERT INTO [dbo].[InventoryAdjustmentCode]
           ([Abbreviation]
           ,[AdjustmentDescription]
           ,[AllowsInventoryAdd]
           ,[AllowsInventoryDelete]
           ,[AllowsInventoryReset]
           ,[GLAccount]
           ,[ActiveForWarehouse]
           ,[ActiveForStore]
           ,[Adjustment_Id]
           ,[LastUpdateUser_Id]
           ,[LastUpdateDateTime])
     VALUES
           ('IL',
           'Needs Approval-Inventory Loss',
           0,
           1,
           0,
           520500,
           0,
           1,
           1,
           0,
           CURRENT_TIMESTAMP)

GO
DECLARE @InventoryAdjustmentCode_ID int
SELECT @InventoryAdjustmentCode_ID = InventoryAdjustmentCode_ID FROM dbo.InventoryAdjustmentCode WHERE Abbreviation= 'IL'

SET IDENTITY_INSERT dbo.ShrinkSubType ON; 
IF NOT EXISTS(SELECT 1 FROM dbo.ShrinkSubType WHERE ReasonCodeDescription = 'Needs Approval-Inventory Loss')
INSERT INTO [dbo].[ShrinkSubType]
           ([ShrinkSubType_ID]
		   ,[InventoryAdjustmentCode_ID]
           ,[ReasonCodeDescription]
           ,[LastUpdateUser_Id]
           ,[LastUpdateDateTime])
     VALUES
           (15,
		   @InventoryAdjustmentCode_ID,
           'Needs Approval-Inventory Loss',
           0,
           CURRENT_TIMESTAMP)
		   
SET IDENTITY_INSERT dbo.ShrinkSubType OFF;
GO