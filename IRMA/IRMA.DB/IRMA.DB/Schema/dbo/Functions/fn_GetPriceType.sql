CREATE FUNCTION [dbo].[fn_GetPriceType]
    (@PriceChgTypeID int)
RETURNS varchar (5)
AS
BEGIN
	DECLARE @result varchar (5)

	SELECT @result = (SELECT DISTINCT PriceChgType.PriceChgTypeDesc
					  FROM Price
					  INNER JOIN PriceChgType ON PriceChgType.PriceChgTypeID = Price.PriceChgTypeID
					  WHERE PriceChgType.PriceChgTypeID =@PriceChgTypeID)
    RETURN @result
END