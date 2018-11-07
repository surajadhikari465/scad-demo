SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_GetItemScaleDetails]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_GetItemScaleDetails]
GO


CREATE PROCEDURE [dbo].[Scale_GetItemScaleDetails]
	@Item_Key int
AS 
	BEGIN
		SELECT 
			ItemScale_ID,
			ItemScale.Item_Key, 
			Nutrifact_ID, 
			ItemScale.Scale_ExtraText_ID, 
			Scale_ExtraText.Description AS Scale_ExtraText,
			ItemScale.Scale_Ingredient_ID,
			Scale_Ingredient.Description AS Scale_Ingredient,
			ItemScale.Scale_Allergen_ID,			
			Scale_Allergen.Description AS Scale_Allergen,
			Scale_Tare_ID, 
			Scale_Alternate_Tare_ID, 
			Scale_LabelStyle_ID, 
			Scale_EatBy_ID, 
			Scale_Grade_ID, 
			Scale_RandomWeightType_ID, 
			Scale_ScaleUOMUnit_ID,
			Scale_FixedWeight, 
			Scale_ByCount, 
			ForceTare, 
			PrintBlankShelfLife,
			PrintBlankEatBy, 
			PrintBlankPackDate, 
			PrintBlankWeight, 
			PrintBlankUnitPrice,
			PrintBlankTotalPrice,
			Scale_Description1, 
			Scale_Description2, 
			Scale_Description3,
			Scale_Description4, 
			ShelfLife_Length,
			ItemCustomerFacingScale.CustomerFacingScaleDepartment,
			ItemCustomerFacingScale.SendToScale
		FROM 
			ItemScale
		LEFT OUTER JOIN Scale_ExtraText 
			ON ItemScale.Scale_ExtraText_ID = Scale_ExtraText.Scale_ExtraText_ID
		LEFT OUTER JOIN Scale_Ingredient
			ON ItemScale.Scale_Ingredient_ID = Scale_Ingredient.Scale_Ingredient_ID
		LEFT OUTER JOIN Scale_Allergen
			ON ItemScale.Scale_Allergen_ID = Scale_Allergen.Scale_Allergen_ID
		LEFT JOIN ItemCustomerFacingScale 
			ON ItemScale.Item_Key = ItemCustomerFacingScale.Item_Key
		WHERE 
			ItemScale.Item_Key = @Item_Key
	END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

