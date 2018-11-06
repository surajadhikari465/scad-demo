IF EXISTS (SELECT * from dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetAgeCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetAgeCode]
GO

CREATE PROCEDURE [dbo].[GetAgeCode]
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @AgeCode TABLE (
	AgeCode		INT,
	AgeCodeDesc	VARCHAR(2) )
	
	INSERT INTO @AgeCode (AgeCode, AgeCodeDesc) VALUES (SPACE(2), SPACE(2))
	INSERT INTO @AgeCode (AgeCode, AgeCodeDesc) VALUES (1, '18')
	INSERT INTO @AgeCode (AgeCode, AgeCodeDesc) VALUES (2, '21')
	
	SELECT AgeCodeDesc, AgeCode FROM @AgeCode  
	
END
GO