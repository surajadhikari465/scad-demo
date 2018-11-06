SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryAdjustmentCodeList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryAdjustmentCodeList]
GO


CREATE PROCEDURE dbo.GetInventoryAdjustmentCodeList
	(@Distribution_Center bit,
	@WFM_Store bit)
AS 

SELECT [InventoryAdjustmentCode_ID]
      ,[Abbreviation] + '-' + [AdjustmentDescription] as LongDescription
      ,[Abbreviation]
      ,[AdjustmentDescription]
      ,[AllowsInventoryAdd]
      ,[AllowsInventoryDelete]
      ,[GLAccount]
      ,[Adjustment_Id]
      ,[LastUpdateUser_Id]
      ,[LastUpdateDateTime]
  FROM [dbo].[InventoryAdjustmentCode]
WHERE [ActiveForWarehouse] = isnull(@Distribution_Center,[ActiveForWarehouse])
  OR [ActiveForStore] = isnull(@WFM_Store,[ActiveForStore])

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 