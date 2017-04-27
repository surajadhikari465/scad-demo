/****** Object:  UserDefinedFunction [dbo].[fn_GetPriceType]    Script Date: 04/28/2008 15:38:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER FUNCTION [dbo].[fn_GetPriceType]
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
GO
