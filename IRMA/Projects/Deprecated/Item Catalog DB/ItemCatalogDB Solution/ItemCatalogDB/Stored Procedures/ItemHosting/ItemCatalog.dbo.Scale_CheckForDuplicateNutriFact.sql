if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_CheckForDuplicateNutriFact]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_CheckForDuplicateNutriFact]
GO

CREATE PROCEDURE dbo.Scale_CheckForDuplicateNutriFact 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	NutriFacts
WHERE 
	Description = @Description 
	AND NutriFactsID <> @ID
GO


