
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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAgeCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAgeCode] TO [IRSUser]
    AS [dbo];

