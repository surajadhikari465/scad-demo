CREATE PROCEDURE dbo.GetDistributionCreditOrderList
	@OrderHeader_ID int
AS

-- **************************************************************************
-- Procedure: GetDisstributionCreditOrderList
--    Author: n/a
--      Date: n/a
--
-- Description:	n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/14	KM		3744	added update history template; coding standards
-- **************************************************************************
BEGIN

	SELECT *,
		PackageSize =	CASE
							WHEN RetailWeight_Unit = 1 THEN Total_Weight_Previous / Quantity_Previous
								ELSE Package_Desc1
						END
	FROM
		(SELECT 
				i.Item_Key, 
				OrderItem_ID			= MIN(oi.OrderItem_ID),
				Identifier, 
				i.Item_Description, 
				oi.Units_Per_Pallet, 
				Quantity = 0,
				Quantity_Previous		= SUM(oi.QuantityReceived - ISNULL(ReturnQuantity, 0)), 
				QuantityUnit_Text		= iu.Unit_Name,
				oi.QuantityUnit, 
				oi.Package_Desc1, 
				oi.Package_Desc2, 
				oi.Package_Unit_ID, 
				oi.LandedCost, 
				oi.MarkupPercent, 
				oi.MarkupCost, 
				oi.Cost, 
				oi.UnitCost,
				oi.UnitExtCost,
				oi.CostUnit, 
				oi.Handling, 
				oi.HandlingUnit, 
				oi.LineItemCost, 
				oi.LineItemFreight, 
				oi.Freight, 
				oi.FreightUnit, 
				oi.QuantityDiscount, 
				oi.DiscountType, 
				Total_Weight = 0,
				Total_Weight_Previous	= SUM(oi.Total_Weight - ISNULL(ReturnWeight, 0)), 
				oi.Retail_Unit_ID,
				CreditReason			= NULL,
				CreditReason_ID			= NULL,
				Total_ReceivedItemCost	= SUM(oi.ReceivedItemCost + oi.ReceivedItemFreight - ISNULL(ReturnCost, 0)),
				NetVendorItemDiscount	= ISNULL(oi.NetVendorItemDiscount, 0),
				HandlingCharge			= ISNULL(oi.HandlingCharge, 0),
				RetailUnit_Text			= ru.Unit_Name,
				ReturnUnit_Id			= oi.QuantityUnit,
				RetailWeight_Unit		= ru.Weight_Unit
		
		FROM OrderHeader					(nolock) oh
		INNER JOIN OrderItem				(nolock) oi				ON	oh.OrderHeader_ID			= oi.OrderHeader_ID
		INNER JOIN Item						(nolock) i				ON	oi.Item_Key					= i.Item_Key
		INNER JOIN ItemIdentifier			(nolock) ii				ON	i.Item_Key					= ii.Item_Key 
																	AND ii.Default_Identifier		= 1
		INNER JOIN ItemUnit					(nolock) iu				ON	oi.QuantityUnit				= iu.Unit_ID
		INNER JOIN ItemUnit					(nolock) ru				ON	oi.Retail_Unit_ID			= ru.Unit_ID
		LEFT  JOIN
			(SELECT 
				Item_Key,
				ReturnQuantity			= SUM(QuantityReceived),
				ReturnWeight			= SUM(Total_Weight),
				ReturnCost				= SUM(ReceivedItemCost + ReceivedItemFreight)
				
			 FROM 
				OrderItem					(nolock) oi
				INNER JOIN ReturnOrderList	(nolock) rol			ON	oi.OrderHeader_ID = rol.ReturnOrderHeader_ID
				
			 WHERE
				rol.OrderHeader_ID = @OrderHeader_ID
			 
			 GROUP BY
				Item_Key) ri ON oi.Item_Key = ri.Item_Key
															
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
			oi.UnitCost,
			oi.UnitExtCost,
			oi.CostUnit, 
			oi.Handling, 
			oi.HandlingUnit, 
			oi.QuantityDiscount, 
			oi.DiscountType, 
			oi.LineItemCost, 
			oi.LineItemFreight, 
			oi.Freight, 
			oi.FreightUnit, 
			oi.MarkupPercent, 
			oi.MarkupCost, 
			oi.Retail_Unit_ID, 
			iu.Unit_Name,
			oi.NetVendorItemDiscount,
			oi.HandlingCharge,
			ru.Unit_Name,
			oi.QuantityUnit,
			ru.Weight_Unit
			
		HAVING
			SUM(oi.QuantityReceived - ISNULL(ReturnQuantity, 0)) > 0) AS Orders
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionCreditOrderList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionCreditOrderList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionCreditOrderList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionCreditOrderList] TO [IRMAReportsRole]
    AS [dbo];

