
CREATE FUNCTION [dbo].[fn_IsMultipleMatchedVendors]
(
	@PartialVendorName varchar(50)
)
RETURNS bit
AS
/*
	[fn_IsLeadTimeVendor]
	Returns true if there are multiple matched vendors with the partial vendorname 

	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	02/05/2013	Faisal Ahmed	10223	Added function.
	10/23/2013	Benjamin Loving	14320	Check first if the partialvendorname is an exact match
										to a vendor. 
	--------------------------------------------
*/
BEGIN

	DECLARE @flag	BIT
	DECLARE @count	INT

	SELECT @flag = 0		

	SELECT	@count = COUNT(*)
	FROM	Vendor (NOLOCK)
	WHERE	CompanyName = @PartialVendorName

	IF @count = 0
		SELECT	@count = COUNT(*)
		FROM	Vendor (NOLOCK)
		WHERE	CompanyName LIKE '%' + @PartialVendorName + '%'

	IF @count > 1 
		SELECT @flag =  1

	RETURN @flag
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsMultipleMatchedVendors] TO [IRMAClientRole]
    AS [dbo];

