create procedure dbo.Scale_UpdateIngredients
	@Scale_Ingredient_ID int,
	@IngredientsDescription varchar(50),
	@Ingredients varchar(4200),
	@LabelTypeId int
as
begin
	update Scale_Ingredient
	set Description = @IngredientsDescription,
		Ingredients = @Ingredients,
		Scale_LabelType_ID = @LabelTypeId
	where Scale_Ingredient_ID = @Scale_Ingredient_ID
end