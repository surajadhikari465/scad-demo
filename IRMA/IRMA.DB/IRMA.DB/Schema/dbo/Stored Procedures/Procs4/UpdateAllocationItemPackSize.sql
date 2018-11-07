CREATE PROCEDURE dbo.UpdateAllocationItemPackSize
AS 
	UPDATE tmpOrdersAllocateOrderItems SET Package_Desc1 = OOH.PackSize
	FROM 
		tmpOrdersAllocateOrderItems tmp
		INNER JOIN dbo.fn_OrderAllocItemsOnePackSizeOH() AS OOH ON OOH.Item_Key = tmp.Item_Key
	WHERE 
		(QuantityAllocated IS NULL) AND PackSize <> Package_Desc1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateAllocationItemPackSize] TO [IRMAClientRole]
    AS [dbo];

