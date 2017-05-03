CREATE PROCEDURE [dbo].[Conversion_RegionAddToItemTemp]  
--	@fromRow int, 
--	@toRow int  
AS
declare 
    @tempEaUnitId int,
    @tempLbUnitId int,
    @tempCsUnitId int,
    @regCost int,
    @promoCost int,
    @edvCost int,
    @issCost int,
    @ldrCost int,
    @dscCost int,
    @newCost int,
    @tprCost int,
    @vendorCouponType int,
	@storeCouponType int,
	@vendorCouponSubteam int,
	@storeCouponSubteam int

 select @tempEaUnitId = Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA';
 select @tempLbUnitId = unit_id from itemunit where unit_abbreviation = 'lb';
 select @tempCsUnitId = Unit_Id from ItemUnit where  Unit_Abbreviation = 'CS';
 
select @regCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'REG'
select @promoCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'SAL'
select @edvCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'EDV'
select @issCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'ISS'
select @ldrCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'LDR'
select @dscCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'DSC'
select @newCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'NEW'
select @tprCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'TPR'
select @vendorCouponType = itemType_id from ItemType where itemType_Name = 'Manufacturer Coupon'
select @storeCouponType =  itemType_id from ItemType where itemType_Name = 'Store Coupon'
select @vendorCouponSubteam = subteam_no from subteam where subteam_name = 'VENDOR COUPON'
select @storeCouponSubteam = subteam_no from subteam where subteam_name = 'MKTG COUPONS'


--select pricechgtypeid from pricechgtype where pricechgtypedesc = 'DSC'

--------------
--start proc--
--------------

begin 
--get a the subset of the item_temp2 table

print cast(getdate() as char(20)) + ' Begin Conversion_RegionAddToItemTemp' --+  cast(@fromRow as varchar(10)) + ' ' + cast(@toRow as varchar(10));
select * into #item_test 
from 
item_temp2 it 
 where 
--lkey between @fromRow and @toRow
--and 
Category_ID > 0 --and identifier in (select upcno from dbo.cxspricd p where store > 0)
--and cast(identifier as bigint) in ('65005514217');

--where cast(identifier as bigint) in (20408600000, 50215,  29919500000, 4622, 9999007, 26374, 8974436499, 2991, 4849, 32969, 9999068, 1589,
--28407, 94670, 18071, 29898400000, 29898000000, 39221, 4253, 94448, 33080, 23910400000, 4774, 9999050, 2936, 29889100000, 15678, 16349,
--9999016, 9999011, 29885700000, 67593, 33079, 26372,  16211, 67566, 26926,  9999032, 25642, 9999088, 4328, 4933100230, 46483, 15870,
--29196, 94558, 94271, 8912181002, 34211, 50216, 9999063, 9999076, 29853200000, 20475800000, 29921400000, 29323700000, 47307)


--join cost tables
update #item_test --(Package_Desc1,Vendor_Item_ID,AvgCost,UnitCost)values(
	set Package_Desc1 = isnull(cst.casesize,1),
		   Vendor_Item_ID = cst.warehouse,
		   AvgCost	= isnull(cst.avgcost,0),
		   UnitCost = isnull(cst.casecost,0)
		   --Vendor_Key = v.Vendor_Key 
	from 
		cost_vendr cst with (nolock)
				--inner join vendor v with (nolock) on cst.vendor  = v.Vendor_Key 
			where
				cst.upcno = cix_upcno and
				cst.vendor = vendor_key and
				(cst.store = #item_test.store or cst.store=0)


print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: About to update cost info';
--update variables accordingly
update #item_test set Package_Desc1 = 1 where Package_Desc1 = 0
update #item_test set UnitCost = (UnitCost/Package_Desc1)  -- set to unit cost
update #item_test set Vendor_Unit_Id = @tempEaUnitId where  (scaleUom = 'BC') or (Package_Desc1 = 1)
update #item_test set Vendor_Unit_Id = @tempCsUnitId where  (scaleUom <> 'BC') or (Package_Desc1 <> 1)
update #item_test set Distribution_Unit_Id = Vendor_Unit_Id
update #item_test set CostedByWeight = 0,
				  Retail_Unit_Id = @tempEaUnitId,
				  Cost_Unit_Id = @tempEaUnitId
				  where  (scaleUom = 'BC')
update #item_test set CostedByWeight = 1,
				  Retail_Unit_Id = @tempLbUnitId,
				  Cost_Unit_Id = @tempLbUnitId
				  where  (isScaleItem = 1)
update #item_test set Cost_Unit_Id = Retail_Unit_Id where  (Cost_Unit_Id = 0)	


--select * from #item_test
--select dept, identifier, saleprice, price, posprice from #item_test
--select Identifier, tempChar1, tempChar2, pricechgtypeid from #item_test
--order by Identifier, tempChar1, tempChar2, pricechgtypeid


print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: updated cost info, about to get price info';



--select Identifier, tempChar1, tempChar2, pricechgtypeid from #item_test
--order by Identifier, tempChar1, tempChar2, pricechgtypeid
print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: updating zone store price info';
update #item_test 
--	set price = case cx.authrzd when 'Y' then p.holdprice else 0 end,
	set price = p.holdprice,
		Sale_End_Date = cast(DATEADD(day, -1, p.salprend) as smalldatetime),
		tempChar1 = p.ptype,
		tempChar2 = p.pzone,
		StopSale = case p.rescode when '9' then 1 when '09' then 1 else  0  end,
--		SalePrice = case cx.authrzd when 'Y' then p.effprice else 0 end,
		SalePrice = p.effprice ,
--select top 10 * from cxssstat
		Sale_Start_Date = cast(p.lastdate as smalldatetime),
		PosTare = p.tare,
		LinkCode = cast(p.linkcode as varchar(12)),
		GrillPrint = case p.jrnprint when 'Y' then 1 else 0 end,
		AgeCode = case p.rescode when '9' then 9 when '09' then 9 else  p.ageCode  end,
		VisualVerify = case p.visual when 'Y' then 1	else 0 end,
		SrCitizenDiscount = case p.si3 when 'Y' then 1 else 0 end,
		Multiple = p.holdpm,
		MsrpPrice = p.msrp,
		Sale_Multiple = p.effpm,
		IbmDiscount = case p.itemdisc when 'Y' then 1 else 0 end,
		CMP = case p.ptype when 'CMP' then 1 else 0      end,
		Restricted_Hours = case p.agecode when 2 then 1 else 0 end,
		PosLinkCode = p.miscint1
		from cxspricd p with (nolock)
				JOIN #item_test ON p.upcno = cix_upcno
				JOIN store st on st.businessunit_id = business_unit 
				JOIN dbo.cxssstat cx ON  cx.upcno = p.upcno and cx.store = st.store_no
				JOIN dbo.zone zo on st.zone_id = zo.zone_id --and cx.pzone = zo.glmarketingexpenseacct 
				where p.store=0 and p.pzone = cx.pzone and cx.prim_vend = 'Y'
--select top 10 * from dbo.cxssstat
--get price attributes for this UPC
print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: updating zone store price info';
update #item_test 
	set price = p.holdprice,
		Sale_End_Date = cast(DATEADD(day, -1, p.salprend) as smalldatetime),
		tempChar1 = p.ptype,
		tempChar2 = p.pzone,
		StopSale = case p.rescode when '9' then 1 when '09' then 1 else  0  end,
		SalePrice = p.effprice,
		Sale_Start_Date = cast(p.lastdate as smalldatetime),
		PosTare = p.tare,
		LinkCode = cast(p.linkcode as varchar(12)),
		GrillPrint = case p.jrnprint when 'Y' then 1 else 0 end,
		AgeCode = case p.rescode when '9' then 9 when '09' then 9 else  p.ageCode  end,
		VisualVerify = case p.visual when 'Y' then 1	else 0 end,
		SrCitizenDiscount = case p.si3 when 'Y' then 1 else 0 end,
		Multiple = p.holdpm,
		MsrpPrice = p.msrp,
		Sale_Multiple = p.effpm,
		IbmDiscount = case p.itemdisc when 'Y' then 1 else 0 end,
		CMP = case p.ptype when 'CMP' then 1 else 0      end,
		Restricted_Hours = case p.agecode when 2 then 1 else 0 end,
		PosLinkCode = p.miscint1
		from cxspricd p with (nolock)
				JOIN #item_test ON p.upcno = cix_upcno
				JOIN store st on st.businessunit_id = business_unit 
				JOIN dbo.zone zo on st.zone_id = zo.zone_id
				JOIN dbo.cxssstat cx ON  cx.upcno = p.upcno and cx.store = st.store_no
				where p.store = st.store_no and cx.prim_vend = 'Y'

		
print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: update poslinkcode to 0';
update #item_test
	set poslinkcode = 0
		
		

print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: updated cost info, about to get price info';
--get price attributes for this UPC
UPdate #item_test
Set LinkCode = '', PosLinkCode = ''
FROM #item_test it
JOIN  
(
select i.upcno, st.store
from
   dbo.cxbupcmr i
   JOIN dbo.cxssstat s ON s.upcno = i.upcno
   JOIN dbo.cxbstorr st ON st.store = s.store
   JOIN dbo.catflags cf ON cf.dept = i.dept and cf.setnumber = st.district and cf.commodity = i.commodity
   JOIN dbo.cxspricd p ON p.upcno = i.upcno and p.pzone = s.pzone
where i.upcno in (select identifier from #item_test where LinkCode > 1)
   --and s.prim_vend  = 'Y'
   and p.store      = 0
and cf.deposit = '0000000000000'
UNION
select i.upcno, st.store
from
   dbo.cxbupcmr i
   JOIN dbo.cxssstat s ON s.upcno = i.upcno
   JOIN dbo.cxbstorr st ON st.store = s.store
   JOIN dbo.catflags cf ON cf.dept = i.dept and cf.setnumber = st.district and cf.commodity = i.commodity
   JOIN dbo.cxspricd p ON p.upcno = i.upcno and p.pzone = s.pzone
where i.upcno in (select identifier from #item_test where LinkCode > 1)
   --and s.prim_vend  = 'Y'
   and p.store      = st.store
and cf.deposit = '0000000000000'
GROUP BY i.upcno, st.store
) as a
ON a.upcno = it.identifier 
				inner join store st on st.businessunit_id = it.business_unit AND a.store = st.store_no
				
--Done getting Price
	
print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: update onPromotion ';	
--now update corresponding price data
update #item_test set onPromotion = 1,
						edlp = 0,
						posSalePrice = SalePrice
		where tempChar1 = 'SAL'  
				or tempChar1 = 'DSC'
					or tempChar1 = 'ISS'
						or tempChar1 = 'LIN'
							or tempChar1 = 'MKT'
							   or tempChar1 = 'NEW'
							      or tempChar1 = 'LDR'
							         or tempChar1 = 'TPR'

print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: update EDV ';	
update #item_test set onPromotion = 1,
						edlp = 1,
						posSalePrice = SalePrice
			where tempChar1 = 'EDV' 
			 

print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp: update to cost variables ';	
update #item_test set pricechgtypeid = 
				case tempChar1
					when 'EDV' then @edvCost
					when 'SAL' then @promoCost
					when 'ISS' then @issCost
					when 'LDR' then @ldrCost
					when 'DSC' then @dscCost
					when 'NEW' then @newCost
					when 'TPR' then @tprCost
					else @regCost
				end
				
update #item_test set onPromotion = 0
			where SalePrice = Price

update #item_test set sale_End_Date = null,
			onPromotion = 0,
			edlp = 0,
--			salePrice = 0,
--			posSalePrice = 0,
		--	Sale_Start_Date = null,
			PriceChgTypeId = @regCost
		where sale_End_Date < getdate() or sale_End_Date is null 

update #item_test
	set Price = 0,
		SalePrice = 0,
		PosPrice = 0,
		PosSalePrice = 0,
		msrpPrice = 0,
		Restricted_Hours = 0,
		Sale_Multiple = 1,
		multiple = 1,
		ibmdiscount = 0,
		onPromotion = 0,
		PriceChgTypeId = @regCost
		 where Price is null or Price = 0

update #item_test set msrpPrice = price / multiple where MsrpPrice < 0 or MsrpPrice is null

update #item_test set posprice = price

--update items that have a subteam of coupon
update #item_test set item_type_id = @storeCouponType where subteam_no = @storeCouponSubteam  
update #item_test set item_type_id = @vendorCouponType where subteam_no = @vendorCouponSubteam      

Print cast(getdate() as char(20)) +'Conversion_RegionAddToItemTemp:  Done getting Price...'

--update #item_test set tempChar1 = cuv.prim_vend,
--					  tempChar2 = cuv.authrzd
--		from #item_test it, upc_vendr cuv with (nolock)
--			where 
--				cuv.upcno = it.cix_upcno and
--				cuv.store = it.store and
--				cuv.vendor = it.Vendor_Key

--update #item_test set isPrimary = 1 where tempChar1 = 'Y'
--update #item_test set isAuthorized = 1 where tempChar2 = 'Y'

--now update IRMA fields that are used to determine if an item is a scaleItem
Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  update scaleItem .....'
update #item_test set IdentifierType = 'O',
						 CheckDigit = null,
						 scale_identifier = 1 
					where 
						 isScaleItem = 1
	 
-- do some final cleanup
Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  Package_Desc2 .....'
update #item_test set Package_Desc2 = 1 where Package_Desc2 is null

--Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  taxclassid .....'
--update #item_test set taxclassid = 1 where taxclassid = 0

Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  scaletare .....'
update #item_test set scaletare = 0 where scaletare is null

Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  Discontinue_Item .....'
update #item_test set Discontinue_Item = 0 --where PriceChgTypeId = @dscCost

Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  Default Label Types .....'

UPDATE #item_test SET LabelTypeID = sa.LabelTypeID
from dbo.catflags c
JOIN dbo.cxbupcmr u ON c.commodity = u.commodity AND u.dept = c.dept
JOIN dbo.shelftagattribute sa ON sa.shelftag_type = Cast(c.tagtype as INT)
JOIN #item_test ii on  u.upcno = ii.Identifier
WHERE isNumeric(c.tagtype) = 1



UPDATE #item_test SET LabelTypeID = 1
WHERE cast( Identifier as bigint) between 20000000000 and 29999900000

UPDATE #item_test SET LabelTypeID = 1
WHERE cast( Identifier as bigint) < 100000 and subteam_no = 4040

--
--update item_temp set labeltypeid = 4 where labeltypeid = 5
--
--select * from shelftagattribute

--select distinct sa.LabelTypeID
--from dbo.catflags c
--JOIN dbo.cxbupcmr u ON c.commodity = u.commodity AND u.dept = c.dept
--JOIN dbo.shelftagattribute sa ON sa.shelftag_type = Cast(c.tagtype as INT)
--JOIN item_temp ii on  u.upcno = ii.Identifier
--WHERE isNumeric(c.tagtype) = 1

UPDATE #item_test SET LabelTypeID = 1
from dbo.catflags c
JOIN dbo.cxbupcmr u ON c.commodity = u.commodity AND u.dept = c.dept
JOIN #item_test ii on  u.upcno = ii.Identifier
WHERE LTRIM(RTRIM(c.tagtype)) IN ('00', '0', '000') 

UPDATE #item_test SET LabelTypeID = 3
WHERE LabelTypeID is null


--print 'store price'
--select  * from #item_test
Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  Done updating all data .....'

--now insert all of #item_test into item_temp
--clear item_temp first
Print cast(getdate() as char(20)) + 'Conversion_RegionAddToItemTemp:  Truncate, populate Item_Temp .....'
truncate table dbo.item_temp





	insert into Item_Temp   ([item_description]
           ,[subteam_no]
           ,[package_desc2]
           ,[package_unit_id]
           ,[category_id]
           ,[deleted_item]
           ,[discontinue_item]
           ,[pos_description]
           ,[price_required]
           ,[item_type_id]
           ,[not_availablenote]
           ,[insert_date]
           ,[category_name]
           ,[identifier]
           ,[price]
           ,[sale_end_date]
           ,[avgcost]
           ,[vendor_key]
           ,[unitcost]
           ,[package_desc1]
           ,[team_no]
           ,[team_name]
           ,[subteam_name]
           ,[dept_no]
           ,[target_margin]
           ,[default_identifier]
           ,[business_unit]
           ,[isPrimary]
           ,[onPromotion]
           ,[CheckDigit]
           ,[IdentifierType]
           ,[TaxClassId]
           ,[StopSale]
           ,[LabelTypeId]
           ,[CostedByWeight]
           ,[Cost_Unit_id]
           ,[freight_unit_id]
           ,[vendor_unit_id]
           ,[distribution_unit_id]
           ,[retail_unit_id]
           ,[vendor_item_id]
           ,[saleprice]
           ,[posprice]
           ,[possaleprice]
           ,[master_upc]
           ,[sale_start_date]
           ,[cmp]
           ,[food_stamps]
           ,[PosTare]
           ,[LinkCode]
           ,[GrillPrint]
           ,[AgeCode]
           ,[VisualVerify]
           ,[SrCitizenDiscount]
           ,[QtyProhibit]
           ,[GroupList]
           ,[Item_Key_Temp]
           ,[Item_Key_Temp2]
           ,[PricingMethod_ID]
           ,[ShelfLife_Id]
           ,[ShelfLife_Length]
           ,[Multiple]
           ,[MSRPPrice]
           ,[MSRPMultiple]
           ,[Sale_Multiple]
           ,[Sale_Earned_Disc1]
           ,[Sale_Earned_Disc2]
           ,[Sale_Earned_Disc3]
           ,[scaledesc1]
           ,[ingredients]
           ,[brandid]
           ,[discountable]
           ,[natclassid]
           ,[ScaleDesc2]
           ,[ScaleForcedTare]
           ,[ScaleTare]
           ,[ScaleDesc3]
           ,[ScaleDesc4]
           ,[restricted_hours]
           ,[edlp]
           ,[ibm_discount]
           ,[isAuthorized]
           ,[national_identifier]
           ,[PriceChgTypeId]
           ,[poslinkcode]
           ,[scale_identifier]
           ,[item_package_desc1]
           ,[by_count]
           ,[fixed_weight]
           ,[pzone]
           ,[scale_uom]
           ,[conversion_key])
    select
Item_Description	,
SubTeam_No	,
Package_Desc2	,
Package_Unit_ID	,
Category_ID	,
Deleted_Item	,
Discontinue_Item	,
POS_Description	,
Price_Required	,
Item_Type_ID	,
Not_AvailableNote	,
Insert_Date	,
Category_Name	,
Identifier	,
Price	,
sale_End_Date	,
AvgCost	,
Vendor_Key	,
UnitCost	,
Package_Desc1	,
Team_No	,
Team_Name	,
SubTeam_Name	,
Dept_No	,
Target_Margin	,
Default_Identifier	,
Business_Unit	,
isPrimary	,
onPromotion	,
CheckDigit	,
IdentifierType	,
TaxClassId	,
StopSale	,
LabelTypeID	,
CostedByWeight	,
Cost_Unit_ID	,
Freight_Unit_ID	,
Vendor_Unit_ID	,
Distribution_Unit_ID	,
Retail_Unit_ID	,
Vendor_Item_ID	,
SalePrice	,
PosPrice	,
PosSalePrice	,
master_upc	,
Sale_Start_Date	,
Cmp	,
Food_Stamps	,
PosTare	,
LinkCode	,
GrillPrint	,
AgeCode	,
VisualVerify	,
SrCitizenDiscount	,
QtyProhibit	,
GroupList	,
item_key_temp	,
item_key_temp2	,
PricingMethod_ID	,
ShelfLife_Id	,
ShelfLife_Length	,
Multiple	,
MSRPPrice	,
MSRPMultiple	,
Sale_Multiple	,
Sale_Earned_Disc1	,
Sale_Earned_Disc2	,
Sale_Earned_Disc3	,
scaledesc1	,
ingredients	,
brandid	,
discountable	,
natclassid	,
scaledesc2	,
scaleforcedtare	,
scaletare	,
scaledesc3	,
scaledesc4	,
Restricted_Hours	,
EDLP	,
IBMDiscount	,
isAuthorized,
nationalidentifier,
priceChgTypeId,
poslinkcode,
scale_identifier,
item_package_desc1,
by_count,
fixed_weight,
tempChar2,
scaleuom,
lkey		
from #item_test
--
----where cast(identifier as bigint) in (
----1259912709, 1553210056
----)

Print cast(getdate() as char(20)) +' Inserted into IRMA Item_Temp successfully .....'
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Conversion_RegionAddToItemTemp] TO [DataMigration]
    AS [dbo];

