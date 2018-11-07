CREATE  FUNCTION dbo.fn_VendorExists
	(@Vendor_ID int)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
    
    IF EXISTS (SELECT * FROM dbo.Vendor WHERE Vendor_ID = @Vendor_ID)
		SELECT @return = 1
	ELSE
		SELECT @return = 0
        
	RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorExists] TO [IRMAClientRole]
    AS [dbo];

