/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 08/04/2006 16:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetRandomWeightTypes]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetRandomWeightTypes]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetRandomWeightTypes AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_RandomWeightType_ID, 
		Description 
	FROM 
		Scale_RandomWeightType
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END

GO