CREATE FUNCTION [infor].[GetValidationError]
(
	@errorCode nvarchar(50),
	@appId nvarchar(50),
	@propertyValue nvarchar(50)
)
RETURNS nvarchar(255)
AS
BEGIN
	DECLARE @errorDetails nvarchar(255)

	SELECT @errorDetails = REPLACE(ErrorDetails, '{PropertyValue}', @propertyValue) 
	FROM infor.Errors e
	WHERE e.AppId = @appId 
		AND e.ErrorCode = @errorCode

	RETURN @errorDetails
END
