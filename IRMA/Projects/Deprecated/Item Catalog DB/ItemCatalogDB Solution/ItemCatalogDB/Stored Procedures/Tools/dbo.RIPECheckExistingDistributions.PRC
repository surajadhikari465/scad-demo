
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RIPECheckExistingDistributions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RIPECheckExistingDistributions]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


