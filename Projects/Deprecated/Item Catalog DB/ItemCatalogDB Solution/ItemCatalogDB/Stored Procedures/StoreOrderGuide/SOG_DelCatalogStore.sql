SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_DelCatalogStore')
	BEGIN
		DROP Procedure [dbo].SOG_DelCatalogStore
	END
GO

CREATE PROCEDURE dbo.SOG_DelCatalogStore
	@CatalogStoreID	int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_DelCatalogStore()
--    Author: Billy Blackerby
--      Date: 3/19/2009
--
-- Description:
-- Utilized by StoreOrderGuide to delete a CatalogStore relationship
--
-- Modification History:
-- Date			Init	Comment
-- 03/19/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	DELETE FROM
		CatalogStore	
	WHERE
		CatalogStoreID = @CatalogStoreID

    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 