SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_MassPublishCatalogs')
	BEGIN
		DROP Procedure [dbo].SOG_MassPublishCatalogs
	END
GO

CREATE PROCEDURE dbo.SOG_MassPublishCatalogs
	@CatalogIDs		varchar(500),
	@Published		bit,
	@UpdateUser		varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_MassPublishCatalogs()
--    Author: M. Zhao
--      Date: 4/26/2012
--
-- Description:
-- Utilized by StoreOrderGuide to mass update catalogs (mass publish/unpublish them)
--
-- Modification History:
-- Date			Init	Comment
-- 04/26/2012	MZ		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	UPDATE
		[Catalog]
	SET
		Published		= @Published,
		UpdateUser		= @UpdateUser
	WHERE
		CatalogID	in (SELECT Key_Value FROM dbo.fn_Parse_List(@CatalogIDs,NULL))

    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 