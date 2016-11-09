
CREATE PROCEDURE [dbo].[PDX_ItemVendorLaneFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

set transaction isolation level read uncommitted

select RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, 
convert(varchar,s.BusinessUnit_ID) as STORE_NUMBER,
rtrim(v.PS_Vendor_ID) as VENDOR_NUMBER, rtrim(v.CompanyName) as VENDOR_NAME, convert(varchar, vch.Package_Desc1) as CASE_SIZE, 
rtrim(ciu.Unit_Name) as CASE_UOM, vch.UnitCost as REG_COST, rtrim(riu.Unit_Name) as RETAIL_UOM
from StoreItemVendor siv
join Store s on siv.Store_No = s.Store_No
join Item i on siv.Item_key = i.Item_Key
join ItemIdentifier ii on i.Item_key = ii.Item_Key 
join VendorCostHistory vch on vch.StoreItemVendorID = siv.StoreItemVendorID
join Vendor v on siv.Vendor_ID = v.Vendor_ID
join ItemUnit ciu on vch.CostUnit_ID = ciu.Unit_ID
join ItemUnit riu on i.Retail_Unit_ID = riu.Unit_ID
where --s.Mega_Store = 1
s.Store_Name like '%BelMar%'
and i.deleted_item = 0 and i.Remove_item = 0
and i.retail_sale = 1
and ii.deleted_identifier = 0 and ii.remove_identifier = 0 and ii.default_identifier = 1
and (siv.deleteDate is null or siv.deleteDate > getdate())
and siv.PrimaryVendor = 1
and vch.VendorCostHistoryID = (select max(VendorCostHistoryID) from VendorCostHistory vch2 where vch2.StoreItemVendorID = vch.StoreItemVendorID and vch2.StartDate <= getdate())
order by ii.Identifier, s.BusinessUnit_ID
END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [PDX_ItemVendorLaneFile.sql]'