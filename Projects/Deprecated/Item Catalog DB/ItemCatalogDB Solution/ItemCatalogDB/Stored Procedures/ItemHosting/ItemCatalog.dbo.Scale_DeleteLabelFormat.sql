if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_DeleteLabelFormat]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_DeleteLabelFormat]
GO


CREATE PROCEDURE dbo.Scale_DeleteLabelFormat
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_LabelFormat 
	WHERE 
		Scale_LabelFormat_ID = @ID
END
GO

