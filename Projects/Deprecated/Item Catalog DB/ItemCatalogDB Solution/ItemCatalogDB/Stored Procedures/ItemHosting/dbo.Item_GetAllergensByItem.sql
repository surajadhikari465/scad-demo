CREATE PROCEDURE [dbo].[Item_GetAllergensByItem] 
	@Item_Key as INT
AS

BEGIN

	SELECT
		INF.ItemNutrifactId,		
		SAL.Scale_Allergen_ID, 
		SAL.Allergens, 
		SAL.[Description]
	FROM	ItemNutrifact INF (NOLOCK) INNER JOIN Scale_Allergen SAL (NOLOCK)
			ON INF.Scale_Allergen_ID = SAL.Scale_Allergen_ID
	WHERE
			INF.ItemKey = @Item_Key
END