IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Replenishment_POSPull_GetIdentifier]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Replenishment_POSPull_GetIdentifier]
GO

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
