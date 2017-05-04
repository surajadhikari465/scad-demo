if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateRandomWeightType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateRandomWeightType]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateRandomWeightType 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_RandomWeightType
WHERE 
	Description = @Description 
	AND Scale_RandomWeightType_ID <> @ID
GO


