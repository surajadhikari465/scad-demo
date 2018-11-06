CREATE PROCEDURE dbo.ShipperGetAllContainingItem
	@Item_Key int 
AS 

select
	Shipper_Key
	,Item_Key
	,Quantity
from
	Shipper (nolock)
where
	Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperGetAllContainingItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperGetAllContainingItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperGetAllContainingItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperGetAllContainingItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperGetAllContainingItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperGetAllContainingItem] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperGetAllContainingItem] TO [IRMAPromoRole]
    AS [dbo];

