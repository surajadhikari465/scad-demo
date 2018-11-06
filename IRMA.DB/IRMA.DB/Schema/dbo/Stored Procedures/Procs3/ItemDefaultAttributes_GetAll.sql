CREATE PROCEDURE [dbo].[ItemDefaultAttributes_GetAll]
	--(
	--@ProdHierarchyLevel4_ID As Int
	--,@Category_ID As Int
	--)
AS

	SELECT ida.ItemDefaultAttribute_ID,
		ida.AttributeName,
		ida.AttributeField,
		ida.Active,
		ida.ControlOrder,
		CASE ida.ControlType
			WHEN 1 THEN 'Text Field'
			WHEN 2 THEN 'Dropdown List'
			WHEN 3 THEN 'Checkbox'
		END as 'ControlTypeName',
		ida.ControlType
	FROM ItemDefaultAttribute (NOLOCK) ida 
       ORDER BY ida.ControlOrder
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDefaultAttributes_GetAll] TO [IRMAClientRole]
    AS [dbo];

