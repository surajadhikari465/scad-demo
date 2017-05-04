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
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetAdminSettings] TO [IRMASLIMRole]
    AS [dbo];

