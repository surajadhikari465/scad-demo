if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateLabelStyle]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateLabelStyle]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateLabelStyle 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_LabelStyle
WHERE 
	Description = @Description 
	AND Scale_LabelStyle_ID <> @ID
GO


