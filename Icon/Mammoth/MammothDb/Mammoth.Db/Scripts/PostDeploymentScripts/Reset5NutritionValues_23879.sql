DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'Reset5NutritionValues_23879'

IF NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	IF(OBJECT_ID('ItemAttributes_Nutrition_Fix') IS NOT NULL) DROP TABLE ItemAttributes_Nutrition_Fix;
	--List of items that are part of script to BA. We'll need to refresh these items to update OnePlum (after LF1 value is in place).
	SELECT ItemID
	INTO ItemAttributes_Nutrition_Fix
	FROM dbo.ItemAttributes_Nutrition
	WHERE AddedDate  <= Cast('2019-09-05' AS DATE)
	  AND (AddedSugarsWeight = 0
	   OR AddedSugarsPercent = 0
	   OR CalciumWeight = 0
	   OR IronWeight = 0
	   OR VitaminDWeight = 0);

	UPDATE dbo.ItemAttributes_Nutrition
	SET AddedSugarsWeight = CASE 
			WHEN IsNull(AddedSugarsWeight, 0) = 0 THEN NULL
			ELSE AddedSugarsWeight
			END
		,AddedSugarsPercent = CASE 
			WHEN IsNull(AddedSugarsPercent, 0) = 0 THEN NULL
			ELSE AddedSugarsPercent
			END
		,CalciumWeight = CASE 
			WHEN IsNull(CalciumWeight, 0) = 0 THEN NULL
			ELSE CalciumWeight
			END
		,IronWeight = CASE 
			WHEN IsNull(IronWeight, 0) = 0 	THEN NULL
			ELSE IronWeight
			END
		,VitaminDWeight = CASE 
			WHEN IsNull(VitaminDWeight, 0) = 0 THEN NULL
			ELSE VitaminDWeight
			END
	WHERE AddedDate  <= Cast('2019-09-05' AS DATE);

	INSERT INTO app.PostDeploymentScriptHistory(ScriptKey, RunTime)
	VALUES(@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Script already applied: ' + @scriptKey
END
GO


