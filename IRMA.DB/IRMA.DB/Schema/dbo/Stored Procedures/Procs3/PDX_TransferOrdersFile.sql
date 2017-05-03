
CREATE PROCEDURE [dbo].[PDX_TransferOrdersFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

select oh.OrderHeader_Id as PO_NUMBER, ROW_NUMBER() OVER(PARTITION BY oh.OrderHeader_Id ORDER BY oi.OrderItem_ID) as PO_LINE_NUMBER, 
RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, 
case when return_order = 1 
     then oi.QuantityOrdered * -1
	 else oi.QuantityOrdered
end as PO_QTY, 
s.BusinessUnit_ID as FROM_STORE_NUMBER, 
(case when oh.OrderType_ID = 3 then Cast(s.BusinessUnit_ID as varchar(10))
else RIGHT('0000000000'+ISNULL(v.PS_Vendor_ID,''),10) end) as TO_STORE_NUMBER,
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
join OrderItem oi on oh.OrderHeader_Id = oi.OrderHeader_Id 
join Item i on i.Item_Key = oi.Item_Key
join Vendor psl on oh.PurchaseLocation_ID = psl.Vendor_ID
join ItemIdentifier ii on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1 and ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
join Store s on s.Store_No = psl.Store_no 
join Vendor v on oh.Vendor_ID = v.Vendor_ID
join SubTeam tst on tst.Subteam_No = oh.Transfer_To_SubTeam
join SubTeam fst on fst.Subteam_No = oh.Transfer_SubTeam
join ItemUnit iu on iu.Unit_Id = oi.QuantityUnit
where s.mega_store = 1
 --   s.Store_Name like 'Belmar%'
and  oh.Sent = 1
and i.Retail_Sale = 1
and oh.OrderType_ID = 3
and oh.PurchaseLocation_ID = oh.Vendor_ID --In-store transfer only
and (
		(OrderDate < Convert(Date, getdate(), 102) and OrderDate >= Convert(Date, getdate() - 1, 102))
		or  (CloseDate < Convert(Date, getdate(), 102) and CloseDate >= Convert(Date, getdate() - 1, 102))
        or  (vsc.InsertDate < Convert(Date, getdate(), 102) and vsc.InsertDate >= Convert(Date, getdate() - 1, 102)
             and OrderDate < Convert(Date, getdate(), 102) and OrderDate >= Convert(Date, getdate() - 5, 102))
	)
--and ((OrderDate >= '2016-02-01' and OrderDate < '2016-02-02')
-- or (CloseDate >= '2016-02-01' and CloseDate < '2016-02-02'))
order by oh.OrderHeader_Id, PO_LINE_NUMBER, oi.OrderItem_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_TransferOrdersFile] TO [IRMAPDXExtractRole]
    AS [dbo];

