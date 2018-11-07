-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:36:04 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: InsertUser;1 - Script Date: 11/3/2008 4:36:04 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertUser]
GO
CREATE PROCEDURE dbo.InsertUser
	
	(
	@UserName varchar(50),
	@RegionID int,
	@GlobalBuyer bit,
	@Administrator bit,
	@Active bit, 
	@Email varchar(100),
	@CCEmail varchar(100), 
	@Output int OUTPUT 
	)
	
AS
		SET NOCOUNT ON;
		
		INSERT INTO Users
		                      (UserName, RegionID, GlobalBuyer, Administrator, Active, Email, CCEmail)
		VALUES     (@UserName,@RegionID,@GlobalBuyer,@Administrator,@Active,@Email,@CCEmail)
		
	set @Output =  scope_identity()
	RETURN


GO
