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
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEImportErrors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEImportErrors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEImportErrors] TO [IRMAReportsRole]
    AS [dbo];

