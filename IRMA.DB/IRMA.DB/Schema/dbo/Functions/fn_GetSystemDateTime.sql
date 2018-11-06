CREATE FUNCTION dbo.fn_GetSystemDateTime 
	()
RETURNS datetime
AS
BEGIN
    DECLARE @Result datetime, @CentralTimeZoneOffset int
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region
	
	SELECT @Result = DATEADD(HOUR, @CentralTimeZoneOffset, GETDATE())
	
	RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetSystemDateTime] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetSystemDateTime] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetSystemDateTime] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetSystemDateTime] TO [IRMAReportsRole]
    AS [dbo];

