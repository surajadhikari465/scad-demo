IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_UserAdmin_GetUserList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_UserAdmin_GetUserList]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


