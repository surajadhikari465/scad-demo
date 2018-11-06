IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item_GetExtraTextByItem]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Item_GetExtraTextByItem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
