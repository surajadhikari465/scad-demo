



-- THIS FUNCTION LOOKS ONLY AT THE IDENTIFIER PASSED IN TO DETERMINE IF IT MEETS SCALE CRITERIA
-- IT DOES NOT LOOK AT OTHER FIELDS THAT DETERMINE WHAT MAKES A SCALE ITEM (IE: ItemIdentifier = 'O')

CREATE FUNCTION [dbo].[fn_IsInternalVendor]
	(@Vendor_ID int
)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit	
    DECLARE @n int

    SELECT @n = count(*) 
	FROM Vendor (NOLOCK)
	INNER JOIN Store (NOLOCK) 
		ON Store.Store_No = Vendor.Store_No
	WHERE Vendor_ID = @Vendor_ID AND 
		((Distribution_Center = 1) OR (Manufacturer = 1) OR (Mega_Store = 1) OR (WFM_Store = 1))
		AND dbo.fn_VendorType(Vendor.PS_Vendor_ID, Vendor.WFM, Vendor.Store_No, Store.Internal) = 3 -- regional

	IF @n >= 1    
		SELECT @return = 1				
    ELSE  
        select @return = 0
        
	RETURN @return
END




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsInternalVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsInternalVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsInternalVendor] TO [IRMASchedJobsRole]
    AS [dbo];

