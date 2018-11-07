CREATE PROCEDURE dbo.ShipperCheck
	@Item_Key int
	,@IsShipper bit output
AS 

/*
	This SP simply returns a (Boolean) Shipper flag for an item via output parameter.
*/

select
	@IsShipper = i.Shipper_Item 
from 
	Item i (nolock)
where
	i.Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperCheck] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperCheck] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperCheck] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperCheck] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperCheck] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperCheck] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperCheck] TO [IRMAPromoRole]
    AS [dbo];

