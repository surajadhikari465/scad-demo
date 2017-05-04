if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_DeleteEatBy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_DeleteEatBy]
GO


CREATE PROCEDURE dbo.Scale_DeleteEatBy
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_EatBy 
	WHERE 
		Scale_EatBy_ID = @ID
END
GO

