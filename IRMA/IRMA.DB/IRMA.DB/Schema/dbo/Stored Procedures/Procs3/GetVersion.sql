CREATE PROCEDURE dbo.GetVersion
	@ApplicationName varchar(50)
AS 

	SELECT * FROM Version WHERE ApplicationName = @ApplicationName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVersion] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVersion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVersion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVersion] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVersion] TO [IRMASLIMRole]
    AS [dbo];

