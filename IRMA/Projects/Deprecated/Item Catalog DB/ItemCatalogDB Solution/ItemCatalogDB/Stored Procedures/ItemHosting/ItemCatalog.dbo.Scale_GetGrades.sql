/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 08/04/2006 16:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetGrades]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetGrades]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetGrades AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_Grade_ID, 
		Description,
		Zone1,
		Zone2,
		Zone3,
		Zone4,
		Zone5,
		Zone6,
		Zone7,
		Zone8,
		Zone9,
		Zone10
	FROM 
		Scale_Grade
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END

GO