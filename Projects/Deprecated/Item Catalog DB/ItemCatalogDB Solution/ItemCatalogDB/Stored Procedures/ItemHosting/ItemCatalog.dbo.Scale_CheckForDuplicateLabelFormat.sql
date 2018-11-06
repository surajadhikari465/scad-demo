if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateLabelFormat]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateLabelFormat]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateLabelFormat 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_LabelFormat
WHERE 
	Description = @Description 
	AND Scale_LabelFormat_ID <> @ID
GO


