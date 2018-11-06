SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE TYPE = 'P' AND name = 'UpdateOrderItemFreight3PartyAll')
	BEGIN
		DROP  PROCEDURE  UpdateOrderItemFreight3PartyAll
	END

GO

CREATE PROCEDURE dbo.UpdateOrderItemFreight3PartyAll
	@OrderHeader_ID		int,
    @Avg3PartyFreight	money,
    @UseQtyReceived		bit
	
AS
-- **************************************************************************
-- Procedure: UpdateOrderItemFreight3PartyAll
--    Author: Rick Kelleher
--      Date: 2007/12
--
-- Description:
-- Used for updating 3rd Party Freight values.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/22	KM		3744	Added update history template; coding standards;
-- **************************************************************************
BEGIN
    SET NOCOUNT ON

    DECLARE
		@OrderDiscountPct	decimal(9,2),
		@DiscountType		int

	SELECT
		@DiscountType = DiscountType
	FROM
		OrderHeader (nolock)
	WHERE 
		OrderHeader_ID = @OrderHeader_ID

	IF @DiscountType = 2
		SELECT
			@OrderDiscountPct = ISNULL(QuantityDiscount, 0) / 100
		FROM
			OrderHeader (nolock)
		WHERE 
			OrderHeader_ID = @OrderHeader_ID
	ELSE
		SET @OrderDiscountPct = 0		
		
	--handle divide by 0 for percentage calculations & 100% discounts
	IF @OrderDiscountPct = 1
		SET @OrderDiscountPct = 0
		
    IF @UseQtyReceived = 1
	    UPDATE 
			OrderItem 
	    SET
			-- if this line has QuantityReceived = 0, then don't include the calculated @Avg3PartyFreight
			Freight3Party			=	CASE QuantityReceived
											WHEN 0 THEN 0
											ELSE @Avg3PartyFreight
										END,
		    LineItemFreight3Party	=	CASE QuantityReceived 
											WHEN 0 THEN 0
											ELSE ROUND(@Avg3PartyFreight * QuantityReceived, 2)
										END
		WHERE 
			OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END