-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description: Reads an Item's Ingerdient override data
--    for the provided alternate jurisdiciton
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE [dbo].[Item_GetIngredientsOverrideByItem] 
	@Item_Key as INT,
	@Jurisdiction as INT
AS

BEGIN
	SELECT
		INO.ItemNutritionOverride_ID AS ItemNutritionId,		
		SIG.Scale_Ingredient_ID, 
		SIG.Ingredients, 
		SIG.[Description]
	FROM ItemNutritionOverride INO (NOLOCK) 
        INNER JOIN Scale_Ingredient SIG (NOLOCK) ON INO.Scale_Ingredient_ID = SIG.Scale_Ingredient_ID
	WHERE
		INO.ItemKey = @Item_Key AND INO.StoreJurisdictionID = @Jurisdiction
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetIngredientsOverrideByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetIngredientsOverrideByItem] TO [IRSUser]
    AS [dbo];

