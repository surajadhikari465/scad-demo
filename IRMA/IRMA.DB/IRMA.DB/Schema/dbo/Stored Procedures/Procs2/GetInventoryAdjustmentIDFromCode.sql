CREATE PROCEDURE dbo.GetInventoryAdjustmentIDFromCode
	(@Abbreviation char(2),
	 @InventoryAdjustmentCode_ID int OUTPUT)
AS 

SET NOCOUNT ON

/*
	if the Abbreviation doesnt exist in InventoryAdjustmentCode -99 will be returned. The output parameter expects and integer and was failing when NULL
	was being returned. 
*/

SELECT @InventoryAdjustmentCode_ID = InventoryAdjustmentCode_ID
  FROM dbo.InventoryAdjustmentCode
WHERE Abbreviation = @Abbreviation

set @InventoryAdjustmentCode_ID = isnull(@InventoryAdjustmentCode_ID,-99)

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentIDFromCode] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentIDFromCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentIDFromCode] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentIDFromCode] TO [IRMAReportsRole]
    AS [dbo];

