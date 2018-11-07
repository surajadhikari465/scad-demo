CREATE PROCEDURE dbo.SecurityGetApplications
	@AppStatus as int  
AS
BEGIN
    SET NOCOUNT ON

	IF @AppStatus = 2

		BEGIN
			SELECT ApplicationID, [Name] FROM Applications
		END

	ELSE

		BEGIN
			SELECT ApplicationID, [Name] FROM Applications
			WHERE [Enabled] = @AppStatus
		END
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SecurityGetApplications] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SecurityGetApplications] TO [IRMAClientRole]
    AS [dbo];

