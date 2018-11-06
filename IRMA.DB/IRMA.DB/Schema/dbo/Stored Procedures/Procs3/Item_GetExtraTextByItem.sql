CREATE PROCEDURE [dbo].[Item_GetExtraTextByItem] 
	@Item_Key as INT
AS

BEGIN

	SELECT
		INF.ItemNutritionId,		
		IET.Item_ExtraText_ID, 
		IET.ExtraText, 
		IET.[Description],
		IET.Scale_LabelType_ID
	FROM	ItemNutrition INF (NOLOCK) INNER JOIN Item_ExtraText IET (NOLOCK)
			ON INF.Item_ExtraText_ID = IET.Item_ExtraText_ID
	WHERE
				INF.ItemKey = @Item_Key
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetExtraTextByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Item_GetExtraTextByItem] TO [IRSUser]
    AS [dbo];

