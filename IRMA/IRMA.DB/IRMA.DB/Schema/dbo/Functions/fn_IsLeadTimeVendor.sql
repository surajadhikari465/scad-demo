CREATE FUNCTION dbo.fn_IsLeadTimeVendor
(
	@Vendor_ID INT
)
RETURNS bit
AS
/*
	[fn_IsLeadTimeVendor]
	Returns bit to show whether or not the vendor is a lead-time vendor (uses lead-time dates when pulling cost for items).

	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/18/2011	Tom Lux			759		Added function.

	--------------------------------------------
*/
BEGIN

	declare @usesLeadTime bit
	
	select @usesLeadTime =
		case
			when
				v.LeadTimeDays > 0 
				or
				isnull(v.LeadTimeDayOfWeek, 0) > 0
			then 1
			else 0
		end
	from
		Vendor v
	where
		v.Vendor_ID = @Vendor_ID

	return @usesLeadTime
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsLeadTimeVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsLeadTimeVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsLeadTimeVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsLeadTimeVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsLeadTimeVendor] TO [IRMAReportsRole]
    AS [dbo];

