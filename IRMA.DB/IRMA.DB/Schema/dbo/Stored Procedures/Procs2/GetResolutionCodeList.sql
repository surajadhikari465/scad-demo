CREATE PROCEDURE [dbo].[GetResolutionCodeList]
@ActiveOnly bit
AS 
BEGIN
	SET NOCOUNT ON

	IF (@ActiveOnly = 1)
		BEGIN
			SELECT * FROM ResolutionCodes WHERE Active = 1
		END
	ELSE
		BEGIN
			SELECT * FROM ResolutionCodes
		END			
	
	SET NOCOUNT OFF 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetResolutionCodeList] TO [IRMAClientRole]
    AS [dbo];

