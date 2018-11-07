CREATE FUNCTION [dbo].[fn_GetWeightOrQtySold]
(
	@Weight decimal(9,2),
	@QuantitySold decimal (9,2),
	@QuantityReturn decimal (9,2)
)
	 RETURNS Decimal(9,2)
AS
BEGIN
	DECLARE @Return Decimal(9,2)
  
	SELECT @Return = 
		CASE
			WHEN @Weight is null then @QuantitySold - @QuantityReturn
			WHEN @Weight = 0 then @QuantitySold - @QuantityReturn
			ELSE @Weight
		END
	RETURN @Return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetWeightOrQtySold] TO [IRMAReportsRole]
    AS [dbo];

