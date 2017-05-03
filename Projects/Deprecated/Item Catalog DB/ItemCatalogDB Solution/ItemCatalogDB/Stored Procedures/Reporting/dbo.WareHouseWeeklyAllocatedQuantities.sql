SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].WareHouseWeeklyAllocatedQuantities') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].WareHouseWeeklyAllocatedQuantities
GO

/*
	grant exec on WareHouseWeeklyAllocatedQuantities to IRMAAdminRole
	grant exec on WareHouseWeeklyAllocatedQuantities to IRMAClientRole
	grant exec on WareHouseWeeklyAllocatedQuantities to IRMAReportsRole
*/

CREATE PROCEDURE dbo.WareHouseWeeklyAllocatedQuantities
    @Vendor_ID int,
	@SubTeam_ID int

WITH RECOMPILE
AS

BEGIN

SET NOCOUNT ON

--Fetching current Available Quantities from Warehouse_Inventory Table.
DECLARE @AvlQty TABLE(Item_Key int, DateCreated datetime, Tot_BOH int)
INSERT INTO @AvlQty
select I.item_key,WH.DateCreated,WH.Tot_BOH from 
warehouse_inventory WH
left join ItemIdentifier II
on WH.product_id=ii.identifier and II.default_identifier=1
Left Join Item I
on I.Item_key=II.Item_key and I.Subteam_no=ISNULL(@SubTeam_ID,I.Subteam_no)
where I.Item_Key is not null and WH.DateCreated=GetDate()
order by I.Item_Key



--Fetching Allocated Quantities with respect to the Item(s),  from the Orderheader and OrderItem Tables.
DECLARE @AllocQty TABLE(Item_Key int,QtyAllocated int)
INSERT INTO @AllocQty
select 
OI.Item_key As Item_Key,
SUM(OI.QuantityAllocated) as QtyAllocated
from orderitem OI
inner join orderheader OH
on OH.Orderheader_ID=OI.Orderheader_ID
where OH.Expected_Date =dateadd(day,1,Getdate())
and OH.Vendor_ID=ISNULL(@Vendor_ID,OH.Vendor_ID)
and OH.Transfer_SubTeam=ISNULL(@SubTeam_ID,OH.Transfer_SubTeam)
and OH.Sent=1 and OH.SentDate is not null
group by OI.Item_key


-- Merging the data for displaying in the report Output.
select T2.Item_key,
T1.ToT_BOH,
T2.QtyAllocated,
REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier) as Identifier,
I.Item_description 
from @AvlQty T1
left join @AllocQty T2
ON T1.item_key=T2.item_key 
Left join dbo.Item I on T2.Item_Key=I.Item_key
Left join dbo.ItemIdentifier II on T2.Item_Key = II.Item_Key  
AND II.Default_Identifier = 1
order by T2.Item_key


   SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
 