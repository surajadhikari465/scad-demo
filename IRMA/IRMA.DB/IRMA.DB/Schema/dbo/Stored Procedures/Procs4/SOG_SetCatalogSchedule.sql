CREATE PROCEDURE dbo.SOG_SetCatalogSchedule
	@CatalogScheduleID	int,
	@ManagedByID		tinyint,
	@StoreNo			int,
	@SubTeamNo			int,
	@Mon				bit,
	@Tue				bit,
	@Wed				bit,
	@Thu				bit,
	@Fri				bit,
	@Sat				bit,
	@Sun				bit
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_SetCatalogSchedule()
--    Author: Billy Blackerby
--      Date: 6/26/2009
--
-- Description:
-- Utilized by StoreOrderGuide to update a catalog item
--
-- Modification History:
-- Date			Init	Comment
-- 6/26/2009	BBB		Creation
-- 07/01/2009	BBB		Added ManagedByID parameter
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	UPDATE
		[CatalogSchedule]
	SET
		ManagedByID	= @ManagedByID,
		StoreNo		= @StoreNo,
		SubTeamNo	= @SubTeamNo,
		Mon			= @Mon,
		Tue			= @Tue,
		Wed			= @Wed,
		Thu			= @Thu,
		Fri			= @Fri,
		Sat			= @Sat,
		Sun			= @Sun
	WHERE
		CatalogScheduleID = @CatalogScheduleID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_SetCatalogSchedule] TO [IRMASLIMRole]
    AS [dbo];

