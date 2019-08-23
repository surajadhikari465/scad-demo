-- =============================================
-- Author:		Ed McNab
-- Create date: 2019-08-22
-- Description: Reads an Item's ExtraText override data
--    for the provided alternate jurisdiciton
-- Date			Modified By		TFS		Description
-- 
-- =============================================

CREATE PROCEDURE dbo.Item_GetExtraTextOverrideByItem 
	@Item_Key as INT,
	@Jurisdiction as INT
AS

BEGIN

	SELECT
		INO.ItemNutritionOverride_ID As ItemNutritionId,		
		IET.Item_ExtraText_ID, 
		IET.ExtraText, 
		IET.[Description],
		IET.Scale_LabelType_ID
	FROM ItemNutritionOverride INO (NOLOCK)
        INNER JOIN Item_ExtraText IET (NOLOCK) ON INO.Item_ExtraText_ID = IET.Item_ExtraText_ID
	WHERE
		INO.ItemKey = @Item_Key AND INO.StoreJurisdictionID = @Jurisdiction
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetExtraTextOverrideByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetExtraTextOverrideByItem] TO [IRSUser]
    AS [dbo];

