-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:29:15 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: DeletePOs;1 - Script Date: 11/3/2008 4:29:15 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeletePOs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeletePOs]
GO

CREATE PROCEDURE dbo.DeletePOs
	@UserID int,
	@POHeaderID int

AS
BEGIN

	update POHeader
	set DeletedDate = getdate()
	from
	POHeader H
	inner join Regions R on H.RegionID = R.RegionID
	inner join PONumber N on H.PONumberID = N.PONumberID
	inner join UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
	where 
	U.UploadUserID = @UserID
	and H.POHeaderID = @POHeaderID

END

GO
