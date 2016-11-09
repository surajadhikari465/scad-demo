CREATE VIEW [dbo].[DC_ITEMHISTORY]
AS
SELECT dbo.SubTeam.SubTeam_Name, NULL AS ICHQuantity, dbo.ItemHistory.Store_No, dbo.ItemHistory.Item_Key, dbo.ItemHistory.DateStamp, 
               dbo.ItemHistory.Quantity, dbo.ItemHistory.Weight, dbo.ItemHistory.Cost, dbo.ItemHistory.ExtCost, dbo.ItemHistory.Retail, dbo.ItemHistory.Adjustment_ID, 
               dbo.ItemHistory.AdjustmentReason, dbo.ItemHistory.CreatedBy, dbo.ItemHistory.SubTeam_No, dbo.ItemHistory.Insert_Date, dbo.ItemHistory.ItemHistoryID, 
               dbo.ItemHistory.OrderItem_ID, NULL AS PackSize, dbo.OrderItem.OrderHeader_ID, dbo.OrderItem.Total_Weight, dbo.OrderItem.QuantityReceived, 
               dbo.OrderItem.QuantityOrdered, dbo.Store.Store_Name, dbo.Users.UserName
FROM  dbo.ItemHistory INNER JOIN
               dbo.SubTeam ON dbo.ItemHistory.SubTeam_No = dbo.SubTeam.SubTeam_No INNER JOIN
               dbo.OrderItem ON dbo.ItemHistory.OrderItem_ID = dbo.OrderItem.OrderItem_ID INNER JOIN
               dbo.Store ON dbo.ItemHistory.Store_No = dbo.Store.Store_No INNER JOIN
               dbo.Users ON dbo.Store.Store_No = dbo.Users.Telxon_Store_Limit
GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_ITEMHISTORY] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_ITEMHISTORY] TO [IRMADCAnalysisRole]
    AS [dbo];

