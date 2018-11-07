Create PROCEDURE [dbo].[Configuration_GetValue]
	@ApplicationId varchar(255),
	@EnvironmentId varchar(255),
	@Key varchar(1024)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM ApplicationConfiguration
	WHERE	ApplicationId = @ApplicationId AND
			EnvironmentId = @EnvironmentId AND
			[Key] = @Key

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_GetValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_GetValue] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_GetValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_GetValue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_GetValue] TO [IRMAReportsRole]
    AS [dbo];

