-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:40:00 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetAllUsers;1 - Script Date: 11/3/2008 4:40:00 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAllUsers]
GO
CREATE PROCEDURE dbo.GetAllUsers
	/*
	(
	@parameter1 int = 5,
	@parameter2 datatype OUTPUT
	)
	*/
AS
	SET NOCOUNT ON 
	
	SELECT     Users.UserID, Users.UserName, Users.RegionID, Users.GlobalBuyer, Users.Administrator, Users.Active, Users.InsertDate, Users.Email, 
	                      Users.CCEmail, Regions.RegionName
	FROM         Users INNER JOIN
	                      Regions ON Users.RegionID = Regions.RegionID
	                      Order by Users.UserID Desc
	RETURN


GO
