
create procedure dbo.EXEInterfaces_FindZeroShippedOrders
(
@uniqueid varchar(255)
) as
begin
	declare @ZeroShipped table
	(
	  OrderHeaderId int not null,
	  QtyOnOrder int null,
	  QtyZeroShipped int null
	)
	
	insert into @ZeroShipped (OrderHeaderId)
	select distinct orderheader_id from EXEInterfaces_ZeroShippedOrdersValidationWorkspace where uniqueid = @uniqueid
	
	update @ZeroShipped
	set QtyOnOrder = ( select sum(quantityordered * package_desc1) from OrderItem where OrderItem.OrderHeader_ID = oh.OrderHeader_ID ),
	QtyZeroSHipped = ( select sum(value) from EXEInterfaces_ZeroShippedOrdersValidationWorkspace exe where exe.OrderHeader_Id = oh.OrderHeader_Id  and exe.uniqueid = @uniqueid)
        from OrderHeader oh inner join @ZeroShipped zs
	on oh.OrderHeader_ID = zs.OrderheaderId
	
	
	select distinct OrderHeaderId  from @ZeroShipped where QtyOnOrder = QtyZeroShipped and not QtyOnOrder is null
	
	
	
	end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_FindZeroShippedOrders] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_FindZeroShippedOrders] TO [IRMAClientRole]
    AS [dbo];

