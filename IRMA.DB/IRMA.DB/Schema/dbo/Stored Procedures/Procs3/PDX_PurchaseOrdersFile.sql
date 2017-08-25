CREATE PROCEDURE [dbo].[PDX_PurchaseOrdersFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @ETAChangedPOs as table (OrderHeader_Id int)
declare @IncludedStores as table (Store_No int)
declare @today as datetime, @yesterday as datetime

set @today = Convert(Date, getdate(), 102)
set @yesterday = Convert(Date, getdate() - 1, 102)

insert into @ETAChangedPOs
select distinct(OrderHeader_Id)
from   infor.OrderExpectedDateChangeQueue edcq
where  edcq.InsertDate >= @yesterday and edcq.InsertDate < @today 

insert into @IncludedStores
select store_no
from   Store
where  mega_store = 1
union
select s.store_no
from   Store s
join   [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('WFMBannerStoresForOrdering', 'IRMA CLIENT'), '|') bs 
	   on s.BusinessUnit_ID = bs.Key_Value

declare @PDXSourceID int
select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

select oh.OrderHeader_Id as PO_NUMBER, ROW_NUMBER() OVER(PARTITION BY oh.OrderHeader_Id ORDER BY oi.OrderItem_ID) as PO_LINE_NUMBER, 
eoi.ExternalOrder_Id as PDX_PO_SUGGESTED_ID, 
RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, 
case when return_order = 1 
     then oi.QuantityOrdered * -1
	 else oi.QuantityOrdered
end as PO_QTY, 
s.BusinessUnit_ID as STORE_NUMBER, 
(case when oh.OrderType_ID = 3 then Cast(s.BusinessUnit_ID as varchar(10))
else RIGHT('0000000000'+ISNULL(v.PS_Vendor_ID,''),10) end) as VENDOR_NUMBER,
convert(varchar, oh.OrderDate, 112) as PO_CREATE_DATE, 
convert(varchar, oh.Expected_Date, 112) as PO_ETA_DATE, convert(varchar, oh.OriginalCloseDate, 112) as PO_CLOSE_DATE,
st.SubTeam_Name as PROD_SUBTEAM,
CASE 
    WHEN oh.Transfer_To_SubTeam = i.SubTeam_No
	THEN 'Y'
	ELSE 'N'
END AS HOST_SUBTEAM_FLAG,
rtrim(iu.Unit_Name) as CASE_UOM
from OrderHeader oh
join OrderItem oi on oh.OrderHeader_Id = oi.OrderHeader_Id 
join Item i on i.Item_Key = oi.Item_Key
join Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join @IncludedStores ist on psl.Store_no = ist.Store_No
join ItemIdentifier ii on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
join Store s on s.Store_No = psl.Store_no 
join Vendor v on oh.Vendor_ID = v.Vendor_ID
join SubTeam st on st.Subteam_No = oh.Transfer_To_SubTeam
join ItemUnit iu on iu.Unit_Id = oi.QuantityUnit
full outer join @ETAChangedPOs edcq
	 on edcq.OrderHeader_ID = oh.OrderHeader_ID
left outer join (select distinct OrderHeader_ID, ExternalOrder_Id from ExternalOrderInformation e
				  where e.ExternalSource_ID = @PDXSourceID) eoi on oh.OrderHeader_ID = eoi.OrderHeader_ID
where oh.Sent = 1
and i.Retail_Sale = 1
and oh.OrderType_ID <> 3
and (
		(SentDate < @today and SentDate >= @yesterday)
		or  (OriginalCloseDate < @today and OriginalCloseDate >= @yesterday)
        or  (vsc.InsertDate < @today and vsc.InsertDate >= @yesterday
             and SentDate < @today and SentDate >= @today - 5)
		or (edcq.OrderHeader_ID is not null)
	)
order by oh.OrderHeader_Id, PO_LINE_NUMBER, oi.OrderItem_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_PurchaseOrdersFile] TO [IRMAPDXExtractRole]
    AS [dbo];

