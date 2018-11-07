/**********************************************************************************
Author.......: Maria Younes
Date Modified: 11/06/2008
Description..: Get menu access record to edit
***********************************************************************************/

CREATE PROCEDURE [dbo].[Administration_MenuAccess_GetMenuItemToEdit]
	
	@intMenuAccessID	int 
AS

BEGIN
	
	SET NOCOUNT ON
	
	SELECT     
		MenuAccessID, 
		MenuName, 
		Visible
	FROM MenuAccess
	WHERE MenuAccessID = @intMenuAccessID
    
    SET NOCOUNT OFF
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_GetMenuItemToEdit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_GetMenuItemToEdit] TO [IRMAClientRole]
    AS [dbo];

