CREATE PROCEDURE dbo.GetAllocationItemPackSizes
	@ItemKey int
AS 
		SELECT DISTINCT PackSize 
		FROM tmpOrdersAllocateItems 
		WHERE Item_Key = @ItemKey AND PackSize IS NOT NULL 
		ORDER BY PackSize
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllocationItemPackSizes] TO [IRMAClientRole]
    AS [dbo];

