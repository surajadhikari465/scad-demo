if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateExtraText]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateExtraText]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateExtraText 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_ExtraText
WHERE 
	Description = @Description 
	AND Scale_ExtraText_ID <> @ID
GO


