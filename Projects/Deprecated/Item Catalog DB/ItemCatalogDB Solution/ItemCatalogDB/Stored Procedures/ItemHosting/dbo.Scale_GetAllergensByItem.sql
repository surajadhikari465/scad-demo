create procedure dbo.Scale_GetAllergensByItem
	@Item_Key int
as
begin
	select top 1 
		sa.Scale_Allergen_ID,
		sa.Description,
		sa.Allergens,
		'' as LabelTypeDescription,
		sxt.Scale_LabelType_ID as Scale_LabelType_ID
	from Scale_Allergen sa
	left join ItemScale its on its.Scale_Allergen_ID = sa.Scale_Allergen_ID
	left join Scale_ExtraText sxt on its.Scale_ExtraText_ID = sxt.Scale_ExtraText_ID
	where its.Item_Key = @Item_Key
end