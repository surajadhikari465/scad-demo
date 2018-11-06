  SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetAdminSettings')
	BEGIN
		DROP Procedure [dbo].SOG_GetAdminSettings
	END
GO

CREATE PROCEDURE [dbo].[SOG_GetAdminSettings]

WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetAdminSettings()
--    Author: Brian Strickland
--      Date: 3/23/2009
--
-- Description:
-- Utilized by StoreOrderGuide to read the Catalog Administrator Setting and Values
--
-- Modification History:
-- Date			Init	Comment
-- 03/23/2009	BS		Creation
-- 03/30/2009	BBB		Removed security grants from this SP and added to master
--						IRMADB project SecurityGrants.sql
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT 
		[AdminID],
		[AdminKey],
		[AdminValue]
	FROM 
		[dbo].[CatalogAdmin]

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 