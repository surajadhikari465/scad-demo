/**********************************************************************************
Author.......: Maria Younes
Date Modified: 11/06/2008
Description..: Get menu access records
***********************************************************************************/

CREATE PROCEDURE [dbo].[Administration_MenuAccess_GetMenuAccessRecords]
AS

BEGIN
	
	SET NOCOUNT ON
	
	SELECT     
		MenuAccessID, MenuName, Visible
	FROM 
		MenuAccess
	ORDER BY
		MenuName
	
	SET NOCOUNT OFF
	    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_GetMenuAccessRecords] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_GetMenuAccessRecords] TO [IRMAClientRole]
    AS [dbo];

