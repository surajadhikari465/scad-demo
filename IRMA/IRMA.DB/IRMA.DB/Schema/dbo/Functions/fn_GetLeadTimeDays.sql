CREATE FUNCTION dbo.fn_GetLeadTimeDays
(
	@Vendor_ID INT
)
RETURNS int
AS
/*
	[fn_GetLeadTimeDays]
	Returns an integer representing the number of days that must be added to an order's Sent Date to 
	determine the date to use for the cost of items in the order.
	This only applies for vendors configured in IRMA to use lead-times.

	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/18/2011	Tom Lux			759		Added function.

	--------------------------------------------
*/
BEGIN

	-- Make sure the vendor ID coming in is valid.
	if @Vendor_ID is null
	begin
		return 0
	end

	declare
		@vendorUsesLeadTime bit
		,@vendorLeadTimeDays int
		,@vendorLeadTimeDayOfWeek int
	
	-- One trip to Vendor table to pull lead-time attributes.
	select
		@vendorUsesLeadTime =
			case
				when v.LeadTimeDays > 0 or isnull(v.LeadTimeDayOfWeek, 0) > 0
				then 1
				else 0
			end
		,@vendorLeadTimeDays = v.LeadTimeDays
		,@vendorLeadTimeDayOfWeek = isnull(v.LeadTimeDayOfWeek, 0)
	from
		Vendor v
	where
		v.Vendor_ID = @Vendor_ID
	
	-- Make sure the vendor is configured to use lead-times.
	if isnull(@vendorUsesLeadTime, 0) = 0
	begin
		return 0
	end
	
	-- If we are here, the vendor is setup as a lead-time vendor, so we'll determine the number of lead-time days.
	
	-- If a 'lead-time days' is defined, we return that value.  Day-of-week should not be defined when number-of-days is > 0.
	if @vendorLeadTimeDays > 0
	begin
		return @vendorLeadTimeDays
	end
	
	/*
		If we are here, the vendor is using a specific day of the week to mark their lead-time date.
		The day-of-week index is 1-7 for Sunday-Saturday respectively.
		To get a number of lead-time days, we find the next occurrence of the specified weekday.
		If today is the lead-time day, we add seven days, effectively skipping today and taking the next occurrence.
	*/
	declare @todayDOWName varchar(16) = (select day_name from [Date] where date_key = convert(date, getdate(), 101))
	
	declare @todayDOW int = (select day_of_week from [Date] where date_key = convert(date, getdate(), 101))
	if @todayDOW = @vendorLeadTimeDayOfWeek
	begin
		return 7
	end
	
	return ( select
		case when @vendorLeadTimeDayOfWeek < @todayDOW
		then @vendorLeadTimeDayOfWeek + 7 - @todayDOW
		else @vendorLeadTimeDayOfWeek - @todayDOW
		end
	)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLeadTimeDays] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLeadTimeDays] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLeadTimeDays] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLeadTimeDays] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetLeadTimeDays] TO [IRMAReportsRole]
    AS [dbo];

