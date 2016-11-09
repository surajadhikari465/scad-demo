CREATE FUNCTION [dbo].[fn_Price] 
	(@PriceChgTypeID int, 
     @Multiple tinyint,
     @Price money, 
     @PricingMethod_ID int, 
     @Sale_Multiple tinyint,  
     @Sale_Price money
    )
RETURNS money
AS
	-- ******************************************************************************************************************************
	-- Function: fn_Price
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 06.29.2011	DBS		TFS 1396 Add ISNULL handling for the Pricing Method number
	-- 07.15.2011	DBS		TFS 1396 Back change out due to lack of testing
	-- ******************************************************************************************************************************
BEGIN

RETURN 
	(SELECT 
		dbo.fn_PricingMethodMoney
			(@PriceChgTypeID, 
			 @PricingMethod_ID, 
			 @Price,
			 @Sale_Price)
	/
	 		dbo.fn_PricingMethodInt
			(@PriceChgTypeID, 
			 @PricingMethod_ID, 
			 @Multiple,
			 @Sale_Multiple)
	)
	 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Price] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Price] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Price] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Price] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Price] TO [IRMAReportsRole]
    AS [dbo];

