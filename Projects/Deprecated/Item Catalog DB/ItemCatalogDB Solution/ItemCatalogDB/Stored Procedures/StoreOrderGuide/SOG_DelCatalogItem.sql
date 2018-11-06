SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_DelCatalogItem')
	BEGIN
		DROP Procedure [dbo].SOG_DelCatalogItem
	END
GO

CREATE PROCEDURE [dbo].[SOG_DelCatalogItem]
	@CatalogItemID	int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_DelCatalogItem()
--    Author: Billy Blackerby
--      Date: 3/19/2009
--
-- Description:
-- Utilized by StoreOrderGuide to delete a CatalogItem relationship
--
-- Modification History:
-- Date			Init	Comment
-- 03/19/2009	BBB		Creation
-- 08/18/2011	BAF		Added additional delete from catalogorderitem to resolve FK violations
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	DELETE FROM
		CatalogOrderItem
	WHERE
		CatalogItemID = @CatalogItemID

	
	DELETE FROM
		CatalogItem
	WHERE
		CatalogItemID = @CatalogItemID

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 