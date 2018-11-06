SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetCatalogStores')
	BEGIN
		DROP Procedure [dbo].SOG_GetCatalogStores
	END
GO

CREATE PROCEDURE dbo.SOG_GetCatalogStores
	@CatalogID	int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetCatalogStores()
--    Author: Billy Blackerby
--      Date: 3/18/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of stores for a specific catalog
--
-- Modification History:
-- Date			Init	Comment
-- 03/18/2009	BBB		Creation
-- 03/19/2009	BBB		Added CatalogStoreID to output
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT
		[CatalogStoreID]	= cs.CatalogStoreID,
		[StoreNo]			= s.Store_No,
		[StoreName]			= s.Store_Name,
		[StoreAbbr]			= s.StoreAbbr
	FROM
		[Catalog]						(nolock) c
		INNER JOIN	CatalogStore		(nolock) cs		ON	c.CatalogID					= cs.CatalogID
		INNER JOIN	Store				(nolock) s		ON	cs.StoreNo					= s.Store_No
	WHERE
		c.CatalogID	= @CatalogID

    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 