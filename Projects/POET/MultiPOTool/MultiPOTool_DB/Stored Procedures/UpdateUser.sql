-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:36:34 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: UpdateUser;1 - Script Date: 11/3/2008 4:36:34 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateUser]
GO
Create PROCEDURE dbo.UpdateUser
	
	(
	@UserID int,
	@UserName varchar(50),
	@RegionID int,
	@GlobalBuyer bit,
	@Administrator bit,
	@Active bit, 
	@Email varchar(100),
	@CCEmail varchar(100)
	)
	
AS
		SET NOCOUNT ON;
		
		UPDATE    Users
		SET              UserName = @UserName, RegionID = @RegionID, GlobalBuyer = @GlobalBuyer, Administrator = @Administrator, Active = @Active, 
		                      InsertDate = getdate(), Email = @Email, CCEmail = @CCEmail
		                      WHERE UserID = @UserID
		
	RETURN


GO
