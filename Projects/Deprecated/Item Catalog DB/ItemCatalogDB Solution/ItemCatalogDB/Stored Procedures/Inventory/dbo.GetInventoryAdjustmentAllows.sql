SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryAdjustmentAllows]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryAdjustmentAllows]
GO


CREATE PROCEDURE dbo.GetInventoryAdjustmentAllows
	(@InventoryAdjustmentCode_ID int)
AS 

SELECT [AllowsInventoryAdd]
      ,[AllowsInventoryDelete]
      ,[AllowsInventoryReset]
  FROM [dbo].[InventoryAdjustmentCode]
WHERE InventoryAdjustmentCode_ID = @InventoryAdjustmentCode_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

  