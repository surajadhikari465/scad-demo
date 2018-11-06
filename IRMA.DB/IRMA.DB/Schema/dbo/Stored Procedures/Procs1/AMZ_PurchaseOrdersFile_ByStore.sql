CREATE PROCEDURE [dbo].[AMZ_PurchaseOrdersFile_ByStore]
	@BusinessUnitID int,
	@RunAsDate datetime
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @ETAChangedPOs as table (OrderHeader_Id int)
declare @today as datetime, @yesterday as datetime

set @today = Convert(Date, @RunAsDate, 102)
set @yesterday = Convert(Date, @today - 1 , 102)

insert into @ETAChangedPOs
select distinct(OrderHeader_Id)
from   infor.OrderExpectedDateChangeQueue edcq
where  edcq.InsertDate >= @yesterday and edcq.InsertDate < @today 

declare @PDXSourceID int
select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

select 
	oh.OrderHeader_Id as PO_NUMBER, 
	oi.OrderItem_ID as PO_LINE_NUMBER, 
	eoi.ExternalOrder_Id as PDX_PO_SUGGESTED_ID, 
	ii.Identifier as UPC, 
	case 
		when return_order = 1 
			then oi.QuantityOrdered * -1
		else 
			oi.QuantityOrdered
		end as PO_QTY, 
	s.BusinessUnit_ID as STORE_NUMBER, 
	RIGHT('0000000000'+ISNULL(v.PS_Vendor_ID,''),10) as VENDOR_NUMBER,
	convert(varchar, oh.OrderDate, 112) as PO_CREATE_DATE, 
	convert(varchar, oh.Expected_Date, 112) as PO_ETA_DATE, 
	convert(varchar, oh.OriginalCloseDate, 112) as PO_CLOSE_DATE,
	st.SubTeam_Name as PROD_SUBTEAM,
	case 
		when oh.Transfer_To_SubTeam = i.SubTeam_No
			then 'Y'
		else 
			'N'
	end as HOST_SUBTEAM_FLAG,
	rtrim(iu.Unit_Name) as CASE_UOM
from OrderHeader oh
join OrderItem oi on oh.OrderHeader_Id = oi.OrderHeader_Id 
join Item i on i.Item_Key = oi.Item_Key
join Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join Store s on s.Store_No = psl.Store_no
join ItemIdentifier ii on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
join Vendor v on oh.Vendor_ID = v.Vendor_ID
join SubTeam st on st.Subteam_No = oh.Transfer_To_SubTeam
join ItemUnit iu on iu.Unit_Id = oi.QuantityUnit
full outer join @ETAChangedPOs edcq
	 on edcq.OrderHeader_ID = oh.OrderHeader_ID
left outer join (select distinct OrderHeader_ID, ExternalOrder_Id from ExternalOrderInformation e
				  where e.ExternalSource_ID = @PDXSourceID) eoi on oh.OrderHeader_ID = eoi.OrderHeader_ID
where oh.Sent = 1
and s.BusinessUnit_ID = @BusinessUnitID
and i.Retail_Sale = 1
and oh.OrderType_ID <> 3
and (
		(SentDate < @today and SentDate >= @yesterday)
		or  (OriginalCloseDate < @today and OriginalCloseDate >= @yesterday)
		or (edcq.OrderHeader_ID is not null)
	)
order by oh.OrderHeader_Id, oi.OrderItem_ID
END

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[AMZ_PurchaseOrdersFile_ByStore] TO [IRMAPDXExtractRole]
    AS [dbo];

GO