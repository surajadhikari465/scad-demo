CREATE FUNCTION dbo.fn_ReceivedInvoiceCost 
	(
	 @InvoiceExtendedCost	money,
	 @InvoiceTotalWeight	decimal(18,4),
	 @eInvoiceQuantity		decimal(18,4),
	 @CostedByWeight		bit,
	 @QuantityUnit			int,
	 @Package_Desc1			decimal(9,4),
	 @Package_Desc2			decimal(9,4),
	 @Package_Unit_ID		int,
	 @UnitsReceived			decimal(18,4),
	 @unit					int, 
	 @pound					int
	)
RETURNS decimal(38,28)
AS

-- **************************************************************************
-- Procedure: fn_ReceivedInvoiceCost
--    Author: n/a
--      Date: n/a
--
-- Description:
-- Returns ReceivedInvoiceCost as InvoiceExtendedCost / [UnitsShipped] * [UnitsReceived]
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/15	KM		3744	Added update history template; minor coding standards
-- **************************************************************************

BEGIN
	-- ReceivedInvoiceCost = ([Invoice Line Item Total] (which includes discounts, if any, applied) / [units shipped]) * [units received]
	RETURN (ISNULL(@InvoiceExtendedCost, 0) / CASE WHEN ISNULL(NULLIF(@InvoiceTotalWeight, 0), dbo.fn_CostConversion(ISNULL(@eInvoiceQuantity, 0), CASE WHEN @CostedByWeight = 1 THEN @pound ELSE @unit END, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID)) > 0 THEN ISNULL(NULLIF(@InvoiceTotalWeight, 0), dbo.fn_CostConversion(ISNULL(@eInvoiceQuantity, 0), CASE WHEN @CostedByWeight = 1 THEN @pound ELSE @unit END, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID)) ELSE 1 END) * @UnitsReceived
END