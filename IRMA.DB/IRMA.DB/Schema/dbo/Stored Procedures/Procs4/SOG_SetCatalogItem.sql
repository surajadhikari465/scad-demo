CREATE PROCEDURE dbo.SOG_SetCatalogItem
	@CatalogItemID	int,
	@ItemNote		varchar(max)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_SetCatalogItem()
--    Author: Billy Blackerby
--      Date: 3/23/2009
--
-- Description:
-- Utilized by StoreOrderGuide to update a catalog item
--
-- Modification History:
-- Date			Init	Comment
-- 3/23/2009	BBB		Creation
-- 4/01/2009	BBB		Resolved issue with ItemNotes column 
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	UPDATE
		[CatalogItem]
	SET
		ItemNotes		= @ItemNote
	WHERE
		CatalogItemID	= @CatalogItemID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_SetCatalogItem] TO [IRMASLIMRole]
    AS [dbo];

