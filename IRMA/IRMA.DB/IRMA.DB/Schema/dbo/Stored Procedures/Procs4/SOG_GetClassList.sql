CREATE PROCEDURE dbo.SOG_GetClassList
	@Catalog	bit,
	@SubTeamID	int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetClassList
--    Author: Billy Blackerby
--      Date: 4/11/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of class hierarchies for filters
--
-- Modification History:
-- Date			Init	Comment
-- 04/11/2009	BBB		Creation
-- 04/16/2009	BBB		Converted from NationalItemClass to ItemCategory; added
--						SubTeamID parameter
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @Catalog = 1
		BEGIN
			SELECT DISTINCT
				[ClassID]	= ic.Category_ID,
				[ClassName]	= ic.Category_Name
			FROM 
				ItemCategory (nolock) ic
			WHERE
				ic.Category_ID		> 0
				AND ic.SubTeam_No	= @SubTeamID
			ORDER BY 
				[ClassID],
				[ClassName]
		END
	ELSE
		BEGIN
			SELECT
				[ClassID]	= 0,
				[ClassName]	= 'All Classes'
				
			UNION
			
			SELECT DISTINCT
				[ClassID]	= ic.Category_ID,
				[ClassName]	= ic.Category_Name
			FROM 
				ItemCategory (nolock) ic
			WHERE
				ic.Category_ID		> 0
				AND ic.SubTeam_No	= @SubTeamID
			ORDER BY 
				[ClassID],
				[ClassName]
		END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetClassList] TO [IRMASLIMRole]
    AS [dbo];

