Create PROCEDURE [dbo].[GetAllPriceTypes] 

AS 
BEGIN
    
    SELECT 
		CAST(PriceChgTypeId AS int) AS PriceChgTypeId,
		PriceChgTypeDesc
    FROM 
		PriceChgType (NOLOCK)
    ORDER BY 
		Priority asc

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllPriceTypes] TO [IRMAClientRole]
    AS [dbo];

