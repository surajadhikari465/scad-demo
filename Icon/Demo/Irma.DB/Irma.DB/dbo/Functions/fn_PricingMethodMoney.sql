CREATE FUNCTION dbo.fn_PricingMethodMoney
	(@PriceChgTypeID int, 
     @PricingMethod_ID int, 
	 @Reg money,
	 @Sale money
    )
RETURNS money
AS
BEGIN
RETURN CASE WHEN 1 = (SELECT On_Sale
					FROM PriceChgType
					WHERE PriceChgTypeID = @PriceChgTypeID)
			THEN 
				CASE WHEN 1 = (SELECT UseSalePrice 
								FROM PricingMethod
								WHERE PricingMethod_Id = @PricingMethod_Id)
					THEN ISNULL(@Sale,0)
					ELSE 0 END
				+
				CASE WHEN 1 = (SELECT UseRegPrice 
								FROM PricingMethod
								WHERE PricingMethod_Id = @PricingMethod_Id)
					THEN ISNULL(@Reg,0)
					ELSE 0 END
			ELSE ISNULL(@Reg,0) END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PricingMethodMoney] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PricingMethodMoney] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PricingMethodMoney] TO [IRMAReportsRole]
    AS [dbo];

