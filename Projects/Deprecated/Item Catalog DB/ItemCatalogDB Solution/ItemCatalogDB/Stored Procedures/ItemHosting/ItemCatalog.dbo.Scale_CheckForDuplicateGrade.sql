if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateGrade]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateGrade]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateGrade 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_Grade
WHERE 
	Description = @Description 
	AND Scale_Grade_ID <> @ID
GO


