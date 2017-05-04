

CREATE PROCEDURE [dbo].[PDX_DeletedOrdersFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @PDXSourceID int
select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

select dod.OrderHeader_ID as PO_NUMBER, eoi.ExternalOrder_Id as PDX_PO_SUGGESTED_ID, convert(varchar, dod.DeleteDate, 112) as PO_DELETE_DATE
from deletedorder dod
join Vendor psl on dod.ReceiveLocation_ID = psl.Vendor_ID
join Store s on s.Store_No = psl.Store_no
left outer join ExternalOrderInformation eoi on dod.OrderHeader_ID = eoi.OrderHeader_ID and eoi.ExternalSource_ID = @PDXSourceID
where s.mega_store = 1
  --  s.Store_Name like 'Belmar%'
  and dod.Sent = 1
  and dod.OrderType_ID <> 3
  and dod.DeleteDate < Convert(Date, getdate(), 102) and dod.DeleteDate >= Convert(Date, getdate() - 1, 102)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_DeletedOrdersFile] TO [IRMAPDXExtractRole]
    AS [dbo];

