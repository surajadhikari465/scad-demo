-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:32:53 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetExceptionItemsByUploadSession;1 - Script Date: 11/3/2008 4:32:53 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExceptionItemsByUploadSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExceptionItemsByUploadSession]
GO

CREATE PROCEDURE dbo.GetExceptionItemsByUploadSession
	@UploadSessionHistoryID int

AS
BEGIN


select
	U.UserName
	, rtrim(cast(S.UploadSessionHistoryID as char(10))) + ': ' + isnull(FileName,'...') as SessionName
	, S.UploadedDate
	, H.VendorName
	, H.VendorPSNumber
	, R.RegionName
	, N.PONumber
	, H.BusinessUnit
	, H.StoreAbbr
	, I.Identifier
	, I.VendorItemNumber
	, H.Subteam
	, ItemBrand
	, I.ItemDescription
	, E.ExceptionDescription
	, H.ValidationAttemptDate
	from
	POItemException IE
	inner join Exception E on IE.ExceptionID = E.ExceptionID
	inner join POItem I on IE.POItemID = I.POItemID
	inner join POHeader H on I.POHeaderID = H.POHeaderID
	inner join PONumber N on H.PONumberID = N.PONumberID
	inner join Regions R on H.RegionID = R.RegionID
	inner join UploadSessionHistory S on H.UploadSessionHistoryID = S.UploadSessionHistoryID
	inner join Users U on S.UploadUserID = U.UserID
	where 
	isnull(S.ValidationSuccessful,0) = 0
	and isnull(H.Expired,0) = 0 
	and S.UploadSessionHistoryID = @UploadSessionHistoryID
	order by E.ExceptionDescription, I.Identifier

END

GO
