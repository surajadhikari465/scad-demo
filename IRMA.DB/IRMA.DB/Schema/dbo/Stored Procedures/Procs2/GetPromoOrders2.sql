CREATE PROCEDURE dbo.[GetPromoOrders2]
@team char(50)
AS


SELECT     ppo.Item_Key, ib.Brand_Name, i.Item_Description, i.Package_Desc1, i.Package_Desc2, iu.Unit_Name, pbp.Sale_Price, 
                      pbp.Sale_Cost, ROUND((pbp.Sale_Price - pbp.Sale_Cost) / pbp.Sale_Price * 100, 2) AS mgn, 
pbp.Price, pbp.Comment1, pbp.Comment2, 
						 ppo.Identifier, 
                       v.CompanyName, iv.item_id,ppo.[104], ppo.[106],ppo.[108],ppo.[109],ppo.[110],ppo.[111],ppo.[112],ppo.[113],ppo.[114],ppo.[115],ppo.[116],ppo.[117],ppo.[119],ppo.[120],
						ppo.[122],ppo.[123],ppo.[124],ppo.[125],ppo.[126],ppo.[127],ppo.[128],ppo.[129],ppo.[131],ppo.[133],ppo.[134]
FROM         PivotPreOrders AS ppo INNER JOIN
                      dbo.PriceBatchPromo AS pbp ON ppo.Item_Key = pbp.Item_Key INNER JOIN
                      dbo.Item AS i ON ppo.Item_Key = i.Item_Key INNER JOIN
                      dbo.ItemBrand AS ib ON i.Brand_ID = ib.Brand_ID INNER JOIN
                      dbo.Vendor AS v ON pbp.Vendor_Id = v.Vendor_ID INNER JOIN
					  dbo.ItemVendor iv ON iv.item_key = i.item_key and iv.vendor_id = v.vendor_ID  INNER JOIN
					  dbo.ItemUnit AS iu  ON i.Package_Unit_ID = iu.Unit_ID
GROUP BY  ppo.Item_Key, iv.Item_ID, ib.Brand_Name, i.Item_Description,i.Package_Desc1, 
			i.Package_Desc2, iu.Unit_Name, pbp.Sale_Price, 
                      pbp.Sale_Cost, pbp.Price, pbp.Comment1, pbp.Comment2, 
						ppo.Identifier,  v.CompanyName, iv.item_id,ppo.[104], ppo.[106],ppo.[108],ppo.[109],ppo.[110],ppo.[111],ppo.[112],ppo.[113],ppo.[114],ppo.[115],ppo.[116],ppo.[117],ppo.[119],ppo.[120],
						ppo.[122],ppo.[123],ppo.[124],ppo.[125],ppo.[126],ppo.[127],ppo.[128],ppo.[129],ppo.[131],ppo.[133],ppo.[134]
ORDER BY v.CompanyName, ib.Brand_Name, ppo.Item_Key;