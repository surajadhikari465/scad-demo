CREATE PROCEDURE dbo.GetSystemDateCmd 
    @Date datetime OUTPUT
AS 
BEGIN
    SET @Date = GETDATE()
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDateCmd] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDateCmd] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDateCmd] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystemDateCmd] TO [IRMAReportsRole]
    AS [dbo];

