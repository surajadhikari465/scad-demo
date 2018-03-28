CREATE PROCEDURE [dbo].[AMZ_DeletedOrdersFile_ByStore]
	@BusinessUnitID int,
	@RunAsDate datetime
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @today as datetime, @yesterday as datetime

set @today = Convert(Date, @RunAsDate, 102)
set @yesterday = Convert(Date, @today - 1, 102)

declare @PDXSourceID int
select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

select dod.OrderHeader_ID as PO_NUMBER, eoi.ExternalOrder_Id as PDX_PO_SUGGESTED_ID, convert(varchar, dod.DeleteDate, 112) as PO_DELETE_DATE
from deletedorder dod
join Vendor psl on dod.ReceiveLocation_ID = psl.Vendor_ID
join Store s on psl.Store_No = s.Store_no
left outer join ExternalOrderInformation eoi on dod.OrderHeader_ID = eoi.OrderHeader_ID and eoi.ExternalSource_ID = @PDXSourceID
where dod.Sent = 1
  and s.BusinessUnit_ID = @BusinessUnitID
  and dod.OrderType_ID <> 3
  and dod.DeleteDate < @today and dod.DeleteDate >= @yesterday
  and (dod.SentDate < @yesterday or eoi.ExternalOrder_Id is not NULL)
END

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[AMZ_DeletedOrdersFile_ByStore] TO [IRMAPDXExtractRole]
    AS [dbo];

GO