-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:31:35 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetAvailablePONumbersByUser;1 - Script Date: 11/3/2008 4:31:35 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUnusedPONumbersByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUnusedPONumbersByUser]
GO

CREATE PROCEDURE dbo.GetUnusedPONumbersByUser
	@UserID int

AS
BEGIN
	select 
		N.PONumber
		, R.RegionName
		, T.POTypeDescription
		, N.DateAssigned
	from PONumber N
		inner join POType T on N.POTypeID = T.POTypeID
		inner join Regions R on N.RegionID = R.RegionID
	where 
		N.AssignedByUserID = @UserID
		and N.PONumberID not in (select PONumberID from POHeader)
		and N.DeleteDate is null
	order by convert(char(20),N.DateAssigned,109) desc, N.PONumber asc
END

GO
