CREATE PROCEDURE dbo.ItemAttributes_GetAttributeIdentifiersByItemKey
	@Item_Key int
	
AS

	-- Created By:	James Winfield
	-- Created   :	Feb 23, 2007

	SELECT
		[AttributeIdentifier_ID],
		[Screen_Text],
		[field_type],
		[combo_box],
		[max_width],
		[default_value],
		[field_values]
	FROM AttributeIdentifier (NOLOCK) 
	WHERE @Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_GetAttributeIdentifiersByItemKey] TO [IRMAClientRole]
    AS [dbo];

