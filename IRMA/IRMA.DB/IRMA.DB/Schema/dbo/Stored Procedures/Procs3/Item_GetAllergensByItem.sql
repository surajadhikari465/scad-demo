CREATE PROCEDURE [dbo].[Item_GetAllergensByItem] 
	@Item_Key as INT
AS

BEGIN

	SELECT
		INF.ItemNutritionId,		
		SAL.Scale_Allergen_ID, 
		SAL.Allergens, 
		SAL.[Description]
	FROM	ItemNutrition INF (NOLOCK) INNER JOIN Scale_Allergen SAL (NOLOCK)
			ON INF.Scale_Allergen_ID = SAL.Scale_Allergen_ID
	WHERE
			INF.ItemKey = @Item_Key
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetAllergensByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetAllergensByItem] TO [IRSUser]
    AS [dbo];

