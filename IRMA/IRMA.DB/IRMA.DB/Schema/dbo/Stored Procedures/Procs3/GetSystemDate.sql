CREATE PROCEDURE dbo.GetSystemDate 
AS 
	DECLARE @CentralTimeZoneOffset int
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

	SELECT DATEADD(HOUR, @CentralTimeZoneOffset, GETDATE()) As SystemDate
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDate] TO [IRMAReportsRole]
    AS [dbo];

