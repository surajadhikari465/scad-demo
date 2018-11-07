create procedure dbo.Scale_AddIngredientsToItem
	@Item_Key int,
	@IngredientsDescription varchar(50),
	@Ingredients varchar(4200),
	@LabelTypeID int
as
begin
	declare @ingredientId int

	insert into Scale_Ingredient(Description, Scale_LabelType_ID, Ingredients)
	values(@IngredientsDescription, @LabelTypeID, @Ingredients)

	set @ingredientId = SCOPE_IDENTITY()

	update ItemScale
	set Scale_Ingredient_ID = @ingredientId
	where Item_Key = @Item_Key
end