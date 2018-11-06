SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

/**************************************************************************
Author.....: Maria Younes
Create date: 07/15/2009
Description: Get corresponding Inventory Adjustment record for requested ID
***************************************************************************/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryAdjustmentCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryAdjustmentCode]
GO

CREATE PROCEDURE [dbo].[GetInventoryAdjustmentCode]

	@intInventoryCodeID AS int 
	
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT [InventoryAdjustmentCode_ID]
		  ,[Abbreviation]
		  ,[AdjustmentDescription]
		  ,[AllowsInventoryAdd]
		  ,[AllowsInventoryDelete]
		  ,[GLAccount]
		  ,[Adjustment_Id]
		  ,[LastUpdateUser_Id]
		  ,[LastUpdateDateTime]
		  ,[ActiveForWarehouse]
          ,[ActiveForStore]
	  FROM [dbo].[InventoryAdjustmentCode]
	WHERE [InventoryAdjustmentCode_ID] = @intInventoryCodeID


END

GO