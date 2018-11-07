
CREATE FUNCTION [dbo].[fn_HasIngredientIdentifier]
(
	@Item_Key int
)
RETURNS bit
AS

BEGIN
	declare @retVal bit

	if exists
		 (
		   SELECT top 1 Identifier
			 FROM ItemIdentifier (nolock)
			WHERE
				item_key = @Item_key
			  AND
				Deleted_Identifier = 0
			  AND
				Remove_Identifier = 0
			  AND 
				((CONVERT(FLOAT, Identifier) >= 46000000000 And CONVERT(FLOAT, Identifier)  <= 46999999999) Or (CONVERT(FLOAT, Identifier) >= 48000000000 And CONVERT(FLOAT, Identifier) <= 48999999999))
		  )
		select @retVal = 1
	else
		select @retVal = 0
	
	return @retVal
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasIngredientIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasIngredientIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasIngredientIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasIngredientIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasIngredientIdentifier] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasIngredientIdentifier] TO [IRSUser]
    AS [dbo];

