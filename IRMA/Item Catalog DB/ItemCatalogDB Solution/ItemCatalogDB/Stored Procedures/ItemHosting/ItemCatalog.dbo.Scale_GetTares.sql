/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 08/04/2006 16:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scale_GetTares]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Scale_GetTares]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Scale_GetTares AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_Tare_ID, 
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
		Scale_Tare
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END

GO