if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_DeleteGrade]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_DeleteGrade]
GO


CREATE PROCEDURE dbo.Scale_DeleteGrade
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_Grade 
	WHERE 
		Scale_Grade_ID = @ID
END
GO

