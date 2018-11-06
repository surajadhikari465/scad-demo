CREATE PROCEDURE dbo.[GetPromoOrders]
@strtDt smalldatetime,
@endDt smalldatetime,
@deptno int
AS
SELECT     ppo.PromoPreOrderID, ppo.PriceBatchPromoID, pbp.Item_Key, ib.Brand_Name, i.Item_Description, i.Package_Desc1, i.Package_Desc2, iu.Unit_Name, pbp.Sale_Price, 
                      pbp.Sale_Cost, ROUND((pbp.Sale_Price - pbp.Sale_Cost) / pbp.Sale_Price * 100, 2) AS mgn, pbp.Price, pbp.Comment1, pbp.Comment2, ppo.Identifier, 
                      ppo.OrderQty, v.CompanyName, ppo.Store_No, iv.item_id
FROM         dbo.PromoPreOrders AS ppo INNER JOIN
                      dbo.PriceBatchPromo AS pbp ON ppo.PriceBatchPromoID = pbp.PriceBatchPromoID AND ppo.Item_Key = pbp.Item_Key INNER JOIN
                      dbo.Item AS i ON ppo.Item_Key = i.Item_Key INNER JOIN
                      dbo.ItemBrand AS ib ON i.Brand_ID = ib.Brand_ID INNER JOIN
                      dbo.Vendor AS v ON pbp.Vendor_Id = v.Vendor_ID INNER JOIN
					  dbo.ItemVendor iv ON iv.item_key = i.item_key and iv.vendor_id = v.vendor_ID  INNER JOIN
					  dbo.ItemUnit AS iu WITH (nolock) ON i.Package_Unit_ID = iu.Unit_ID
WHERE     (pbp.Sale_End_Date = @endDt) AND (pbp.Start_Date = @strtDt) AND (pbp.Dept_No IN (@deptno)) 
GROUP BY ppo.PromoPreOrderID, ppo.PriceBatchPromoID, pbp.Item_Key, iv.Item_ID, ib.Brand_Name, i.Item_Description,i.Package_Desc1, i.Package_Desc2, iu.Unit_Name, pbp.Sale_Price, 
                      pbp.Sale_Cost, pbp.Price, pbp.Comment1, pbp.Comment2, ppo.Identifier, ppo.OrderQty, v.CompanyName,ppo.Store_No
ORDER BY v.CompanyName, ib.Brand_Name, pbp.Item_Key, ppo.Store_No;
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPromoOrders] TO [IRMAPromoRole]
    AS [dbo];

