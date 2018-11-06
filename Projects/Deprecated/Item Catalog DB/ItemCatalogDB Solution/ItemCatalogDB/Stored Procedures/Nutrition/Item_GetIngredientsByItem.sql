IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item_GetIngredientsByItem]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Item_GetIngredientsByItem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
