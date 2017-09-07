CREATE PROCEDURE [dbo].[PDX_ItemSubscriptionFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

declare @IncludedStores as table (Store_No int)

insert into @IncludedStores
select store_no
from   Store
where  mega_store = 1
union
select s.store_no
from   Store s
join   [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('WFMBannerStoresForOrdering', 'IRMA CLIENT'), '|') bs 
	   on s.BusinessUnit_ID = bs.Key_Value

select convert(varchar, getdate(), 112) as EFFECTIVE_DATE, RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) as UPC, 
convert(varchar,s.BusinessUnit_ID) as STORE_NUMBER, v.PS_Vendor_ID as VENDOR_NUMBER,
case si.AUTHORIZED when 1 then 'Y' else 'N' end as AUTHORIZED, 
case isnull(ia.Check_Box_20,0) when 1 then 'Y' else 'N' end as ORDER_BY_PDX
from StoreItem si
join @IncludedStores ist on si.Store_no = ist.Store_No
join Store s on si.Store_No = s.Store_No
join StoreItemVendor siv on siv.Item_key = si.Item_key and siv.Store_No = si.Store_No and siv.PrimaryVendor = 1
join Vendor v on siv.Vendor_ID = v.Vendor_ID
join ItemIdentifier ii on si.Item_Key = ii.Item_key
join item i on i.item_key = ii.Item_Key
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier 
left join ItemAttribute ia on ia.Item_key = i.Item_key
where i.retail_sale = 1
and i.Deleted_Item = 0 and i.Remove_Item = 0
and ii.Deleted_Identifier = 0 and Remove_Identifier = 0
order by s.BusinessUnit_ID, ii.Identifier
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_ItemSubscriptionFile] TO [IRMAPDXExtractRole]
    AS [dbo];

