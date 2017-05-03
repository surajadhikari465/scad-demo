/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 08/04/2006 16:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetScaleUOMs]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetScaleUOMs]
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetScaleUOMs AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Unit_ID, 
		RTRIM(Unit_Name) + ' (' + Unit_Abbreviation + ')' AS Description
	FROM 
		ItemUnit
	WHERE
		Weight_Unit = 1
	ORDER BY 
		Unit_Name
    
    SET NOCOUNT OFF
END

GO