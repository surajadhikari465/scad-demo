CREATE  FUNCTION [dbo].[fn_GetRetailUnitAbbreviation]
	(@Item_Key int)
RETURNS varchar(5)
AS

BEGIN 
-- returns empty string if unit id not found
	DECLARE @return varchar(5)    
    
    SELECT @return = Unit_Abbreviation FROM ItemUnit WHERE Unit_ID = (SELECT Retail_Unit_ID FROM Item WHERE Item_Key = @Item_Key)
			        
	RETURN ISNULL(@return,'')
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRetailUnitAbbreviation] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRetailUnitAbbreviation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRetailUnitAbbreviation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRetailUnitAbbreviation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetRetailUnitAbbreviation] TO [IRMAReportsRole]
    AS [dbo];

