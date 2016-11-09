Create  FUNCTION dbo.fn_IsCaseItemUnit
	(@Unit_ID int)
RETURNS bit
AS

BEGIN 
	--RETURNS 1 (TRUE) IF THE ItemUnit.Unit_ID PASSED IN IS A 'CASE' ITEM;  RETURNS 0 (FALSE) IF NOT A CASE ITEM 
	DECLARE @return bit    
    
    SELECT @return = IsPackageUnit FROM ItemUnit WHERE Unit_ID = @Unit_ID
			        
	RETURN ISNULL(@return,0)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsCaseItemUnit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsCaseItemUnit] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsCaseItemUnit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsCaseItemUnit] TO [IRMAReportsRole]
    AS [dbo];

