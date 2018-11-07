/**********************************************************************************
Author.......: Maria Younes
Date Modified: 11/06/2008
Description..: Get menu names
***********************************************************************************/

CREATE PROCEDURE [dbo].[Administration_MenuAccess_GetMenuNamesList]
	
AS

BEGIN
	
	SET NOCOUNT ON
	
	SELECT     
		MenuName,
		MenuAccessID
	FROM MenuAccess
    ORDER BY
		MenuName
		
	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_GetMenuNamesList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_GetMenuNamesList] TO [IRMAClientRole]
    AS [dbo];

