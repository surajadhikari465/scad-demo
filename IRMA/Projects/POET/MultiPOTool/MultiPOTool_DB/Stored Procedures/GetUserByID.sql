-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:35:08 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetUserByID;1 - Script Date: 11/3/2008 4:35:09 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUserByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUserByID]
GO
CREATE PROCEDURE dbo.GetUserByID
	
	(
	@UserID int
	)
	
AS
		SET NOCOUNT ON;
		
		SELECT     Users.UserName, Users.RegionID, Users.GlobalBuyer, Users.Administrator, Users.Active, Users.InsertDate, Users.Email, Users.CCEmail, 
		                      Regions.RegionName, Users.UserID
		FROM         Users INNER JOIN
		                      Regions ON Users.RegionID = Regions.RegionID
		WHERE     (Users.UserID = @UserID)
		
	RETURN


GO
