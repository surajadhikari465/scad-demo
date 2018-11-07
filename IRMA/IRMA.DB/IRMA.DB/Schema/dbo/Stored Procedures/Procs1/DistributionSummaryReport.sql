CREATE PROCEDURE dbo.DistributionSummaryReport
@StartDate DateTime,
@EndDate DateTime,
@SendLocation_ID int,
@ReceiveLocation_ID int
AS 

SELECT SubTeam.SubTeam_No, SubTeam.SubTeam_Name, 
       SUM(OrderItem.ReceivedItemCost) AS Cost, 
       SUM(OrderItem.ReceivedItemFreight+OrderItem.ReceivedItemCost) AS ExtCost
FROM SubTeam INNER JOIN ( 
       Item INNER JOIN (
         OrderItem INNER JOIN OrderHeader ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
       ) ON (OrderItem.Item_Key = Item.Item_Key)
     ) ON (SubTeam.SubTeam_No = OrderHeader.Transfer_SubTeam)
WHERE CloseDate >= @StartDate AND CloseDate <= @EndDate AND ReceiveLocation_ID = @ReceiveLocation_ID AND Vendor_ID = @SendLocation_ID
GROUP BY SubTeam.SubTeam_No, SubTeam.SubTeam_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistributionSummaryReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistributionSummaryReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistributionSummaryReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistributionSummaryReport] TO [IRMAReportsRole]
    AS [dbo];

