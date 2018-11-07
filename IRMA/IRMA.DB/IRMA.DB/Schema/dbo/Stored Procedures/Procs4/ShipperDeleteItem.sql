CREATE PROCEDURE dbo.ShipperDeleteItem
	@Shipper_Key int, 
	@Item_Key int 
AS 

/*
	This SP removes a row from the Shipper table, which represents removing an item from a Shipper.
*/

delete
from
	Shipper
where
	Shipper_Key = @Shipper_Key
	and Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperDeleteItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperDeleteItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperDeleteItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperDeleteItem] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperDeleteItem] TO [IRMAPromoRole]
    AS [dbo];

