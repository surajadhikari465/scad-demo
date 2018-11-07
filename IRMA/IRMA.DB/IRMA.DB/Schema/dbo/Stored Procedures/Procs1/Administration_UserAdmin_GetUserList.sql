CREATE PROCEDURE dbo.Administration_UserAdmin_GetUserList
AS
BEGIN
SELECT [User_ID]
      ,[UserName]
      ,[FullName]
      --,[EMail]
      ,[AccountEnabled]
      --,[Phone_Number]
      ,(SELECT Title_Desc FROM Title WHERE Title_ID = [Users].[Title]) As [Title]
  FROM [Users]
  ORDER BY [UserName]
  END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_GetUserList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_GetUserList] TO [IRMAClientRole]
    AS [dbo];

