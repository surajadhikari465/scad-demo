IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item_GetAllergensByItem]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Item_GetAllergensByItem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
