-- This function checks to see if a Primary Vendor relationship exists for a Store-Item.
Create  FUNCTION dbo.fn_HasPrimaryVendor (
	@Item_Key int,
	@Store_No int
)
RETURNS bit
AS

BEGIN  
	RETURN
	   CASE WHEN (SELECT COUNT(1) FROM StoreItemVendor WHERE 
				Item_Key = @Item_Key AND 
				Store_No = @Store_No AND 
				PrimaryVendor = 1 AND 
				DeleteDate IS NULL) > 0
		THEN 1
		ELSE 0 
		END  
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasPrimaryVendor] TO [IRMAClientRole]
    AS [dbo];

