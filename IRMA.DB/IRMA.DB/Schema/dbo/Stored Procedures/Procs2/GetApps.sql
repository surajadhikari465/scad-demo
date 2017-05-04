CREATE PROCEDURE dbo.GetApps
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT App.AppID, App.AppName
    FROM App
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetApps] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetApps] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetApps] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetApps] TO [IRMAReportsRole]
    AS [dbo];

