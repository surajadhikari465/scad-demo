  SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_DelAdminSetting')
	BEGIN
		DROP Procedure [dbo].SOG_DelAdminSetting
	END
GO

CREATE PROCEDURE [dbo].[SOG_DelAdminSetting]
	@AdminID		int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_DelAdminSetting()
--    Author: Brian Strickland
--      Date: 3/23/2009
--
-- Description:
-- Utilized by StoreOrderGuide to delete a Catalog Administrator Setting and Value
--
-- Modification History:
-- Date			Init	Comment
-- 03/23/2009	BS		Creation
-- 03/30/2009	BBB		Removed security grants from this SP and added to master
--						IRMADB project SecurityGrants.sql; changed parameter from
--						Key to AdminID matching PK
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	DELETE FROM 
		[dbo].[CatalogAdmin]
	WHERE 
		[dbo].[CatalogAdmin].[AdminID] = @AdminID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 