



CREATE  PROCEDURE [dbo].[Convertion_AddItemFromTemp] 
AS
declare @costEndDate smalldatetime,
		@costStartDate smalldatetime,
		@regCost int
		
set @costEndDate = '2079-06-06'
set @costStartDate = '2004-10-04'
select @regCost = pricechgtypeid from pricechgtype where pricechgtypedesc = 'REG'


INSERT INTO Item (master_upc,Item_Description, SubTeam_No, Package_Desc1, Package_Desc2, 
		Package_Unit_ID, Category_ID, Deleted_Item, Discontinue_Item, POS_Description, 
		Price_Required, ItemType_ID, Not_Available,Insert_Date, 
			distribution_unit_id, vendor_unit_id,retail_unit_id,sign_description,
			taxClassId,labelType_Id,CostedByWeight,retail_sale,User_ID,food_stamps,qtyprohibit,
			grouplist,shelflife_id,shelflife_length,--scaledesc1
			brand_id,discountable,classid,
			--scaledesc2,
			ingredients
			--,scaletare,scaleForcedTare,scaledesc3,scaledesc4
			)
			(select master_upc, Item_Description, SubTeam_No, package_desc1, Package_Desc2, 
				Package_Unit_ID, Category_ID, Deleted_Item, Discontinue_Item, POS_Description, 
				Price_Required, item_type_id, Not_AvailableNote,Insert_Date, 
				distribution_unit_id, vendor_unit_id,retail_unit_id,
				item_description,
				taxClassId,labelTypeId,CostedByWeight,1,NULL,food_stamps,qtyprohibit,
				grouplist,shelflife_id,shelflife_length,--scaledesc1
				brandid,discountable,natclassid,
				--scaledesc2,
				ingredients
				--,scaletare,scaleForcedTare,scaledesc3,scaledesc4 
					from item_temp it
					--where 0 = (select count(1) from item im where
					--				im.item_Description = it.Item_Description
					---				and im.subTeam_No	= it.SubTeam_No
					--				and im.package_desc1 = it.Package_Desc1
					--				and im.package_desc2 =	it.Package_Desc2
					--				and im.Package_Unit_ID = it.Package_Unit_ID
					--				and im.Category_ID = it.Category_ID
					--				and im.Master_Upc = it.master_upc
					--			)
			)
			

PRINT cast(getdate() as char(20)) + ' Inserted into Item, check items'

update item_temp set conversion_key = it.item_key
		from item it
		where item_temp.item_Description = it.Item_Description
			  and item_temp.subTeam_No	= it.SubTeam_No
			  --and item_temp.package_desc1 = it.package_desc1
			  and item_temp.package_desc2 =	it.Package_Desc2
			  and item_temp.Package_Unit_ID = it.Package_Unit_ID
			  and item_temp.Category_ID = it.Category_ID
			  and item_temp.BrandID = it.Brand_ID
			  and item_temp.Master_Upc = it.master_upc
			  
--update with non (1) taxclass ids, this might be region specific and 
--defintely CIX only DCs
update item set taxclassid = it2.taxclassid
	from item_temp it2
	where
		item.item_key = it2.conversion_key
		and it2.taxclassid <> 1  

PRINT cast(getdate() as char(20)) + ' Updated Item Temp keys, check items'


	IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into Item, check items'
		END	
--insert into Scale_ExtraText
INSERT into Scale_ExtraText
	(Scale_LabelType_Id, Description, ExtraText,Item_Key)
	(select (select top 1 scale_labeltype_id from scale_labelType), 
		it.identifier,isnull(it.ingredients,''), it.conversion_key 
			from item_temp it
					where it.IdentifierType = 'O')
	

--update scale info
INSERT into ItemScale
		(item_key,Scale_ExtraText_ID,forcetare,printblankshelflife,printblankeatby,printblankpackdate,printblankweight,
		printblankunitprice,printblanktotalprice,Scale_Description1,Scale_Description2,Scale_Description3,
		Scale_Description4,shelflife_length,scale_scaleuomunit_id,scale_bycount,scale_fixedweight)
		(select it.conversion_Key, se.Scale_ExtraText_id, it.scaletare,0,0,0,0,
			0,0,it.scaledesc1,it.scaledesc2,it.scaledesc3,
			it.scaledesc4,it.shelflife_length,it.scale_uom,it.By_Count,it.Fixed_Weight
			from item_temp it inner join Scale_ExtraText se on se.item_key = it.conversion_key 
				where it.IdentifierType = 'O')
			
PRINT cast(getdate() as char(20)) + ' Updated Item Scale, check items'


	IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into ItemScale, check items'
		END	
			

INSERT INTO ItemChangeHistory 
		(Item_Key, POS_Description ,Item_Description,SubTeam_No,Category_ID,
		Retail_Unit_ID,Package_Unit_ID,Package_Desc1,Package_Desc2, 
		Vendor_Unit_ID, Distribution_Unit_ID)
(select Item_Key, POS_Description ,Item_Description,SubTeam_No,Category_ID,
		Retail_Unit_ID,Package_Unit_ID,package_desc1,Package_Desc2, 
		Vendor_Unit_ID, Distribution_Unit_ID
			from item im --where 0 = (select count(1) from ItemChangeHistory ih 
						--					where ih.item_key = im.item_key)  
 )
PRINT cast(getdate() as char(20)) + ' Inserted into ItemChangeHistory, check items'

	IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into ItemChangeHistory, check items'
		END	

	update item_temp set price = 0.0,
						SalePrice = 0.0,
						PosPrice = 0.0,
						PosSalePrice = 0.0, 
						PriceChgTypeId = @regCost,
						onPromotion = 0,
						sale_start_date = null,
						sale_end_date = null
						where isAuthorized = 0
	
	
	INSERT INTO ItemIdentifier (Item_Key,Identifier,Default_Identifier,Deleted_Identifier,
					Add_Identifier,Remove_Identifier,National_Identifier,IdentifierType,CheckDigit,Scale_Identifier)
				(select it.conversion_Key,it.Identifier,it.Default_Identifier,it.Deleted_Item,
					0,0,it.National_Identifier,IdentifierType,CheckDigit,Scale_Identifier
				from item_temp it
					--where 0 = (select count(1) from ItemIdentifier id
					--				where id.Item_key = it.conversion_Key
					--						and identifier = it.identifier 
					--						and default_identifier = it.default_Identifier
					--			)
				)
	PRINT cast(getdate() as char(20)) + ' Inserted into ItemIdentifier, check items'			
	
IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into ItemIdentifier, check items'
		END
	
	insert into Price (Item_key, store_no,price,sale_price,POSPrice,POSSale_Price,
			sale_start_date,sale_end_date,notAuthorizedForSale,compFlag,
			grillprint,agecode,visualverify,srcitizendiscount,postare,pricingMethod_Id,
			sale_multiple,MSRPPrice,MSRPMultiple,Multiple,Restricted_Hours,ibm_discount,
			PriceChgTypeId,poslinkcode,linkcodeupc)
  			(select it.conversion_key, so.store_no,it.price,it.saleprice,it.POSPrice,it.POSSalePrice,
			it.sale_start_date,it.sale_end_date,it.stopSale,it.cmp,
			it.grillprint,it.agecode,it.visualverify,it.srcitizendiscount,it.postare,
			it.pricingMethod_Id,
			it.sale_multiple,it.MSRPPrice,it.MSRPMultiple,
			it.Multiple,it.Restricted_Hours,it.ibm_discount,
			it.PriceChgTypeId,it.poslinkcode,it.linkcode
				
				from item_temp it inner join store so on it.business_unit = so.businessunit_id 
			--where  0 = (select count(1) from Price p where p.item_key = it.conversion_key 
			--					and store_no = so.store_no)
			)
  	PRINT cast(getdate() as char(20)) + ' Inserted into PRICE, check items'
	
  	IF @@ERROR <>0
	BEGIN
		PRINT 'Error Occured: Could not insert into PRICE, check items'
	END
  		

		insert into signqueue (Item_Key,Store_No,Sign_Description,Identifier,Price,Case_Price,
		SubTeam_No,Retail_Unit_Abbr,Retail_Unit_Full,Package_Unit,Package_Desc1,Package_Desc2,
		Vendor_Id,POS_Description,Price_Required,
		Sold_By_Weight,Multiple,MSRPMultiple,MSRPPrice,Sale_Multiple,Sale_Price,
		Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Organic,
		user_id,User_ID_Date,ItemType_ID,Restricted_Hours,Quantity_Required,Retail_Sale,
		Discountable,Food_Stamps,IBM_Discount,LastQueuedType,POSPrice,POSSale_Price,
		ingredients,scaledesc1,scaledesc2, PriceChgTypeId) 
		(select 
		it.conversion_Key,so.store_no,it.pos_Description,it.identifier,it.price,
		round(it.posPrice * it.package_desc1,2),
		it.subTeam_No, ut.unit_abbreviation, ut.unit_name, ut.unit_abbreviation,
		it.package_desc1,it.package_Desc2,
		v.vendor_id,it.pos_Description,it.price_Required,
		0,it.Multiple,it.MSRPMultiple,it.MSRPPrice,it.Sale_Multiple,it.salePrice,
		it.Sale_Earned_Disc1,it.Sale_Earned_Disc2,it.Sale_Earned_Disc3,0,
		1,getDate(),it.item_Type_Id,0,0,1,
		it.discountable,it.food_Stamps,it.ibm_discount,0,
		it.posPrice,it.posSalePrice,
		it.ingredients,it.scaledesc1,it.scaledesc2,
		it.PriceChgTypeId
		from item_temp it
					 inner join	itemUnit ut on ut.unit_id = it.package_unit_id
					 inner join vendor	v on v.vendor_key = it.vendor_key 
					 inner join store so on it.business_unit = so.businessunit_id 
			where --0 = (select count(1) from SignQueue where item_key = it.conversion_key 
					--			and store_no = so.store_no)
				--and 
				it.isAuthorized = 1
		)
PRINT cast(getdate() as char(20)) +' Inserted into signQueue, check items'		

		IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not insert into signQueue, check items'
		END	
	
		update Price set price = it.price,
						sale_price = it.salePrice,
						POSPrice = it.posPrice,
						POSSale_Price = it.posSalePrice,
						sale_start_date = it.sale_Start_Date,
						sale_end_date = it.sale_End_Date,
						notAuthorizedForSale = it.stopSale,
						compFlag = it.cmp,
						grillprint = it.grillPrint,
						agecode = it.ageCode,
						visualverify = it.visualVerify,
						srcitizendiscount = it.srCitizenDiscount,
						postare = it.postare,
						pricingmethod_id = it.pricingMethod_Id,
						sale_multiple = it.Sale_Multiple,
						MSRPPrice = it.MSRPPrice,
						MSRPMultiple = it.MSRPMultiple,
						Multiple = it.Multiple,
						Restricted_Hours = it.restricted_Hours,
						IBM_Discount = it.ibm_discount,
						PriceChgTypeId = it.PriceChgTypeId,
						PosLinkCode = it.poslinkcode,
						linkcodeupc = it.linkcode
  			from item_temp it 
						 inner join store so on it.business_unit = so.businessunit_id  
			where --1 = (select count(1) from Price where item_key = im.item_key 
					--		and store_no = so.store_no)
					price.item_key  = it.conversion_key
					and price.store_no = so.store_no
					and price.price <> it.price

  PRINT cast(getdate() as char(20)) + ' Updated into PRICE, check items'
--commit;  	
	IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not update into PRICE, check items'
		END

		update signqueue set Sign_Description = it.pos_Description,
							Identifier = it.identifier,
							Price = it.price,
							Case_Price = round(it.posPrice * it.package_desc1,2),
							SubTeam_No = it.subTeam_No,
							Retail_Unit_Abbr = ut.unit_abbreviation,
							Retail_Unit_Full = ut.unit_name,
							Package_Unit = ut.unit_abbreviation,
							Package_Desc1 = it.package_desc1,
							Package_Desc2 = it.package_Desc2,
							Vendor_Id = v.vendor_Id,
							POS_Description=it.pos_Description,
							Price_Required = it.price_Required,
							Sold_By_Weight = 0,
							Multiple =it.Multiple,
							MSRPMultiple=it.MSRPMultiple,
							MSRPPrice=it.MSRPPrice,
							Sale_Multiple=it.Sale_Multiple,
							Sale_Price=it.salePrice,
							Sale_Earned_Disc1=it.Sale_Earned_Disc1,
							Sale_Earned_Disc2=it.Sale_Earned_Disc2,
							Sale_Earned_Disc3=it.Sale_Earned_Disc3,
							Organic=0,
							user_id=1,
							User_ID_Date=getDate(),
							ItemType_ID=it.item_Type_Id,
							Restricted_Hours=0,
							Quantity_Required=0,
							Retail_Sale=1,
							Discountable = it.discountable,
							Food_Stamps=it.food_Stamps,
							IBM_Discount=it.ibm_discount,
							LastQueuedType=0,
							POSPrice=it.posPrice,
							POSSale_Price=it.posSalePrice,
							ingredients=it.ingredients,
							scaledesc1=it.scaledesc1,
							scaledesc2=it.scaledesc2,
							PriceChgTypeId = it.PriceChgTypeId
			from item_temp it 
					 inner join	itemUnit ut on ut.unit_id = it.package_unit_id
					 inner join vendor	v on v.vendor_key = it.vendor_key
					 inner join store so on it.business_unit = so.businessunit_id  
			where signqueue.item_key = it.conversion_key
					and signqueue.store_no = so.store_no
					and signqueue.price <> it.price
				and it.isAuthorized = 1

PRINT cast(getdate() as char(20)) +' Updated into signQueue,check items'
--commit;
			IF @@ERROR <>0
		BEGIN
			PRINT 'Error Occured: Could not update into signQueue,check items'
		END	

	insert into itemvendor (item_key,vendor_id,item_id)
			(select it.conversion_key, v.vendor_Id, it.vendor_Item_Id
				from item_temp it 
							inner join vendor	v on v.vendor_key = it.vendor_key  
					--where 0 = (select count(1) from itemVendor iv 
					--				where iv.Item_key = it.conversion_key
					--						and iv.vendor_id = v.vendor_id)
			)
PRINT cast(getdate() as char(20)) + ' Inserted into itemVendor, check items'

	IF @@ERROR <>0
				BEGIN
					PRINT 'Error Occured: Could not insert into itemVendor, check items'
				END	
	insert into storeItemVendor (store_no,item_key,vendor_id,primaryVendor)
		(select so.store_no,it.conversion_key,v.vendor_Id,it.isPrimary
			from item_temp it
						inner join vendor	v on v.vendor_key = it.vendor_key
						inner join store so on it.business_unit = so.businessunit_id 
			where  
				 --0 = (select count(1) from storeItemVendor where Item_key = it.conversion_key 
				--	and vendor_id = v.vendor_Id and store_no = so.store_no)
				--and 
				it.isAuthorized = 1
		)
PRINT cast(getdate() as char(20)) +' Inserted into storeitemVendor, check items'

	insert into vendorCostHistory (StoreItemVendorId, promotional,unitcost,package_desc1,
						startdate,enddate,fromvendor, insertdate,insertworkstation,costUnit_Id,freightUnit_Id)
				(select st.storeItemVendorId,0,it.unitCost,it.package_Desc1,
								@costStartDate,@costEndDate,0,it.insert_Date,'O',it.cost_unit_id, it.freight_unit_id
				from item_temp it 
						inner join vendor v on v.vendor_key = it.vendor_key
						inner join store so on it.business_unit = so.businessunit_id
						inner join storeItemVendor st on st.item_key = it.conversion_key
														and st.store_no = so.store_no
														and st.vendor_id = v.vendor_id 
			--where  
			--	 0 = (select count(1) from vendorCostHistory where 
			--		StoreItemVendorId = st.StoreItemVendorId) 
			) 
	--insert promocost if it exists
	insert into vendordealhistory 
				(StoreItemVendorId, caseQty,package_desc1,
					caseAmt,startdate,enddate,
				VendorDealTypeID,
				fromVendor,insertdate,insertworkstation,CostPromoCodeTypeId)
				(select st.storeItemVendorId,0,it2.package_Desc1,
					it.discount,it.startdate,it.enddate,
						case when it.isAllowance = 1 then
							(select vendorDealTypeId from vendorDealType where code = 'A')
							else
							(select vendorDealTypeId from vendorDealType where code = 'D') 
						end,
				0,GETDATE(),'O',
						  case when it.enddate >= dateadd(year,1,getDate()) then 4 --ongoing promotion
							else 3 -- promotion , see CostPromoCodeType tables for values
						end
				from item_promocost_temp it
						inner join item_temp it2 on it.vendor = it2.vendor_key
													and it.upcno = it2.identifier 
						inner join vendor v on v.vendor_key = it.vendor
						inner join store so on it2.business_unit = so.businessunit_id
						inner join storeItemVendor st on st.item_key = it2.conversion_key
														and st.store_no = so.store_no
														and st.vendor_id = v.vendor_id 
				)

	--insert into vendorcosthistory 
	--			(StoreItemVendorId, promotional,unitcost,unitfreight,package_desc1,
	--				startdate,enddate,fromvendor,msrp, insertdate,insertworkstation)
	--			(select st.storeItemVendorId,1,it.unitcost,null,it2.package_Desc1,
	--					it.startdate,it.enddate,0,it2.MSRPPrice,GETDATE(),'O'  
	--			from item_cost_temp it 
	--					inner join item_temp it2 on it.vendor_key = it2.vendor_key
	--												and it.identifier = it2.identifier 
	--					inner join vendor v on v.vendor_key = it.vendor_key
	--					inner join store so on it2.business_unit = so.businessunit_id
	--					inner join storeItemVendor st on st.item_key = it2.conversion_key
	--													and st.store_no = so.store_no
	--													and st.vendor_id = v.vendor_id 
	--			)
			
PRINT cast(getdate() as char(20)) +' Inserted into vendorCostHistory, check items'

			IF @@ERROR <>0
			BEGIN
				PRINT 'Error Occured: Could not insert into vendorCostHistory, check items'
			END		

		
		INSERT INTO PriceBatchDetail (Item_Key, Store_No, PriceChgTypeID, Price, StartDate,Multiple,
				pricingmethod_id,sale_multiple,sale_price,sale_end_date,
				sale_earned_disc1,sale_earned_disc2,sale_earned_disc3,
				posprice,possale_price,insert_date,poslinkcode)
        --(select it.conversion_key, so.store_no, it.PriceChgTypeID, DATEADD(day, 1, it.sale_End_Date)
        (select it.conversion_key, so.store_no, it.PriceChgTypeID, it.Price, it.sale_start_Date, it.multiple,
        it.pricingmethod_id, it.sale_multiple,it.saleprice,it.sale_End_Date,
        it.sale_earned_disc1, it.sale_earned_disc2, it.sale_earned_disc3, 
        it.posprice, it.possaleprice,getDate(), it.poslinkcode 
		from item_temp it 
					 inner join store so on it.business_unit = so.businessunit_id
			where it.onPromotion = 1
		)
		
PRINT cast(getdate() as char(20)) +' Inserted into PriceBatchDetail, check items'

			IF @@ERROR <>0
			BEGIN
				PRINT 'Error Occured: Could not insert into PriceBatchDetail, check items'
			END		
		--insert futures in priceHistory
		insert into PriceHistory (Item_key, store_no,price,sale_price,POSPrice,POSSale_Price,
			sale_start_date,sale_end_date,notAuthorizedForSale,compFlag,
			grillprint,agecode,visualverify,srcitizendiscount,postare,pricingMethod_Id,
			sale_multiple,MSRPPrice,MSRPMultiple,Multiple,Restricted_Hours,ibm_discount,
			PriceChgTypeId, poslinkcode,linkcodeupc)
  			(select it2.conversion_key, so.store_no,it.fut_price,0,it.fut_Price,0,
			it.start_date,end_date,0,it2.cmp,
			it2.grillprint,it2.agecode,it2.visualverify,it2.srcitizendiscount,it2.postare,
			it2.pricingMethod_Id,
			it2.sale_multiple,it2.MSRPPrice,it2.MSRPMultiple,
			it2.Multiple,it2.Restricted_Hours,it2.ibm_discount,
			it.PriceChgTypeId, it2.poslinkcode, it2.linkcode
  			from item_promoprice_temp it
  						 inner join item_temp it2 on it.upcno = it2.identifier  
						 inner join store so on it2.business_unit = so.businessunit_id   
			where 
					it.pzone = it2.pzone
					)
					
			--now update PriceHistory poslinkcode and linkcode for pzone = 1
			update PriceHistory set poslinkcode = it.poslinkcode,
									linkcodeupc = it.linkcode
						from item_temp it where it.conversion_key = pricehistory.item_key
						  
PRINT cast(getdate() as char(20)) +' Inserted into PriceHistory, check items'

			IF @@ERROR <>0
			BEGIN
				PRINT 'Error Occured: Could not insert into PriceHistory, check items'
			END		
					
		--now insert future price
		INSERT INTO PriceBatchDetail --(Item_Key, Store_No, PriceChgTypeID, StartDate)
		(Item_Key, Store_No, PriceChgTypeID, price,StartDate,Multiple,
				pricingmethod_id,sale_multiple,sale_price,sale_end_date,
				sale_earned_disc1,sale_earned_disc2,sale_earned_disc3,
				posprice,possale_price,insert_date,poslinkcode)
        (
        --select it2.conversion_key, so.store_no, it.PriceChgTypeID, DATEADD(day, 1, End_Date)
        select it2.conversion_key, so.store_no, it.PriceChgTypeID, it2.price, it.start_Date, it2.multiple,
        it2.pricingmethod_id, it2.sale_multiple,it.fut_price,it.End_Date,
        it2.sale_earned_disc1, it2.sale_earned_disc2, it2.sale_earned_disc3, 
        it2.posprice, it.fut_price,getDate(), it2.poslinkcode 
		from item_promoprice_temp it
						inner join item_temp it2 on it.upcno = it2.identifier 
						inner join store so on it2.business_unit = so.businessunit_id
					where it.pzone = it2.pzone
						
		)
					
		
				
		
PRINT cast(getdate() as char(20)) +' Inserted into PriceBatchDetail, check items'

--update Price.LinkedItem
--update price set LinkedItem = 
--		(select max(id.item_key)
--				from item_temp it inner join store so on it.business_unit = so.businessunit_id
--				inner join itemIdentifier id on cast(it.linkcode as bigint) = id.identifier 
--				where 
--						it.conversion_key = price.item_key 
--						and so.store_no = price.store_no
--						--and cast(it.linkcode as bigint) = id.identifier 
--		)
--where item_key in (select conversion_key from item_temp where rtrim(ltrim(linkcode)) <> ''  
--							and linkcode is not null)


 

--PRINT cast(getdate() as char(20)) +' Inserted into Price linkcodes, check items'


INSERT INTO ItemAttribute 
		(Item_Key, Check_Box_1, Check_Box_2, Check_Box_5) -- note Text_1 = Vegan and Text_2 = Mandatory in AttributeIdentifier table
        (
        select it2.conversion_key, 
			case wf.vegan
				when 'Y' then 1
				else 0
			end,
			case wf.mandatory
				when 'Y' then 1
				else 0
			end,
			case wf.hicode1
				when 'Y' then 1
				else 0
			end  
		from wfmman wf
						inner join item_temp it2 on  cast(cast(wf.upcno as bigint) as nvarchar) = it2.identifier 	
		)
		
PRINT cast(getdate() as char(20)) +' Inserted into Item attributes, check items'

--return 0
