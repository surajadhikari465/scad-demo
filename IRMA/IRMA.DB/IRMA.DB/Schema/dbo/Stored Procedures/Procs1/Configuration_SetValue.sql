CREATE PROCEDURE dbo.Configuration_SetValue
	@ApplicationId varchar(255),
	@EnvironmentId varchar(255),
	@Key varchar(1024),
	@Value varchar(1024)
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (
					SELECT	*
					FROM	ApplicationConfiguration
					WHERE	ApplicationId = @ApplicationId AND
							EnvironmentId = @EnvironmentId AND
							[Key] = @Key
			   )

			UPDATE	ApplicationConfiguration
			SET		[value] = @value
			WHERE	ApplicationId = @ApplicationId AND
					EnvironmentId = @EnvironmentId AND
					[Key] = @Key
	ELSE
			INSERT INTO ApplicationConfiguration
			(
				ApplicationId, 
				EnvironmentId, 
				[Key], 
				[Value]
			)
			VALUES
			(
				@ApplicationId, 
				@EnvironmentId,
				@Key,
				@Value
			)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_SetValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_SetValue] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_SetValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_SetValue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Configuration_SetValue] TO [IRMAReportsRole]
    AS [dbo];

