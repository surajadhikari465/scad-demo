IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_MenuAccess_AddMenuAccessRecord]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Administration_MenuAccess_AddMenuAccessRecord]
GO

CREATE PROCEDURE [dbo].[Administration_MenuAccess_AddMenuAccessRecord] 

	@MenuName varchar(50),
	@IsVisible bit
AS

BEGIN

    SET NOCOUNT ON
	
	INSERT INTO MenuAccess (MenuName, Visible) VALUES (@MenuName, @IsVisible)
		
    SET NOCOUNT OFF	

END

GO  