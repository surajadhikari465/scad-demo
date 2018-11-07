/**********************************************************************************
Author.......: Maria Younes
Date Modified: 11/06/2008
Description..: Toggles visible flag in MenuAccess table for the passed in Menu Id
***********************************************************************************/

CREATE PROCEDURE [dbo].[Administration_MenuAccess_UpdateMenuAccessRecord] 

	@MenuAccessID int,
	@UpdateAll bit,
	@AllValue bit
AS

BEGIN

    SET NOCOUNT ON
	
	IF @UpdateAll = 1
		UPDATE [dbo].[MenuAccess] 	SET Visible = @AllValue
	ELSE
		UPDATE 
			[dbo].[MenuAccess] 
		SET 
			Visible = CASE WHEN Visible = 1 THEN 0 ELSE 1 END
		WHERE 
			MenuAccessID = @MenuAccessID		
		
    SET NOCOUNT OFF	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_UpdateMenuAccessRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_UpdateMenuAccessRecord] TO [IRMAClientRole]
    AS [dbo];

