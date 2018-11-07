CREATE FUNCTION dbo.fn_GetUnsentOrders(
    @store_no int
	,@vendor_id int
)
RETURNS TABLE     
AS    
RETURN (
    select oh.orderheader_id
	from 
		orderheader oh (nolock)
	join
		vendor rl (nolock)
		on rl.vendor_id = oh.receiveLocation_id
	join
		store s (nolock)
		on s.store_no = rl.store_no
	where
		s.store_no = @store_no
		and oh.vendor_id = @vendor_id
		and sent = 0
)
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetUnsentOrders] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetUnsentOrders] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetUnsentOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetUnsentOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetUnsentOrders] TO [IRMAReportsRole]
    AS [dbo];

