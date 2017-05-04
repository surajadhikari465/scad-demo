
CREATE PROCEDURE [dbo].[PDX_PurchaseOrdersFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @ETAChangedPOs as table (OrderHeader_Id int)
declare @today as datetime, @yesterday as datetime

set @today = Convert(Date, getdate(), 102)
set @yesterday = Convert(Date, getdate() - 1, 102)

-- Catch the Expected Date change when there're at least two rows on the OrderHeaderHistory table with InsertDate of yesterday, and the Expected Dates on the two rows are different.
insert into @ETAChangedPOs
select ohh.OrderHeader_ID 
from orderheaderhistory ohh
join orderheader oh on ohh.OrderHeader_ID = oh.OrderHeader_ID
join Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join Store s on s.Store_No = psl.Store_no 
where s.mega_store = 1
and oh.Sent = 1
and oh.OrderType_ID <> 3
and ohh.insertdate < @today and ohh.insertdate >= @yesterday
and oh.OriginalCloseDate is null
group by ohh.orderheader_id
having max(ohh.Expected_Date) <> min(ohh.Expected_Date)

-- Catch the Expected Date change when the Expected Date change is recorded on two OrderHeaderHistory records with different InsertDate.
insert into @ETAChangedPOs
select ohh1.OrderHeader_ID 
from orderheaderhistory ohh1
join orderheader oh on ohh1.OrderHeader_ID = oh.OrderHeader_ID
join Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join Store s on s.Store_No = psl.Store_no 
where s.mega_store = 1
and oh.Sent = 1
and oh.OrderType_ID <> 3
and ohh1.insertdate < @today and ohh1.insertdate >= @yesterday
and oh.OriginalCloseDate is null
and ohh1.OrderHeader_ID not in (select OrderHeader_ID from @ETAChangedPOs)
group by ohh1.orderheader_id
having max(ohh1.Expected_Date) <>
    (    
        select top 1 Expected_Date 
        from OrderHeaderHistory ohh2
        where ohh2.OrderHeader_ID = ohh1.OrderHeader_ID
        and ohh2.insertdate < @yesterday
        order by ohh2.InsertDate desc, Expected_Date desc
    )

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
join ItemIdentifier ii on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
join Store s on s.Store_No = psl.Store_no 
join Vendor v on oh.Vendor_ID = v.Vendor_ID
join SubTeam st on st.Subteam_No = oh.Transfer_To_SubTeam
join ItemUnit iu on iu.Unit_Id = oi.QuantityUnit
left outer join (select distinct OrderHeader_ID, ExternalOrder_Id from ExternalOrderInformation e
				  where e.ExternalSource_ID = @PDXSourceID) eoi on oh.OrderHeader_ID = eoi.OrderHeader_ID
where s.mega_store = 1
  --  s.Store_Name like 'Belmar%'
and  oh.Sent = 1
and i.Retail_Sale = 1
and oh.OrderType_ID <> 3
and (
		(SentDate < @today and SentDate >= @yesterday)
		or  (OriginalCloseDate < @today and OriginalCloseDate >= @yesterday)
        or  (vsc.InsertDate < @today and vsc.InsertDate >= @yesterday
             and SentDate < @today and SentDate >= Convert(Date, getdate() - 5, 102))
	    or  (oh.OrderHeader_ID in (select OrderHeader_ID from @ETAChangedPOs))
	)
order by oh.OrderHeader_Id, PO_LINE_NUMBER, oi.OrderItem_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_PurchaseOrdersFile] TO [IRMAPDXExtractRole]
    AS [dbo];

