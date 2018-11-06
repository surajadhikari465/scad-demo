SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetCategoriesByMultiSubTeam]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetCategoriesByMultiSubTeam]
GO
CREATE PROCEDURE [dbo].[GetCategoriesByMultiSubTeam]
	@SubteamList varchar (8000)
AS 
BEGIN
    SET NOCOUNT ON

    SELECT 
		Category_ID, 
		Category_Name
    FROM  
		ItemCategory ic (nolock)
		INNER JOIN dbo.fn_Parse_List(''+ @SubteamList + '', ',') stl
			ON stl.Key_Value = ic.SubTeam_No 
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 