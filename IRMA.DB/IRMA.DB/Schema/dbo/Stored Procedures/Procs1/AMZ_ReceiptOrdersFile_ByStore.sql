CREATE PROCEDURE [dbo].[AMZ_ReceiptOrdersFile_ByStore]
	@BusinessUnitID int,
	@RunAsDate datetime
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @PDXSourceID int
select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

declare @today as datetime, @yesterday as datetime

set @today = Convert(Date, @RunAsDate, 102)
set @yesterday = Convert(Date, @today - 1, 102)

select 
	oh.OrderHeader_Id as PO_NUMBER, 
	eoi.ExternalOrder_Id as PDX_PO_SUGGESTED_ID, 
	oi.OrderItem_ID as PO_LINE_NUMBER, 
	RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, 
	s.BusinessUnit_ID as STORE_NUMBER, 
	CONVERT(varchar, oi.DateReceived, 112) as LI_RECEIVE_DAY, 
	ISNULL(oi.QuantityReceived, 0) as RCV_Qty,
	st.SubTeam_Name as PROD_SUBTEAM,
	CASE 
		WHEN oh.Transfer_To_SubTeam = i.SubTeam_No
			THEN 'Y'
		ELSE 
			'N'
	END AS HOST_SUBTEAM_FLAG,
	RTRIM(iu.Unit_Name) as CASE_UOM
from OrderHeader oh
join Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join Store s on s.Store_No = psl.Store_no 
join SubTeam st on st.Subteam_No = oh.Transfer_To_SubTeam
join OrderItem oi on oh.OrderHeader_Id = oi.OrderHeader_Id 
join Item i on oi.Item_Key = i.Item_Key
join ItemIdentifier ii on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
join ItemUnit iu on iu.Unit_Id = oi.QuantityUnit
left outer join ExternalOrderInformation eoi on oh.OrderHeader_ID = eoi.OrderHeader_ID and eoi.ExternalSource_ID = @PDXSourceID
where oh.Sent = 1
and s.BusinessUnit_ID = @BusinessUnitID
and oh.OrderType_ID <> 3
and oh.Return_Order = 0
and ((oi.DateReceived < @today and oi.DateReceived >= @yesterday)
 or (oh.ApprovedDate < @today and oh.ApprovedDate >= @yesterday and oi.DateReceived IS NULL))
and i.Retail_Sale = 1
order by s.BusinessUnit_ID, oh.OrderHeader_Id, oi.OrderItem_ID
END

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[AMZ_ReceiptOrdersFile_ByStore] TO [IRMAPDXExtractRole]
    AS [dbo];

GO