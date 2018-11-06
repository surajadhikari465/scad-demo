CREATE PROCEDURE [dbo].[Item_GetIngredientsByItem] 
	@Item_Key as INT
AS

BEGIN

	SELECT
		INF.ItemNutritionId,		
		SIG.Scale_Ingredient_ID, 
		SIG.Ingredients, 
		SIG.[Description]
	FROM	ItemNutrition INF (NOLOCK) INNER JOIN Scale_Ingredient SIG (NOLOCK)
			ON INF.Scale_Ingredient_ID = SIG.Scale_Ingredient_ID
	WHERE
			INF.ItemKey = @Item_Key
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetIngredientsByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetIngredientsByItem] TO [IRSUser]
    AS [dbo];

