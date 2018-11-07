CREATE PROCEDURE [dbo].[Reporting_GeneralMarginAudit]
	@MinGM int ,
	@MaxGM int ,
	@Zone_ID int  ,
	@Store_No int ,
	@Store_No2 int ,
	@Store_No3 int ,
	@Store_No4 int ,
	@Store_No5 int ,
	@SubTeam_No int ,
	@SubTeam_No2 int ,
	@SubTeam_No3 int ,
	@Category_ID int ,
	@Vendor_ID varchar(30) , 
	@Region_ID int
WITH RECOMPILE
AS
--**************************************************************************
-- Procedure: Reporting_GeneralMarginAudit
--
-- Revision:
-- 01/11/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with 
--                   storeitemvendor.DiscontinueItem
--**************************************************************************
BEGIN
    SET NOCOUNT ON
   
    DECLARE @SQL varchar(8000)
    SELECT @SQL = 
	'SELECT 
		item.item_key,
		'''' AS comment, 
		item.SubTeam_no AS subteam, 
		cat.category_name AS category, 
		'''' AS commodity,
		itemid.identifier AS UPC, 
		Item_Description AS Description, 
		brand_name AS brand, 
		convert(varchar,convert(smallmoney,item.package_desc2)) AS Size,
		iunit.unit_Name AS UOM, 
		prc.multiple AS PM, 
		round(prc.Price,2) AS ''current price'', 
		'''' AS new_price,
		''REG'' AS ''price type'', 
		(select  top 1 vch.package_desc1 from vendorcosthistory vch (nolock) 		
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)
		Order by vch.InsertDate desc) AS [Case Size] ,
		'''' AS New_Case_Size,  
		convert(smallmoney, dbo.fn_GetCurrentCost (sitvnd.item_key , sitvnd.store_no)) AS case_cost, 
		convert(smallmoney, dbo.fn_GetCurrentNetCost (sitvnd.item_key , sitvnd.store_no)) AS net_cost, 
		'''' AS new_cost,  
		convert(smallmoney,(dbo.fn_GetCurrentCost (sitvnd.item_key , sitvnd.store_no)/(select  top 1 vch.package_desc1 from vendorcosthistory vch (nolock) 		
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)
		Order by vch.InsertDate desc))) AS unit_cost, 
		dbo.fn_getMargin (prc.price, prc.multiple, dbo.fn_GetCurrentNetCost (sitvnd.item_key , sitvnd.store_no)/(select  top 1 vch.package_desc1 from vendorcosthistory vch (nolock) 		
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)
		Order by vch.InsertDate desc)) AS gm_net, 
		dbo.fn_getMargin (prc.price, prc.multiple, dbo.fn_GetCurrentCost (sitvnd.item_key , sitvnd.store_no)/(select  top 1 vch.package_desc1 from vendorcosthistory vch (nolock) 		
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)
		Order by vch.InsertDate desc)) AS gm_reg, 
		'''' AS new_gm,
		vend.vendor_key AS Dist,  
		vend.companyname AS vendor_name, 
		vend.vendor_id AS vendor_id, 
		iven.item_id AS order_no,
		--dbo.fn_GetLastCost(item.item_Key, prc.Store_no) AS Last_Cost,
		(select  top 1 vch.unitcost from vendorcosthistory vch (nolock) 
		LEFT JOIN vendorcosthistory vch2 on vch.storeitemvendorid=vch2.storeitemvendorid
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)
		AND vch.unitcost <> vch2.unitcost and vch.InsertDate < vch2.InsertDate
		Order by vch.InsertDate desc) AS [Last_Cost] ,
		prc.store_no AS Store_No, 
		--vcosth.insertdate AS Last_Date,
		(select  top 1 vch.InsertDate from vendorcosthistory vch (nolock) 
		LEFT JOIN vendorcosthistory vch2 on vch.storeitemvendorid=vch2.storeitemvendorid
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)		
		AND  vch.unitcost <> vch2.unitcost AND vch.InsertDate < vch2.InsertDate
		Order by vch.InsertDate desc) AS [Last_Date] ,
		''?'' AS inventory_count,
		region.region_id,
		zone.zone_id,
		store.storeabbr AS StoreAbbr
  
	FROM 
		item (nolock) 

		INNER JOIN itembrand ib (nolock)
			ON item.brand_id = ib.brand_id

		INNER JOIN itemcategory cat (nolock)
			ON item.category_id = cat.category_id

		INNER JOIN itemvendor iven  (nolock)
			ON iven.item_key = item.item_key

		INNER JOIN vendor vend (nolock)						
			ON vend.vendor_id = iven.vendor_id	

		INNER JOIN price prc (nolock)  
			ON prc.item_key = item.item_key

		INNER JOIN itemidentifier itemid (nolock)
			ON itemid.item_key = item.item_key

		INNER JOIN itemunit iunit (nolock) 
			ON iunit.unit_id = item.package_unit_id

		INNER JOIN storeitemvendor sitvnd (nolock)			
			ON item.item_key = sitvnd.item_key	

		--INNER JOIN vendorcosthistory vcosth (nolock)		
		--ON vcosth.storeitemvendorid = sitvnd.storeitemvendorid		
		
		INNER JOIN store (nolock)
			ON store.store_no = prc.store_no

		INNER JOIN zone (nolock)
			ON zone.zone_id = store.zone_id

		INNER JOIN region (nolock)
			ON region.region_id = zone.region_id

		INNER JOIN subteam (nolock)
			ON subteam.subteam_no = item.subteam_no

		INNER JOIN team (nolock)
			ON team.team_no = subteam.team_no 
			
		INNER JOIN dbo.StoreItem (nolock)
			ON dbo.Store.Store_No = dbo.StoreItem.Store_No 
			AND dbo.Item.Item_Key = dbo.StoreItem.Item_Key
	WHERE
		iven.vendor_id = sitvnd.vendor_id and		
		sitvnd.store_no = prc.store_no and 
		sitvnd.primaryVendor = 1 and
		itemid.default_identifier = 1 and 
		dbo.StoreItem.Authorized = 1 and
		sitvnd.DiscontinueItem <> 1 and

	  ((dbo.fn_getMargin (prc.price, prc.multiple, dbo.fn_GetLastCost(item.item_Key, prc.Store_no)/(select  top 1 vch.package_desc1 from vendorcosthistory vch (nolock) 		
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)
		Order by vch.InsertDate desc)) <  ' + CONVERT(varchar(255), @MinGM) + ')		
or (dbo.fn_getMargin (prc.price, prc.multiple, dbo.fn_GetLastCost(item.item_Key, prc.Store_no)/(select  top 1 vch.package_desc1 from vendorcosthistory vch (nolock) 		
		where vch.storeitemvendorid = (select top 1 siv.storeitemvendorid from storeitemvendor siv (nolock) where
					siv.item_key = Item.Item_key and siv.store_no = prc.store_no and siv.primaryvendor = 1)
		Order by vch.InsertDate desc)) >  ' + CONVERT(varchar(255), @MaxGM) + '))'


    IF @Zone_ID IS NOT NULL
        SELECT @SQL = @SQL + 
				' and zone.zone_id = ' + CONVERT(varchar(255), @Zone_ID) 

    IF @Region_ID IS NOT NULL
        SELECT @SQL = @SQL + 
				' and Region.Region_ID = ' + CONVERT(varchar(255), @Region_ID)  

    IF @SubTeam_No IS NOT NULL
        SELECT @SQL = @SQL + 
				' and (item.SubTeam_no = ' + CONVERT(varchar(255), @SubTeam_No)  

    IF @SubTeam_No IS NOT NULL and @SubTeam_No2 IS NOT NULL
        SELECT @SQL = @SQL + 
				' or item.SubTeam_no = ' + CONVERT(varchar(255), @SubTeam_no2)  
	ELSE IF @SubTeam_No2 IS NOT NULL
		SELECT @SQL = @SQL + 
				' and (item.SubTeam_no = ' + CONVERT(varchar(255), @SubTeam_no2) 	

    IF (@SubTeam_No IS NOT NULL or @SubTeam_No2 IS NOT NULL) and @SubTeam_No3 IS NOT NULL
        SELECT @SQL = @SQL + 
				' or item.SubTeam_no = ' + CONVERT(varchar(255), @SubTeam_no3)  
	ELSE IF @SubTeam_No3 IS NOT NULL
		SELECT @SQL = @SQL + 
				' and (item.SubTeam_no = ' + CONVERT(varchar(255), @SubTeam_no3)

	IF @SubTeam_No IS NOT NULL or @SubTeam_No2 IS NOT NULL or @SubTeam_No3 IS NOT NULL
		SELECT @SQL = @SQL + ')'

	IF @Category_ID IS NOT NULL
        SELECT @SQL = @SQL + 
				' and cat.category_ID = ' + CONVERT(varchar(255), @Category_ID)  

    IF @Vendor_ID IS NOT NULL
        SELECT @SQL = @SQL + 
				' and vend.vendor_key = ' + '''' + @Vendor_ID + '''' 

    IF @Store_no IS NOT NULL
        SELECT @SQL = @SQL + 
				' and (prc.store_no = ' + CONVERT(varchar(255), @Store_no)  

    IF @Store_no IS NOT NULL and @Store_no2 IS NOT NULL
        SELECT @SQL = @SQL + 
				' or prc.store_no = ' + CONVERT(varchar(255), @Store_no2)  
	ELSE IF @Store_no2 IS NOT NULL
		SELECT @SQL = @SQL + 
				' and (prc.store_no = ' + CONVERT(varchar(255), @Store_no2) 		

    IF (@Store_no IS NOT NULL or @Store_no2 IS NOT NULL) and @Store_no3 IS NOT NULL
        SELECT @SQL = @SQL + 
				' or prc.store_no = ' + CONVERT(varchar(255), @Store_no3)  
	ELSE IF @Store_no3 IS NOT NULL
		SELECT @SQL = @SQL + 
				' and (prc.store_no = ' + CONVERT(varchar(255), @Store_no3) 	

    IF (@Store_no IS NOT NULL or @Store_no2 IS NOT NULL or @Store_no3 IS NOT NULL) and @Store_no4 IS NOT NULL
        SELECT @SQL = @SQL + 
				' or prc.store_no = ' + CONVERT(varchar(255), @Store_no4)  
	ELSE IF @Store_no4 IS NOT NULL
		SELECT @SQL = @SQL + 
				' and (prc.store_no = ' + CONVERT(varchar(255), @Store_no4) 

    IF (@Store_no IS NOT NULL or @Store_no2 IS NOT NULL or @Store_no3 IS NOT NULL or @Store_no4 IS NOT NULL) and @Store_no5 IS NOT NULL
        SELECT @SQL = @SQL + 
				' or prc.store_no = ' + CONVERT(varchar(255), @Store_no5)  
	ELSE IF @Store_no5 IS NOT NULL
		SELECT @SQL = @SQL + 
				' and prc.store_no = ' + CONVERT(varchar(255), @Store_no5) 

	IF (@Store_no IS NOT NULL or @Store_no2 IS NOT NULL or @Store_no3 IS NOT NULL or @Store_no4 IS NOT NULL or @Store_no5 IS NOT NULL)
		SELECT @SQL = @SQL + ')'

		SELECT @SQL = @SQL + ' order by subteam,upc '

    EXEC(@SQL)
	--PRINT(@SQL)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GeneralMarginAudit] TO [IRMAReportsRole]
    AS [dbo];

