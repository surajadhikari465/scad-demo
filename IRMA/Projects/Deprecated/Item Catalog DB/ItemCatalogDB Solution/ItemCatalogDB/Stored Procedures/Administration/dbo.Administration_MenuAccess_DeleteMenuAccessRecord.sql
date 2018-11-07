IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_MenuAccess_DeleteMenuAccessRecord]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Administration_MenuAccess_DeleteMenuAccessRecord]
GO

CREATE PROCEDURE [dbo].[Administration_MenuAccess_DeleteMenuAccessRecord] 

	@MenuAccessID int
AS

BEGIN

    SET NOCOUNT ON
	
	DELETE FROM MenuAccess WHERE MenuAccessId = @MenuAccessID
		
    SET NOCOUNT OFF	

END

GO 