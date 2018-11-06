CREATE PROCEDURE dbo.ShipperItemUpdateInfo
	@Shipper_Key int
	,@Item_Key int
	,@Qty int
AS

/*
	This SP sets the unit qty for an item in a Shipper.
*/

UPDATE
	Shipper
SET
	Quantity = @Qty
WHERE
	Shipper_Key = @Shipper_Key
	and Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperItemUpdateInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperItemUpdateInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperItemUpdateInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperItemUpdateInfo] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperItemUpdateInfo] TO [IRMAPromoRole]
    AS [dbo];

