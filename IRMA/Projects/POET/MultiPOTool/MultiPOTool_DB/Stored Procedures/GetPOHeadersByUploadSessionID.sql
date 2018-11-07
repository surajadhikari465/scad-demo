-- This script was created using WinSQL Professional
-- Timestamp: 11/6/2008 11:04:05 AM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetPOHeadersByUploadSessionID;1 - Script Date: 11/6/2008 11:04:05 AM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOHeadersByUploadSessionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOHeadersByUploadSessionID]
GO
CREATE PROCEDURE dbo.GetPOHeadersByUploadSessionID

        (
        @UploadSessionID int
        )

AS
        SET NOCOUNT ON


        SELECT     PONumber.PONumber, POHeader.VendorPSNumber, POHeader.BusinessUnit, POHeader.Subteam, POHeader.ExpectedDate, POHeader.AutoPushDate,
                      POHeader.OrderItemCount, POHeader.POHeaderID, POHeader.Notes, POHeader.DiscountType, POHeader.DiscountAmount
FROM         POHeader INNER JOIN
                      UploadSessionHistory ON POHeader.UploadSessionHistoryID = UploadSessionHistory.UploadSessionHistoryID INNER JOIN
                      PONumber ON POHeader.PONumberID = PONumber.PONumberID
where UploadSessionHistory.UploadSessionHistoryID = @UploadSessionID
        order by POHeader.POHeaderID

        RETURN



GO

