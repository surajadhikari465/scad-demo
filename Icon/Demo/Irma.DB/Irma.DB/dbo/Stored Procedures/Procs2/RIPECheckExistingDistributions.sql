CREATE PROCEDURE dbo.RIPECheckExistingDistributions
    @DistDate varchar(20),
    @LocationID int,
    @CustomerID int
AS

SELECT IRS_OH.IRSOrderHeaderID, RIPE_OH.OrderDescription, RIPE_Cust.CompanyName
FROM Recipe..IRSOrderHistory IRS_OH(nolock)
INNER JOIN 
    Recipe..Orders1 RIPE_OH (nolock)
    on RIPE_OH.Orders1ID = IRS_OH.RIPEOrders1ID
INNER JOIN 
    Recipe..Distribution Distribution (nolock)
    ON Distribution.DistributionDate = IRS_OH.DistributionDate AND Distribution.Orders1ID = IRS_OH.RIPEOrders1ID
INNER JOIN
    Recipe..Customer RIPE_Cust (nolock)
    on RIPE_Cust.CustomerID = RIPE_OH.CustomerID
WHERE Distribution.DistributionDate = @DistDate 
      and RIPE_OH.LocationID = @LocationID 
      and RIPE_OH.CustomerID = isNull(@CustomerID, RIPE_OH.CustomerID)
      and not(IRS_OH.ImportDateTime is null)
GROUP BY IRS_OH.IRSOrderHeaderID, RIPE_OH.OrderDescription, RIPE_Cust.CompanyName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPECheckExistingDistributions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPECheckExistingDistributions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPECheckExistingDistributions] TO [IRMAReportsRole]
    AS [dbo];

