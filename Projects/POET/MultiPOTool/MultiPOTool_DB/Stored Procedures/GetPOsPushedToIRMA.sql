
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOsPushedToIRMA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PushPOsToIRMA]
GO

CREATE PROCEDURE [dbo].[GetPOsPushedToIRMA]
	@UserID int,
	@Top int = 100,
	@StartDate datetime = null,
	@EndDate as datetime = null,
	@Store as nvarchar(10) = null,
	@Vendor as int = null,
	@Subteam as int= null,
	@POType as int = 1 -- 1 = PO uploaded by me, 2= PO pushed by me
AS
BEGIN
	IF @POType = 1
		BEGIN
			SELECT TOP (@Top)
				U.UploadSessionHistoryID
				, H.POHeaderID
				, H.RegionID
				, R.RegionName
				, N.PONumber
				, CASE WHEN H.IRMAPONumber IS NULL THEN 0 ELSE H.IRMAPONumber END as IRMAPONumber
				, H.BusinessUnit
				, H.StoreAbbr
				, H.Subteam
				, H.VendorName
				, H.VendorPSNumber
				, H.OrderItemCount
				, H.TotalPOCost
				, H.ExpectedDate
				, H.CreatedDate
				, H.PushedToIRMADate
				, H.Notes
				, H.AutoPushDate
				, Uru.UserName AS UploadedBy
				, Urp.UserName AS PushedBy	 
			FROM
				POHeader H
				INNER JOIN Regions R on H.RegionID = R.RegionID
				INNER JOIN PONumber N on H.PONumberID = N.PONumberID
				INNER JOIN UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
				INNER JOIN Users Uru ON U.UploadUserID = Uru.UserID
				INNER JOIN Users Urp ON H.PushedByUserID = Urp.UserID
			WHERE U.ValidationSuccessful = 1
				AND ISNULL(H.Expired,0) = 0
				AND H.DeletedDate is null
				AND H.PushedToIRMADate is not null 
				AND U.UploadUserID = CONVERT(VARCHAR, @UserID)
				AND ((H.IRMAVendor_ID = @Vendor) or (@Vendor is null)) 
				AND  ((H.subteam = @Subteam) or (@Subteam is null))
				AND ((H.StoreAbbr = @Store) or (@Store is null))
				AND  H.PushedToIRMADate BETWEEN ISNULL(@StartDate,H.PushedToIRMADate) AND
					ISNULL(@EndDate,H.PushedToIRMADate)
			order by H.PushedToIRMADate desc
		END
	ELSE
		IF @POType = 2
			BEGIN
				SELECT TOP (@Top)
					U.UploadSessionHistoryID
					, H.POHeaderID
					, H.RegionID
					, R.RegionName
					, N.PONumber
					, CASE WHEN H.IRMAPONumber IS NULL THEN 0 ELSE H.IRMAPONumber END as IRMAPONumber
					, H.BusinessUnit
					, H.StoreAbbr
					, H.Subteam
					, H.VendorName
					, H.VendorPSNumber
					, H.OrderItemCount
					, H.TotalPOCost
					, H.ExpectedDate
					, H.CreatedDate
					, H.PushedToIRMADate
					, H.Notes
					, H.AutoPushDate
					, Uru.UserName AS UploadedBy
					, Urp.UserName AS PushedBy		 
				FROM
					POHeader H
					INNER JOIN Regions R on H.RegionID = R.RegionID
					INNER JOIN PONumber N on H.PONumberID = N.PONumberID
					INNER JOIN UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
					INNER JOIN Users Uru ON U.UploadUserID = Uru.UserID
					INNER JOIN Users Urp ON H.PushedByUserID = Urp.UserID
				WHERE U.ValidationSuccessful = 1
					AND ISNULL(H.Expired,0) = 0
					AND H.DeletedDate is null
					AND H.PushedToIRMADate is not null 
					AND H.PushedByUserID = CONVERT(VARCHAR, @UserID) 
					AND ((H.IRMAVendor_ID = @Vendor) or (@Vendor is null)) 
					AND  ((H.subteam = @Subteam) or (@Subteam is null))
					AND ((H.StoreAbbr = @Store) or (@Store is null))
					AND  H.PushedToIRMADate BETWEEN ISNULL(@StartDate,H.PushedToIRMADate) AND
					ISNULL(@EndDate,H.PushedToIRMADate)
				ORDER BY H.PushedToIRMADate DESC
			END
END
GO



