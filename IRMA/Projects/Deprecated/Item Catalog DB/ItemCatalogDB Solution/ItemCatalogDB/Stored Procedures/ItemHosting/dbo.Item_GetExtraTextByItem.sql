CREATE PROCEDURE [dbo].[Item_GetExtraTextByItem] 
	@Item_Key as INT
AS

BEGIN

	SELECT
		INF.ItemNutrifactId,		
		IET.Item_ExtraText_ID, 
		IET.ExtraText, 
		IET.[Description],
		IET.Scale_LabelType_ID
	FROM	ItemNutrifact INF (NOLOCK) INNER JOIN Item_ExtraText IET (NOLOCK)
			ON INF.Item_ExtraText_ID = IET.Item_ExtraText_ID
	WHERE
				INF.ItemKey = @Item_Key
END