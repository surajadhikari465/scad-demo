CREATE PROCEDURE dbo.[GetPromo]
@strtDt smalldatetime,
@endDt smalldatetime,
@deptno int
AS
SELECT     p.PriceBatchPromoID, p.Store_No, p.Item_Key, id.Identifier, v.Vendor_ID, v.CompanyName, u.Brand_ID, u.Item_Description, u.Package_Desc2, 
                      u.Package_Unit_ID, p.Sale_Price, p.Sale_Multiple, p.Dept_No, p.Comment1, p.Comment2, p.ProjUnits, p.Price, x.Unit_Name, p.Sale_Cost, 
                      br.Brand_Name
FROM         dbo.StoreItemVendor AS s CROSS JOIN
                      dbo.Vendor AS v INNER JOIN
                      dbo.ItemBrand AS br WITH (nolock) INNER JOIN
                      dbo.Item AS u ON br.Brand_ID = u.Brand_ID INNER JOIN
                      dbo.ItemUnit AS x ON u.Package_Unit_ID = x.Unit_ID INNER JOIN
                      dbo.ItemIdentifier AS id ON u.Item_Key = id.Item_Key INNER JOIN
                      dbo.PriceBatchPromo AS p ON u.Item_Key = p.Item_Key ON v.Vendor_ID = p.Vendor_Id
WHERE     (p.Sale_End_Date = @endDt) AND (p.Start_Date = @strtDt) AND (p.Dept_No IN (@deptno))
GROUP BY p.PriceBatchPromoID, p.Store_No, p.Item_Key, id.Identifier, v.Vendor_ID, u.Brand_ID, u.Item_Description, u.Package_Desc2, u.Package_Unit_ID, 
                      p.Sale_Cost, p.Sale_Price, p.Sale_Multiple, p.Dept_No, p.Comment1, p.Comment2, p.ProjUnits, p.Price, x.Unit_Name, br.Brand_Name, 
                      v.CompanyName
ORDER BY id.Identifier DESC, p.Store_No;
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPromo] TO [IRMAPromoRole]
    AS [dbo];

