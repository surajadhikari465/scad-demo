/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 08/04/2006 16:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetScaleDetailCombos]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetScaleDetailCombos]
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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