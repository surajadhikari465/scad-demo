CREATE PROCEDURE [dbo].[Item_GetIngredientsByItem] 
	@Item_Key as INT
AS

BEGIN

	SELECT
		INF.ItemNutrifactId,		
		SIG.Scale_Ingredient_ID, 
		SIG.Ingredients, 
		SIG.[Description]
	FROM	ItemNutrifact INF (NOLOCK) INNER JOIN Scale_Ingredient SIG (NOLOCK)
			ON INF.Scale_Ingredient_ID = SIG.Scale_Ingredient_ID
	WHERE
			INF.ItemKey = @Item_Key
END