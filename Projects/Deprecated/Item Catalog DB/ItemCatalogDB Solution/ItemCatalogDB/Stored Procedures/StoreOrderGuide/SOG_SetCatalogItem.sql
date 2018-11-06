SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_SetCatalogItem')
	BEGIN
		DROP Procedure [dbo].SOG_SetCatalogItem
	END
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 