if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateTare]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateTare]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateTare 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_Tare
WHERE 
	Description = @Description 
	AND Scale_Tare_ID <> @ID
GO


