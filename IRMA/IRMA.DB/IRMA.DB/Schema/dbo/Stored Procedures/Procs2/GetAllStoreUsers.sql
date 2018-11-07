CREATE PROCEDURE dbo.GetAllStoreUsers 
	@Store_No int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT User_ID, UserName, FullName
    FROM Users (NOLOCK)
    WHERE ISNULL(Telxon_Store_Limit, @Store_No) = @Store_No
    ORDER BY FullName

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStoreUsers] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStoreUsers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStoreUsers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllStoreUsers] TO [IRMAReportsRole]
    AS [dbo];

