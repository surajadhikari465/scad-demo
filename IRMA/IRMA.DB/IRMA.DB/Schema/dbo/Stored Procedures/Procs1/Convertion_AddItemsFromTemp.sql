CREATE   PROCEDURE [dbo].[Convertion_AddItemsFromTemp]  AS

--**********************************************************************************************
-- Procedure: Convertion_AddItemsFromTemp()
--
-- Description:
-- This procedure builds a dynamic query to return information for the ItemSearch screen.
--
-- Change History:
-- Date			Init.	TFS		Description
-- 2013/01/14	BS		8755	Removed reference to Item.Discontinue_Item
--***********************************************************************************************

declare @item_key int,
    @itemDescription varchar(60),
    @subTeamNo int,
    @packageDesc2 decimal(9,4),
    @packageUnitId int,
    @categoryId int,
    @deletedItem int,
    @discontinueItem int,
    @posDescription varchar(26),
    @priceRequired int,
    @itemTypeId int,
    @notAvailableNote int,
    @insertDate smalldatetime,
    @categoryName varchar(35),
    @identifier varchar(13),
    @price smallmoney,
    @saleEndDate smalldatetime,
    @avgCost smallmoney,
    @vendorKey varchar(10),
    @unitCost smallmoney,
    @packageDesc1 decimal(9,4),
    @teamNo int,
    @teamName varchar(100),
    @subTeamName varchar(100),
    @deptNo int,
    @targetMargin decimal,
    @defaultIdentifier int,
    @business_unit int,
    @isPrimary int,
    @onPromotion int,
    @Error_No int	,
    @max_Item_Key int,
    @max_StoreItemVendorId int,
    @today smalldatetime,
    @vendorId int,
	@max_count int,
	@max_count2 int,
	@max_count3 int,
	@taxClassId int,
	@identifier_count int,
	@checkDigit varchar(1),
	@identifierType varchar(1),	
	@stopSale int,
	@labelTypeId int,
	@costedByWeight int,
	@costUnitId int,
	@freightUnitId int,
	@vendorUnitId int,
	@distributionUnitId int,
	@retailUnitId int,
	--@taxPercent decimal,
	@posPrice smallmoney,
	@vendorItemId varchar(20),
	@tempStr1 varchar(25),
	@tempStr2 varchar(25),
	@tempSmallMoney smallmoney,
	@salePrice smallmoney,
	@posSalePrice smallmoney,
	@masterUpc varchar(13),
	@saleStartDate smalldatetime,
	@cmp int,
	@foodStamps bit,
	@linkcode int,
	@postare int,
	@grillPrint bit,
	@ageCode int,
	@visualVerify bit,
	@srCitizenDiscount bit,
	@qtyProhibit bit,
	@groupList int,
	@item_key_temp int,
	@item_key_temp2 int,
	@pricingMethodId int,
	@shelfLife_Id int,
	@shelfLife_Length int,
	@Multiple tinyint,
	@MSRPPrice smallmoney,
    @MSRPMultiple tinyint,
    @Sale_Multiple tinyint,
	@Sale_Earned_Disc1 tinyint,
    @Sale_Earned_Disc2 tinyint,
    @Sale_Earned_Disc3 tinyint,
    @scaledesc1 varchar(64),
    @ingredients varchar(2000),
    @brandid integer,
    @store_no int,
    @discountable bit,
    @natclassid int,
    @scaledesc2 varchar(64),
    @scaleForcedTare varchar(1),
    @scaletare int,
    @scaledesc3 varchar(64),
    @scaledesc4 varchar(64),
    @costStartDate smalldatetime,
    @costEndDate smalldatetime,
    @restrictedHours bit,
    @edlp bit,
    @ibm_discount bit,
    @isAuthorized bit,
	@national_id bit,
	@PriceChgTypeId	int	

declare @CrsrVar Cursor

set @costEndDate = '2079-06-06'
set @costStartDate = '2004-10-04'

set @CrsrVar = Cursor for
	select * from Item_Temp order by master_upc asc

open @CrsrVar

Fetch Next from @CrsrVar Into @item_key,
    @itemDescription,
    @subTeamNo,
    @packageDesc2,
    @packageUnitId,
    @categoryId,
    @deletedItem,
    @discontinueItem,
    @posDescription,
    @priceRequired,
    @itemTypeId,
    @notAvailableNote,
    @insertDate,
    @categoryName,
    @identifier,
    @price,
    @saleEndDate,
    @avgCost,
    @vendorKey,
    @unitCost,
    @packageDesc1,
    @teamNo,
    @teamName,
    @subTeamName,
    @deptNo,
    @targetMargin,
    @defaultIdentifier,
    @business_unit,
    @isPrimary,
    @onPromotion,
    @checkDigit,
    @identifierType,
    @taxClassId,
    @stopSale,
    @labelTypeId,
    @costedByWeight,
    @costUnitId,
	@freightUnitId,
	@vendorUnitId,
	@distributionUnitId,
	@retailUnitId,
	@vendorItemId,
	@salePrice,
	@posPrice,
	@posSalePrice,
	@masterUpc,
	@saleStartDate,
	@cmp,
	@foodStamps,
	@linkcode,
	@postare,
	@grillPrint,
	@ageCode,
	@visualVerify,
	@srCitizenDiscount,
	@qtyProhibit,
	@groupList,
	@item_key_temp,
	@item_key_temp2,
	@pricingMethodId,
	@shelfLife_Id,
	@shelfLife_Length,
	@Multiple,
	@MSRPPrice,
    @MSRPMultiple,
    @Sale_Multiple,
	@Sale_Earned_Disc1,
    @Sale_Earned_Disc2,
    @Sale_Earned_Disc3,
    @scaledesc1,
    @ingredients,
    @brandid,
    @discountable,
    @natclassid,
    @scaledesc2,
    @scaleForcedTare,
    @scaletare,
    @scaledesc3,
    @scaledesc4,
    @restrictedHours,
    @edlp,
    @ibm_discount,
    @isAuthorized,
	@national_id

while (@@FETCH_STATUS = 0)

begin

	select @PriceChgTypeId = CASE	WHEN @edlp = 1 THEN 3
									WHEN @onPromotion = 1 THEN 2
									ELSE 1 END

	select @vendorId = max(vendor_id) from vendor where vendor_key =@vendorKey

	set @store_no = 0
	select @store_no = store_no from store where businessunit_id = @business_unit 
		
	set @max_Item_Key = 0
	select @max_Item_Key = its.item_key 
		from  iriskeytoirmakey its where its.iris_prod_code = @masterUpc
	
	if(@isAuthorized = 0)
	BEGIN
		set @price = 0.0
		set @salePrice = 0.0
		set @posPrice = 0.0
		set @posSalePrice = 0.0
	END
	
	if (@max_Item_Key is null or @max_Item_Key = 0)
	Begin
		INSERT INTO Item (Item_Description, SubTeam_No, Package_Desc1, Package_Desc2, 
		Package_Unit_ID, Category_ID, Deleted_Item, POS_Description, 
		Price_Required, ItemType_ID, Not_Available,Insert_Date, 
			freight_unit_id, cost_unit_id, distribution_unit_id, vendor_unit_id,retail_unit_id,sign_description,
			taxClassId,labelType_Id,CostedByWeight,retail_sale,User_ID,food_stamps,qtyprohibit,
			grouplist,shelflife_id,shelflife_length,scaledesc1,brand_id,discountable,classid,scaledesc2,
			ingredients,scaletare,scaleForcedTare,scaledesc3,scaledesc4)
			VALUES (@itemDescription,@subTeamNo,@packageDesc1,@packageDesc2,@packageUnitId,@categoryId,@deletedItem,
			@posDescription,@priceRequired,@itemTypeId,@notAvailableNote,@insertDate,
			@freightUnitId, @costUnitId, @distributionUnitId, @vendorUnitId, @retailUnitId, 
			@itemDescription,@taxClassId,@labelTypeId,@costedByWeight,1,null,@foodStamps,@qtyProhibit,@groupList,@shelfLife_Id,
			@shelfLife_Length,null,@brandid,@discountable,@natclassid,null,null,
			null,null,null,null)
			
		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into item, key:'+ CAST(@max_Item_Key AS VARCHAR) 
		END	
  
		SELECT  @max_Item_Key = MAX(Item_Key)  FROM   Item 
		
		if (dbo.fn_IsScaleIdentifier(@identifier) = 1) 
		--if (@costedByWeight = 1) -- make sure it is a scale item
		begin
		   	update Item set ScaleDesc1 = @scaledesc1,
							ScaleDesc2 = @scaledesc2,
							ScaleDesc3 = @scaledesc3,
							ScaleDesc4 = @scaledesc4,
							ingredients = @ingredients,
							scaletare = @scaletare,
							scaleForcedTare = @scaleForcedTare
						 where item_key = @max_Item_Key
			IF @@ERROR <>0
			BEGIN
				PRINT 'Error Occured: Could not update into item, key:'+ CAST(@max_Item_Key AS VARCHAR) 
			END	
		end

		INSERT INTO ItemChangeHistory (Item_Key, POS_Description ,Item_Description,SubTeam_No,Category_ID,
		Retail_Unit_ID,Package_Unit_ID,Package_Desc1,Package_Desc2, 
		Vendor_Unit_ID, Distribution_Unit_ID, Cost_Unit_ID, Freight_Unit_ID)
			VALUES (@max_Item_Key, @posDescription ,@itemDescription,@subTeamNo,@categoryId,
		@retailUnitId,@packageUnitId,@packageDesc1,@packageDesc2,
		@vendorUnitId, @distributionUnitId, @costUnitId, @freightUnitId)

		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into ItemChangeHistory, key:'+ CAST(@max_Item_Key AS VARCHAR) 
		END	
		
		--now insert into IrisKeyToIrmaKey table
		insert into iriskeytoirmakey (item_key, iris_prod_code) values (@max_Item_Key, @masterUpc)
		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into iriskeytoirmakey, key:'+ CAST(@max_Item_Key AS VARCHAR) 
		END	
	End --end of inserting original item
	
	if (dbo.fn_IsScaleIdentifier(@identifier) = 1) 
		begin
		    --update identifierType
		    set @identifierType = 'O'
		    set @checkDigit = null
		end
		
	set @identifier_count = 0	
	
	select @identifier_count = count(1) from itemIdentifier where  Item_key = @max_Item_Key
		and identifier = @identifier and default_identifier = @defaultIdentifier

	if (@identifier_count = 0)
	Begin
		INSERT INTO ItemIdentifier (Item_Key,Identifier,Default_Identifier,Deleted_Identifier,Add_Identifier,Remove_Identifier,National_Identifier,IdentifierType,CheckDigit)
			VALUES(@max_Item_Key, @identifier,@defaultIdentifier,@deletedItem,0,0,0,@identifierType,@checkDigit)
		
		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into ItemIdentifier, key:'+ CAST(@max_Item_Key AS VARCHAR) 
		END
	End
	
	set @max_count = 0	
	select @max_count = count(1) from Price where Item_key = @max_Item_Key and store_no = @store_no
	
	--select @taxPercent = taxPercent from taxdefinition where taxflagkey like @taxClassId
	
	--set @posPrice = round(@price / (1 + round((@taxPercent/100),4)), 2)
	--set @posSalePrice = round(@salePrice / (1 + round((@taxPercent/100),4)), 2)
	
	IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not get PRICE 1 into item, key:' + CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
		END	
	set @tempSmallMoney = round(@posPrice * @packageDesc1, 2)
		
		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not get PRICE 2 into item, key:' + CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
		END	
	
	select @tempStr1 = unit_abbreviation, @tempStr2 = unit_name from itemUnit where unit_id = @packageUnitId
		
	if (@max_count = 0)
	BEGIN
		
		insert into Price (Item_key, store_no,price,sale_price,POSPrice,POSSale_Price,
			sale_start_date,sale_end_date,notAuthorizedForSale,compFlag,
			grillprint,agecode,visualverify,srcitizendiscount,postare,pricingMethod_Id,
			sale_multiple,MSRPPrice,MSRPMultiple,Multiple,Restricted_Hours,ibm_discount, PriceChgTypeId)
  			values(@max_Item_Key,@store_no,@price,@salePrice,@posPrice,@posSalePrice,
  			@saleStartDate,@saleEndDate,@stopSale,@cmp,@grillPrint,
  			@ageCode,@visualVerify,@srCitizenDiscount,@postare,@pricingMethodId,
  			@Sale_Multiple,@MSRPPrice,@MSRPMultiple,@Multiple,@restrictedHours,@ibm_discount, @PriceChgTypeId)
  		
  		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into PRICE, key:' + CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
		END
  		
  		if(@isAuthorized = 1)
		BEGIN
		insert into signqueue (Item_Key,Store_No,Sign_Description,Identifier,Price,Case_Price,
		SubTeam_No,Retail_Unit_Abbr,Retail_Unit_Full,Package_Unit,Package_Desc1,Package_Desc2,
		Vendor_Id,POS_Description,Price_Required,
		Sold_By_Weight,Multiple,MSRPMultiple,MSRPPrice,Sale_Multiple,Sale_Price,
		Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Organic,
		user_id,User_ID_Date,ItemType_ID,Restricted_Hours,Quantity_Required,Retail_Sale,
		Discountable,Food_Stamps,IBM_Discount,LastQueuedType,POSPrice,POSSale_Price,
		ingredients,scaledesc1,scaledesc2,PriceChgTypeId) Values
		(@max_Item_Key,@store_no,@posDescription,@identifier,@price,@tempSmallMoney,
		@subTeamNo, @tempStr1, @tempStr2, @tempStr1,@packageDesc1,@packageDesc2,
		@vendorId,@posDescription,@priceRequired,
		0,@Multiple,@MSRPMultiple,@MSRPPrice,@Sale_Multiple,@salePrice,
		@Sale_Earned_Disc1,@Sale_Earned_Disc2,@Sale_Earned_Disc3,0,
		1,'10/3/2006',@itemTypeId,0,0,1,
		@discountable,@foodStamps,@ibm_discount,0,@posPrice,@posSalePrice,
		@ingredients,@scaledesc1,@scaledesc2,@PriceChgTypeId)
		END
		
		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into signQueue, key:' + CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
		END	
	END
	else
	Begin
		--set @posPrice = @price / (1 + (@taxPercent/100))
		--set @posSalePrice = @salePrice / (1 + (@taxPercent/100))
	
		update Price set price = @price,
						sale_price = @salePrice,
						POSPrice = @posPrice,
						POSSale_Price = @posSalePrice,
						sale_start_date = @saleStartDate,
						sale_end_date = @saleEndDate,
						notAuthorizedForSale = @stopSale,
						compFlag = @cmp,
						grillprint = @grillPrint,
						agecode = @ageCode,
						visualverify = @visualVerify,
						srcitizendiscount = @srCitizenDiscount,
						postare = @postare,
						pricingmethod_id = @pricingMethodId,
						sale_multiple = @Sale_Multiple,
						MSRPPrice = @MSRPPrice,
						MSRPMultiple = @MSRPMultiple,
						Multiple = @Multiple,
						Restricted_Hours = @restrictedHours,
						IBM_Discount = @ibm_discount,
						PriceChgTypeId = @PriceChgTypeId
  			where item_key = @max_Item_Key and store_no = @store_no
  			
  		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not update into PRICE, key:' + CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
		END
  		
  		--set @tempSmallMoney = @posPrice * @packageDesc2
		--select @tempStr1 = unit_abbreviation, @tempStr2 = unit_name from itemUnit where unit_id = @packageUnitId
		if(@isAuthorized = 1)
		BEGIN
		update signqueue set Sign_Description = @posDescription,
							Identifier = @identifier,
							Price = @price,
							Case_Price = @tempSmallMoney,
							SubTeam_No = @subTeamNo,
							Retail_Unit_Abbr = @tempStr1,
							Retail_Unit_Full = @tempStr2,
							Package_Unit = @tempStr1,
							Package_Desc1 = @packageDesc1,
							Package_Desc2 = @packageDesc2,
							Vendor_Id = @vendorId,
							POS_Description=@posDescription,
							Price_Required = @priceRequired,
							Sold_By_Weight = 0,
							Multiple =@Multiple,
							MSRPMultiple=@MSRPMultiple,
							MSRPPrice=@MSRPPrice,
							Sale_Multiple=@Sale_Multiple,
							Sale_Price=@salePrice,
							Sale_Earned_Disc1=@Sale_Earned_Disc1,
							Sale_Earned_Disc2=@Sale_Earned_Disc2,
							Sale_Earned_Disc3=@Sale_Earned_Disc3,
							Organic=0,
							user_id=1,
							User_ID_Date='10/3/2006',
							ItemType_ID=@itemTypeId,
							Restricted_Hours=0,
							Quantity_Required=0,
							Retail_Sale=1,
							Discountable = @discountable,
							Food_Stamps=@foodStamps,
							IBM_Discount=@ibm_discount,
							LastQueuedType=0,
							POSPrice=@posPrice,
							POSSale_Price=@posSalePrice,
							ingredients=@ingredients,
							scaledesc1=@scaledesc1,
							scaledesc2=@scaledesc2,
							PriceChgTypeId=@PriceChgTypeId
			where item_key = @max_Item_Key and store_no = @store_no
		END
			IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not update into signQueue, key:' + CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
		END	
	end

	
	IF @vendorId IS NOT null
	BEGIN
		set @max_count2 = 0
			select @max_count2 = count(1) from itemVendor where Item_key = @max_Item_Key 
					and vendor_id = @vendorId
			if (@max_count2 = 0)
			begin
				insert into itemvendor (item_key,vendor_id,item_id)
					values(@max_Item_Key, @vendorId, @vendorItemId)
				IF @@ERROR <>0
				BEGIN
					PRINT 'Error Occured: Could not insert into itemVendor, key:'+ CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
				END	
				set @max_count2 = 1
			end
			select @max_count3 = count(1) from storeItemVendor where Item_key = @max_Item_Key 
					and vendor_id = @vendorId and store_no = @store_no
			if (@max_count3 = 0)
			BEGIN
			
				if(@isAuthorized = 1)
				BEGIN
					insert into storeItemVendor (store_no,item_key,vendor_id,primaryVendor)
						values(@store_no,@max_Item_Key,@vendorId,@isPrimary) -- how do we determine if primary vendor or not
				
					IF @@ERROR <>0
					BEGIN
						PRINT 'Error Occured: Could not insert into storeItemVendor, key:'+ CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
					END	
					select @max_StoreItemVendorId = max(storeItemVendorId) from StoreItemVendor
			
					--insert regular cost
					insert into vendorCostHistory (StoreItemVendorId, promotional,unitcost,package_desc1,
						startdate,enddate,fromvendor, insertdate,insertworkstation)
							values(@max_StoreItemVendorId,0,@unitCost,@packageDesc1,
								@costStartDate,@costEndDate,0,@insertDate,'O')
			
					--insert promocost if it exists
					insert into vendorcosthistory 
					(StoreItemVendorId, promotional,unitcost,unitfreight,package_desc1,
					startdate,enddate,fromvendor,msrp, insertdate,insertworkstation)
						(select @max_StoreItemVendorId,1,it.unitcost,null,@packageDesc1,
						it.startdate,it.enddate,0,@MSRPPrice,GETDATE(),'O'  from item_cost_temp it 
						where 
						identifier = @identifier 
						and vendor_key = @vendorKey )
				END -- is AUTHORIZED
			END	
			IF @@ERROR <>0
			BEGIN
				PRINT 'Error Occured: Could not insert into vendorCostHistory, key:'+ CAST(@max_Item_Key AS VARCHAR) + ' item_key:' + CAST(@max_Item_Key AS VARCHAR)
			END		
	END

-- write record to pricebatchdetail only for onsale items
	IF (@onPromotion = 1)
	BEGIN
		
		INSERT INTO PriceBatchDetail (Item_Key, Store_No, PriceChgTypeID, StartDate)
        VALUES (@max_Item_Key, @store_no, 4, DATEADD(day, 1, @saleEndDate))
	END



    SELECT @Error_No = @@ERROR


    IF @Error_No = 0
    BEGIN
        DECLARE @LastItem int
        SELECT @LastItem = SCOPE_IDENTITY()

        SELECT @Error_No = @@ERROR
    END
	

	Fetch Next from @CrsrVar
	Into @item_key,
    @itemDescription,
    @subTeamNo,
    @packageDesc2,
    @packageUnitId,
    @categoryId,
    @deletedItem,
    @discontinueItem,
    @posDescription,
    @priceRequired,
    @itemTypeId,
    @notAvailableNote,
    @insertDate,
    @categoryName,
    @identifier,
    @price,
    @saleEndDate,
    @avgCost,
    @vendorKey,
    @unitCost,
    @packageDesc1,
    @teamNo,
    @teamName,
    @subTeamName,
    @deptNo,
    @targetMargin,
    @defaultIdentifier,
    @business_unit,
    @isPrimary,
    @onPromotion,
    @checkDigit,
    @identifierType,
    @taxClassId,
    @stopSale,
    @labelTypeId,
    @costedByWeight,
    @costUnitId,
	@freightUnitId,
	@vendorUnitId,
	@distributionUnitId,
	@retailUnitId,
	@vendorItemId,
	@salePrice,
	@posPrice,
	@posSalePrice,
	@masterUpc,
	@saleStartDate,
	@cmp,
	@foodStamps,
	@linkcode,
	@postare,
	@grillPrint,
	@ageCode,
	@visualVerify,
	@srCitizenDiscount,
	@qtyProhibit,
	@groupList,
	@item_key_temp,
	@item_key_temp2,
	@pricingMethodId,
	@shelfLife_Id,
	@shelfLife_Length,
	@Multiple,
	@MSRPPrice,
    @MSRPMultiple,
    @Sale_Multiple,
	@Sale_Earned_Disc1,
    @Sale_Earned_Disc2,
    @Sale_Earned_Disc3,
    @scaledesc1,
    @ingredients,
    @brandid,
    @discountable,
    @natclassid,
    @scaledesc2,
    @scaleForcedTare,
    @scaletare,
    @scaledesc3,
    @scaledesc4,
    @restrictedHours,
    @edlp,
    @ibm_discount,
    @isAuthorized,
	@national_id
	 
end

begin
-- update Price.LinkCode
update item_temp set item_key_temp = (select max(ir.item_key) from iriskeytoirmakey ir,item_temp it where 
ir.iris_prod_code = REPLICATE('0', (12 - LEN(linkcode))) + linkcode )
where linkcode <> ''  and linkcode > 0


update item_temp set item_key_temp2 = (select max(ir.item_key) from iriskeytoirmakey ir, item_temp it where ir.iris_prod_code = it.master_upc )
where linkcode <> ''  and linkcode > 0

update price set LinkedItem = (select max(it.item_key_temp) from item_temp it, price p where it.item_key_temp2 = p.item_key )
where item_key in (select item_key_temp from item_temp where linkcode <> ''  and linkcode > 0)

--cleanup price batch detail
delete from pricebatchdetail where 
	pricechgtypeid <> 4 or itemchgtypeid <> 6 
--delete promo cost records of the past	
--delete from pricebatchdetail where startdate < getdate() and itemchgtypeid = 6

--cleanup promo cost temp table
--delete from item_cost_temp


end

Close @CrsrVar
Deallocate @CrsrVar



--return 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_AddItemsFromTemp] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_AddItemsFromTemp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_AddItemsFromTemp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_AddItemsFromTemp] TO [IRMAReportsRole]
    AS [dbo];

