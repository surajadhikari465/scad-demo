create procedure [dbo].[GetPackSizesByOrderItem]
	@OrderItem_Id as int
as
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	08/15/2008	?				?		Date last scripted.
	01/19/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
										Removed dynamic SQL being used to define/create the function.
										Moved security grants at bottom of this script to dedicated security script.
*/
begin
	declare @CostDate datetime

	-- We make a pass through OH and OI to get vendor ID and base date for lead-time FN.
	select
		@CostDate = isnull(oh.sentdate, getdate()) + dbo.fn_GetLeadTimeDays(oh.vendor_id)
	from
		orderheader oh 
		join orderitem oi
			on oi.orderheader_id = oh.orderheader_id 
	where
		oi.orderitem_id = @OrderItem_Id


	select	vch.vendorcosthistoryid, 
			vch.unitcost, 
			vch.package_desc1 as packsize,
			startdate, 
			enddate
	from orderheader oh 
		inner join orderitem oi
		on oi.orderheader_id = oh.orderheader_id 
		inner join vendor v
			on oh.receivelocation_id = v.vendor_id
		inner join storeitemvendor siv
			on oi.item_key = siv.item_key and 
				oh.vendor_id = siv.vendor_id and
				v.store_no = siv.store_no			
		inner join vendorcosthistory vch
			on siv.storeitemvendorid = vch.storeitemvendorid
	where oi.orderitem_id = @OrderItem_Id
	and   vch.startdate <= @CostDate and @CostDate < vch.enddate
	order by startdate desc 
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPackSizesByOrderItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPackSizesByOrderItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPackSizesByOrderItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPackSizesByOrderItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPackSizesByOrderItem] TO [IRMAReportsRole]
    AS [dbo];

