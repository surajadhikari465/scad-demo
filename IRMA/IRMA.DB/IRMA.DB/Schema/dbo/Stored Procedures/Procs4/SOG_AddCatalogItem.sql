CREATE PROCEDURE dbo.SOG_AddCatalogItem
	@CatalogID	int,
	@ItemKey	int,
	@User		varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddCatalogItem()
--    Author: Billy Blackerby
--      Date: 3/23/2009
--
-- Description:
-- Utilized by StoreOrderGuide to insert a CatalogItem relationship
--
-- Modification History:
-- Date			Init	Comment
-- 03/23/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	INSERT INTO 
		CatalogItem
	(
		CatalogID,
		ItemKey,
		InsertUser
	)
	VALUES
	(
		@CatalogID,
		@ItemKey,
		@User
	)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_AddCatalogItem] TO [IRMASLIMRole]
    AS [dbo];

