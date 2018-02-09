CREATE PROCEDURE [dbo].[GetShrinkSubTypes]
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT sst.ShrinkSubType_ID AS ShrinkSubTypeID
          ,iac.InventoryAdjustmentCode_ID as InventoryAdjustmentCodeID
	      ,sst.ReasonCodeDescription AS ShrinkSubType
		  ,iac.AdjustmentDescription AS ShrinkType
		  ,iac.Abbreviation AS  Abbreviation
		  ,iac.AdjustmentDescription + '-' + sst.ReasonCodeDescription AS ReasonCode
		  ,sst.LastUpdateUser_Id AS LastUpdateUserId ,
		  sst.LastUpdateDateTime AS LastUpdateDateTime
	  FROM [dbo].[ShrinkSubType] sst
	  INNER JOIN [dbo].[InventoryAdjustmentCode] iac ON iac.InventoryAdjustmentCode_ID = sst.InventoryAdjustmentCode_ID 
	  ORDER BY ReasonCodeDescription ASC

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetShrinkSubTypes] TO [IRMAClientRole]
    AS [dbo];