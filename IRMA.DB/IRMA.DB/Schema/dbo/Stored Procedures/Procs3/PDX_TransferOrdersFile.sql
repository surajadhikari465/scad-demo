CREATE PROCEDURE [dbo].[PDX_TransferOrdersFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @IncludedStores as table (Store_No int, WFMStore bit)
declare @IncludedPOs as table (OrderHeader_ID int, To_BusinessUnit_ID int, From_BusinessUnit_ID int)
declare @today as datetime, @yesterday as datetime

set @today = Convert(Date, getdate(), 102)
set @yesterday = Convert(Date, getdate() - 1, 102)

insert into @IncludedStores
select store_no, 0
from   Store
where  mega_store = 1
union
select s.store_no, 1
from   Store s
join   [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('WFMBannerStoresForOrdering', 'IRMA CLIENT'), '|') bs 
	   on s.BusinessUnit_ID = bs.Key_Value

insert into @IncludedPOs
-- Transfer orders with the selected stores as a "transfer from" stores. This is for in-store transfers.
select oh.OrderHeader_ID, s.BusinessUnit_ID, sv.BusinessUnit_ID
from   OrderHeader oh
join   Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join   @IncludedStores ist on ist.Store_No = psl.Store_no
join   Store s on s.Store_No = psl.Store_no
join   Vendor v on oh.Vendor_ID = v.Vendor_ID
join   Store sv on sv.Store_No = v.Store_no
where  oh.OrderType_ID = 3
and    oh.Sent = 1
and    ((OrderDate < @today and OrderDate >= @today - 5)
 or    (CloseDate < @today and CloseDate >= @yesterday))
and    oh.PurchaseLocation_ID = oh.Vendor_ID 

select oh.OrderHeader_Id as PO_NUMBER, ROW_NUMBER() OVER(PARTITION BY oh.OrderHeader_Id ORDER BY oi.OrderItem_ID) as PO_LINE_NUMBER, 
RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, 
case when return_order = 1 
     then oi.QuantityOrdered * -1
	 else oi.QuantityOrdered
end as PO_QTY, 
ipo.From_BusinessUnit_ID as FROM_STORE_NUMBER, 
Cast(ipo.To_BusinessUnit_ID as varchar(10)) as TO_STORE_NUMBER,
convert(varchar, oh.OrderDate, 112) as PO_CREATE_DATE, 
convert(varchar, oh.Expected_Date, 112) as PO_ETA_DATE, convert(varchar, oh.CloseDate, 112) as PO_CLOSE_DATE,
tst.SubTeam_Name as TO_PROD_SUBTEAM,
CASE 
    WHEN oh.Transfer_To_SubTeam = i.SubTeam_No
	THEN 'Y'
	ELSE 'N'
END AS TO_HOST_SUBTEAM_FLAG,
fst.SubTeam_Name as FROM_PROD_SUBTEAM,
CASE 
    WHEN oh.Transfer_SubTeam = i.SubTeam_No
	THEN 'Y'
	ELSE 'N'
END AS FROM_HOST_SUBTEAM_FLAG,
rtrim(iu.Unit_Name) as CASE_UOM
from OrderHeader oh
join @IncludedPOs ipo on oh.OrderHeader_ID = ipo.OrderHeader_ID
join SubTeam tst on tst.Subteam_No = oh.Transfer_To_SubTeam
join SubTeam fst on fst.Subteam_No = oh.Transfer_SubTeam
join OrderItem oi on oh.OrderHeader_Id = oi.OrderHeader_Id 
join Item i on i.Item_Key = oi.Item_Key
join ItemIdentifier ii on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
join ItemUnit iu on iu.Unit_Id = oi.QuantityUnit
where i.Retail_Sale = 1
and (
		(SentDate < @today and SentDate >= @yesterday)
		or  (OriginalCloseDate < @today and OriginalCloseDate >= @yesterday)
        or  (vsc.InsertDate < @today and vsc.InsertDate >= @yesterday
             and SentDate < @today and SentDate >= @today - 5)
	)
order by oh.OrderHeader_Id, oi.OrderItem_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_TransferOrdersFile] TO [IRMAPDXExtractRole]
    AS [dbo];