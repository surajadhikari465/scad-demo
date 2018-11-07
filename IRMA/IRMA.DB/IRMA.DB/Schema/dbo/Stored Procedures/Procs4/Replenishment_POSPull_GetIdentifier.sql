CREATE PROCEDURE dbo.[Replenishment_POSPull_GetIdentifier] 
AS 

BEGIN
	SELECT 
		Item_Key, 
		Identifier,
		SUBSTRING ('000000000000', 1, (12 - LEN(Identifier)))+ Identifier AS PaddedIdentifier 
	FROM 
		ItemIdentifier
	WHERE 
		Deleted_Identifier = 0 
		AND Add_Identifier = 0 
		AND LEN(Identifier)< 13
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_GetIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_GetIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_GetIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_GetIdentifier] TO [IRMARSTRole]
    AS [dbo];

