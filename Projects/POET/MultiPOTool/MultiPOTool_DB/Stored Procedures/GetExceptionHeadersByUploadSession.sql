if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExceptionHeadersByUploadSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExceptionHeadersByUploadSession]
GO

CREATE  PROCEDURE [dbo].[GetExceptionHeadersByUploadSession]
	@UploadSessionHistoryID int

AS
BEGIN
select
	I.Identifier
	, ItemBrand
	, I.ItemDescription
	, H.Subteam
	, H.VendorName
	, R.RegionName
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
GROUP BY 
	I.Identifier
	, ItemBrand
	, I.ItemDescription
	, H.Subteam
	, H.VendorName
	, R.RegionName
	order by 
	I.Identifier
	, ItemBrand
	, I.ItemDescription
	, H.Subteam
	, H.VendorName
	, R.RegionName
END

GO

