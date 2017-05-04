if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_DeleteLabelType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_DeleteLabelType]
GO


CREATE PROCEDURE dbo.Scale_DeleteLabelType
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_LabelType 
	WHERE 
		Scale_LabelType_ID = @ID
END
GO

