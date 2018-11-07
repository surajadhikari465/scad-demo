CREATE proc [dbo].[DVO_ImportOrderHeader]
  (
   @rectype            varchar(1),
   @DvoOrderId         varchar(10),
   @BuyerId            varchar(10),
   @PSVendorId         varchar(10),
   @TotalCost          varchar(10),
   @POEntryDate        varchar(20),
   @Region             varchar(2),
   @Note               varchar(2050),
   @BusinessUnitId     varchar(10),
   @CustomerId         varchar(10),
   @TotalCases         varchar(10),
   @TotalUnits         varchar(10),
   @TransDate          varchar(30),
   @SubTeam            varchar(4),
   @BuyerName          varchar(200),
   @POS_SubTeam        varchar(4),
   @PS_SubTeam		   varchar(4),
   @IRMAPONo		   varchar(10),
   @ExpectedDate	   varchar(20),
   @IsCredit		   varchar(5) = 'false'
   )
as
   -- **************************************************************************
   -- Procedure: DVO_ImportOrderHeader()
   --    Author: Ron Savage
   --      Date: 10/25/2007
   --
   -- Description:
   -- This procedure imports order header information sent from DVO in order
   -- export files.
   --
   -- Modification History:
   -- Date        Init Comment
   -- 2/11/2009   RDE  TFS 11717 - Add DVO Credit support.
   -- 12/21/2009  BSR  Bug 9784 Update statement was updating Transfer_To_Subteam with @POS_Subteam
   --				   changed to @IRMASubteam to match the insert statement.
   -- 09/17/2009  BSR  Added Insert/Update OrderExternalSourceOrderID and OrderExternalSourceID = 2
   -- 08/26/2009  MU   Added ExpectedDate
   -- 07/17/2009  TL   [Tom Lux] Subteam mapping fix.  Will be part of IRMA v3.4.8.
   -- 07/14/2009  BBB  Added CurrencyID value to insert of OrderHeader based upon Vendor;   
   -- 08/20/2008  RS   Added " from DVO" to the search for an existing order number to avoid
   --                  updating an OrderLink order with the same order number.
   -- 06/17/2008  RS   Added the DVOOrderId field to the insert/update statements for orders.
   -- 06/10/2008  RS   Added a section to check incoming arguments for being null,
   --                  to set them to '' as they were coming in before the 
   --                  DataMonkey 1.2.0 fix to start sending nulls.
   -- 02/19/2008  RS   Removed cast of PS_Vendor_Id to bigint to handle ids with
   --                  alpha characters added for DVO integration ... used alternate
   --                  method to match regardless of left padded 0s.
   -- 01/24/2008  RS   Cast PS_Vendor_Id and @PSVendorId to bigint for the
   --                  joins to clear off any left padded 0s.
   -- 11/27/2007  RS   Added setting SentDate to getdate() in the OrderHeader
   --                  table, to enable the recieving button on the order form.
   -- 11/26/2007  RS   Fixed second Users join to use u1.FullName
   -- 11/20/2007  RS   Added code to update an existing header, or insert a
   --                  new order if it doesn't exist.
   -- 10/20/2007  RS   Created.
   -- **************************************************************************
begin
   --**************************************************************************
   -- Create a log table variable to store messages to be returned to the
   -- calling application.
   --**************************************************************************
   declare @log             as table ( msg  varchar(MAX) );
   declare @OrderHeader_id  as int;
   declare @IRMASubTeam     as int;

   --**************************************************************************
   -- Check for null incoming values and set them to '' 
   --**************************************************************************
   if (@rectype is null)         set @rectype = '';
   if (@DvoOrderId is null)      set @DvoOrderId = '';
   if (@BuyerId is null)         set @BuyerId = '';
   if (@PSVendorId is null)      set @PSVendorId = '';
   if (@TotalCost is null)       set @TotalCost = '';
   if (@POEntryDate is null)     set @POEntryDate = '';
   if (@Region is null)          set @Region = '';
   if (@Note is null)            set @Note = '';
   if (@BusinessUnitId is null)  set @BusinessUnitId = '';
   if (@CustomerId is null)      set @CustomerId = '';
   if (@TotalCases is null)      set @TotalCases = '';
   if (@TotalUnits is null)      set @TotalUnits = '';
   if (@TransDate is null)       set @TransDate = '';
   if (@SubTeam is null)         set @SubTeam = '';
   if (@BuyerName is null)       set @BuyerName = '';
   if (@POS_SubTeam is null)     set @POS_SubTeam = '';
   if (@IsCredit is null)		 set @IsCredit = 'false';
   
   
   
   
--    insert into @log values('OrderHeader Start');

   --**************************************************************************
   -- If this order has already been imported, update the record in case
   -- something has been corrected or changed.
   --**************************************************************************
   select @OrderHeader_id = max(OrderHeader_Id) from OrderHeader where OrderHeaderDesc like rtrim(@DvoOrderId) + ' from DVO%';
   
   if ((@OrderHeader_id is null) and ((@IRMAPONo is not null) or (@IRMAPONo <> '')))
	  select @OrderHeader_id = @IRMAPONo
	  
	-- Tom Lux, 7/17/2009, Subteam mapping fix.  Will be part of IRMA v3.4.8.
	if @PS_SubTeam is not null and @PS_SubTeam <> ''
	begin
		-- We use a temp table because there could be multiple subteams in the StoreSubteam table for the PS subteam coming in and we need to handle this scenario.
		declare @subteamList table ( subteam_no int null );
		insert into @subteamList
			select SubTeam_No
			from StoreSubTeam
			where PS_SubTeam_No = @PS_SubTeam
			AND Store_No = (SELECT Store_No FROM Store WHERE BusinessUnit_Id = @BusinessUnitId)

		if exists ( select * from @subteamList where subteam_no = @POS_SubTeam )
			select @IRMASubTeam = @POS_SubTeam -- Take the subteam passed from DVO if it exists in StoreSubteam results.
		else
			select top 1 @IRMASubTeam = subteam_no from @subteamList -- Take the first subteam from the StoreSubteam results if the DVO subteam is not in the results.
	end
   select @IRMASubTeam = ISNULL(@IRMASubTeam,@POS_SubTeam)

   begin try
   if (@OrderHeader_id is null)
      begin
      --**************************************************************************
      -- Insert the new header information
      --**************************************************************************
      INSERT INTO OrderHeader (Vendor_ID,
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
							   OrderExternalSourceID)
          select top 1
             v.Vendor_Id                                               as Vendor_ID,
             substring(@DvoOrderId + ' from DVO (' + @BuyerName + ') ' + @Note,1,255) as OrderHeaderDesc,
             cast(@POEntryDate as smalldatetime)                       as OrderDate,
             1                                                         as OrderType_ID,
             1                                                         as ProductType_ID,
             vp.Vendor_Id                                              as PurchaseLocation_ID,
             vr.Vendor_Id                                              as ReceiveLocation_ID,
             null                                                      as Transfer_SubTeam,
             @IRMASubTeam                                              as Transfer_To_SubTeam,
             0                                                         as Fax_Order,
             cast(@ExpectedDate as datetime)				           as Expected_Date,
             isnull(u1.User_Id,u.User_Id)                              as CreatedBy,
             case when @IsCredit='true' then 1 else 0 end			   as Return_Order,
             1                                                         as Sent,
             getdate()                                                 as SentDate,
             @DvoOrderId                                               as DVOOrderId,
             v.CurrencyID											   as CurrencyID,
			 @DvoOrderId											   as OrderExternalSourceOrderID,
			 2														   as OrderExternalSourceID
          from
             Store s

              LEFT OUTER JOIN Vendor v
                on ( right('00000000' + v.PS_Vendor_Id, len(v.PS_Vendor_Id) ) = right('00000000' + @PSVendorId, len(v.PS_Vendor_Id) ) )

              LEFT OUTER JOIN Vendor vp
                on ( vp.Store_No = s.Store_No )

              LEFT OUTER JOIN Vendor vr
                on ( vr.Store_No = s.Store_No )

              LEFT OUTER JOIN Users u
                on ( u.FullName = 'DVO Import' )

              LEFT OUTER JOIN Users u1
                on ( u1.FullName = @BuyerName )
          where
             s.BusinessUnit_Id = @BusinessUnitId;

      insert into @log values ('Inserted a new DVO' + case when @IsCredit = 'true' then ' Credit ' else ' ' end +  'Order into IRMA, order id: [' + convert(varchar(10), @@IDENTITY ) + ']');

      end
   else
      begin
      --**************************************************************************
      -- Update the existing record
      --**************************************************************************
      insert into @log values ('Updating an existing IRMA order from DVO [' + case when @IsCredit = 'true' then 'Credit ' else '' end  + convert(varchar(10),@OrderHeader_Id) + ']');

      update oh set
         Vendor_Id					= v.Vendor_Id,
         OrderHeaderDesc			= substring(@DvoOrderId + ' from DVO (' + @BuyerName + ') ' + ISNULL(@Note,''),1,255),
         OrderDate					= cast(@POEntryDate as smalldatetime),
         PurchaseLocation_ID		= vp.Vendor_Id,
         ReceiveLocation_ID			= vr.Vendor_Id,
--         Transfer_To_SubTeam		= @IRMASubTeam,
         Expected_Date				= @ExpectedDate,
         CreatedBy					= isnull(u1.User_Id,u.User_Id),
         SentDate					= getdate(),
         DVOOrderId					= @DvoOrderId,
         CurrencyID					= v.CurrencyID,
		 OrderExternalSourceOrderID = @DvoOrderId,
		 OrderExternalSourceID		= 2,
		 Return_Order				= case when @IsCredit='true' then 1 else 0 end				   
      from
         OrderHeader oh

         JOIN Store s
            on ( s.BusinessUnit_Id = @BusinessUnitId )

         LEFT OUTER JOIN Vendor v
                on ( right('00000000' + v.PS_Vendor_Id, len(v.PS_Vendor_Id) ) = right('00000000' + @PSVendorId, len(v.PS_Vendor_Id) ) )

          LEFT OUTER JOIN Vendor vp
            on ( vp.Store_No = s.Store_No )

          LEFT OUTER JOIN Vendor vr
            on ( vr.Store_No = s.Store_No )

          LEFT OUTER JOIN Users u
            on ( u.FullName = 'DVO Import' )

          LEFT OUTER JOIN Users u1
            on ( u1.FullName = @BuyerName )
      where
         oh.OrderHeader_Id = @OrderHeader_id;
         
      if ((@IRMAPONo is not null) or (@IRMAPONo <> ''))
		exec dbo.UpdateOrderSentDate @OrderHeader_id
      end
   end try
   begin catch
      insert into @log values (error_message());
   end catch

   select msg from @log;
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DVO_ImportOrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];

