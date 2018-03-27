CREATE PROCEDURE [dbo].[AMZ_ItemSubscriptionFile_ByStore]
	@BusinessUnitID int
AS 
BEGIN
SET NOCOUNT ON

select 
	convert(varchar, getdate(), 112) as EFFECTIVE_DATE, 
	RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, 
	convert(varchar,s.BusinessUnit_ID) as STORE_NUMBER, 
	v.PS_Vendor_ID as VENDOR_NUMBER,
	case si.AUTHORIZED 
		when 1 
			then 'Y' 
		else 
			'N' 
	end as AUTHORIZED, 
	case isnull(sie.OrderedByInfor,0) 
		when 1 
			then 'Y' 
		else 
			'N' 
	end as ORDER_BY_PDX
from StoreItem si
join Store s on si.Store_No = s.Store_No
join StoreItemVendor siv on siv.Item_key = si.Item_key and siv.Store_No = si.Store_No and siv.PrimaryVendor = 1
join Vendor v on siv.Vendor_ID = v.Vendor_ID
join ItemIdentifier ii on si.Item_Key = ii.Item_key
join item i on i.item_key = ii.Item_Key
left join StoreItemExtended sie on sie.Item_key = i.Item_key and sie.Store_No = s.Store_No
where i.retail_sale = 1
and   s.BusinessUnit_ID = @BusinessUnitID
and   i.Deleted_Item = 0 and i.Remove_Item = 0
and   ii.Deleted_Identifier = 0 and Remove_Identifier = 0
order by s.BusinessUnit_ID, ii.Identifier
END

GRANT EXECUTE
    ON OBJECT::[dbo].[AMZ_ItemSubscriptionFile_ByStore] TO [IRMAPDXExtractRole]
    AS [dbo];