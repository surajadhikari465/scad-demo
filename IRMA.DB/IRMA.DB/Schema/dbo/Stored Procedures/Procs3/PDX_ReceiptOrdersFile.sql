CREATE PROCEDURE [dbo].[PDX_ReceiptOrdersFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @PDXSourceID int
select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

declare @today as datetime, @yesterday as datetime, @fiveDaysAgo as datetime

set @today = Convert(Date, getdate(), 102)
set @yesterday = Convert(Date, getdate() - 1, 102)
set @fiveDaysAgo = Convert(Date, getdate()-5, 102)

select oh.OrderHeader_Id as PO_NUMBER, eoi.ExternalOrder_Id as PDX_PO_SUGGESTED_ID, ROW_NUMBER() OVER(PARTITION BY oh.OrderHeader_Id ORDER BY oi.OrderItem_ID) as PO_LINE_NUMBER, 
RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, s.BusinessUnit_ID as STORE_NUMBER, 
convert(varchar, oi.DateReceived, 112) as LI_RECEIVE_DAY, oi.QuantityReceived as RCV_Qty,
st.SubTeam_Name as PROD_SUBTEAM,
CASE 
    WHEN oh.Transfer_To_SubTeam = i.SubTeam_No
	THEN 'Y'
	ELSE 'N'
END AS HOST_SUBTEAM_FLAG,
rtrim(iu.Unit_Name) as CASE_UOM
from OrderHeader oh
join OrderItem oi on oh.OrderHeader_Id = oi.OrderHeader_Id 
join Item i on oi.Item_Key = i.Item_Key
join Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join ItemIdentifier ii on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
join Store s on s.Store_No = psl.Store_no 
join SubTeam st on st.Subteam_No = oh.Transfer_To_SubTeam
join ItemUnit iu on iu.Unit_Id = oi.QuantityUnit
left outer join ExternalOrderInformation eoi on oh.OrderHeader_ID = eoi.OrderHeader_ID and eoi.ExternalSource_ID = @PDXSourceID
where s.Mega_Store = 1
and oh.Sent = 1
and oh.OrderType_ID <> 3
and oh.Return_Order = 0
and ((oi.DateReceived < @today and oi.DateReceived >= @yesterday)
 or (oi.DateReceived < @today and oi.DateReceived >= @fiveDaysAgo 
and vsc.InsertDate < @today and vsc.InsertDate >= @yesterday)
 or (oh.ApprovedDate < @today and oh.ApprovedDate >= @yesterday and oi.DateReceived IS NULL)
 or (oh.ApprovedDate < @today and oh.ApprovedDate >= @fiveDaysAgo and oi.DateReceived IS NULL  
and vsc.InsertDate < @today and vsc.InsertDate >= @yesterday))
and i.Retail_Sale = 1
order by s.BusinessUnit_ID, oh.OrderHeader_Id, PO_LINE_NUMBER, oi.OrderItem_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_ReceiptOrdersFile] TO [IRMAPDXExtractRole]
    AS [dbo];

