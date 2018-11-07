 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_AddAdminSetting')
	BEGIN
		DROP Procedure [dbo].SOG_AddAdminSetting
	END
GO

CREATE PROCEDURE [dbo].[SOG_AddAdminSetting]
	@AdminKey	varchar(50),
	@AdminValue	varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddAdminSetting()
--    Author: Brian Strickland
--      Date: 3/23/2009
--
-- Description:
-- Utilized by StoreOrderGuide to insert a Catalog Administrator Setting and Value
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
	INSERT INTO 
		[dbo].[CatalogAdmin]
           ([AdminKey]
           ,[AdminValue])
     VALUES
           (@AdminKey
           , @AdminValue)

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 
