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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentCodeList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentCodeList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentCodeList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentCodeList] TO [IRMAReportsRole]
    AS [dbo];

