CREATE PROCEDURE [dbo].[StoreItemVendorExtract]
	@Region nchar(2)
AS 
BEGIN
SET NOCOUNT ON

declare @today datetime = GETDATE()
declare @todayUTC datetime2(7) = GETUTCDATE()
declare @IncludedStores table (Store_No int, BusinessUnit_ID int)

insert into @IncludedStores (Store_No, BusinessUnit_ID)
select s.Store_No, s.BusinessUnit_ID
from Store s
join StoreRegionMapping srm on s.Store_No = srm.Store_No
where Internal = 1 
and (s.WFM_Store = 1 or s.Mega_Store = 1)
and srm.Region_Code = @Region

select 
	@Region as Region, 
	vsc.InforItemId as ItemID, 
	s.BusinessUnit_ID as BusinessUnitID, 
	v.CompanyName as SupplierName, 
	iv.Item_ID as SupplierItemID,
	vdch.Package_Desc1 as SupplierCaseSize,  
	v.Vendor_Key as IrmaVendorKey, 
	@todayUTC as AddedDateUtc
from StoreItemVendor siv
join item i on siv.Item_Key = i.Item_Key
join itemidentifier ii on ii.Item_key = i.Item_key
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
join Store s on s.Store_No = siv.Store_No
join Vendor v on v.Vendor_ID = siv.Vendor_ID
join ItemVendor iv on iv.Item_Key = siv.Item_key and iv.Vendor_ID = siv.Vendor_ID
join (SELECT MAX(VendorCostHistoryID) AS VendorCostHistoryID, vch.StoreItemVendorID
		  FROM VendorCostHistory vch
		  JOIN StoreItemVendor siv ON vch.StoreItemVendorID = siv.StoreItemVendorID
		  JOIN Item i on siv.Item_Key = i.Item_Key
		  JOIN @IncludedStores s on s.Store_No = siv.Store_No
		 WHERE vch.StartDate <= @today
		   AND siv.DeleteDate IS NULL
		   AND siv.PrimaryVendor = 1
		   AND i.Deleted_Item = 0 and i.Remove_Item = 0
	  GROUP BY vch.StoreItemVendorID) vchmc
	  on vchmc.StoreItemVendorID = siv.StoreItemVendorID
JOIN VendorCostHistory vdch on vdch.VendorCostHistoryID = vchmc.VendorCostHistoryID
where siv.PrimaryVendor = 1
and   siv.DeleteDate is null
and   i.Deleted_Item = 0 and i.Remove_Item = 0
and   ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0
END

GO
