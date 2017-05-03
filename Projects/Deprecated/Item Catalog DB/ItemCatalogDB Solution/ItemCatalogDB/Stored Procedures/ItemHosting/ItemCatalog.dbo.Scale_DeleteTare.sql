if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_DeleteTare]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_DeleteTare]
GO


CREATE PROCEDURE dbo.Scale_DeleteTare
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_Tare 
	WHERE 
		Scale_Tare_ID = @ID
END
GO

