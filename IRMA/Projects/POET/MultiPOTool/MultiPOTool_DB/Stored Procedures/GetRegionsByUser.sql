-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:33:59 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetRegionsByUser;1 - Script Date: 11/3/2008 4:34:00 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRegionsByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRegionsByUser]
GO

CREATE PROCEDURE [dbo].[GetRegionsByUser]
	@UserID int

AS
BEGIN
	declare @GlobalBuyer bit
	select @GlobalBuyer = GlobalBuyer from Users where UserID = @UserID
	
	if @GlobalBuyer = 1
		begin
			select *
			from Regions
			where RegionName != 'Global' 
		end
	else
		begin
			select R.RegionName, R.RegionID
			from Regions R inner join Users U on U.RegionID = R.RegionID
			where U.UserID = @UserID
		end
END
