SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PurchToSalesComp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PurchToSalesComp]
GO


CREATE PROCEDURE dbo.PurchToSalesComp
        @store int,
        @subteam int,
        @startdate varchar(10),
        @enddate varchar(10)
AS 
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

declare @PWS table (Item_key int primary key, SoldByWeight bit, identifier varchar(13), item_description varchar (65), Package_Desc1 decimal(9,4), Package_Desc2 decimal(9,4), Unit_Name varchar(25), Purchases numeric(9,4), Sales numeric(9,4), Waste decimal(18,4), Transfers numeric(9,4), OnHand decimal(18,4))

insert into @pws
select distinct item_key, null, null, null, null, null, null, null, null, null, null, null
FROM OrderHeader (nolock)
    INNER JOIN
        OrderItem WITH (nolock)
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN 
        Vendor (nolock)
        ON Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID
WHERE
    Vendor.Store_No = @Store
    AND isnull(OrderHeader.Transfer_to_SubTeam, OrderHeader.Transfer_SubTeam)  = @SubTeam
    AND OrderItem.DateReceived >= @startDate
    AND OrderItem.DateReceived <= @EndDate

insert into @pws
select distinct ssbi.item_key, null, null, null, null, null, null, null, null, null, null, null
from     sales_sumbyitem ssbi(nolock)
where 
store_no = @store and subteam_no = @subteam
AND Date_Key >= @startDate
 and  Date_Key <= @EndDate
and item_key not in (
                     select item_key from @pws)

update @pws
set identifier = ii.identifier, item_description = i.item_description, SoldByWeight = IU.Weight_Unit, Package_Desc1 = i.Package_Desc1, Package_Desc2 = i.Package_Desc2, Unit_Name = iu.Unit_Name
--select pws.identifier, ii.identifier 
from item i(nolock)
join @pws pws on i.item_key = pws.item_key
join itemidentifier ii(nolock) on i.item_key = ii.item_key and default_identifier = 1
join itemunit iu(nolock) on i.retail_unit_id = iu.unit_id
where deleted_identifier = 0 

declare @Purchases table (item_key int, purchases numeric(9,4))
insert into @purchases
select item_key, isnull(SUM(OrderItem.UnitsReceived),0)
FROM OrderHeader (nolock)
    INNER JOIN
        OrderItem WITH(nolock)
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN 
        Vendor (nolock)
        ON Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID
WHERE
    Vendor.Store_No = @Store
    AND isnull(OrderHeader.Transfer_to_SubTeam, OrderHeader.Transfer_SubTeam)  = @SubTeam
    AND OrderItem.DateReceived >= @startDate
    AND OrderItem.DateReceived <= @EndDate
group by item_key

update @pws
set purchases = p.purchases
from @pws pws
join @purchases p on p.item_key = pws.item_key

declare @sales table (item_key int, sales numeric(9,4))
insert into @sales
select ssbi.item_key, SUM(dbo.fn_itemSalesQty(PWS.Identifier, PWS.SoldByWeight, SSBI.Price_Level, SSBI.Sales_Quantity, SSBI.Return_Quantity, PWS.Package_Desc1, SSBI.Weight))
from sales_sumbyitem ssbi(nolock)
join @pws pws on pws.item_key = ssbi.item_key
WHERE
    Store_No = @Store
    AND SSBI.SubTeam_No = @SubTeam
    AND Date_Key >=  @startDate
    AND Date_Key <= @endDate
GROUP BY ssbi.item_key, SoldByWeight

update @pws
set sales = p.sales
from @pws pws
join @sales p on p.item_key = pws.item_key

declare @waste table (item_key int, waste decimal(18,4))
insert into @waste
select ih.item_key, case when SoldByWeight = 0 then sum(quantity) else sum(weight) end as waste
from itemhistory ih(nolock) 
join @pws pws on pws.item_key = ih.item_key
WHERE
    Store_No = @Store
    AND ih.SubTeam_No = @SubTeam
    AND Datestamp >=  @startDate
    AND Datestamp <= @endDate
and adjustment_id = 1
GROUP BY ih.item_key, SoldByWeight

update @pws
set waste = w.waste
from @pws pws
join @waste w on w.item_key = pws.item_key

declare @Transfers table (item_key int, transfers numeric(9,4))
insert into @Transfers
select item_key, isnull(SUM(OrderItem.UnitsReceived),0)
FROM OrderHeader (nolock)
    INNER JOIN
        OrderItem WITH(nolock)
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN 
        Vendor (nolock)
        ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
WHERE
    Vendor.Store_No = @Store
    AND OrderHeader.Transfer_SubTeam  = @SubTeam
    AND OrderItem.DateReceived >= @startDate
    AND OrderItem.DateReceived <= @EndDate
group by item_key

update @pws
set transfers = t.transfers
from @pws pws
join @transfers t on t.item_key = pws.item_key


update @pws
set purchases = 0 where purchases is null

update @pws
set transfers = 0 where transfers is null

update @pws
set sales = 0 where sales is null

update @pws
set waste = 0 where waste is null


select 
    item_key, 
    identifier, 
    item_description as Description, 
    package_desc1 as 'pack', 
    package_desc2 as 'Size', 
    Unit_name as 'Unit', 
    purchases as 'UnitsPurchased', 
    transfers as 'Trfs Out', 
    waste, 
    sales as 'UnitsSold', 
    purchases - transfers-waste - sales as 'PurchtoSales Diff' 
from @pws
order by identifier



SET NOCOUNT OFF


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



