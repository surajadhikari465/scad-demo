CREATE PROCEDURE dbo.[UpdatePromoPreOrder]
@strQty int,
@strID  int,
@ppoID int
AS
update 
PromoPreOrders 
set 
OrderQty = @strQty 
where 
Item_Key = @strID and 
PromoPreOrderID = @ppoID;
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePromoPreOrder] TO [IRMAPromoRole]
    AS [dbo];

