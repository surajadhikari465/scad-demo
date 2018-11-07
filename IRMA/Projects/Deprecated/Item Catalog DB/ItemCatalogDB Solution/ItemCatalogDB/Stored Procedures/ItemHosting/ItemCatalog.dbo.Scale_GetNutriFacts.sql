IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetNutriFacts]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetNutriFacts]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetNutriFacts AS

BEGIN

	SELECT 
		NutrifactsID, 
		Description 
	FROM 
		NutriFacts
	ORDER BY 
		Description
    
END



GO