
CREATE procedure dbo.Scale_GetIngredientsByItem
	@Item_Key int
as
begin
	select top 1 
		si.Scale_Ingredient_ID,
		si.Description,
		si.Ingredients,
		'' as LabelTypeDescription,
		sxt.Scale_LabelType_ID as Scale_LabelType_ID
	from Scale_Ingredient si
	left join ItemScale its on its.Scale_Ingredient_ID = si.Scale_Ingredient_ID
	left join Scale_ExtraText sxt on its.Scale_ExtraText_ID = sxt.Scale_ExtraText_ID
	where its.Item_Key = @Item_Key
end

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [Stored Procedures\ItemHosting\dbo.Scale_GetIngredientsByItem.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetIngredientsByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetIngredientsByItem] TO [IRSUser]
    AS [dbo];

