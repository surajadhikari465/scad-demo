CREATE FUNCTION [dbo].[fn_VendorKeyExists]
(
	@VendorKey varchar(10)
)
RETURNS bit
AS

BEGIN  

	DECLARE @result bit
	
	SELECT @result = (SELECT CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END
					  FROM Vendor WHERE Vendor_Key = @VendorKey)

	RETURN @result

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorKeyExists] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorKeyExists] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorKeyExists] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorKeyExists] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorKeyExists] TO [IMHARole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorKeyExists] TO [IRMASLIMRole]
    AS [dbo];

