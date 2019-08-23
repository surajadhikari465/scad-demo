
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
				ISNULL(ino.NutriFactsID, inu.NutriFactsID) AS Nutrifact_ID
			FROM
				ItemNutrition inu
                LEFT JOIN ItemNutritionOverride ino on ino.ItemKey = inu.ItemKey AND ino.StoreJurisdictionID = @Jurisdiction
			WHERE
				inu.ItemKey = @Item_Key
		END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetNutriFactByItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetNutriFactByItem] TO [IRMAClientRole]
    AS [dbo];

