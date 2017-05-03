  SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_DelCatalogSchedule')
	BEGIN
		DROP Procedure [dbo].SOG_DelCatalogSchedule
	END
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 