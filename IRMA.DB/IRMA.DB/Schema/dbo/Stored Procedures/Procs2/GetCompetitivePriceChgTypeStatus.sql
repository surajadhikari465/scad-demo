CREATE PROCEDURE [dbo].[GetCompetitivePriceChgTypeStatus] 
	@PriceChgTypeID INT, 
	@CompStatus bit OUTPUT
AS
BEGIN

	
SELECT 
	@CompStatus = Competitive
	FROM
	PriceChgType (NOLOCK)
	WHERE PriceChgTypeID = @PriceChgTypeID
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitivePriceChgTypeStatus] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitivePriceChgTypeStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitivePriceChgTypeStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitivePriceChgTypeStatus] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitivePriceChgTypeStatus] TO [IRMAPromoRole]
    AS [dbo];

