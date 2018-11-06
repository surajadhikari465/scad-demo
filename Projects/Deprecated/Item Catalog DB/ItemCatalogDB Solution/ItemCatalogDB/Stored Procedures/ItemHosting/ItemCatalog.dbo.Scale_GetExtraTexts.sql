IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetExtraTexts]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetExtraTexts]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetExtraTexts AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_ExtraText_ID, 
		Scale_LabelType_ID, 
		Description,
		ExtraText
		
	FROM 
		Scale_ExtraText
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END

GO