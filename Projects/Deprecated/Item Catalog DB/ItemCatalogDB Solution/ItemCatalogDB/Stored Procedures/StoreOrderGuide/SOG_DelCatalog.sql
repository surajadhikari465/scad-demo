SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_DelCatalog')
	BEGIN
		DROP Procedure [dbo].SOG_DelCatalog
	END
GO

CREATE PROCEDURE dbo.SOG_DelCatalog
	@CatalogID	int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_DelCatalog()
--    Author: Billy Blackerby
--      Date: 3/24/2009
--
-- Description:
-- Utilized by StoreOrderGuide to delete a Catalog
--
-- Modification History:
-- Date			Init	Comment
-- 03/24/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	UPDATE
		[Catalog]
	SET
		Deleted = 1
	WHERE
		CatalogID = @CatalogID
		
	DELETE FROM
		CatalogItem
	WHERE
		CatalogID = @CatalogID
		
	DELETE FROM
		CatalogStore
	WHERE
		CatalogID = @CatalogID

    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 