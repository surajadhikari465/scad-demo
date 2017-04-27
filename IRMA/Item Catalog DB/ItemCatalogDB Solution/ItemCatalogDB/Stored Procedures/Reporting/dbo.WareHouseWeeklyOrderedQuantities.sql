SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].WareHouseWeeklyOrderedQuantities') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].WareHouseWeeklyOrderedQuantities
GO

/*
	grant exec on WareHouseWeeklyOrderedQuantities to IRMAAdminRole
	grant exec on WareHouseWeeklyOrderedQuantities to IRMAClientRole
	grant exec on WareHouseWeeklyOrderedQuantities to IRMAReportsRole
*/

CREATE PROCEDURE dbo.WareHouseWeeklyOrderedQuantities
    @Vendor_ID int,
	@SubTeam_ID int

WITH RECOMPILE
AS

BEGIN

SET NOCOUNT ON


--To fetch the week startDate(Between sunday to Monday).
Declare @StartDate varchar(12)
select @StartDate=CONVERT(varchar(12), dateadd("d",-(datepart(dw,GetDate())-1),Getdate()), 101)



--Fetching Current OnHand Quantities from Warehouse_Inventory Table.
DECLARE @OnHandQty TABLE(Item_Key int, DateCreated datetime, Tot_BOH decimal(19,4))
INSERT INTO @OnHandQty
select I.item_key,WH.DateCreated,(WH.Tot_BOH/dbo.fn_GetVendorPack(I.item_key,@Vendor_ID)) as Tot_BOH  from 
warehouse_inventory WH
left join ItemIdentifier II
on WH.product_id=ii.identifier and II.default_identifier=1
Left Join Item I
on I.Item_key=II.Item_key and I.Subteam_no=ISNULL(@SubTeam_ID,I.Subteam_no)
where I.Item_Key is not null
order by I.Item_Key,WH.DateCreated




--Fetching Ordered Quantities with respect to the Item(s),  from the Orderheader and OrderItem Tables.
DECLARE @QtyOrdered TABLE(Item_Key int, ExpectedDate datetime, QtyOrd int)
INSERT INTO @QtyOrdered
select 
OI.Item_key As Item_Key,
OH.Expected_Date as ExpectedDate,
SUM(OI.QuantityOrdered) as QtyOrd
from orderitem OI
inner join orderheader OH
on OH.Orderheader_ID=OI.Orderheader_ID
where (OH.Expected_Date >=@StartDate and OH.Expected_Date <= dateadd("d",6,@StartDate)) -- Expected before end of the week.
and OH.PurchaseLocation_ID=ISNULL(@Vendor_ID,OH.PurchaseLocation_ID)
and OH.Transfer_To_SubTeam=ISNULL(@SubTeam_ID,OH.Transfer_To_SubTeam)
and OH.Sent=1 and OH.SentDate is not null
group by OI.Item_key,OH.Expected_Date


-- Merging the required data from the two Temporary Tables for displaying into the report.
select T1.Item_key,
T1.ToT_BOH,
T2.QtyOrd,
T2.ExpectedDate,
T1.DateCreated,
REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier) as Identifier,
I.Item_description 
from @OnHandQty T1
Left join @QtyOrdered T2
ON T1.item_key=T2.item_key and (datename(weekday,T1.DateCreated) = datename(weekday,T2.ExpectedDate)
and day(T1.DateCreated)=day(T2.ExpectedDate))
Left join dbo.Item I on T1.Item_Key=I.Item_key
Left join dbo.ItemIdentifier II on T1.Item_Key = II.Item_Key  AND II.Default_Identifier = 1
order by T1.Item_key,T2.ExpectedDate


   SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO