SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_AddCatalogItem')
	BEGIN
		DROP Procedure [dbo].SOG_AddCatalogItem
	END
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 