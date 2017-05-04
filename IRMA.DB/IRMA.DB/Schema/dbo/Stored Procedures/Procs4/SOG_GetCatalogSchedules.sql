CREATE PROCEDURE [dbo].[SOG_GetCatalogSchedules]
	@CatalogScheduleID	int,
	@ManagedByID		int,
	@StoreNo			int,
	@SubTeamNo			int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetCatalogSchedules()
--    Author: Billy Blackerby
--      Date: 6/26/2009
--
-- Description:
-- Utilized by StoreOrderGuide to read the Catalog Schedule
--
-- Modification History:
-- Date			Init	Comment
-- 06/26/2009	BBB		Creation
-- 07/01/2009	BBB		Added ManagedByID parameter
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Treat Variables
	--**************************************************************************
	IF @CatalogScheduleID = 0
		SET @CatalogScheduleID = NULL

	IF @ManagedByID = 0
		SET @ManagedByID = NULL

	IF @StoreNo = 0
		SET @StoreNo = NULL

	IF @SubTeamNo = 0
		SET @SubTeamNo = NULL
    
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT 
		[CatalogScheduleID],
		[ManagedByID],
		[Value],
		[StoreNo],
		[SubTeamNo],
		[Store_Name],
		[SubTeam_Name],
		[Mon],
		[Tue],
		[Wed],
		[Thu],
		[Fri],
		[Sat],
		[Sun]
	FROM 
		CatalogSchedule		(nolock) cs
		JOIN Store			(nolock) s	ON cs.StoreNo		= s.Store_No
		JOIN SubTeam		(nolock) st	ON cs.SubTeamNo		= st.SubTeam_No
		JOIN ItemManager	(nolock) im	ON cs.ManagedByID	= im.Manager_ID
	WHERE
		cs.CatalogScheduleID	= ISNULL(@CatalogScheduleID, cs.CatalogScheduleID)
		AND cs.ManagedByID		= ISNULL(@ManagedByID, cs.ManagedByID)
		AND cs.StoreNo			= ISNULL(@StoreNo, cs.StoreNo)
		AND cs.SubTeamNo		= ISNULL(@SubTeamNo, cs.SubTeamNo)
		
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetCatalogSchedules] TO [IRMASLIMRole]
    AS [dbo];

