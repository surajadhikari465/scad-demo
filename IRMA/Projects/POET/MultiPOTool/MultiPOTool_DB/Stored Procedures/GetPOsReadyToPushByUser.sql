-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:33:31 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetPOsReadyToPushByUser;1 - Script Date: 11/3/2008 4:33:31 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOsReadyToPushByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOsReadyToPushByUser]
GO

CREATE PROCEDURE dbo.GetPOsReadyToPushByUser
	@UserID int

AS
BEGIN

	select
	U.UploadSessionHistoryID
	, H.POHeaderID
	, R.RegionName
	, N.PONumber
	, H.BusinessUnit
	, H.StoreAbbr
	, H.Subteam
	, H.VendorName
	, H.VendorPSNumber
	, H.OrderItemCount
	, H.TotalPOCost
	, H.ExpectedDate
	, H.AutoPushDate
	, H.Notes
	from
	POHeader H
	inner join Regions R on H.RegionID = R.RegionID
	inner join PONumber N on H.PONumberID = N.PONumberID
	inner join UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
	where U.ValidationSuccessful = 1
	and isnull(H.Expired,0) = 0
	and H.DeletedDate is null
	and H.PushedToIRMADate is null
		and H.POHeaderID not in (select POHeaderID from PushToIRMAQueue)
	and U.UploadUserID = @UserID

END

GO
