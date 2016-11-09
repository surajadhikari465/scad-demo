
CREATE PROCEDURE [dbo].[PDX_ReceiptOrdersFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @PDXSourceID int
select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

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
where --s.Mega_Store = 1
s.Store_Name like 'Belmar%'
and oh.Sent = 1
and oh.OrderType_ID <> 3
--and ((oi.DateReceived < Convert(Date, getdate(), 102) and oi.DateReceived > Convert(Date, getdate() - 1, 102))
-- or (oi.DateReceived < Convert(Date, getdate(), 102) and oi.DateReceived > Convert(Date, getdate()-5, 102) 
--and vsc.InsertDate < Convert(Date, getdate(), 102) and vsc.InsertDate > Convert(Date, getdate() - 1, 102)))
and oi.DateReceived < '2016-02-16' and oi.DateReceived > '2016-02-15'
and i.Retail_Sale = 1
order by s.BusinessUnit_ID, oh.OrderHeader_Id, PO_LINE_NUMBER, oi.OrderItem_ID
END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [PDX_ReceiptOrdersFile.sql]'