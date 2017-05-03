CREATE PROCEDURE [dbo].[GetResolutionCode]
	@ResolutionCodeID AS int 	
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT *
	FROM [dbo].[ResolutionCodes]
	WHERE [ResolutionCodeID] = @ResolutionCodeID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetResolutionCode] TO [IRMAClientRole]
    AS [dbo];

