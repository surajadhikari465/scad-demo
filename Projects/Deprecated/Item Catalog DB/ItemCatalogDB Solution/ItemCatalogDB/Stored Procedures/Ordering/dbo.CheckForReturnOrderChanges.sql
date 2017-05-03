if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForReturnOrderChanges]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForReturnOrderChanges]
GO
CREATE PROCEDURE dbo.CheckForReturnOrderChanges (
@Instance INT,
@User_ID  INT)
AS
BEGIN
    -- Change to fix the Bug 5534.
    -- SELECT COUNT(*) AS Changes 
    
    SELECT 
    Cost,
	CostUnit,CreditReason_ID,
	DiscountType,
	Freight,
	FreightUnit,
	Item_Key,
	LandedCost,
	MarkupPercent,
	MarkupCost,
	NetVendorItemDiscount,
	Package_Desc1,
	Package_Desc2,
	Package_Unit_ID,
	QuantityUnit,
	Quantity,
	QuantityDiscount,
	Retail_Unit_ID,
	Total_Weight,
	Units_Per_Pallet,
	HandlingCharge
    FROM ReturnOrder 
    WHERE ReturnOrder.[Instance] = @Instance AND
          ReturnOrder.[User_ID]  = @User_ID  AND
          Quantity > 0
END
GO

