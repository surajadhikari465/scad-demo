if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemWebQueryDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemWebQueryDetail]
GO

CREATE Procedure dbo.ItemWebQueryDetail
	@item_key int
as
	-- **************************************************************************
	-- Procedure: ItemWebQueryDetail()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	SELECT
			s.Store_Name, 
			convert(varchar(5),p.Multiple)+'/'+convert(varchar(10),p.posprice) as RegPrice,
			convert(varchar(5),P.Sale_Multiple)+'/'+convert(varchar(10),p.Sale_Price) as SalePrice, 
			pi.unit_price as POS_Price,
			convert(varchar,pi.load_Date,101) as POS_Price_Date,
			0 as SaleUnits, 
			0 as SaleDollars,
			v.CompanyName as Vendor,
			iv.Item_ID as VendorItemID,
			dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No) as PackSize,
			CAST((dbo.fn_GetCurrentCost(@item_key,SI.Store_No)/dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No)) As money) As UnitCost,
			CAST((dbo.fn_GetCurrentNetCost(@item_key, SI.Store_No)/dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No)) as money) As NetCost,
			CASE when not dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No) > 0 
				then '0' 
			ELSE
				cast(dbo.fn_GetMargin(
										p.POSPrice
										, p.Multiple
										,((dbo.fn_GetCurrentCost(@item_key, SI.Store_No))/dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No)))/100 as Decimal(9,4)) 
			END AS Margin, -- margin is reg price v reg cost 
			CASE when not dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No) > 0 
				then '0' 
			ELSE
				cast(dbo.fn_GetMargin(
										case when dbo.fn_OnSale(p.PriceChgTypeID) = 1 then p.Sale_Price else p.POSPrice end
										,case when dbo.fn_OnSale(p.PriceChgTypeID) = 1 then p.Sale_Multiple else p.Multiple end
										,((dbo.fn_GetCurrentNetCost(@item_key, SI.Store_No))/dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No)))/100 as Decimal(9,4)) 
			END AS NetMargin, -- net margin is current retail v net cost
			CASE when si.Authorized = 1 
				then 'P' 
				else 'N' 
			END AS Auth, 
			pct.PriceChgTypeDesc ,
			ib.Brand_Name, 
			st.subteam_name,
			ic.category_name, 
			tc.TaxClassDesc,
			case when i.Food_Stamps = 1 THEN 'Y' ELSE 'N' END AS Food_Stamps,
			siv.DiscontinueItem
	FROM	Store s (nolock)
			LEFT JOIN StoreItemVendor siv (nolock)
				ON s.Store_no = siv.store_no and siv.item_key = @item_key and PrimaryVendor = 1
			LEFT OUTER JOIN StoreItem SI (nolock)
				ON S.Store_No = SI.Store_No AND siv.Item_Key = SI.Item_Key
			LEFT JOIN Price p (nolock)
				ON s.store_no = p.store_no and siv.item_key = p.item_key
			LEFT JOIN PriceChgType pct (nolock)
				ON p.PriceChgTypeID = pct.PriceChgTypeID
			LEFT JOIN Vendor v (nolock)
				ON siv.Vendor_ID = v.Vendor_ID
			LEFT JOIN ItemVendor iv (nolock)
				ON iv.Vendor_ID = v.Vendor_ID and iv.item_key = siv.item_key
			LEFT JOIN Item i (nolock)
				ON p.item_key = i.item_key
			/*	The following subquery was added 1/14/2009 in response to TFS bug #8290 (SLIM ItemWebQuery procedures return multiple records).
				This replaces the join to POSItem. */
			LEFT JOIN (
				select pi.*
				from POSItem pi (nolock)
					JOIN itemidentifier ii (nolock) -- Making this inner join because we filtering bad data (no identifier rec) out of POSItem.
						ON pi.item_key = ii.item_key
						AND pi.identifier = ii.identifier 
						AND pi.item_key = @item_key
						/*	To guarantee we only get one valid POSItem record, get a non-deleted default identifier.  There could be multiple, active identifiers in the POSItem table for a store, 
							and it is unlikely, but possible that none of these are the default identifier, which would mean the user does not get POSItem data in the Item Details screen.
							The POSItem data should be the same for multiple identifiers because price is at the item level, not identifier. */
						AND ii.default_identifier = 1
			) pi
				ON i.item_key = pi.item_key
				AND s.store_no = pi.Store_no	
			LEFT JOIN ItemBrand ib (nolock)
				ON i.brand_id = ib.brand_id
			LEFT JOIN TaxClass tc 
				ON i.TaxClassId = tc.TaxClassId
			LEFT JOIN Subteam st (nolock) 
				ON i.Subteam_No = st.Subteam_No
			LEFT JOIN ItemCategory ic (nolock) ON i.Category_ID = ic.Category_ID
	WHERE (s.WFM_Store = 1 OR s.Mega_Store = 1)
	ORDER BY s.store_name

	COMMIT TRAN
END
GO
