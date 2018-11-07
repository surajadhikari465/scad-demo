CREATE PROCEDURE [dbo].[ItemPriceListByItemAndStore] 
	@Items varchar(MAX),
	@Stores varchar(MAX)
AS
BEGIN

DECLARE @SQL varchar(MAX)

SET @SQL = 
'select distinct
	i.item_key, 
	i.item_description, 
	s.store_no, 
	s.store_name, 
	CASE WHEN PCT.On_Sale = 1 THEN P.Sale_Price
		ELSE P.Price
		END AS Price, 
	CASE WHEN PCT.On_Sale = 1 THEN P.Multiple
		ELSE P.Sale_Multiple
		END AS Multiple, 
	p.msrpprice,
	p.msrpmultiple,
	CAST(ROUND((dbo.fn_GetCurrentCost(SIV.Item_Key, SIV.Store_No) / VCH.Package_Desc1), 4) AS decimal(10,4)) AS UnitCost,
	CASE WHEN P.Price <= 0 THEN NULL
				 WHEN PCT.On_Sale = 1 AND (P.Sale_Price <= 0 OR P.Sale_Multiple <= 0 OR VCH.Package_Desc1 <= 0) THEN NULL
				 WHEN PCT.On_Sale = 1 THEN CAST(ROUND((((P.Sale_Price / P.Sale_Multiple) - (dbo.fn_GetCurrentNetCost(SIV.Item_Key, SIV.Store_No) / VCH.Package_Desc1)) / (p.Sale_Price / P.Sale_Multiple)) * 100, 2) AS decimal(9,2))
				 ELSE CAST(ROUND((((P.Price / P.Multiple) - (dbo.fn_GetCurrentNetCost(SIV.Item_Key, SIV.Store_No) / VCH.Package_Desc1)) / (P.Price / P.Multiple)) * 100, 2) AS decimal(9,2))
				 END AS Margin,
	(SELECT TOP 1 Identifier FROM ItemIdentifier 
		WHERE ItemIdentifier.Item_Key = I.Item_Key
		ORDER BY Default_Identifier DESC) AS Identifier 
from 
	price p
	INNER JOIN PriceChgType PCT ON P.PriceChgTypeID = PCT.PriceChgTypeID
	INNER JOIN item i ON p.item_key = i.item_key
	INNER JOIN store s ON p.store_no = s.store_no
	INNER JOIN StoreItemVendor siv ON siv.Item_Key = i.item_key AND siv.Store_No = p.store_no
	INNER JOIN vendorcosthistory vch ON vch.StoreItemVendorID=siv.StoreItemVendorID
where 
	i.item_key in (' + @Items + ') 
	AND p.store_no in (' + @Stores + ')
	AND getdate() BETWEEN VCH.startdate AND VCH.enddate
	AND SIV.PrimaryVendor = 1
order by 
	I.item_description, 
	S.store_name'

EXECUTE(@SQL)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemPriceListByItemAndStore] TO [IRMAClientRole]
    AS [dbo];

