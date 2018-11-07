CREATE proc dbo.DVO_ImportOrderDetail
   (
   @rectype               varchar(1),
   @DvoOrderId            varchar(10),
   @Note                  varchar(2050),
   @sku                   varchar(17),
   @upc                   varchar(17),
   @vin                   varchar(17),
   @casepack              varchar(10),
   @buyunitordered        varchar(10),
   @buyunitcost           varchar(10),
   @packsize              varchar(10),
   @uom                   varchar(10),
   @description           varchar(255),
   @weight                varchar(10),
   @posdept               varchar(5),
   @brand                 varchar(25),
   @iscase                varchar(5),
   @eaches                varchar(10),
   @caseUomName           varchar(10),
   @eachUomName           varchar(10)
   )
as
   -- **************************************************************************
   -- Procedure: DVO_ImportOrderDetail()
   --    Author: Ron Savage
   --      Date: 10/25/2007
   --
   -- Description:
   -- This procedure imports order detail information sent from DVO in order
   -- export files.
   --
   -- Modification History:
   -- Date        Init Comment
   -- 07/14/2009  BBB  Added left joins to ItemOverride table from Item in
   --				   all SQL calls; added IsNull on column values that should
   --				   pull from override table if value available;
   -- 08/20/2008  RS   Added " from DVO" to the search for an existing order number to avoid
   --                  updating an OrderLink order with the same order number.
   --                  Also updated where clause on item insert to use @OrderHeader_id instead
   --                  of linking on the description again.
   -- 06/10/2008  RS   Added a section to check incoming arguments for being null,
   --                  to set them to '' as they were coming in before the 
   --                  DataMonkey 1.2.0 fix to start sending nulls.
   -- 06/04/2008  RS   Added an isnull() when converting the buyunitordered into
   --                  QuantityOrdered and an error message and check for QuantityOrdered
   --                  being 0.  Those items are not imported.
   -- 02/27/2008  RS   Multiplied up the cost from DVO by the case size to get back
   --                  to the case cost for bug 5460, inserted Casepack_size into
   --                  Package_desc1 field in OrderItem.
   -- 02/25/2008  RS   Added insertion of the buyunitcost to the UnitCost,
   --                  UnitExtCost and MarkupCost fields of OrderItem for bug
   --                  5436. Added conversion of cost fields to number variables
   --                  and calculations at the top of the proc.
   --                  Set NetVendorItemDiscount to 0, so GetOrderOrderedCost() can
   --                  get the correct cost for the order.
   -- 11/28/2007  RS   Added a check for UPCs that don't exist in IRMA and
   --                  appended them to the Notes field in the OrderHeader.
   -- 11/20/2007  RS   Added code to update an existing record, or insert a
   --                  new record if it doesn't exist.
   -- 10/20/2007  RS   Created.
  -- **************************************************************************
begin
   declare @log                 as table ( msg  varchar(MAX) );
   declare @OrderHeader_id      as int;
   declare @OrderItem_id        as int;
   declare @Item_Key            as int;
   declare @QuantityOrdered     as decimal(18,4);
   declare @CasePack_Size       as decimal(18,4);
   declare @Cost                as money;
   declare @UnitCost            as money;
   declare @UnitExtCost         as money;
   declare @LineItemCost        as money;
   declare @AdjustedCost        as money;
   declare @MarkupCost          as money;
   declare @IsElectronic        as bit;


   --**************************************************************************
   -- Check for null incoming values and set them to '' 
   --**************************************************************************
   if (@rectype is null)         set @rectype = '';
   if (@DvoOrderId is null)      set @DvoOrderId = '';
   if (@Note is null)            set @Note = '';
   if (@sku is null)             set @sku = '';
   if (@upc is null)             set @upc = '';
   if (@vin is null)             set @vin = '';
   if (@casepack is null)        set @casepack = '';
   if (@buyunitordered is null)  set @buyunitordered = '';
   if (@buyunitcost is null)     set @buyunitcost = '';
   if (@packsize is null)        set @packsize = '';
   if (@uom is null)             set @uom = '';
   if (@description is null)     set @description = '';
   if (@weight is null)          set @weight = '';
   if (@posdept is null)         set @posdept = '';
   if (@brand is null)           set @brand = '';
   if (@iscase is null)          set @iscase = '';
   if (@eaches is null)          set @eaches = '';
   if (@caseUomName is null)     set @caseUomName = '';
   if (@eachUomName is null)     set @eachUomName = '';
   
   --**************************************************************************
   -- Convert the quantity and cost values to numbers and calculate out the
   -- other cost fields.
   --**************************************************************************
   begin try
      set @CasePack_Size           = cast(@Casepack as decimal(18,4));
      set @QuantityOrdered         = isnull(cast(@buyunitordered as decimal(18,4)),0);
      set @Cost                    = cast(@buyunitcost as money); --DVO Unit Cost
      
      if ( @iscase = 'true' )
         begin
			set @Cost                    = cast(@buyunitcost as money) * @CasePack_Size; -- Multiply up to case cost
         end
      
      set @UnitCost                = @Cost;
      set @UnitExtCost             = @UnitCost;
      set @LineItemCost            = @Cost * @QuantityOrdered;
      set @AdjustedCost            = @Cost;
      set @MarkupCost              = @Cost;
   end try
   begin catch
      insert into @log values ('Error converting one of Casepack [' + @Casepack + '], Quantity [' + @buyunitordered + '], Cost [' + @buyunitcost + '] to numbers for cost calculations.');
   end catch

   --**************************************************************************
   -- Make sure we have an IRMA order for this DVO order already ...
   --**************************************************************************
   select @OrderHeader_id = max(OrderHeader_Id) from OrderHeader where OrderHeaderDesc like rtrim(@DvoOrderId) + ' from DVO%';

   --**************************************************************************
   -- Check for this item key
   --**************************************************************************
   select @Item_Key = Item_Key from ItemIdentifier where Identifier = cast(cast(@upc as bigint) as varchar(15)) and Deleted_Identifier = 0;

   begin try
   --**************************************************************************
   -- Check to see if this item exists in IRMA and has a quantity > 0
   --**************************************************************************
   if ( @Item_Key is not null and @QuantityOrdered > 0 )
      begin
      if (@OrderHeader_id is not null)
         begin
         --**************************************************************************
         -- See if we have entered this item already for this order
         --**************************************************************************
         select
            @OrderItem_id   = max(oi.OrderItem_id)
         from
            OrderItem oi
         where
            oi.OrderHeader_id  = @OrderHeader_id
            and oi.Item_Key    = @Item_Key;







		 select @IsElectronic = Electronic_Order from OrderHeader where OrderHeader_Id = @OrderHeader_id
		 
         --**************************************************************************
         -- Add the item if we haven't already entered it
         --**************************************************************************
         if ( @OrderItem_Id is null )
            begin
            INSERT INTO OrderItem (OrderHeader_ID,
                                    Item_Key,
                                    Comments,
                                    Units_Per_Pallet,
                                    QuantityUnit,
                                    QuantityOrdered,
                                    Cost,
                                    CostUnit,
                                    Handling,
                                    HandlingUnit,
                                    Freight,
                                    FreightUnit,
                                    AdjustedCost,
                                    QuantityDiscount,
                                    DiscountType,
                                    LandedCost,
                                    LineItemCost,
                                    LineItemFreight,
                                    LineItemHandling,
                                    UnitCost,
                                    UnitExtCost,
                                    Package_Desc1,
                                    Package_Desc2,
                                    Package_Unit_ID,
                                    MarkupPercent,
                                    MarkupCost,
                                    Retail_Unit_ID,
                                    Origin_ID,
                                    CountryProc_ID,
                                    NetVendorItemDiscount)
               SELECT top 1
                   oh.OrderHeader_Id                     as OrderHeader_ID,
                   ii.Item_Key                           as Item_Key,
                   substring(@Note,1,255)                as Comments,
                   0                                     as Units_Per_Pallet,
                   case @iscase
                      when 'true' then iuc.Unit_Id
                      else iue.Unit_Id
                   end                                   as QuantityUnit,
                   @QuantityOrdered                      as QuantityOrdered,
                   @Cost                                 as Cost,
                   case @iscase
                      when 'true' then iuc.Unit_Id
                      else iue.Unit_Id
                   end                                   as CostUnit,
                   0                                     as Handling,
                   case @iscase
                      when 'true' then iuc.Unit_Id
                      else iue.Unit_Id
                   end                                   as HandlingUnit,
                   0                                     as Freight,
                   case @iscase
                      when 'true' then iuc.Unit_Id
                      else iue.Unit_Id
                   end                                   as FreightUnit,
                   @AdjustedCost                         as AdjustedCost,
                   0                                     as QuantityDiscount,
                   0                                     as DiscountType,
                   0                                     as LandedCost,
                   @LineItemCost                         as LineItemCost,
                   0                                     as LineItemFreight,
                   0                                     as LineItemHandling,
                   @UnitCost                             as UnitCost,
                   @UnitExtCost                          as UnitExtCost,
                   @CasePack_Size                        as Package_Desc1, -- Case Size
                   ISNULL(iov.Package_Desc2,i.Package_Desc2)		as Package_Desc2,
                   ISNULL(iov.Package_Unit_ID,i.Package_Unit_ID)    as Package_Unit_ID,
                   0												as MarkupPercent,
                   @MarkupCost										as MarkupCost,
                   ISNULL(iov.Retail_Unit_ID,i.Retail_Unit_ID)		as Retail_Unit_ID,
                   i.Origin_ID                           as Origin_ID,
                   i.CountryProc_ID                      as CountryProc_ID,
                   0                                     as NetVendorItemDiscount
               FROM
                  OrderHeader oh (nolock)

                  JOIN ItemIdentifier ii (nolock)
                     on ( ii.Identifier = cast(cast(@upc as bigint) as varchar(15))) and Deleted_Identifier = 0

                  JOIN Item i (nolock)
                     on ( i.Item_Key = ii.Item_Key )

                  JOIN ItemUnit iuc (nolock)
                     on ( iuc.Unit_Name = @caseUomName )

                  JOIN ItemUnit iue (nolock)
                     on ( iue.Unit_Name = @eachUomName )
                     
                  LEFT JOIN ItemOverride iov (nolock)
					 on ( i.Item_Key = iov.Item_Key AND iov.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store JOIN Vendor ON Store.Store_No = Vendor.Store_No WHERE Vendor.Vendor_ID = oh.PurchaseLocation_ID))
               WHERE
                  oh.OrderHeader_Id = @OrderHeader_id;
                  -- oh.OrderHeaderDesc like @DvoOrderId + '%'; removed to use Orderheader_id. RS

               insert into @log values ('Inserted a new order item entry, id: [' + convert(varchar(10), @@IDENTITY ) + '] for ['+ @upc +']');
            end
         else
            begin
            
            if @IsElectronic = 0
				begin
					--**************************************************************************
					-- Update the entry if we already entered it
					--**************************************************************************
					insert into @log values ('Updating item entry, id: [' + convert(varchar(10), @OrderItem_Id ) + '] for ['+ @upc +']');

					update oi set
						Comments                = substring(@Note,1,255),
						QuantityUnit            = case @iscase
													 when 'true' then iuc.Unit_Id
													 else iue.Unit_Id
												end,
						QuantityOrdered         = @QuantityOrdered,
						Cost                    = @Cost,
						CostUnit                = case @iscase
													 when 'true' then iuc.Unit_Id
													 else iue.Unit_Id
												end,
						HandlingUnit            = case @iscase
													when 'true' then iuc.Unit_Id
													else iue.Unit_Id
												end,
						FreightUnit             = case @iscase
													 when 'true' then iuc.Unit_Id
													 else iue.Unit_Id
												  end,
						AdjustedCost            = @AdjustedCost,
						LineItemCost            = @LineItemCost,
						UnitCost                = @UnitCost,
						UnitExtCost             = @UnitExtCost,
						MarkupCost              = @MarkupCost,
						Package_Desc1           = @CasePack_Size,
						Package_Desc2           = ISNULL(iov.Package_Desc2,  i.Package_Desc2),
						Package_Unit_ID         = ISNULL(iov.Package_Unit_ID,i.Package_Unit_ID),
						Retail_Unit_ID          = ISNULL(iov.Retail_Unit_ID, i.Retail_Unit_ID),
						Origin_ID               = i.Origin_ID,
						CountryProc_ID          = i.CountryProc_ID,
						NetVendorItemDiscount   = 0
					FROM
					   OrderItem oi (nolock)
					   
					   JOIN OrderHeader oh (nolock)
						  on ( oi.OrderHeader_ID = oh.OrderHeader_ID )

					   JOIN ItemIdentifier ii (nolock)
						  on ( ii.Item_Key = oi.Item_Key )

					   JOIN Item i (nolock)
						  on ( i.Item_Key = ii.Item_Key )

					   JOIN ItemUnit iuc (nolock)
						  on ( iuc.Unit_Name = @caseUomName )

					   JOIN ItemUnit iue (nolock)
						  on ( iue.Unit_Name = @eachUomName )
						  
		               LEFT JOIN ItemOverride iov (nolock)
 						  on ( i.Item_Key = iov.Item_Key AND iov.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store JOIN Vendor ON Store.Store_No = Vendor.Store_No WHERE Vendor.Vendor_ID = oh.PurchaseLocation_ID))
					WHERE
					   oi.OrderItem_Id = @OrderItem_Id ;
				end
            end
         end
      else
         begin
         insert into @log values ('Could not find order number [' + convert(varchar(10), @DvoOrderId) + '].');
         end
      end
   else
      begin
      --**************************************************************************
      -- Added error log messages
      --**************************************************************************
      if ( @QuantityOrdered = 0 ) insert into @log values ('Item [' + @upc + '], not imported due to an order quantity of [' + @buyunitordered + '].');
      if ( @Item_Key is null ) insert into @log values ('Item [' + @upc + '] not found in IRMA.');

      --**************************************************************************
      -- Update the Order header notes with the NOF items
      --**************************************************************************
      if (@OrderHeader_id is not null)
         begin
         update OrderHeader set OrderHeaderDesc = substring(OrderHeaderDesc + ' ' + @upc + ' NOF,'  ,1,255)
         where OrderHeader_Id = @OrderHeader_Id and OrderHeaderDesc not like '%' + @upc + '%';
         end
      end
   end try
   begin catch
      insert into @log values (error_message() + ' at line: ' + convert(varchar(4),error_line()));
   end catch

   select msg from @log;
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DVO_ImportOrderDetail] TO [IRMASchedJobsRole]
    AS [dbo];

