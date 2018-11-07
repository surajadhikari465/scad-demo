CREATE PROCEDURE dbo.GetReturnOrderList
	@Instance       int,
	@User_ID        int,
	@OrderHeader_ID int

AS

-- **************************************************************************
-- Procedure: GetReturnOrderList
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/14	KM		3744	added update history template; changed extension to .sql; coding standards updates
-- **************************************************************************

BEGIN
	DELETE FROM 
		ReturnOrder

	WHERE  
		ReturnOrder.[Instance] = @Instance AND
		ReturnOrder.[User_ID]  = @User_ID

	INSERT INTO 
		ReturnOrder

	SELECT 
		@Instance,
		i.Item_Key, 
		OrderItem_ID				= MIN(oi.OrderItem_ID),
		@User_ID,
		Identifier, 
		i.Item_Description, 
		oi.Units_Per_Pallet, 
		0,
		QuantityReceived			= SUM(oi.QuantityReceived - ISNULL(ReturnQuantity, 0)), 
		iuq.Unit_Name,
		oi.QuantityUnit, 
		oi.Package_Desc1, 
		oi.Package_Desc2, 
		oi.Package_Unit_ID, 
		oi.LandedCost, 
		oi.MarkupPercent, 
		oi.MarkupCost, 
		oi.Cost, 
		oi.CostUnit, 
		oi.Freight, 
		oi.FreightUnit, 
		oi.QuantityDiscount, 
		oi.DiscountType, 
		Total_Weight				= 0,
		Total_Weight_Previous		= SUM(oi.Total_Weight - ISNULL(ReturnWeight, 0)), 
		oi.Retail_Unit_ID,
		NULL,
		NULL,
		Total_ReceivedItemCost		= SUM(oi.ReceivedItemCost + oi.ReceivedItemFreight - ISNULL(ReturnCost, 0)),
		oi.NetVendorItemDiscount,
		HandlingCharge				= ISNULL(oi.HandlingCharge, 0)
		
	FROM
		OrderHeader						(nolock) oh
		INNER JOIN	OrderItem			(nolock) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN	Item				(nolock) i		ON	oi.Item_Key				= i.Item_Key
		INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key				= ii.Item_Key 
														AND	ii.Default_Identifier	= 1
		INNER JOIN	ItemUnit			(nolock) iuq	ON	iuq.Unit_ID				= oi.QuantityUnit
		LEFT  JOIN
					(SELECT
						Item_Key,
						ReturnQuantity	= SUM(QuantityReceived),
						ReturnWeight	= SUM(Total_Weight),
						ReturnCost		= SUM(ReceivedItemCost + ReceivedItemFreight)
						
					FROM 
						OrderItem					(nolock) oi
						INNER JOIN	ReturnOrderList (nolock) rol	ON rol.ReturnOrderHeader_ID = oi.OrderHeader_ID
						
					WHERE
						rol.OrderHeader_ID = @OrderHeader_ID
						
					GROUP BY
						Item_Key) RI	ON	oi.Item_Key = RI.Item_Key
						
										WHERE 
											oh.OrderHeader_ID = @OrderHeader_ID
										
										GROUP BY
											i.Item_Key, 
											Identifier, 
											i.Item_Description, 
											oi.Units_Per_Pallet, 
											oi.Package_Desc1, 
											oi.Package_Desc2, 
											oi.Package_Unit_ID, 
											oi.QuantityUnit, 
											oi.LandedCost, 
											oi.Cost, 
											oi.CostUnit, 
											oi.QuantityDiscount, 
											oi.DiscountType, 
											oi.Freight, 
											oi.FreightUnit, 
											oi.MarkupPercent, 
											oi.MarkupCost, 
											oi.Retail_Unit_ID, 
											iuq.Unit_Name,
											oi.NetVendorItemDiscount,
											oi.HandlingCharge
												 
										HAVING SUM(oi.QuantityReceived - ISNULL(ReturnQuantity, 0)) > 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReturnOrderList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReturnOrderList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReturnOrderList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReturnOrderList] TO [IRMAReportsRole]
    AS [dbo];

