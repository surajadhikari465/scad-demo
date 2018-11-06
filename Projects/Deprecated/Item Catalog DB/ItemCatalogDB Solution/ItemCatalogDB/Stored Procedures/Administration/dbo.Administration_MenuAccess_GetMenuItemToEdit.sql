IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_MenuAccess_GetMenuItemToEdit]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Administration_MenuAccess_GetMenuItemToEdit]
GO

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






