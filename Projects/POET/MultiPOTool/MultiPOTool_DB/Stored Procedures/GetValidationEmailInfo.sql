-- This script was created using WinSQL Professional
-- Timestamp: 12/2/2008 11:40:45 AM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetValidationEmailInfo;1 - Script Date: 12/2/2008 11:40:45 AM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetValidationEmailInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetValidationEmailInfo]
GO
CREATE PROCEDURE dbo.GetValidationEmailInfo
        
        
AS
         SET NOCOUNT ON 
         
         SELECT     ValidationQueue.UploadSessionHistoryID, UploadSessionHistory.FileName, UploadSessionHistory.UploadedDate, Users.UserName, isNUll(Users.Email,'') as Email, 
                               isNUll(Users.CCEmail,'') as CCEmail, Regions.RegionName, COUNT(POHeader.POHeaderID) AS NumberOfOrders, SUM(POHeader.OrderItemCount) AS NumberOfItems
         FROM         ValidationQueue INNER JOIN
                               UploadSessionHistory ON ValidationQueue.UploadSessionHistoryID = UploadSessionHistory.UploadSessionHistoryID INNER JOIN
                               Users ON UploadSessionHistory.UploadUserID = Users.UserID INNER JOIN
                               POHeader ON UploadSessionHistory.UploadSessionHistoryID = POHeader.UploadSessionHistoryID INNER JOIN
                               Regions ON Users.RegionID = Regions.RegionID
         WHERE     (ValidationQueue.ProcessingFlag = 0)
         GROUP BY ValidationQueue.UploadSessionHistoryID, UploadSessionHistory.FileName, UploadSessionHistory.UploadedDate, Users.UserName, Users.Email, 
                               Users.CCEmail, Regions.RegionName
         
        RETURN


GO
