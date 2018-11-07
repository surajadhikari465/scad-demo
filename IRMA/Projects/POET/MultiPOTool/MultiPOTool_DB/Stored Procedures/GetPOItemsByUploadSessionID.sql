-- This script was created using WinSQL Professional
-- Timestamp: 11/6/2008 11:15:50 AM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetPOItemsByUploadSessionID;1 - Script Date: 11/6/2008 11:15:50 AM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOItemsByUploadSessionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOItemsByUploadSessionID]
GO
CREATE PROCEDURE dbo.GetPOItemsByUploadSessionID
        
        (
        @UploadSessionID int
        )
        
AS
        SET NOCOUNT ON  
        
        SELECT     POItem.Identifier, POItem.VendorItemNumber, POItem.ItemBrand, POItem.ItemDescription, POItem.FreeQuantity, POItem.OrderQuantity, 
                      POItem.POHeaderID, POItem.DiscountType, POItem.DiscountAmount
FROM         POItem INNER JOIN
                      POHeader ON POItem.POHeaderID = POHeader.POHeaderID INNER JOIN
                      UploadSessionHistory ON POHeader.UploadSessionHistoryID = UploadSessionHistory.UploadSessionHistoryID
WHERE     (UploadSessionHistory.UploadSessionHistoryID = @UploadSessionID)
                order by POItem.POHeaderID
        
        RETURN


GO
