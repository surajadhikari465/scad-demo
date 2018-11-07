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
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_AddAdminSetting] TO [IRMASLIMRole]
    AS [dbo];

