IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_MenuAccess_UpdateMenuAccessRecord]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Administration_MenuAccess_UpdateMenuAccessRecord]
GO

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