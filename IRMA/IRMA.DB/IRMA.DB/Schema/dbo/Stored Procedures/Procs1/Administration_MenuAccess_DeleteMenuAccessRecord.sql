CREATE PROCEDURE [dbo].[Administration_MenuAccess_DeleteMenuAccessRecord] 

	@MenuAccessID int
AS

BEGIN

    SET NOCOUNT ON
	
	DELETE FROM MenuAccess WHERE MenuAccessId = @MenuAccessID
		
    SET NOCOUNT OFF	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_DeleteMenuAccessRecord] TO [IRMAClientRole]
    AS [dbo];

