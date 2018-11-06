if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOsPushedToIRMA-temp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PushPOsToIRMA]
GO


ALTER PROCEDURE [dbo].[GetPOsPushedToIRMA-temp]
@UserID int,
	@Top int = 100
	,@StartDate datetime = null
	,@EndDate as datetime = null
	,@Store as nvarchar(10) = null
	,@Vendor as int = null
	,@Subteam as int= null
	
AS


DECLARE @sql as nvarchar(max),
@sql2 as nvarchar(max),
  @IRMAServer varchar(6),
          @IRMADatabase varchar(50),
          @DBString varchar(max),
           @PONumberQuote varchar(25),
           @PONO as int,
           @RegionID as int
	
BEGIN

	
SELECT TOP (@Top)
	U.UploadSessionHistoryID
	, H.POHeaderID
	, H.RegionID
	, R.RegionName
	, N.PONumber
	,CASE WHEN H.IRMAPONumber IS NULL THEN 0 ELSE H.IRMAPONumber END as IRMAPONumber
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
	--, H.ConfirmedInIRMADate
	, H.Notes
	
	
	 --into #TEMPMySelect
	 
	from
	POHeader H
	inner join Regions R on H.RegionID = R.RegionID
	inner join PONumber N on H.PONumberID = N.PONumberID
	inner join UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
	where U.ValidationSuccessful = 1
	and isnull(H.Expired,0) = 0
	and H.DeletedDate is null
	and H.PushedToIRMADate is not null 
	and U.UploadUserID =CONVERT(VARCHAR, @UserID) 
	
	and ((H.IRMAVendor_ID = @Vendor) or (@Vendor is null)) 
	and  ((H.subteam = @Subteam) or (@Subteam is null))
	and ((H.StoreAbbr = @Store) or (@Store is null))
	and  H.PushedToIRMADate BETWEEN ISNULL(@StartDate,H.PushedToIRMADate) AND
	ISNULL(@EndDate,H.PushedToIRMADate)
	order by H.PushedToIRMADate desc
	
END


