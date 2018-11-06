SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_AddCatalogStore')
	BEGIN
		DROP Procedure [dbo].SOG_AddCatalogStore
	END
GO

CREATE PROCEDURE dbo.SOG_AddCatalogStore
	@CatalogID	int,
	@StoreNo	int,
	@User		varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddCatalogStore()
--    Author: Billy Blackerby
--      Date: 3/19/2009
--
-- Description:
-- Utilized by StoreOrderGuide to insert a CatalogStore relationship
--
-- Modification History:
-- Date			Init	Comment
-- 03/19/2009	BBB		Creation
-- 03/24/2009	BBB		Added capability for all stores to be added
-- 03/31/2009	BBB		Added in trap to only populate Stores not already in
--						CatalogStore
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON	
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @StoreNo > 0
		INSERT INTO 
			CatalogStore	
		(
			CatalogID,
			StoreNo,
			InsertUser
		)
		VALUES
		(
			@CatalogID,
			@StoreNo,
			@User
		)
	ELSE
		INSERT INTO
			CatalogStore
		(
			CatalogID,
			StoreNo,
			InsertUser
		)
		(
			SELECT
				@CatalogID,
				s.Store_No,
				@User
			FROM 
				Store (nolock) s
			WHERE
				(
				s.WFM_Store		= 1
				OR s.Mega_Store	= 1
				)
				AND s.Store_No NOT IN (SELECT StoreNo FROM CatalogStore WHERE CatalogID = @CatalogID)
		)

    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 