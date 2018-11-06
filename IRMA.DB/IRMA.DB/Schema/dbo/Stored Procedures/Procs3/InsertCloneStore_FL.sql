
create procedure [dbo].[InsertCloneStore_FL] 
	@Old_Store_No			int,
	@Store_No				int,
	@Store_Name				varchar(50),
	@BusinessUnit_Id		int,
	@StoreAbbr				varchar(5),
	@PSI_Store_No			int,
	@RegionCode				varchar(4),
	@Zone_Name				varchar(100),
	@GLMarketingExpenseAcct int,
	@Addr1					varchar(100),
	@Addr2					varchar(100),
	@City					varchar(100),
	@State					varchar(100),
	@Zip					int,
	@Country				varchar(100)

as
begin

declare @Zone_Id int

select @Zone_Id = Zone_Id
from Zone 
where Zone_Name = @Zone_Name

if (@Zone_Id is null)
	begin

	insert into Zone (Zone_Name, Region_Id, GLMarketingExpenseAcct)
	select @Zone_Name, Region_Id, @GLMarketingExpenseAcct
	from Region 
	where RegionCode = @RegionCode

	select @Zone_ID = SCOPE_IDENTITY()

	end

--SET IDENTITY_INSERT TaxJurisdiction ON
--INSERT INTO TaxJurisdiction (TaxJurisdictionID, TaxJurisdictionDesc)
--VALUES (@Store_No, @Store_Name)
--SET IDENTITY_INSERT TaxJurisdiction OFF

insert into Store (Store_No,Store_Name,Phone_Number,Mega_Store,Distribution_Center,Manufacturer,WFM_Store,
	Internal,TelnetUser,TelnetPassword,BatchID,BatchRecords,BusinessUnit_ID,Zone_ID,UNFI_Store,
	LastRecvLogDate,LastRecvLog_No,RecvLogUser_ID,EXEWarehouse,Regional,LastSalesUpdateDate,
	StoreAbbr,PLUMStoreNo,TaxJurisdictionID,POSSystemId,PSI_Store_No)
select @Store_No,@Store_Name,Phone_Number,Mega_Store,Distribution_Center,Manufacturer,WFM_Store,
	Internal,TelnetUser,TelnetPassword,BatchID,BatchRecords,@BusinessUnit_ID,@Zone_ID,UNFI_Store,
	LastRecvLogDate,LastRecvLog_No,RecvLogUser_ID,EXEWarehouse,Regional,LastSalesUpdateDate,
	@StoreAbbr,@Store_No,1,POSSystemId,@PSI_Store_No
from Store 
where Store_No = @Old_Store_No

insert into vendor (Vendor_Key, CompanyName, Address_Line_1,Address_Line_2,City,State,Zip_Code,Country,
	Customer, InternalCustomer, ActiveVendor, Store_No, Order_By_Distribution,Electronic_Transfer,
	WFM,Non_Product_Vendor,EFT,InStoreManufacturedProducts,EXEWarehouseVendSent,EXEWarehouseCustSent, AddVendor)
SELECT @RegionCode + StoreAbbr,
	Store_Name,
	@Addr1,
	@Addr2,
	@City,
	@State,
	@Zip,
	@Country,
	1 as Customer,
	1 as InternalCustomer,
	0 as ActiveVendor,
	Store_No,
        0						AS Order_By_Distribution,
        0						AS Electronic_Transfer,
        0						AS WFM,
        0						AS Non_Product_Vendor,
        0						AS EFT,
        0						AS InStoreManufacturedProducts,
        0						AS EXEWarehouseVendSent,
        1						AS EXEWarehouseCustSent,
		0						AS AddVendor
from Store
where Store_No = @Store_No

insert into StoreSubTeam(Store_No, Team_No, SubTeam_No, CasePriceDiscount, CostFactor, ICVID)
select 
	@Store_No,
	Team_No, SubTeam_No, CasePriceDiscount, CostFactor, ICVID
from StoreSubTeam
where Store_No = @Old_Store_No

--insert into storeftpconfig
--select S.Store_No, 'POS', '10.16.0.14','swplumlab','plumlab','Item\'+S.StoreAbbr,NULL, 0
--from Store S
--where S.Store_No = @Store_No 

--insert into storeftpconfig
--select S.Store_No, 'SCALE', '10.16.0.14','swplumlab','plumlab','Scale\'+S.StoreAbbr,NULL, 0
--from Store S
--where S.Store_No = @Store_No 

--insert into storeftpconfig
--select S.Store_No, 'TAG', '10.80.0.28','IRMATags','Pr1ntL@b','PrintLab_Batches/'+S.StoreAbbr,NULL, 0
--from Store S
--where S.Store_No = @Store_No  

--insert into storeftpconfig
--select S.Store_No, 'POSPull', 'SW'+S.StoreAbbr,'frends','frends1',NULL, NULL, 0
--from Store S
--where S.Store_No = @Store_No 


--insert into StorePOSConfig 
--select @Store_No, max(POSFileWriterKey), 'direct'
--from POSWriter 
--where FileWriterType = 'POS'
--and Disabled = 0


--insert into StoreScaleConfig
--select @Store_No, max(POSFileWriterKey)
--from POSWriter 
--where FileWriterType = 'SCALE'
--and Disabled = 0


--insert into StoreShelfTagConfig (Store_No, POSFileWriterKey, ConfigType)
--select @Store_No, max(POSFileWriterKey), 'direct'
--from POSWriter 
--where FileWriterType = 'TAG'


--insert into taxdefinition 
--select Distinct 
--	J.TaxJurisdictionId, 
--	D.TaxFlagKey,
--	0.0000,
--	NULL
--from TaxJurisdiction J
--cross join taxdefinition D
--where J.TaxJurisdictionId = @Store_No

--insert into TaxFlag
--select distinct
--	C.TaxClassId,
--	J.TaxJurisdictionId,
--	D.TaxFlagKey,
--	0
--from TaxJurisdiction J
--cross join taxdefinition D
--cross join TaxClass C
--where J.TaxJurisdictionId = @Store_No


insert into Price (
	Item_Key,Store_No,Multiple,Price,MSRPPrice,MSRPMultiple,PricingMethod_ID,
	Sale_Multiple,Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Max_Quantity,
	Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Restricted_Hours,
	AvgCostUpdated,IBM_Discount,POSPrice,POSSale_Price,NotAuthorizedForSale,
	CompFlag,PosTare,LinkedItem,GrillPrint,AgeCode,VisualVerify,SrCitizenDiscount,
	PriceChgTypeId,ExceptionSubteam_No,POSLinkCode,KitchenRoute_ID,Routing_Priority,
	Consolidate_Price_To_Prev_Item,Print_Condiment_On_Receipt,Age_Restrict,
	CompetitivePriceTypeID,BandwidthPercentageHigh,BandwidthPercentageLow,MixMatch)
select 
	Item_Key,
	@Store_No,
	Multiple,Price,MSRPPrice,MSRPMultiple,PricingMethod_ID,
	Sale_Multiple,Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Max_Quantity,
	Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Restricted_Hours,
	AvgCostUpdated,IBM_Discount,POSPrice,POSSale_Price,NotAuthorizedForSale,
	CompFlag,PosTare,LinkedItem,GrillPrint,AgeCode,VisualVerify,SrCitizenDiscount,
	PriceChgTypeId,ExceptionSubteam_No,POSLinkCode,KitchenRoute_ID,Routing_Priority,
	Consolidate_Price_To_Prev_Item,Print_Condiment_On_Receipt,Age_Restrict,
	CompetitivePriceTypeID,BandwidthPercentageHigh,BandwidthPercentageLow,MixMatch
from Price
where Store_No = @Old_Store_No

insert into SignQueue (Item_Key,Store_No,Sign_Description,Ingredients,Identifier,
	Sold_By_Weight,Multiple,Price,MSRPMultiple,MSRPPrice,Case_Price,Sale_Multiple,
	Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Earned_Disc1,Sale_Earned_Disc2,
	Sale_Earned_Disc3,PricingMethod_ID,SubTeam_No,Origin_Name,Brand_Name,
	Retail_Unit_Abbr,Retail_Unit_Full,Package_Unit,Package_Desc1,Package_Desc2,
	Sign_Printed,Organic,Vendor_Id,User_ID,User_ID_Date,ItemType_ID,ScaleDesc1,
	ScaleDesc2,POS_Description,Restricted_Hours,Quantity_Required,Price_Required,
	Retail_Sale,Discountable,Food_Stamps,IBM_Discount,New_Item,Price_Change,
	Item_Change,LastQueuedType,POSPrice,POSSale_Price,PriceChgTypeId,TagTypeID,TagTypeID2)
select 
	Item_Key,
	@Store_No,
	Sign_Description,Ingredients,Identifier,
	Sold_By_Weight,Multiple,Price,MSRPMultiple,MSRPPrice,Case_Price,Sale_Multiple,
	Sale_Price,Sale_Start_Date,Sale_End_Date,Sale_Earned_Disc1,Sale_Earned_Disc2,
	Sale_Earned_Disc3,PricingMethod_ID,SubTeam_No,Origin_Name,Brand_Name,
	Retail_Unit_Abbr,Retail_Unit_Full,Package_Unit,Package_Desc1,Package_Desc2,
	Sign_Printed,Organic,Vendor_Id,User_ID,User_ID_Date,ItemType_ID,ScaleDesc1,
	ScaleDesc2,POS_Description,Restricted_Hours,Quantity_Required,Price_Required,
	Retail_Sale,Discountable,Food_Stamps,IBM_Discount,New_Item,Price_Change,
	Item_Change,LastQueuedType,POSPrice,POSSale_Price,PriceChgTypeId,TagTypeID,TagTypeID2
from SignQueue
where Store_No = @Old_Store_No

insert into StoreItemVendor (Store_No, Item_Key, Vendor_Id, AverageDelivery, PrimaryVendor)
Select
	@Store_No,
	Item_Key,
	Vendor_Id,
	AverageDelivery,
	PrimaryVendor
from StoreItemVendor
where Store_No = @Old_Store_No
and DeleteDate is null
order by StoreItemVendorId

insert into storeitem (store_no, Item_Key)
select @Store_No, Item_Key
from Item

insert into VendorCostHistory (StoreItemVendorId, Promotional, UnitCost, UnitFreight, Package_Desc1,
	StartDate, EndDate, FromVendor, MSRP, InsertDate, InsertWorkStation, CostUnit_Id, FreightUnit_Id,
	IsFromJDASync)
select 
	New.StoreItemVendorId,
	Old.Promotional, 
	Old.UnitCost, 
	Old.UnitFreight, 
	Old.Package_Desc1,
	Old.StartDate, 
	Old.EndDate, 
	Old.FromVendor, 
	Old.MSRP, 
	Old.InsertDate, 
	Old.InsertWorkStation, 
	Old.CostUnit_Id, 
	Old.FreightUnit_Id,
	Old.IsFromJDASync
from StoreItemVendor New
inner join StoreItemVendor SIV
	on New.Item_Key = SIV.Item_Key
	and New.Vendor_Id = SIV.Vendor_Id
	and New.Store_No = @Store_No 
	and SIV.Store_No = @Old_Store_No
inner join VendorCostHistory Old 
	on Old.StoreItemVendorId = SIV.StoreItemVendorId
order by Old.VendorCostHistoryId

insert into VendorDealHistory (StoreItemVendorID,CaseQty,Package_Desc1,CaseAmt,StartDate,
	EndDate,VendorDealTypeID,FromVendor,InsertDate,InsertWorkStation,CostPromoCodeTypeID,NotStackable)
select
	New.StoreItemVendorID,
	Old.CaseQty,
	Old.Package_Desc1,
	Old.CaseAmt,
	Old.StartDate,
	Old.EndDate,
	Old.VendorDealTypeID,
	Old.FromVendor,
	Old.InsertDate,
	Old.InsertWorkStation,
	Old.CostPromoCodeTypeID,
	Old.NotStackable
from StoreItemVendor New
inner join StoreItemVendor SIV
	on New.Item_Key = SIV.Item_Key
	and New.Vendor_Id = SIV.Vendor_Id
	and New.Store_No = @Store_No 
	and SIV.Store_No = @Old_Store_No
inner join VendorDealHistory Old 
	on Old.StoreItemVendorId = SIV.StoreItemVendorId
order by Old.VendorDealHistoryId

delete pricebatchdetail
where store_no = @Store_No

-- we're still having problems with the auth stuff.  Arg.  

update StoreItem 
set Authorized = SI2.Authorized
from StoreItem, StoreItem SI2
where StoreItem.Item_Key = SI2.Item_Key
and StoreItem.Store_No = @Store_No 
and SI2.Store_No = @Old_Store_No

end

