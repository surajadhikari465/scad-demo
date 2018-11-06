/****** Object:  StoredProcedure [dbo].[GetCategoriesAndSubTeams]    Script Date: 10/13/2006 14:39:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCategoriesAndSubTeams]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetCategoriesAndSubTeams]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[GetCategoriesAndSubTeams]    Script Date: 10/13/2006 14:39:00 ******/
CREATE PROCEDURE dbo.[GetCategoriesAndSubTeams]

AS

BEGIN
    SET NOCOUNT ON

	----------------------------------------------
	-- return all categories by subteam, sorted by subteam then category
	----------------------------------------------
	SELECT ST.SubTeam_No, 
		ST.SubTeam_Name, 
		IC.Category_ID, 
		IC.Category_Name
	FROM ItemCategory IC (NOLOCK) 
		INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = IC.SubTeam_No
	ORDER BY ST.SubTeam_Name, IC.Category_Name

    SET NOCOUNT OFF
END

GO