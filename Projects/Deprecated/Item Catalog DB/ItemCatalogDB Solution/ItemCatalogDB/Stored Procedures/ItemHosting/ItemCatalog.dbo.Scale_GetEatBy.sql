/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 08/04/2006 16:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetEatBy]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetEatBy]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetEatBy AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_EatBy_ID, 
		Description 
	FROM 
		Scale_EatBy
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END

GO