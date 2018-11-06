if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateLabelType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateLabelType]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateLabelType 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_LabelType
WHERE 
	Description = @Description 
	AND Scale_LabelType_ID <> @ID
GO


