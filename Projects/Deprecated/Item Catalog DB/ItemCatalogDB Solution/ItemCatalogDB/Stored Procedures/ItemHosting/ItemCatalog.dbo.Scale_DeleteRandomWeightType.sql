if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_DeleteRandomWeightType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_DeleteRandomWeightType]
GO


CREATE PROCEDURE dbo.Scale_DeleteRandomWeightType
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_RandomWeightType 
	WHERE 
		Scale_RandomWeightType_ID = @ID
END
GO

