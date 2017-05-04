CREATE PROCEDURE [dbo].[GetRegularPriceChgTypeData]
AS

BEGIN
	-- Return the PriceChgType values for the REGULAR price change type.
    SELECT PriceChgTypeId, PriceChgTypeDesc, Priority, On_Sale, MSRP_Required, LineDrive, Competitive, LastUpdateTimestamp
    FROM PriceChgType
    WHERE On_Sale = 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegularPriceChgTypeData] TO [IRMAClientRole]
    AS [dbo];

