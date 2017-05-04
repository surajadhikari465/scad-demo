IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetAllStoreUsers' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetAllStoreUsers
GO

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

