-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description: Reads an Item's Allergen override data
--    for the provided alternate jurisdiction
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE [dbo].[Item_GetAllergensOverrideByItem] 
	@Item_Key as INT,
	@Jurisdiction as INT
AS

BEGIN

	SELECT
		INO.ItemNutritionOverride_ID AS ItemNutritionId,		
		SAL.Scale_Allergen_ID, 
		SAL.Allergens, 
		SAL.[Description]
	FROM ItemNutritionOverride INO (NOLOCK)
        INNER JOIN Scale_Allergen SAL (NOLOCK) ON INO.Scale_Allergen_ID = SAL.Scale_Allergen_ID
	WHERE
		INO.ItemKey = @Item_Key AND INO.StoreJurisdictionID = @Jurisdiction
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetAllergensOverrideByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetAllergensOverrideByItem] TO [IRSUser]
    AS [dbo];

