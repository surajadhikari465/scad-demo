CREATE PROCEDURE [dbo].[SOG_GetAdminSetting]
	@AdminKey varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetAdminSetting()
--    Author: Billy Blackerby
--      Date: 6/29/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a specific Administrator Value
--
-- Modification History:
-- Date			Init	Comment
-- 06/29/2009	BS		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT 
		[AdminValue]
	FROM 
		[dbo].[CatalogAdmin]
	WHERE
		AdminKey = ISNULL(@AdminKey, AdminKey)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetAdminSetting] TO [IRMASLIMRole]
    AS [dbo];

