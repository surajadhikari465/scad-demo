CREATE PROCEDURE dbo.Scale_GetScaleDetailCombos AS

BEGIN
    SET NOCOUNT ON

	EXEC Scale_GetEatBy    
	EXEC Scale_GetGrades
	EXEC Scale_GetLabelStyles
	EXEC Scale_GetNutriFacts
	--EXEC Scale_GetExtraTexts
	EXEC Scale_GetRandomWeightTypes
	EXEC Scale_GetTares
	EXEC Scale_GetScaleUOMs

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetScaleDetailCombos] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetScaleDetailCombos] TO [IRMAClientRole]
    AS [dbo];

