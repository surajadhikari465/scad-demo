
CREATE PROCEDURE [dbo].[DVO_ProcessBulkOrders](@StoreList varchar(MAX) = '', @DCList varchar(MAX) = '', @debug integer = 0)
AS

   -- **************************************************************************
   -- Procedure: DVO_ProcessBulkOrders()
   --    Author: Ron Savage
   --      Date: 03/23/2010
   --
   -- Description:
   -- This procedure processes the order information loaded into two global
   -- temp tables - one for the header records, one for the detail records.
   --
   -- @StoreList is a pipe delimited list of Store BusinessUnit_Ids that are "turned on"
   --            for Ordering / Receiving (like "00234|00235|00345") it is used to limit
   --            the Store SubTeam list which in turn limits the orders inserted.
   --
   -- @DCList    is a pipe delimited list of DC PS_Vendor_Ids that should NOT
   --            have orders inserted, since those orders will be imported from
   --            OrderLink. (like "00234|00235|00345")
   --
   -- Modification History:
   -- Date        Init Comment
   -- 12/07/2018  MZ   30599    Added AMZ Order number to IRMA       
   -- 08/12/2016  MZ   [TFS 20532:17702] - Corrected another bug in the code which could cause duplicate ExternalOrderInformation records to be 
   --                                      inserted if a DVO order is resent and is with a PDX order number.
   -- 07/08/2016  MZ   [TFS 20138:16741] - Corrected the issue that when an order with Predictix order number is re-sent from DVO, 
   --                                      no more duplicate ExternalOrderInformation records will be created for the Predictix order number.
   -- 02/04/2016  MZ   [TFS 13116:18241] Added PDXOrderNumber to the DVO Import file
   -- 01/25/2016  FA   [TFS 12872], Fixed code to match username between IRMA and DVO 
   -- 11/09/2015  MZ   Rolled back chagnes for TFS 11978 since it created duplicate ExternalOrderInformation and OrderHeader records 
   -- 10/05/2015  FA   [TFS 11978], Fixed code to match userids between IRMA and DVO 
   -- 2013/05/08  Mil  [TFS 12239, v4.8.0] Remove the CreditExpireDate patch (TFS 11853) so that we can test the DVO fix;		
   -- 04/09/2013  Lux  [TFS 11908, v4.7.1] Fixed check for insert into ExternalOrderInformation table for orders being updated to ensure it filters to DVO source ID
   --                  because if the order is already in IRMA from a different source, it was skipping the insert into EOI.
   -- 04/08/2013  BBB  [TFS 11853, v.4.7.1] temporary patch to ignore CreditExpireDate in 4.7.1, this is not merged from 4.7.0 even though it is sourced in 4.7.0
   -- 03/29/2013  Lux  [TFS 11714, v4.7.1] Corrected dup-prevention logic for insert into ExternalOrderInformation table (two spots, OrderExternalSourceOrderID):
   --                  "where oei.ExternalOrder_Id = oh.OrderExternalSourceOrderID..."
   -- 03/23/2012  RS   Commented out updates to OrderHeader to stop deadlock issues until further testng.
   -- 10/19/2011 RDE   TFS 3049: If Cost override populate Adjusted Cost. If $ or % discount only populate DiscountQuantity and NOT Adjusted Cost. Otherwise irma will apply the discount twice.
   -- 10/19/2011 MD    TFS 3325: Fixed the issue with cost override (adjusted cost) always defaulting the discount type to 'Free Items'. DiscountType is needed when we have discounts for the line item.                
   -- 09/28/2011  RDE  TFS 3063: For all ItemUnits (Ordering, Cost, Handling, Freight, etc) match DVO OrderUOM to ItemUnit.UnitName and use that ItemUnit.Unit_Id if the match exists.
   --                  If a match is not found default back to the original logic. if IsCase = true then use Case otherwise use Each. This fixed a bug where the item was "CostedByWeight" and set up as BOX
   --				   in DVO, but once imported from DVO to IRMA it would end up as EACH and cause the OrderItem screen to crash because EACH is not a valid Unit for a CBW item.
   -- 08/04/2011	   Updated to process header and item cost adjustments part of 4.3 
   --				   Please use mapping for discount type
   --						sDiscountType(0) = "No Discount"
   --						sDiscountType(1) = "Cash Discount"
   --						sDiscountType(2) = "Percent Discount"
   --						sDiscountType(3) = "Free Items"
   -- 06/22/2011  DBS  TFS 1800: Disallow OrderItem Updates for DVO Pass-Through usage by using OrderExternalSourceID
   -- 03/11/2011  TTL  TFS 1527: Applied logic to cover issue where POET orders sent to IRMA then up to DVO would have their external order ID field overwritten by this DVO import process.
   --                  We do not want to overwrite the external source order-ID or source-ID fields if one of them is already set, 
   --                  so we just assign the field back to itself if it contains a value (during OrderHeader update, two spots).
   -- 02/15/2011  TTL  TFS 1277: Due to unexplained build errors, I changed this process from using ##DVO_details to using #tmpOrderItems (previous version did ALTER TABLE to add identity field to ##DVO_details).
   --                  So, a new "id" identity column was added to #tmpOrderItems, the ##DVO_details table is copied straight in, then 
   --                  extra or duplicate rows are deleted from #tmpOrderItems.
   -- 02/03/2011  TTL  TFS 1277: Items duplicated in IRMA POs when PO exported from DVO two or more times before the DVO-import job runs.
   --                  Bug was caused by the filename field in the ##DVO_details table, as it caused unique item rows because the filenames from DVO are unique each time you export and order.
   --                  Added unique ID field to ##DVO_details to help remove dups.  
   --                  Added WHERE clause to filter dups out of SELECT into #tmpOrderItems from ##DVO_details.  Logic now takes highest/MAX DVO order 
   --                  filename *AND* the last (highest ID) instance of an order-item to ensure one instance of a unique item in each DVO order.
   -- 07/07/2010  RDS  Added Data check / log / removal for incoming header & detail records with:
   --						NULL/empty POS_Dept
   --						NULL/empty/non-numeric UPCs
   --                  Remove duplicate order headers/order details to avoid creating multiple IRMA orders
   --                  Add query to insert new detail records (items) for orders identified as “updates” from the order header
   --                  Add additional join to the StoreSubTeam Table to search for exact PS_Subteam_No & POS_Dept match as primary subteam assignment
   -- 04/29/2010  RS   Added the dh.IsCreditOrder field handling to the query.
   -- 04/28/2010  RS   Set Vendor JOIN to regular JOIN rather than OUTER to avoid NULL vendor_ids.
   --                  Also converted back to using temp tables to add indexes after Roberts
   --                  freaking 2500 order file "load test". :-)
   -- 04/15/2010  RS   Fixed duplication issue caused by funky JOIN to Vendors
   --                  table in the #tmpHeaders insert query.  Changed len(v.PS_Vendor_Id)
   --                  to len(dh.PSVendorId) to avoid issues with bad PS_Vendor_Ids
   --                  in the Vendor table.
   --                  Also updated the StoreSubTeam selection to include the @POS_SubTeam
   --                  as part of the coalesce() function arguments in the same table.
   -- 04/09/2010  RS   Added Identifier to Update Items log message.
   -- 04/08/2010  RS   Set table names back to OrderHeader, OrderItem instead of
   --                  OrderHeaderCopy, OrderItemCopy.
   -- 03/30/2010  RS   Added primary key indexes to some of the table variables,
   --                  couldn't add to the larger ones because they can possibly
   --                  have duplicate keys and it all blows up. :-)
   -- 03/29/2010  RS   Converted temp tables to table variables to avoid any
   --                  potential locking issues with the tempdb.
   -- 03/23/2010  RS   Created.
  -- ***********************************************************************************************************************************************************************************************************************

BEGIN

--    declare @StoreList varchar(MAX);
--    declare @DCList varchar(MAX);
--    declare @debug integer;
-- 
--    set @StoreList = '';
--    set @DCList = '';
--    set @debug = 0;

   declare @PDXSourceID as Int, @AMZSourceID as Int, @DVOSourceID as Int;

   select @PDXSourceID = ID FROM [dbo].[OrderExternalSource] where [Description] = 'PDX'
   select @AMZSourceID = ID FROM [dbo].[OrderExternalSource] where [Description] = 'AMAZON'
   select @DVOSourceID = ID FROM [dbo].[OrderExternalSource] where [Description] = 'DVO'

   declare @log            as table ( msg  varchar(MAX) );

   insert into @log select 'Starting DVO_ProcessBulkOrders (''' + @StoreList + ''', ''' + @DCList + ''') ...';

   if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Processing orders in table [##DVO_Headers] ...';

  begin try
      -- **************************************************************************
      -- Create a temp table for the allowable stores, as defined in the @StoreList
      -- **************************************************************************
      -- declare @AllowedStores as table ( BusinessUnit_Id Integer );
      create table #AllowedStores ( BusinessUnit_Id Integer );
      create index #allstr on #AllowedStores (BusinessUnit_Id);

      if ( @StoreList = '' )
         insert into #AllowedStores select BusinessUnit_Id from Store;
      else
         insert into #AllowedStores select Key_Value from dbo.fn_Parse_List(@StoreList,'|');

      insert into @log select 'Store ' + cast(s.BusinessUnit_Id as varchar(10)) + ' ' + rtrim(s.Store_Name) + ' is accepting orders ...' from #AllowedStores al JOIN Store s on (s.BusinessUnit_Id = al.BusinessUnit_Id);

      -- **************************************************************************
      -- Create a temp table of Vendor_Ids to exclude
      -- **************************************************************************
      -- declare #ExcludedVendors as table ( Vendor_Id Integer );
      create table #ExcludedVendors ( Vendor_Id Integer );
      create index #exvnd on #ExcludedVendors (Vendor_Id);

      if ( @DCList <> '' )
         begin
            Insert into #ExcludedVendors
               select
                  v.Vendor_Id
               from
                  dbo.fn_ParseStringList(@DCList,'|') dc

                  JOIN Vendor v
                     on ( right('00000000' + v.PS_Vendor_Id, len(v.PS_Vendor_Id) ) = right('00000000' + dc.Key_Value, len(v.PS_Vendor_Id) ) )
         end

      insert into @log select 'Orders from vendor ' + v.PS_Vendor_Id + ' ' + rtrim(v.CompanyName) + ' are being excluded ...' from #ExcludedVendors ev JOIN Vendor v on (v.Vendor_id = ev.Vendor_Id);

--       create index #tmpVnd on #ExcludedVendors (Vendor_Id);

      -- **************************************************************************
      -- Create a temp tables to store order/items with validation issues
      -- **************************************************************************
      create table #tmpOrderErrors (DVOOrderID varchar(10), hError bit, upc varchar(17), ErrorDesc varchar(250));
      
		-- must have a subteam
      insert into #tmpOrderErrors
		select
			DVOOrderID,
			1, -- set this to TRUE if the whole order is to be marked as invalid for failing the subteam validation
			null,
			'VALIDATION ERROR on DVO Order [' + ISNULL(DVOOrderID, 'null') + '] : Order not imported [Invalid PS/POS Team/Subteam info specified on order header]'
		from ##DVO_headers dh
		where (dh.POS_SubTeam is null OR ISNUMERIC(dh.POS_SubTeam) = 0 OR dh.PS_SubTeam = 'null');
			
		-- upc must be numeric
      insert into #tmpOrderErrors
		select
			DVOOrderID,
			0,
			dd.upc,
			'VALIDATION ERROR on DVO Order [' + ISNULL(DVOOrderID, 'null') + '] : Item not imported [UPC ' + ISNULL(dd.upc, 'null') + ' is invalid]'
		from ##DVO_details dd
		where (dd.upc is null OR ISNUMERIC(dd.upc) = 0 OR dd.upc = '0')
		-- no need to flag order lines with issues when the whole order is already marked as invalid for having bad PS/POS Team/Subteam info
		AND NOT EXISTS( select DVOOrderID from #tmpOrderErrors where DVOOrderID = dd.DVOOrderID and hError = 1 );
		
		-- item qty must be > 0
      insert into #tmpOrderErrors
		select
			DVOOrderID,
			0,
			dd.upc,
			'VALIDATION ERROR on DVO Order [' + ISNULL(DVOOrderID, 'null') + '] : Item not imported [UPC ' + ISNULL(dd.upc, 'null') + ' ordered quantity = 0]'
		from ##DVO_details dd
		where isnull(cast(dd.buyunitordered as decimal(18,4)),0) = 0
		AND (NOT ISNUMERIC(dd.upc) = 0 OR dd.upc <> '0') -- don't log these again, we've already caught them above
		-- no need to flag order lines with issues when the whole order is already marked as invalid for having bad PS/POS Team/Subteam info
		AND NOT EXISTS( select DVOOrderID from #tmpOrderErrors where DVOOrderID = dd.DVOOrderID and hError = 1 );
			
	  -- remove the lines	(lines in error only)
	  delete dd from ##DVO_details dd
		JOIN #tmpOrderErrors toe
			ON ( dd.DVOOrderID = toe.DVOOrderID AND dd.upc = toe.upc AND toe.hError = 0 );		

	  -- remove the lines	(full orders)
	  delete dd from ##DVO_details dd
		JOIN #tmpOrderErrors toe
			ON ( dd.DVOOrderID = toe.DVOOrderID AND toe.hError = 1 );
			
		-- must have at least one valid line item
      insert into #tmpOrderErrors
		select
			DVOOrderID,
			1, -- set this to TRUE if the whole order is to be marked as invalid if there are no valid lines associated with it
			null,
			'VALIDATION ERROR on DVO Order [' + ISNULL(DVOOrderID, 'null') + '] : There are no valid lines associated with this order.'
		from ##DVO_headers dh
		where NOT EXISTS(select DVOOrderID from ##DVO_details dd where DVOOrderID = dh.DVOOrderID)
		-- no need to flag order lines with issues when the whole order is already marked as invalid for having bad PS/POS Team/Subteam info
		AND NOT EXISTS( select DVOOrderID from #tmpOrderErrors where DVOOrderID = dh.DVOOrderID and hError = 1 ); 
	  
	  -- remove the headers for the things we flagged in error
	  delete dh from ##DVO_headers dh
		JOIN #tmpOrderErrors toe
			ON ( dh.DVOOrderID = toe.DVOOrderID AND toe.hError = 1 );
	  	
      -- **************************************************************************
      -- Add messages to @log of the orders and items removed for validation errors
      -- **************************************************************************
      insert into @log
         select
             ErrorDesc
         from
            #tmpOrderErrors;

      -- **************************************************************************
      -- Get distinct StoreSubTeam records, can pick the Min or Max subteam no
      -- only for Stores in the @StoreList
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building #tmpStoreSubTeam ...';

      -- declare #tmpStoreSubTeam as table
      create table #tmpStoreSubTeam
         (
         Store_No          Integer,
         PS_SubTeam_No     Integer,
         MinSubTeam_No     Integer,
         MaxSubTeam_No     Integer
         );

      create index #tmpsst on #tmpStoreSubTeam (Store_No);

      insert into #tmpStoreSubTeam
         select
            sst.Store_No,
            sst.PS_SubTeam_No,
            min(sst.SubTeam_No) MinSubTeam_No,
            max(sst.SubTeam_No) MaxSubTeam_No
         from
            StoreSubTeam sst

            JOIN Store s
               on ( s.Store_No = sst.Store_No )
               
             JOIN #AllowedStores al
               on ( al.BusinessUnit_Id = s.BusinessUnit_Id)
         group by
            sst.Store_No,
            sst.PS_SubTeam_No;

      -- **************************************************************************
      -- Get a default subteam per store in case there is no match, so at least
      -- the order can get inserted (even if its the wrong subteam)
      -- **************************************************************************
      -- declare #tmpStoreSubTeamDefaults as table
      create table #tmpStoreSubTeamDefaults
         (
         Store_No          Integer,
         MinSubTeam_No     Integer,
         MaxSubTeam_No     Integer

         Primary Key (Store_No)
         );

      insert into #tmpStoreSubTeamDefaults
         select
            sst.Store_No,
            min(sst.SubTeam_No) MinSubTeam_No,
            max(sst.SubTeam_No) MaxSubTeam_No
         from
            StoreSubTeam sst

            JOIN Store s
               on ( s.Store_No = sst.Store_No )

            JOIN #AllowedStores al
               on ( al.BusinessUnit_Id = s.BusinessUnit_Id )
         group by
            sst.Store_No;

      -- **************************************************************************
      -- Get distinct Users, use first user id
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building #tmpUsers ...';

      -- declare #tmpUsers as table
	 create table #tmpUsers
         (
         Fullname       varchar(50),
         User_Id        Integer,
		 UserName       Varchar(50)

         Primary Key (User_Id)
         );

      insert into #tmpUsers
         select
            Fullname,
            min(User_Id) User_Id,
			UserName

         from
            Users u

		 where u.AccountEnabled = 1

         group by
            Fullname, UserName;

      -- **************************************************************************
      -- Get all the data you need to insert into the OrderHeader table
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building #tmpHeaders ...';

      -- declare #tmpHeaders as table
      create table #tmpHeaders
         (
         Vendor_ID                        Integer,
         OrderHeaderDesc                  varchar(255),
         OrderDate                        smalldatetime,
         OrderType_ID                     Integer,
         ProductType_ID                   Integer,
         PurchaseLocation_ID              Integer,
         ReceiveLocation_ID               Integer,
         Transfer_SubTeam                 Integer,
         Transfer_To_SubTeam              Integer,
         Fax_Order                        bit,
         Expected_Date                    smalldatetime,
         CreatedBy                        Integer,
         Return_Order                     bit,
         Sent                             bit,
         SentDate                         smalldatetime,
         DVOOrderId                       varchar(10),
         CurrencyID                       Integer,
         OrderExternalSourceOrderID       Integer,
         OrderExternalSourceID            Integer,
         OrderHeader_ID					  Integer,
		 Reason_Code_Detail_ID			  Integer,
		 Cost_Adj_Discount                money,
         Cost_Adj_Method                  varchar(1),
		 PDXOrderNumber                   int,
		 AMZOrderNumber                   int
         );

      create index #tmpHdr on #tmpHeaders (DVOOrderId);

      insert into #tmpHeaders
         select distinct
            v.Vendor_Id                                                                    as Vendor_ID,
            substring(dh.DvoOrderId + ' from DVO (' + isnull(dh.BuyerName,'') + ') ' + isnull(dh.Note,''),1,255) as OrderHeaderDesc,
            cast(dh.POEntryDate as smalldatetime)                                          as OrderDate,
            1                                                                              as OrderType_ID,
            1                                                                              as ProductType_ID,
            vp.Vendor_Id                                                                   as PurchaseLocation_ID,
            vr.Vendor_Id                                                                   as ReceiveLocation_ID,
            null                                                                           as Transfer_SubTeam,
            coalesce(sstx.SubTeam_No,sst.MinSubTeam_No,sst2.MaxSubTeam_No,sst3.MinSubTeam_No,sst4.MinSubTeam_No,sstd.MinSubTeam_No)     as Transfer_To_SubTeam,
            0                                                                              as Fax_Order,
            cast(dh.ExpectedDate as smalldatetime)                                         as Expected_Date,
            
			--isnull(u1.User_Id,u.User_Id)                                                 as CreatedBy,
			isnull(u2.User_Id,isnull(u1.User_Id,u.User_id))								   as CreatedBy,
   
            case when dh.IsCreditOrder='true' then 1 else 0 end                            as Return_Order,
            1                                                                              as Sent,
            getdate()                                                                      as SentDate,
            dh.DvoOrderId                                                                  as DVOOrderId,
            v.CurrencyID                                                                   as CurrencyID,
            dh.DvoOrderId                                                                  as OrderExternalSourceOrderID,
            2                                                                              as OrderExternalSourceID,
            ISNULL(dh.IRMAPONo, -1)														   as OrderHeader_ID,
			dh.ReasonCodeDetailId														   as Reason_Code_Detail_ID,
			ISNULL(dh.adjustCostValue, 0)												as Cost_Adj_Discount,
			case dh.adjustCostMethod
				when 'x' then '3'
				when '%' then '2'
				when '$' then '1'
				else '0'                                
			end																			   as Cost_Adj_Method,
			dh.PDXOrderNumber                             								   as PDXOrderNumber,
			dh.AMZOrderNumber                             								   as AMZOrderNumber
         from
            ##DVO_headers dh

            JOIN #AllowedStores al
               on ( al.BusinessUnit_Id = dh.BusinessUnitId )

            JOIN Store s
               on ( s.BusinessUnit_id = dh.BusinessUnitId )
               
            -- Try for a StoreSubTeam match that is an exact match to the POS and PS subteams
            LEFT OUTER JOIN StoreSubTeam sstx
               on ( sstx.Store_No = s.Store_No
                    and sstx.SubTeam_No = cast(dh.POS_SubTeam as int)
                    and sstx.PS_SubTeam_No = dh.PS_SubTeam )
               
            -- Try for a StoreSubTeam match where both the min POS and PS subteams match up
            LEFT OUTER JOIN #tmpStoreSubTeam sst
               on ( sst.Store_No = s.Store_No
                    and sst.MinSubTeam_No = cast(dh.POS_SubTeam as int)
                    and sst.PS_SubTeam_No = dh.PS_SubTeam )

            -- Try for a StoreSubTeam match where both the max POS and PS subteams match up
            LEFT OUTER JOIN #tmpStoreSubTeam sst2
               on ( sst2.Store_No = s.Store_No
                    and sst2.MaxSubTeam_No = cast(dh.POS_SubTeam as int)
                    and sst2.PS_SubTeam_No = dh.PS_SubTeam )

            -- Next try for a StoreSubTeam match where at least the PS subteam matches
            LEFT OUTER JOIN #tmpStoreSubTeam sst3
               on ( sst3.Store_No = s.Store_No
                    and sst3.PS_SubTeam_No = dh.PS_SubTeam )

            -- Next try for a StoreSubTeam match where at least the POS subteam matches
            LEFT OUTER JOIN #tmpStoreSubTeam sst4
               on ( sst4.Store_No = s.Store_No
                    and sst4.MinSubTeam_No = dh.POS_SubTeam )

            -- Worst case, pick a default subTeam for that store - it will be wrong, but at least the order will be there
            LEFT OUTER JOIN #tmpStoreSubTeamDefaults sstd
               on ( sstd.Store_No = s.Store_No )

            JOIN Vendor v
               on ( right('00000000' + v.PS_Vendor_Id, len(dh.PSVendorId) ) = right('00000000' + dh.PSVendorId, len(dh.PSVendorId) ) )

            LEFT OUTER JOIN Vendor vp
               on ( vp.Store_No = s.Store_No )

            LEFT OUTER JOIN Vendor vr
               on ( vr.Store_No = s.Store_No )

            LEFT OUTER JOIN #tmpUsers u
               on ( u.FullName = 'DVO Import' )

            LEFT OUTER JOIN #tmpUsers u1
               on ( u1.FullName = dh.BuyerName )

			LEFT OUTER JOIN #tmpUsers u2
               on ( u2.UserName = dh.BuyerName )
         where
            v.Vendor_Id not in (select Vendor_Id from #ExcludedVendors);

      -- **************************************************************************
      -- Add the calculations to convert the unit costs into case costs
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building #tmpOrderItems ...';

      --declare #tmpOrderItems as table
      create table #tmpOrderItems
         (
         rectype              varchar(1),
         DvoOrderId           varchar(10),
         Note                 varchar(2050),
         sku                  varchar(17),
         upc                  varchar(17),
         vin                  varchar(17),
         casepack             varchar(10),
         buyunitordered       varchar(10),
         buyunitcost          varchar(10),
         packsize             varchar(10),
         uom                  varchar(10),
         description          varchar(255),
         weight               varchar(10),
         posdept              varchar(5),
         brand                varchar(25),
         iscase               varchar(5),
         eaches               varchar(10),
         orderUOM             varchar(10),
         FileName             varchar(300),
         CasePack_Size        decimal(18,4),
         QuantityOrdered      decimal(18,4),
         UnitCost             money,
         Cost                 money,
		 LineItemCost         money,
		 ReasonCodeDetailId   integer,
		 CostAdjDiscount      money,
		 CostAdjMethod        varchar(1),
		 IrmaRegCostHistory   money,
		 CreditReasonCodeIRMAId INTEGER,
		 CreditLotNo          VARCHAR(50),
		 CreditExpireDate		DATETIME,
		 CreditNotes			VARCHAR(255),
		 id                  int identity(1,1)
         );

      create index #tmpItm on #tmpOrderItems (DVOOrderId);

	  -- Copy/convert all rows from ##DVO_details.
      insert into #tmpOrderItems
         select distinct
            rectype,
            DvoOrderId,
            Note,
            sku,
            upc,
            vin,
            casepack,
            buyunitordered,
            buyunitcost,
            packsize,
            uom,
            description,
            weight,
            posdept,
            brand,
            iscase,
            eaches,
            orderUOM,
            FileName,
            cast(Casepack as decimal(18,4))                                           as CasePack_Size,
            isnull(cast(buyunitordered as decimal(18,4)),0)                           as QuantityOrdered,
            cast(buyunitcost as money)                                                as  UnitCost,
            case iscase
               when 'true'
                  then cast(buyunitcost as money) * cast(Casepack as decimal(18,4))
                  else cast(buyunitcost as money)
               end                                                                    as Cost,
			lineItemCost															  as LineItemCost,
			ReasonCodeDetailId														  as ReasonCodeDetailId,
			isnull(adjustCostValue, 0)												  as CostAdjDiscount,
  	        adjustCostMethod														  as CostAdjMethod,
            irmaRegCostHistory														  as IrmaRegCostHistory,
			CreditReasonCodeIRMAId,
			CreditLotNo,
			CreditExpireDate,
			CreditNotes
         from
            ##DVO_details


	  /* Delete extra copies of order-items. */
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Removing extras/dups from #tmpOrderItems ...';

      delete from #tmpOrderItems
      where
		id not in (
			/*
				[Main SELECT of IN() Clause]
				This identifies the last instance of a unique order-item within a DVO order file.
			*/
			select 
				MAX(id)
			from #tmpOrderItems oi join
			(	
				----------------------------------------------------------------------------------------------------------
				/*
					[Sub-SELECT 1 of 1 for IN() Clause]
					This identifies the most recent DVO export of each order-item, based on export filename.
				*/
				select distinct
					DvoOrderId,
					sku,
					upc,
					vin,
					MAX(filename) MaxFile
				from #tmpOrderItems
				group by
					DvoOrderId,
					sku,
					upc,
					vin
				----------------------------------------------------------------------------------------------------------
			) oik -- OIK is the DVO "order-item keep" result set.  We join this back to #tmpOrderItems using the minimum fields to guarantee uniqueness DVO PO#, item IDs, and import filename.
				on oi.DvoOrderId = oik.DvoOrderId
				and oi.sku = oik.sku
				and oi.upc = oik.upc
				and oi.vin = oik.vin
				and oi.FileName = oik.MaxFile
			group by
				oi.DvoOrderId,
				oi.sku,
				oi.upc,
				oi.vin
        ) -- End of IN() clause that identifies the last unique entry for each order-item.



      -- **************************************************************************
      -- Find the orders that are already in IRMA
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building #tmpHeaderUpdates ...';

      --declare #tmpHeaderUpdates as table
      create table #tmpHeaderUpdates
         (
         Vendor_ID                        Integer,
         OrderHeaderDesc                  varchar(255),
         OrderDate                        smalldatetime,
         OrderType_ID                     Integer,
         ProductType_ID                   Integer,
         PurchaseLocation_ID              Integer,
         ReceiveLocation_ID               Integer,
         Transfer_SubTeam                 Integer,
         Transfer_To_SubTeam              Integer,
         Fax_Order                        bit,
         Expected_Date                    smalldatetime,
         CreatedBy                        Integer,
         Return_Order                     bit,
         Sent                             bit,
         SentDate                         smalldatetime,
         DVOOrderId                       varchar(10),
         CurrencyID                       Integer,
         OrderExternalSourceOrderID       Integer,
         OrderExternalSourceID            Integer,
         OrderHeader_ID                   Integer,
		 Reason_Code_Detail_id			  Integer,
		 Cost_Adj_Discount                money,
         Cost_Adj_Method                  varchar(1),
		 PDXOrderNumber                   varchar(10),
		 AMZOrderNumber                   varchar(10)
         );

      create index #tmpHdrupd on #tmpHeaderUpdates (DVOOrderId);

      --declare #tmpHeadersInsert as table
      create table #tmpHeadersInsert
         (
         Vendor_ID                        Integer,
         OrderHeaderDesc                  varchar(255),
         OrderDate                        smalldatetime,
         OrderType_ID                     Integer,
         ProductType_ID                   Integer,
         PurchaseLocation_ID              Integer,
         ReceiveLocation_ID               Integer,
         Transfer_SubTeam                 Integer,
         Transfer_To_SubTeam              Integer,
         Fax_Order                        bit,
         Expected_Date                    smalldatetime,
         CreatedBy                        Integer,
         Return_Order                     bit,
         Sent                             bit,
         SentDate                         smalldatetime,
         DVOOrderId                       varchar(10),
         CurrencyID                       Integer,
         OrderExternalSourceOrderID       Integer,
         OrderExternalSourceID            Integer,
		 Reason_Code_Detail_id			  Integer,
		 Cost_Adj_Discount                money,
         Cost_Adj_Method                  varchar(1),
		 PDXOrderNumber                   varchar(10),
		 AMZOrderNumber                   varchar(10)
         );

      create index #tmpHdrins on #tmpHeadersInsert (DVOOrderId);

      insert into #tmpHeaderUpdates
         select th.*
         from
            #tmpHeaders th
            JOIN OrderHeader oh
               on ( oh.DVOOrderId = th.DVOOrderId )
         Union
         select th.*
         from
            #tmpHeaders th
            JOIN OrderHeader oh
               on ( oh.OrderHeader_ID = th.OrderHeader_ID );

--       create index #tmpHeadUp on #tmpHeaderUpdates (DVOOrderId);

      -- **************************************************************************
      -- Identify the new orders to be inserted
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building #tmpHeadersInsert ...';

      insert into #tmpHeadersInsert
         select
			Vendor_ID,
			OrderHeaderDesc,
			OrderDate,
			OrderType_ID,
			ProductType_ID,
			PurchaseLocation_ID,
			ReceiveLocation_ID,
			Transfer_SubTeam,
			Transfer_To_SubTeam,
			Fax_Order,
			Expected_Date,
			CreatedBy,
			Return_Order,
			Sent,
			SentDate,
			DVOOrderId,
			CurrencyID,
			OrderExternalSourceOrderID,
			OrderExternalSourceID,
			Reason_Code_Detail_ID,
		    Cost_Adj_Discount,
            Cost_Adj_Method,
			PDXOrderNumber,
			AMZOrderNumber
         from #tmpHeaders 
         where DVOOrderId not in ( select DVOOrderId from #tmpHeaderUpdates);

      -- **************************************************************************
      -- Insert the new header information
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Inserting new headers from #tmpHeadersInsert ...';

      insert into OrderHeader
         (
         Vendor_ID,
         OrderHeaderDesc,
         OrderDate,
         OrderType_ID,
         ProductType_ID,
         PurchaseLocation_ID,
         ReceiveLocation_ID,
         Transfer_SubTeam,
         Transfer_To_SubTeam,
         Fax_Order,
         Expected_Date,
         CreatedBy,
         Return_Order,
         Sent,
         SentDate,
         DVOOrderId,
         CurrencyID,
         OrderExternalSourceOrderID,
         OrderExternalSourceID,
		 ReasonCodeDetailID,
		 QuantityDiscount,
		 DiscountType
		  )
      select Vendor_ID,
         OrderHeaderDesc,
         OrderDate,
         OrderType_ID,
         ProductType_ID,
         PurchaseLocation_ID,
         ReceiveLocation_ID,
         Transfer_SubTeam,
         Transfer_To_SubTeam,
         Fax_Order,
         Expected_Date,
         CreatedBy,
         Return_Order,
         Sent,
         SentDate,
         DVOOrderId,
         CurrencyID,
         OrderExternalSourceOrderID,
         OrderExternalSourceID,
		 Reason_Code_Detail_id,
		 Cost_Adj_Discount,
         Cost_Adj_Method 
	from #tmpHeadersInsert;

	  

      -- **************************************************************************
      -- Add ExternalOrderInformation records for each Order. 
      -- **************************************************************************

    if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Inserting new records into ExternalOrderInformation ...';

	
	INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
	SELECT		oh.orderheader_id ,
				@DVOSourceID ,
                th.DVOOrderID
	FROM		OrderHeader oh
				INNER JOIN #tmpHeadersInsert th ON ( oh.DVOOrderId = th.DVOOrderId )
	WHERE oh.OrderHeader_ID NOT IN ( SELECT oei.OrderHeader_Id 
									   FROM ExternalOrderInformation oei 
									  WHERE oei.ExternalOrder_Id = oh.OrderExternalSourceOrderID 
									    AND oei.OrderHeader_Id = oh.orderheader_id
										AND oei.ExternalSource_Id = @DVOSourceID) -- OrderExternalSource.Description = 'DVO'.  Must restrict to DVO source here in case PO got to IRMA from diff system, otherwise it will skip this insert. 

	INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
	SELECT		oh.orderheader_id ,
				@PDXSourceID ,
                PDXOrderNumber
	FROM		OrderHeader oh
	INNER JOIN #tmpHeadersInsert th ON ( oh.DVOOrderId = th.DVOOrderId )
	WHERE oh.OrderHeader_ID NOT IN ( SELECT oei.OrderHeader_Id 
									   FROM ExternalOrderInformation oei 
									  WHERE oei.ExternalSource_Id = @PDXSourceID 
										AND oei.OrderHeader_Id = oh.orderheader_id )
	 AND th.PDXOrderNumber is not NULL and th.PDXOrderNumber <>  ''

	 INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
	 SELECT		oh.orderheader_id ,
				@AMZSourceID ,
                AMZOrderNumber
	 FROM		OrderHeader oh
	 INNER JOIN #tmpHeadersInsert th ON ( oh.DVOOrderId = th.DVOOrderId )
	 WHERE oh.OrderHeader_ID NOT IN ( SELECT oei.OrderHeader_Id 
									   FROM ExternalOrderInformation oei 
									  WHERE oei.ExternalSource_Id = @AMZSourceID 
										AND oei.OrderHeader_Id = oh.orderheader_id )
	 AND th.AMZOrderNumber is not NULL and th.AMZOrderNumber <>  ''
      -- **************************************************************************
      -- Add messages to @log of the new orders inserted for logging
      -- **************************************************************************
      insert into @log
         select
            'Inserted IRMA new order [' + cast(oh.OrderHeader_Id as varchar(10)) + '] from DVO order [' + th.DVOOrderId + '] ...'
         from
            #tmpHeadersInsert th

            JOIN OrderHeader oh
               on ( oh.DVOOrderId = th.DVOOrderId );

      -- **************************************************************************
      -- Update the orders that were already existing
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Updating existing orders from #tmpHeaderUpdates ...';

      update oh set
         Vendor_Id                  = th.Vendor_Id,
         OrderHeaderDesc            = th.OrderHeaderDesc,
         OrderDate                  = th.OrderDate,
		 PurchaseLocation_ID        = th.PurchaseLocation_ID,
         ReceiveLocation_ID         = th.ReceiveLocation_ID,
         Expected_Date              = th.Expected_Date,
         CreatedBy                  = th.CreatedBy,
         SentDate                   = th.SentDate,
         DVOOrderId                 = th.DvoOrderId,
         CurrencyID                 = th.CurrencyID,
		 -- TFS 1527, Tom Lux: We do not want to overwrite *either* source field if one of them is already set, so we just assign the field back to itself if it contains a value.
         OrderExternalSourceOrderID = case when oh.OrderExternalSourceOrderID is null and oh.OrderExternalSourceID is null then th.DvoOrderId else oh.OrderExternalSourceOrderID end,
         OrderExternalSourceID      = case when oh.OrderExternalSourceOrderID is null and oh.OrderExternalSourceID is null then 2 else oh.OrderExternalSourceID end,
         Return_Order               = th.Return_Order,
		 ReasonCodeDetailID         = th.Reason_Code_Detail_id,
		 QuantityDiscount           = th.Cost_Adj_Discount,
         DiscountType               = th.Cost_Adj_Method
      from
         OrderHeader oh
         JOIN #tmpHeaderUpdates th
            on ( th.DVOOrderId = oh.DVOOrderId);
            
      update oh set
         OrderHeaderDesc            = th.OrderHeaderDesc,
         DVOOrderId                 = th.DvoOrderId,
		 -- TFS 1527, Tom Lux: We do not want to overwrite *either* source field if one of them is already set, so we just assign the field back to itself if it contains a value.
         OrderExternalSourceOrderID = case when oh.OrderExternalSourceOrderID is null and oh.OrderExternalSourceID is null then th.DvoOrderId else oh.OrderExternalSourceOrderID end,
         SentDate                   = GetDate(),
         [User_ID]					= NULL
      from
         OrderHeader oh
         JOIN #tmpHeaderUpdates th
            on ( th.OrderHeader_ID = oh.OrderHeader_ID);


    if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Inserting updated records into ExternalOrderInformation ...';

		INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
		SELECT		oh.orderheader_id ,
					2 ,
					th.DVOOrderID
		FROM		OrderHeader oh
				INNER JOIN #tmpHeaderUpdates th ON ( oh.DVOOrderId = th.DVOOrderId )
		WHERE oh.OrderHeader_ID NOT IN 
            ( 
				SELECT    oei.OrderHeader_Id
				FROM      ExternalOrderInformation oei
				WHERE     oei.ExternalOrder_Id = oh.OrderExternalSourceOrderID
						  and oei.ExternalSource_Id = 2 -- OrderExternalSource.Description = 'DVO'.  Must restrict to DVO source here in case PO got to IRMA from diff system, otherwise it will skip this insert.
                          AND oei.OrderHeader_Id = oh.orderheader_id
            )

		INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
		SELECT		oh.orderheader_id ,
					@PDXSourceID ,
					th.PDXOrderNumber
		FROM		OrderHeader oh
				INNER JOIN #tmpHeaderUpdates th ON ( oh.DVOOrderId = th.DVOOrderId )
		WHERE oh.OrderHeader_ID NOT IN 
            ( 
				SELECT    oei.OrderHeader_Id
				FROM      ExternalOrderInformation oei
				WHERE     oei.ExternalOrder_Id = th.PDXOrderNumber
						  and oei.ExternalSource_Id = @PDXSourceID -- OrderExternalSource.Description = 'PDX'. 
                          AND oei.OrderHeader_Id = oh.orderheader_id
            )
		  AND th.PDXOrderNumber is not NULL and th.PDXOrderNumber <>  ''

		INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
		SELECT		oh.orderheader_id ,
					@AMZSourceID ,
					th.AMZOrderNumber
		FROM		OrderHeader oh
				INNER JOIN #tmpHeaderUpdates th ON ( oh.DVOOrderId = th.DVOOrderId )
		WHERE oh.OrderHeader_ID NOT IN 
            ( 
				SELECT    oei.OrderHeader_Id
				FROM      ExternalOrderInformation oei
				WHERE     oei.ExternalOrder_Id = th.AMZOrderNumber
						  and oei.ExternalSource_Id = @AMZSourceID 
                          AND oei.OrderHeader_Id = oh.orderheader_id
            )
		  AND th.AMZOrderNumber is not NULL and th.AMZOrderNumber <>  ''
            
      -- **************************************************************************
      -- Add messages to @log of the new orders inserted for logging
      -- **************************************************************************
      insert into @log
         select
             'Updated IRMA order [' + cast(oh.OrderHeader_Id as varchar(10)) + '] from DVO order [' + th.DVOOrderId + '] ...'
         from
            #tmpHeaderUpdates th
            JOIN OrderHeader oh
               on ( oh.DVOOrderId = th.DVOOrderId );
               
       insert into @log
         select
             'Updated IRMA order [' + cast(oh.OrderHeader_Id as varchar(10)) + '] from DVO order [' + th.DVOOrderId + '] ...'
         from
            #tmpHeaderUpdates th
            JOIN OrderHeader oh
               on ( oh.OrderHeader_ID = th.OrderHeader_ID );

      -- **************************************************************************
      -- Build the records to insert into OrderItem
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building order item records to insert into #tmpItemsInsert ...';

      --declare #tmpItemsInsert as table
      create table #tmpItemsInsert
         (
         OrderHeader_ID               Integer,
         Item_Key                     Integer,
         Comments                     varchar(255),
         Units_Per_Pallet             smallint,
         QuantityUnit                 Integer,
         QuantityOrdered              decimal(18,4),
         Cost                         money,
         CostUnit                     Integer,
         Handling                     money,
         HandlingUnit                 Integer,
         Freight                      money,
         FreightUnit                  Integer,
         AdjustedCost                 money,
         QuantityDiscount             decimal(18,4),
         DiscountType                 Integer,
         LandedCost                   money,
         LineItemCost                 money,
         LineItemFreight              money,
         LineItemHandling             money,
         UnitCost                     money,
         UnitExtCost                  money,
         Package_Desc1                decimal(18,4), -- Case Size
         Package_Desc2                decimal(18,4),
         Package_Unit_ID              Integer,
         MarkupPercent                decimal(18,4),
         MarkupCost                   money,
         Retail_Unit_ID               Integer,
         Origin_ID                    Integer,
         CountryProc_ID               Integer,
         NetVendorItemDiscount        money,
		 ReasonCodeDetailid	          INTEGER,
		 CreditReason_ID			  INTEGER,
		 Lot_No						  VARCHAR(12),
		 ExpirationDate				  DATETIME,
		 AdminNotes					  VARCHAR(5000)
         );

      insert into #tmpItemsInsert
         SELECT
            oh.OrderHeader_Id                                   as OrderHeader_ID,
            ii.Item_Key                                         as Item_Key,
            substring(toi.Note,1,255)                           as Comments,
            0                                                   as Units_Per_Pallet,
            CASE WHEN iui.unit_id IS NULL then
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END	                                                as QuantityUnit,
            toi.QuantityOrdered                                 as QuantityOrdered,
            --case toi.irmaRegCostHistory
			--	when null then toi.Cost
			--	else toi.irmaRegCostHistory									
			--end													as Cost,
			ISNULL(toi.irmaRegCostHistory, toi.Cost)            as Cost,
            CASE WHEN iui.unit_id IS NULL then
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END	                                                as CostUnit,
            0                                                   as Handling,
            CASE WHEN iui.unit_id IS NULL then
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END	                                                as HandlingUnit,
            0                                                   as Freight,
            
            CASE WHEN iui.unit_id IS NULL then
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END	                                                 as FreightUnit,
           -- isnull(toi.irmaRegCostHistory, 0)					as AdjustedCost,
			
			case toi.CostAdjMethod
				when 'x' then toi.Cost
				when '%' then 0
				when '$' then 0
				else '0'                                 
			end													as AdjustedCost,	
			isnull( toi.CostAdjDiscount, 0)                     as QuantityDiscount,
			(CASE WHEN isnull( toi.CostAdjDiscount, 0) = 0 THEN '0'
            ELSE
				case toi.CostAdjMethod
					when 'x' then '3'
					when '%' then '2'
					when '$' then '1'
					else '0'                                
				end
			END)												as DiscountType,	
            0                                                   as LandedCost,
            ISNULL(toi.lineItemCost, toi.Cost * toi.QuantityOrdered) as LineItemCost,
            0                                                   as LineItemFreight,
            0                                                   as LineItemHandling,
            ISNULL(toi.IrmaRegCostHistory,toi.Cost)				as UnitCost,
            ISNULL(toi.IrmaRegCostHistory,toi.Cost)				as UnitExtCost,
            toi.CasePack_Size                                   as Package_Desc1, -- Case Size
            ISNULL(iov.Package_Desc2,i.Package_Desc2)           as Package_Desc2,
            ISNULL(iov.Package_Unit_ID,i.Package_Unit_ID)       as Package_Unit_ID,
            0                                                   as MarkupPercent,
            toi.Cost                                            as MarkupCost,
            ISNULL(iov.Retail_Unit_ID,i.Retail_Unit_ID)         as Retail_Unit_ID,
            i.Origin_ID                                         as Origin_ID,
            i.CountryProc_ID                                    as CountryProc_ID,
            0                                                   as NetVendorItemDiscount,
			toi.ReasonCodeDetailId								as ReasonCodeDetailid,
			toi.CreditReasonCodeIRMAId							as CreditReason_ID,
			LEFT(toi.CreditLotNo,12)							as Lot_No,
			toi.CreditExpireDate								as ExpirationDate,
			toi.CreditNotes									    AS AdminNotes
         FROM
            #tmpHeadersInsert th

            JOIN #tmpOrderItems toi
               on ( toi.DVOOrderId = th.DVOOrderId )

            JOIN OrderHeader oh
               on ( oh.DVOOrderId = th.DVOOrderId )

            JOIN ItemIdentifier ii (nolock)
               on ( ii.Identifier = cast(cast(toi.upc as bigint) as varchar(15))
                    and ii.deleted_identifier = 0
                    and ii.remove_identifier = 0)

            JOIN Item i (nolock)
               on ( i.Item_Key = ii.Item_Key 
                    and i.deleted_Item = 0
                    and i.remove_Item = 0 )

            JOIN ItemUnit iuc (nolock)
               on ( iuc.Unit_Name = 'CASE' )

            JOIN ItemUnit iue (nolock)
               on ( iue.Unit_Name = 'EACH' )
              
           LEFT JOIN ItemUnit iui (NOLOCK)
				ON ( iui.Unit_Name = toi.orderuom )

            LEFT JOIN ItemOverride iov (nolock)
               on ( i.Item_Key = iov.Item_Key
                    AND iov.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store JOIN Vendor ON Store.Store_No = Vendor.Store_No WHERE Vendor.Vendor_ID = oh.PurchaseLocation_ID));

      -- **************************************************************************
      -- Build the records to update in OrderItems
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Building order item records to update into #tmpItemsUpdate ...';

      --declare #tmpItemsUpdate as table
      create table #tmpItemsUpdate
         (
         OrderHeader_ID               Integer,
         Item_Key                     Integer,
         Comments                     varchar(255),
         Units_Per_Pallet             smallint,
         QuantityUnit                 Integer,
         QuantityOrdered              decimal(18,4),
         Cost                         money,
         CostUnit                     Integer,
         Handling                     money,
         HandlingUnit                 Integer,
         Freight                      money,
         FreightUnit                  Integer,
         AdjustedCost                 money,
         QuantityDiscount             decimal(18,4),
         DiscountType                 Integer,
         LandedCost                   money,
         LineItemCost                 money,
         LineItemFreight              money,
         LineItemHandling             money,
         UnitCost                     money,
         UnitExtCost                  money,
         Package_Desc1                decimal(18,4), -- Case Size
         Package_Desc2                decimal(18,4),
         Package_Unit_ID              Integer,
         MarkupPercent                decimal(18,4),
         MarkupCost                   money,
         Retail_Unit_ID               Integer,
         Origin_ID                    Integer,
         CountryProc_ID               Integer,
         NetVendorItemDiscount        money,
		 ReasonCodeDetailid	          INTEGER,
		 CreditReason_ID			  INTEGER,
		 Lot_No						  VARCHAR(12),
		 ExpirationDate				  DATETIME,
		 AdminNotes					  VARCHAR(5000)
         );

      create index #tmpItmUpd on #tmpItemsUpdate (OrderHeader_ID, Item_Key);

      insert into #tmpItemsUpdate
         SELECT
            oh.OrderHeader_Id                                   as OrderHeader_ID,
            ii.Item_Key                                         as Item_Key,
            substring(toi.Note,1,255)                           as Comments,
            0                                                   as Units_Per_Pallet,
            CASE WHEN iui.unit_id IS NULL then
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END	                                                as QuantityUnit,
            toi.QuantityOrdered                                 as QuantityOrdered,
            --case toi.irmaRegCostHistory
			--	when null then toi.Cost
			--	else toi.irmaRegCostHistory									
			--end													as Cost,
			ISNULL(toi.irmaRegCostHistory, toi.Cost)            as Cost,
            CASE WHEN iui.unit_id IS NULL then
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END	                                                as CostUnit,
            0                                                   as Handling,
            CASE WHEN iui.unit_id IS NULL then
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END	                                                as HandlingUnit,
            0                                                   as Freight,
            CASE WHEN iui.unit_id IS NULL THEN
				case toi.iscase
				when 'true' then iuc.Unit_Id
				else iue.Unit_Id
				END
			ELSE
				iui.Unit_ID
			END
			                                                    as FreightUnit,
			
			case toi.CostAdjMethod
				when 'x' then toi.Cost
				when '%' then 0
				when '$' then 0
				else '0'                                
			end													as AdjustedCost,			
		   isnull( CostAdjDiscount, 0)                          as QuantityDiscount,
		   (CASE WHEN isnull( toi.CostAdjDiscount, 0) = 0 THEN '0'
            ELSE
				case toi.CostAdjMethod
					when 'x' then '3'
					when '%' then '2'
					when '$' then '1'
					else '0'                                
				end
			END)												as DiscountType,	
            0                                                   as LandedCost,
            ISNULL(toi.lineItemCost, toi.Cost * toi.QuantityOrdered) as LineItemCost,
            0                                                   as LineItemFreight,
            0                                                   as LineItemHandling,
            ISNULL(toi.IrmaRegCostHistory,toi.Cost)				as UnitCost,
            ISNULL(toi.IrmaRegCostHistory,toi.Cost)				as UnitExtCost,
			toi.CasePack_Size                                   as Package_Desc1, -- Case Size
            ISNULL(iov.Package_Desc2,i.Package_Desc2)           as Package_Desc2,
            ISNULL(iov.Package_Unit_ID,i.Package_Unit_ID)       as Package_Unit_ID,
            0                                                   as MarkupPercent,
            toi.Cost                                            as MarkupCost,
            ISNULL(iov.Retail_Unit_ID,i.Retail_Unit_ID)         as Retail_Unit_ID,
            i.Origin_ID                                         as Origin_ID,
            i.CountryProc_ID                                    as CountryProc_ID,
            0                                                   as NetVendorItemDiscount,
			toi.ReasonCodeDetailId								as ReasonCodeDetailid,
			toi.CreditReasonCodeIRMAId							AS CreditReason_id,
			LEFT(toi.CreditLotNo,12)							AS Lot_No,
			toi.CreditExpireDate								AS ExpirationDate,	
			toi.CreditNotes									    AS AdminNotes
         FROM
            #tmpHeaderUpdates th

            JOIN #tmpOrderItems toi
               on ( toi.DVOOrderId = th.DVOOrderId )

            JOIN OrderHeader oh
               on ( oh.DVOOrderId = th.DVOOrderId )

            JOIN ItemIdentifier ii (nolock)
               on ( ii.Identifier = cast(cast(toi.upc as bigint) as varchar(15))
                    and ii.deleted_identifier = 0
                    and ii.remove_identifier = 0)

            JOIN Item i (nolock)
               on ( i.Item_Key = ii.Item_Key 
                    and i.deleted_Item = 0
                    and i.remove_Item = 0)

            JOIN ItemUnit iuc (nolock)
               on ( iuc.Unit_Name = 'CASE' )

            JOIN ItemUnit iue (nolock)
               on ( iue.Unit_Name = 'EACH' )

			LEFT JOIN dbo.ItemUnit iui (NOLOCK)
				ON (iui.Unit_Name = toi.orderUOM )

            LEFT JOIN ItemOverride iov (nolock)
               on ( i.Item_Key = iov.Item_Key
                    AND iov.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store JOIN Vendor ON Store.Store_No = Vendor.Store_No WHERE Vendor.Vendor_ID = oh.PurchaseLocation_ID))
            WHERE oh.OrderExternalSourceID is not null;

      -- **************************************************************************
      -- Insert the new order detail information for those new orders
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Inserting new item detail records from #tmpItemsInsert ...';

      INSERT INTO OrderItem
         (
         OrderHeader_ID,
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
         NetVendorItemDiscount,
		 ReasonCodeDetailId,
		 CreditReason_ID,
		 Lot_No,
		 ExpirationDate,
		 AdminNotes
         )
      select * from #tmpItemsInsert;

      -- **************************************************************************
      -- Add messages to @log of the new orders inserted for logging
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Inserting new log records from #tmpItemsInsert ...';

      insert into @log
         select
             'Inserted item key [' + cast(th.Item_Key as varchar(12)) + '] Upc: [' + ii.Identifier + '] ' + i.Item_Description + ' into IRMA order [' +  cast(th.OrderHeader_Id as varchar(10)) + '] ...' msg
         from
            #tmpItemsInsert th

            JOIN Item i
               on ( i.Item_key = th.Item_Key )

            JOIN ItemIdentifier ii
               on ( ii.Item_key = i.Item_Key
                    and ii.Default_Identifier = 1 )
         order by
            th.OrderHeader_Id;

      -- **************************************************************************
      -- Update the items for orders already in the table
      -- **************************************************************************
      if ( @debug > 0) print  '[' + convert(varchar(10),getdate(), 101) + ' ' + convert(varchar(10),getdate(), 108) + '] Updating OrderItem from #tmpItemsUpdate ...';

      update oi set
         Comments                = ti.Comments,
         QuantityUnit            = ti.QuantityUnit,
         QuantityOrdered         = ti.QuantityOrdered,
         Cost                    = ti.Cost,
         CostUnit                = ti.CostUnit,
         HandlingUnit            = ti.HandlingUnit,
         FreightUnit             = ti.FreightUnit,
         AdjustedCost            = ti.AdjustedCost,
         LineItemCost            = ti.LineItemCost,
         UnitCost                = ti.UnitCost,
         UnitExtCost             = ti.UnitExtCost,
         MarkupCost              = ti.MarkupCost,
         Package_Desc1           = ti.Package_Desc1,
         Package_Desc2           = ti.Package_Desc2,
         Package_Unit_ID         = ti.Package_Unit_ID,
         Retail_Unit_ID          = ti.Retail_Unit_ID,
         Origin_ID               = ti.Origin_ID,
         CountryProc_ID          = ti.CountryProc_ID,
         NetVendorItemDiscount   = 0,
		 ReasonCodeDetailId		 = ti.ReasonCodeDetailid,
		 CreditReason_ID		 = ti.CreditReason_id,
		 Lot_No					 = ti.Lot_No,
		 ExpirationDate		     = ti.ExpirationDate,
		 AdminNotes			     = LEFT(ti.AdminNotes +  ' ' + oi.AdminNotes ,5000)
      FROM
         #tmpItemsUpdate ti

         JOIN OrderItem oi (nolock)
            on ( oi.OrderHeader_id = ti.OrderHeader_Id
                 and oi.Item_Key = ti.Item_Key);

      -- **************************************************************************
      -- Add new items to existing orders already created
      -- **************************************************************************
      INSERT INTO OrderItem
         (
         tiu.OrderHeader_ID,
         tiu.Item_Key,
         tiu.Comments,
         tiu.Units_Per_Pallet,
         tiu.QuantityUnit,
         tiu.QuantityOrdered,
         tiu.Cost,
         tiu.CostUnit,
         tiu.Handling,
         tiu.HandlingUnit,
         tiu.Freight,
         tiu.FreightUnit,
         tiu.AdjustedCost,
         tiu.QuantityDiscount,
         tiu.DiscountType,
         tiu.LandedCost,
         tiu.LineItemCost,
         tiu.LineItemFreight,
         tiu.LineItemHandling,
         tiu.UnitCost,
         tiu.UnitExtCost,
         tiu.Package_Desc1,
         tiu.Package_Desc2,
         tiu.Package_Unit_ID,
         tiu.MarkupPercent,
         tiu.MarkupCost,
         tiu.Retail_Unit_ID,
         tiu.Origin_ID,
         tiu.CountryProc_ID,
         tiu.NetVendorItemDiscount,
		 tiu.ReasonCodeDetailid,
		 tiu.CreditReason_id,
		 tiu.Lot_No,
		 tiu.ExpirationDate,
		 tiu.AdminNotes
         )
		select tiu.* from #tmpItemsUpdate tiu
		where NOT EXISTS(select Item_Key from OrderItem where Item_Key = tiu.Item_Key and OrderHeader_ID = tiu.OrderHeader_ID)
							  
      -- **************************************************************************
      -- Update OrderHeaderCosts
      -- **************************************************************************
  --       UPDATE
		--	oh
		--SET
		--	OrderedCost				= ISNULL((SELECT SUM(LineItemCost) FROM OrderItem WHERE OrderHeader_ID = oh.OrderHeader_ID),0),
		--	AdjustedReceivedCost	= ISNULL((SELECT SUM(ReceivedItemCost) FROM OrderItem WHERE OrderHeader_ID = oh.OrderHeader_ID),0),
		--	OriginalReceivedCost	= ISNULL((SELECT SUM(OrigReceiveditemCost * ISNULL(QuantityReceived, 0)) FROM OrderItem WHERE OrderHeader_ID = oh.OrderHeader_ID),0)
		-- FROM
  --          #tmpHeaderUpdates th

  --          JOIN OrderHeader oh
  --             on ( oh.DVOOrderId = th.DVOOrderId )

  --       UPDATE
		--	oh
		--SET
		--	OrderedCost				= ISNULL((SELECT SUM(LineItemCost) FROM OrderItem WHERE OrderHeader_ID = oh.OrderHeader_ID),0),
		--	AdjustedReceivedCost	= ISNULL((SELECT SUM(ReceivedItemCost) FROM OrderItem WHERE OrderHeader_ID = oh.OrderHeader_ID),0),
		--	OriginalReceivedCost	= ISNULL((SELECT SUM(OrigReceiveditemCost * ISNULL(QuantityReceived, 0)) FROM OrderItem WHERE OrderHeader_ID = oh.OrderHeader_ID),0)
		-- FROM
  --          #tmpHeadersInsert th

  --          JOIN OrderHeader oh
  --             on ( oh.DVOOrderId = th.DVOOrderId )

      -- **************************************************************************
      -- Add messages to @log of the new orders inserted for logging
      -- **************************************************************************
      insert into @log
         select
             'Updated item key [' + cast(tiu.Item_Key as varchar(12)) + '] Upc: [' + ii.Identifier + '] ' + i.Item_Description + ' into IRMA order [' +  cast(tiu.OrderHeader_Id as varchar(10)) + '] ...' msg
         from
            #tmpItemsUpdate tiu

            JOIN Item i
               on ( i.Item_key = tiu.Item_Key )

            JOIN ItemIdentifier ii
               on ( ii.Item_key = i.Item_Key
                    and ii.Default_Identifier = 1 )
         order by
            tiu.OrderHeader_Id;
            
  	  -- **************************************************************************
      -- Add messages to @log of the new items added to existing orders
      -- **************************************************************************
      insert into @log
         select
             'Added item key [' + cast(tiu.Item_Key as varchar(12)) + '] Upc: [' + ii.Identifier + '] ' + i.Item_Description + ' to existing IRMA order [' +  cast(tiu.OrderHeader_Id as varchar(10)) + '] ...' msg
         from #tmpItemsUpdate tiu
            
            JOIN Item i
               on ( i.Item_key = tiu.Item_Key )

            JOIN ItemIdentifier ii
               on ( ii.Item_key = i.Item_Key
                    and ii.Default_Identifier = 1 )
                    
		 where NOT EXISTS(select Item_Key from OrderItem where Item_Key = tiu.Item_Key and OrderHeader_ID = tiu.OrderHeader_ID)
		 
         order by
            tiu.OrderHeader_Id;

   end try
   begin catch
      insert into @log values (error_message() + ' at line: ' + convert(varchar(4),error_line()));
   end catch

   select msg from @log;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DVO_ProcessBulkOrders] TO [IRMASchedJobsRole]
    AS [dbo];

