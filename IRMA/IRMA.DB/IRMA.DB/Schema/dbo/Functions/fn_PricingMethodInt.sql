CREATE FUNCTION dbo.fn_PricingMethodInt
	(@PriceChgTypeID int, 
     @PricingMethod_ID int, 
	 @Reg tinyint,
	 @Sale tinyint
    )
RETURNS tinyint
AS
BEGIN

IF @Reg < 1 
	SELECT @Reg = 1
IF @Sale < 1 
	SELECT @Sale = 1

RETURN 
	CASE WHEN 1 = (SELECT On_Sale
					FROM PriceChgType
					WHERE PriceChgTypeID = @PriceChgTypeID)
			THEN 
				CASE WHEN 1 = ISNULL((SELECT UseSalePrice 
								FROM PricingMethod
								WHERE PricingMethod_Id = @PricingMethod_Id),0)
					THEN ISNULL(@Sale,1)
					ELSE 0 END
				+
				CASE WHEN 1 = ISNULL((SELECT UseRegPrice 
								FROM PricingMethod
								WHERE PricingMethod_Id = @PricingMethod_Id),1)
					THEN ISNULL(@Reg,1)
					ELSE 0 END
			ELSE ISNULL(@Reg,1) END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PricingMethodInt] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PricingMethodInt] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PricingMethodInt] TO [IRMAReportsRole]
    AS [dbo];

