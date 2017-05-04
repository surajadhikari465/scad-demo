SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RIPEImportErrors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RIPEImportErrors]
GO


CREATE PROCEDURE dbo.RIPEImportErrors
@DistributionDate varchar(20),
@FromStore_No int,
@ToStore_No int
AS

SELECT DISTINCT Recipe.PLU, Recipe.RecipeName, ItemVendor.Item_Key
FROM ItemVendor RIGHT JOIN (
       Vendor RIGHT JOIN (
         Recipe..Location Location INNER JOIN (
           Recipe..Recipe Recipe INNER JOIN (
             Recipe..Customer Customer INNER JOIN Recipe..Distribution Distribution ON (Distribution.CustomerID = Customer.CustomerID) 
           ) ON (Recipe.RecipeID = Distribution.RecipeID)
         ) ON (Location.LocationID = Recipe.LocationID)
       ) ON (Vendor.Store_No = Location.Store_No)
     ) ON (ItemVendor.Vendor_ID = Vendor.Vendor_ID AND ItemVendor.Item_ID = Recipe.PLU)
WHERE Customer.StoreNo = @ToStore_No and Location.Store_No = @FromStore_No AND DistributionDate = @DistributionDate AND 
      (ItemVendor.Item_Key IS NULL OR ISNULL(Recipe.PLU,'') = '')
ORDER BY PLU, RecipeName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


