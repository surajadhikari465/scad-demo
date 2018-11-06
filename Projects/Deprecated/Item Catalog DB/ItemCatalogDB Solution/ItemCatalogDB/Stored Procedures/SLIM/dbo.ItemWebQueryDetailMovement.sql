if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemWebQueryDetailMovement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemWebQueryDetailMovement]
GO
CREATE Procedure dbo.ItemWebQueryDetailMovement

        @item_key int,
        @StartDate datetime,
        @EndDate datetime

as
-- **************************************************************************
-- Procedure: ItemWebQueryDetailMovement()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called to get Item movement by store for SLIM
--
-- Modification History:
-- Date       	Init	TFS   	Comment
-- 2013-05-17	BJL 	12276	Added DiscontinueItem field
-- 2013-09-10   FA		13661   Add transaction isolation level
-- **************************************************************************
--
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	select 
		s.Store_Name, 
		convert(varchar(5), p.Multiple) + '/' + convert(varchar(10), p.price) as RegPrice,
		convert(varchar(5), P.Sale_Multiple) + '/' + convert(varchar(10), p.Sale_Price) as SalePrice, 
		pi.unit_price as POS_Price,
		convert(varchar,pi.load_Date,101) as POS_Price_Date,
		dbo.fn_GetSumSalesQuantity(@item_key, s.store_no, @StartDate, @EndDate) as SaleUnits,
		dbo.fn_GetSumSalesDollars(@item_key, s.store_no, @StartDate, @EndDate) as SaleDollars,
		v.CompanyName as Vendor,
		iv.Item_ID as VendorItemID,
		dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No) as PackSize,
		CAST((dbo.fn_GetCurrentCost(@item_key,SI.Store_No)/dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No)) As money) As UnitCost,
		CAST((dbo.fn_GetCurrentNetCost(@item_key, SI.Store_No)/dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No)) as money) As NetCost,
		CASE when not dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No) > 0 then '0' 
				ELSE
				cast(dbo.fn_GetMargin(
											p.Price
											, p.Multiple
											,((dbo.fn_GetCurrentCost(@item_key, SI.Store_No))/dbo.fn_GetCurrentVendorPackage_Desc1(@item_key,SI.Store_No))) as Decimal(7,4)) 
				END AS Margin, -- margin is reg price v reg cost 
		case when si.Authorized = 1 then 'P' else 'N' end as Auth, 
		pct.PriceChgTypeDesc,
		siv.DiscontinueItem
	from 
		Store s
		left join StoreItemVendor siv
			on s.Store_no = siv.store_no and siv.item_key = @item_key and PrimaryVendor = 1
		LEFT OUTER JOIN StoreItem SI (nolock)
			ON S.Store_No = SI.Store_No AND siv.Item_Key = SI.Item_Key
		left join Price p
			on s.store_no = p.store_no and siv.item_key = p.item_key
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
			ON si.item_key = pi.item_key
			AND s.store_no = pi.Store_no	
		left join PriceChgType pct
			on p.PriceChgTypeID = pct.PriceChgTypeID
		left join Vendor v
			on siv.Vendor_ID = v.Vendor_ID
		left join ItemVendor iv
			on iv.Vendor_ID = v.Vendor_ID and iv.item_key = siv.item_key
	where 
		(s.WFM_Store = 1 OR s.Mega_Store = 1)
	order by 
		s.store_name
	
	COMMIT TRAN

END
GO