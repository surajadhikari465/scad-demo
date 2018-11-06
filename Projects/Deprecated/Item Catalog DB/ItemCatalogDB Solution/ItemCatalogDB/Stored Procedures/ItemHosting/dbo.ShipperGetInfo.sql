IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShipperGetInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ShipperGetInfo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ShipperGetInfo]
	@Shipper_Key int -- This is an item key.
AS 

/*
	ShipperGetInfo, Introduced in v4.0, July 2010
	
	This SP retrieves all the necessary information need to build a Shipper object in IRMA
	Information for the Shipper and all items it contains is returned.
	
	Some of the parent Shipper item information is repeated with each row, with the assumption
	that Shippers will never contain an extraordinary number of items and this saves us from needing
	another SP and DB hit.
	
	
	Change History
	2010.07.09			Tom Lux			Added SP; combined old SPs GetShipperItemInfo, GetShipperItemList.
	
*/

select
	Shipper_Key = s.Shipper_Key
	,ShipperIdentifier = sii.Identifier
	,ShipperDesc = si.Item_Description
	,Item_Key = s.Item_Key
	,Identifier = ii.Identifier
	,Item_Description = i.Item_Description
	,Quantity = s.Quantity
	,Subteam_No = st.SubTeam_No
	,SubteamName = st.SubTeam_Name
	,Brand_ID = ib.Brand_ID
	,BrandName = ib.Brand_Name
	,RetailPackQty = i.Package_Desc1 -- Shows in Retail Package "Pack" field in IRMA item screen.
	,RetailPackSize = i.Package_Desc2 -- Shows in Retail Package "Size" field in IRMA item screen.
from
	Item i (nolock)
	join ItemIdentifier ii (nolock)
		on i.Item_Key = ii.Item_Key
	join Shipper s (nolock)
		on s.Item_Key = i.Item_Key
	join Item si (nolock) -- To get item description for the Shipper.
		on s.Shipper_Key = si.Item_Key
	join ItemIdentifier sii (nolock) -- To get item identifier for the Shipper.
		on s.Shipper_Key = sii.Item_Key
	join ItemBrand ib (nolock)
		on i.Brand_ID = ib.Brand_ID 
	join SubTeam st (nolock)
		on i.SubTeam_No = st.SubTeam_No
where
	s.Shipper_Key = @Shipper_Key -- The "shipper key" in the Shipper table is the Item.Item_Key of a normal IRMA item that has been marked as a Shipper.
	and ii.Default_Identifier = 1
	and sii.Default_Identifier = 1
order by
	i.Item_Description

GO


