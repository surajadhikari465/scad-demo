CREATE FUNCTION [infor].[GetValidationErrorWithTwoValues]
(
	@errorCode nvarchar(50),
	@appId nvarchar(50),
	@propertyValue1 nvarchar(50),
	@propertyValue2 nvarchar(50)
)
RETURNS nvarchar(255)
AS
BEGIN
	DECLARE @errorDetails nvarchar(255)

	SELECT @errorDetails = REPLACE(REPLACE(ErrorDetails, '{PropertyValue1}', @propertyValue1), '{PropertyValue2}', @propertyValue2)
	FROM infor.Errors e
	WHERE e.AppId = @appId 
		AND e.ErrorCode = @errorCode

	RETURN @errorDetails
END
