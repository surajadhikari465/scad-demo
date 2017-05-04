
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RIPEGetDistributions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RIPEGetDistributions]
GO


CREATE PROCEDURE dbo.RIPEGetDistributions
    @DistributionDate varchar(20),
    @LocationID int,
    @CustomerID int
AS

SELECT DISTINCT Item.Item_Key, LD.InventorySubTeam As Transfer_SubTeam, CD.InventorySubTeam As Transfer_To_SubTeam, 
                Distribution.QuantityRequested, Distribution.QuantityShipped, Unit.UnitAbbr, Distribution.Orders1ID 
FROM Recipe..Customer Customer
INNER JOIN 
    Recipe..Distribution Distribution 
    ON Distribution.CustomerID = Customer.CustomerID
INNER JOIN
    Recipe..CustomerDept CD
    ON CD.CustomerID = Distribution.CustomerID
       AND CD.DepartmentID = Distribution.DepartmentID
INNER JOIN 
    Recipe..Recipe Recipe 
    ON Recipe.RecipeID = Distribution.RecipeID
INNER JOIN 
    Recipe..Location Location 
    ON Location.LocationID = Distribution.LocationID
INNER JOIN
    Recipe..LocationDepartment LD
    ON LD.LocationID = Distribution.LocationID
       AND LD.DepartmentID = Distribution.LocationDepartmentID
INNER JOIN 
    Recipe..Unit Unit 
    ON Distribution.UnitID = Unit.UnitID
INNER JOIN 
    Vendor 
    ON Vendor.Store_No = Location.Store_No
INNER JOIN 
    ItemVendor 
    ON ItemVendor.Vendor_ID = Vendor.Vendor_ID AND ItemVendor.Item_ID = Recipe.PLU
INNER JOIN 
    Item 
    --Item.Discontinue_Item is not being used correctly here.  Ripe Distributions are always from a facility and
    --Discontinued items are alowed to be ordered from Facilities.  This was added because Facilities were using
    --the Discontinued flag incorectly.  They should have been using the Not Avalible flag (for Temp "disco" items) 
    --or Delete the item and remove the ItemVendor Reference (for perminant "disco" items).
    ON Item.Item_Key = ItemVendor.Item_Key and Item.Deleted_Item = 0 and 
		--Discontinue_Item = 0 and 
		Not_Available = 0 and 
		dbo.fn_GetDiscontinueStatus(Item.Item_Key,NULL,NULL) = 0
		--isnull(ItemVendor.DeleteDate, dateadd(day, 1, getdate())) > getdate()
WHERE Customer.CustomerID = @CustomerID and Location.LocationID = @LocationID AND Distribution.DistributionDate = @DistributionDate AND 
      (ItemVendor.Item_Key IS NOT NULL AND ISNULL(Recipe.PLU,'') > '') AND 
      Distribution.QuantityRequested > 0 AND
      NOT EXISTS(SELECT R_IRSOrdHist.RipeOrders1ID, R_IRSOrdHist.DistributionDate 
                 FROM recipe..IRSOrderHistory R_IRSOrdHist
                     INNER JOIN
                        Recipe..Orders1 R_ord
                        ON R_Ord.orders1ID = R_IRSOrdHist.RipeOrders1ID    
                WHERE R_Ord.CustomerID = @CustomerID and R_IRSOrdHist.DistributionDate = @DistributionDate and r_ord.LocationID = @LocationID)
        
ORDER BY Distribution.Orders1ID, LD.InventorySubTeam, CD.InventorySubTeam, Item.Item_Key





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



