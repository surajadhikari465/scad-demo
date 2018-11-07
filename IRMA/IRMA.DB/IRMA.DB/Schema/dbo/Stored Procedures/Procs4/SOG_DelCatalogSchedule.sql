CREATE PROCEDURE [dbo].[SOG_DelCatalogSchedule]
	@CatalogScheduleID		int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_DelCatalogSchedule()
--    Author: Billy Blackerby
--      Date: 6/26
--
-- Description:
-- Utilized by StoreOrderGuide to delete a CatalogSchedule
--
-- Modification History:
-- Date			Init	Comment
-- 06/26/2009	BS		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	DELETE FROM 
		[dbo].[CatalogSchedule]
	WHERE 
		[dbo].[CatalogSchedule].[CatalogScheduleID] = @CatalogScheduleID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_DelCatalogSchedule] TO [IRMASLIMRole]
    AS [dbo];

