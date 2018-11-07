-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:35:27 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetUserIDByUserName;1 - Script Date: 11/3/2008 4:35:27 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUserIDByUserName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUserIDByUserName]
GO
Create PROCEDURE dbo.GetUserIDByUserName
	
	(
	@UserName varchar(50)
	)
	
AS
		SET NOCOUNT ON;
		
		SELECT     UserID
		FROM         Users
		WHERE     (UserName = @Username)
		
	RETURN


GO
