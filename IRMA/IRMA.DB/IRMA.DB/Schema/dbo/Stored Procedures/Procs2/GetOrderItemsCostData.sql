CREATE PROCEDURE [dbo].[GetOrderItemsCostData]
    @OrderHeader_ID int
AS 

-- **************************************************************************
-- Procedure: GetOrderItemsCostData
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/19	KM		3744	Added update history template. 
-- 2012/01/04	BJL		3751	Include CurrentVendorCost in the return as CurrentVendorCost = UnitCost + NetVendorItemDiscount.
-- 2012/02/08	BJL		4681	Added ISNULL checks on CurrentVendorCost to return 0 if NULL CurrentVendorCost is returned.
-- **************************************************************************

BEGIN 
    SELECT 
		OrderItem_ID, 
		QuantityOrdered, 
        QuantityReceived, 
        Total_Weight, 
        Cost, 
        CostUnit, 
        Handling, 
        Freight, 
        AdjustedCost, 
        LandedCost, 
        OrderItem.QuantityDiscount, 
        LineItemCost, 
        LineItemFreight, 
        LineItemHandling, 
        ReceivedItemCost, 
        ReceivedItemFreight, 
        ReceivedItemHandling, 
		Freight3Party,
		LineItemFreight3Party,
        UnitCost, 
        UnitExtCost, 
        QuantityUnit, 
        HandlingUnit, 
        FreightUnit, 
        OrderItem.DiscountType, 
        OrderItem.Package_Desc1, 
        OrderItem.Package_Desc2, 
        OrderItem.Package_Unit_ID,
        OrderItem.MarkupPercent, 
        OrderItem.MarkupCost, 
        OrderItem.Retail_Unit_ID, 
        OrderItem.NetVendorItemDiscount,
		ISNULL(ISNULL(Cost,0) + ISNULL(NetVendorItemDiscount,0),0) as 'CurrentVendorCost'
    FROM
		OrderItem (nolock)
    WHERE
		OrderHeader_ID = @OrderHeader_ID AND
		DiscountType = 0
    ORDER BY
		OrderItem_ID
END


SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemsCostData] TO [IRMAClientRole]
    AS [dbo];

