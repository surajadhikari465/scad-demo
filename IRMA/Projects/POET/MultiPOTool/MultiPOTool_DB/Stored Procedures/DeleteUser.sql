-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:29:31 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: DeleteUser;1 - Script Date: 11/3/2008 4:29:31 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteUser]
GO
CREATE PROCEDURE dbo.DeleteUser
	
	(
	@UserID int
	)
	
AS
		SET NOCOUNT ON;
		
		-- Change the delete to update .... deleting users with existing upload sessions causes foriegn key constraint errors - Alex Z 01/30/2009
		UPDATE Users
		SET ACTIVE = 0
		WHERE (UserID = @UserID)
		
	RETURN


GO
