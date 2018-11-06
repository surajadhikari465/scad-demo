CREATE PROCEDURE dbo.ShipperInsertItem
	@Shipper_Key int
	,@Item_Key int
	,@Qty int
	/* These two output params support the Shipper objects in the IRMA code.
		This saves an extra trip to the DB to pull these attributes. */
	,@Identifier varchar(13) output
	,@Desc varchar(60) output
AS 

/*
	This SP inserts a new row into the Shipper table, which represents adding an item to a Shipper.
	The output params provide info to the caller about the item.
*/

insert into
	Shipper (
		Shipper_Key
		,Item_Key
		,Quantity
	) 
values (
	@Shipper_Key
	,@Item_Key
	,@Qty
)

-- Select output values.
select
	@Identifier = ii.identifier
	,@Desc = i.item_description
from
	item i (nolock)
	join itemidentifier ii (nolock)
		on i.item_key = ii.item_key
where
	i.item_key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperInsertItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperInsertItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperInsertItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperInsertItem] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ShipperInsertItem] TO [IRMAPromoRole]
    AS [dbo];

