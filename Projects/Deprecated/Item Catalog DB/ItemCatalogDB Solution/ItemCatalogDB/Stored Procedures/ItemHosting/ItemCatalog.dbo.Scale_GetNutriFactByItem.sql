IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetNutriFactByItem]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetNutriFactByItem]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetNutriFactByItem 
	@Item_Key		AS INT,
	@Jurisdiction	AS INT,
	@IsScaleItem	AS BIT = 1
AS
BEGIN

	IF (@IsScaleItem = 1)
		BEGIN
			SELECT 
				ISNULL(iso.Nutrifact_ID, isc.Nutrifact_ID) AS Nutrifact_ID
			FROM 
				ItemScale isc
				LEFT JOIN ItemScaleOverride iso ON isc.Item_Key = iso.Item_Key AND iso.StoreJurisdictionID = @Jurisdiction
			WHERE
				isc.Item_Key = @Item_Key
		END
	ELSE
		BEGIN
			SELECT
				inu.NutriFactsID AS Nutrifact_ID
			FROM
				ItemNutrition inu
			WHERE
				inu.ItemKey = @Item_Key
		END
END
GO
