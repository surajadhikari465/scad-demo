CREATE PROCEDURE dbo.ShipperUpdatePackInfo
	@Item_Key int
AS 

update Item 
set
	Package_Desc1 = (
		select
			case
				when sum(Quantity) is null then 0
				else sum(Quantity) end
		from Shipper 
		where Shipper_Key = @Item_Key
	)
where
	Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperUpdatePackInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperUpdatePackInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperUpdatePackInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperUpdatePackInfo] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperUpdatePackInfo] TO [IRMAPromoRole]
    AS [dbo];

