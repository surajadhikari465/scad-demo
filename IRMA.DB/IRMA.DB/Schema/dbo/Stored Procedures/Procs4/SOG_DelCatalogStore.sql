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
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_DelCatalogStore] TO [IRMASLIMRole]
    AS [dbo];

