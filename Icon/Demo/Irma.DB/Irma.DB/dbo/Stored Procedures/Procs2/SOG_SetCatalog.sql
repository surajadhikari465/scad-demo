CREATE PROCEDURE dbo.SOG_SetCatalog
	@CatalogID		int,
	@ManagedByID	int,
	@CatalogCode	varchar(20),
	@Description	varchar(max),
	@Details		varchar(max),
	@Published		bit,
	@SubTeam		int,
	@ExpectedDate	bit,
	@UpdateUser		varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_SetCatalog()
--    Author: Billy Blackerby
--      Date: 3/18/2009
--
-- Description:
-- Utilized by StoreOrderGuide to update a catalog
--
-- Modification History:
-- Date			Init	Comment
-- 03/18/2009	BBB		Creation
-- 03/19/2009	BBB		Changed ManagedBy to ManagedByID
-- 03/24/2009	BBB		Added ExpectedDate and UpdateUser parameter
-- 05/09/2009	BBB		Added Details parameter
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	UPDATE
		[Catalog]
	SET
		ManagedByID		= @ManagedByID,
		CatalogCode		= @CatalogCode,
		Description		= @Description,
		Details			= @Details,
		Published		= @Published,
		SubTeam			= @SubTeam,
		ExpectedDate	= @ExpectedDate,
		UpdateUser		= @UpdateUser
	WHERE
		CatalogID	= @CatalogID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_SetCatalog] TO [IRMASLIMRole]
    AS [dbo];

