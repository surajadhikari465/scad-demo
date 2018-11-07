create function [dbo].[fn_GetLastCost]
	(@ItemKey int, @StoreNumber int)
RETURNS SMALLMONEY 

AS

-- ****************************************************************************************************************
-- Procedure: fn_GetLastCost()
--    Author: ???
--      Date: ???
--
-- Description:
-- This function returns the last received cost (sometimes referred to as "last paid cost") for an item/store combination.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013/08/26	KM				This function wasn't being used for anything, so I adopted it and made it my own.
-- ****************************************************************************************************************

BEGIN

	DECLARE 
		@LastReceivedCost	smallmoney,
		@MostRecentOrder	int,
		@StoreVendorId		int

	SET @StoreVendorId		= (SELECT v.Vendor_ID FROM Vendor v WHERE v.Store_no = @StoreNumber)
	SET @MostRecentOrder	= dbo.fn_GetLastItemPOID(@ItemKey, @StoreVendorId)
	
	-- If there is no order to pull last received cost from, there isn't any need to continue with the rest of the function.
	IF @MostRecentOrder IS NULL
		RETURN NULL

	-- Set up some variables that we'll need when we call fn_CostConversion() later.
	DECLARE 
		@CostConversionAmount	smallmoney, 
		@PackageDesc1			int, 
		@PackageDesc2			int, 
		@CostUnitId				int, 
		@PackageUnitId			int, 
		@CostedByWeight			bit
	
	SELECT @CostedByWeight = ISNULL(iov.CostedByWeight, i.CostedByWeight)	FROM 
		Item					(nolock) i 
		LEFT JOIN ItemOverride	(nolock) iov ON i.Item_Key = iov.Item_Key
	WHERE 
		i.Item_Key = @ItemKey
	

	-- Get the cost to be converted (generally, the case cost).
	SELECT 
		@CostConversionAmount = 
								CASE
									--•	For an eInvoiced order, display the line item invoice cost. 
									WHEN oh.eInvoice_ID IS NOT NULL THEN 
										oi.InvoiceExtendedCost / CASE WHEN @CostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END
									--•	For a paper invoiced order, it will be the line item received cost.
									WHEN oh.eInvoice_ID IS NULL THEN 
										oi.ReceivedItemCost / CASE WHEN @CostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END
									ELSE
										0
								END,

		@PackageDesc1		=	Package_Desc1,
		@PackageDesc2		=	Package_Desc2,
		@CostUnitId			=	CostUnit,
		@PackageUnitId		=	Package_Unit_ID

	FROM 
		OrderItem			(nolock) oi
		JOIN OrderHeader	(nolock) oh	ON oi.OrderHeader_ID = oh.OrderHeader_ID
							
	WHERE 
		oh.OrderHeader_ID	= @MostRecentOrder
		AND oi.Item_Key		= @ItemKey
	ORDER BY 
		oi.OrderHeader_ID desc


	-- Call fn_CostConversion() to give us the unit cost.
	SELECT @LastReceivedCost = (SELECT dbo.fn_CostConversion(@CostConversionAmount, @CostUnitId, CASE WHEN @CostedByWeight = 1 THEN (SELECT Unit_ID FROM ItemUnit (nolock) WHERE lower(UnitSysCode) = 'lbs') ELSE (SELECT Unit_ID FROM ItemUnit (nolock) WHERE lower(UnitSysCode) = 'unit') END, @PackageDesc1, @PackageDesc2, @PackageUnitId)) 


RETURN @LastReceivedCost
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLastCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLastCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLastCost] TO [IRMAReportsRole]
    AS [dbo];

