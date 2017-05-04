CREATE PROCEDURE dbo.SOG_GetSubTeamList
	@Catalog	bit
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetSubTeamList()
--    Author: Billy Blackerby
--      Date: 3/13/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of subteams for filters
--
-- Modification History:
-- Date			Init	Comment
-- 03/13/2009	BBB		Creation
-- 03/17/2009	BBB		Added in 'All' option
-- 04/10/2009	BBB		Added Catalog parameter to remove default option
-- 05/28/2009	BBB		Removed WHERE clause from both queries
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @Catalog = 1
		BEGIN
			SELECT 
				[SubTeamID]		= st.SubTeam_No,
				[SubTeamName]	= st.SubTeam_Name
			FROM 
				SubTeam	(nolock) st	
			ORDER BY 
				SubTeamID, 
				SubTeamName
		END
	ELSE
		BEGIN
			SELECT
				[SubTeamID]		= 0,
				[SubTeamName]	= 'All SubTeams'
				
			UNION
			
			SELECT 
				[SubTeamID]		= st.SubTeam_No,
				[SubTeamName]	= st.SubTeam_Name
			FROM 
				SubTeam	(nolock) st
			ORDER BY 
				SubTeamID, 
				SubTeamName
		END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetSubTeamList] TO [IRMASLIMRole]
    AS [dbo];

