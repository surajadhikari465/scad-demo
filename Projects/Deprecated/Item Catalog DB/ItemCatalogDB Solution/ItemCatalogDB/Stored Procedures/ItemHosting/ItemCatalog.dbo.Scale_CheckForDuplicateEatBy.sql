if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateEatBy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateEatBy]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateEatBy 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_EatBy
WHERE 
	Description = @Description 
	AND Scale_EatBy_ID <> @ID
GO


