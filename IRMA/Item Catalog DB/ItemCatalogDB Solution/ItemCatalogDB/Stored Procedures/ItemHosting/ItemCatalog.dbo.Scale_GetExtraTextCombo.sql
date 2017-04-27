 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetExtraTextCombo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetExtraTextCombo]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetExtraTextCombo AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_ExtraText_ID, 
		[Description]
	FROM dbo.Scale_ExtraText
    
    SET NOCOUNT OFF
END

GO