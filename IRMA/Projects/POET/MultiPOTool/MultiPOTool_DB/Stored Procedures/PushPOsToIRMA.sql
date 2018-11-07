
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PushPOsToIRMA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PushPOsToIRMA]
GO

CREATE PROCEDURE [dbo].[PushPOsToIRMA]
	@RegionID int
AS
BEGIN
	declare @ErrorStatus bit
	declare @IRMAServer varchar(6)
	declare @IRMADatabase varchar(50)
	declare @DBString varchar(max)
	
	DECLARE @today datetime 
	SELECT @today = GETDATE()
	
	SELECT @IRMAServer = IRMAServer, @IRMADatabase = IRMADataBase FROM Regions WHERE RegionID = @RegionID
	SET @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'

	-- Set PushedByUserID to PO Creator when auto push date is today or in the past
	UPDATE POHeader 
	SET PushedByUserID = p.AssignedByUserID
	FROM POHeader (nolock) h
	INNER JOIN PONumber (nolock) p ON h.PONumberID = p.PONumberID
	INNER JOIN UploadSessionHistory (nolock) u on h.UploadSessionHistoryID = u.UploadSessionHistoryID
	WHERE u.ValidationSuccessful = 1 AND h.PushedByUserID = 0 AND h.PushedToIRMADate IS NULL AND h.AutoPushDate <= @today AND h.RegionID = @RegionID
	
	-- Insert POs INTO queue when auto push date is today or in the past		
	INSERT INTO PushToIRMAQueue
		SELECT POHeaderID, 0
		FROM POHeader (nolock) h 
		INNER JOIN UploadSessionHistory (nolock) u on h.UploadSessionHistoryID = u.UploadSessionHistoryID
		WHERE u.ValidationSuccessful = 1 and h.PushedToIRMADate IS NULL AND h.AutoPushDate <= @today AND h.RegionID = @RegionID AND h.POHeaderID NOT IN (SELECT POHeaderID FROM PushToIRMAQueue) 

	IF object_id('tempdb..#POQueue') IS NOT NULL
	DROP TABLE #POQueue

	CREATE TABLE #POQueue
	(
		POQueueID int identity (1,1) not null, 
		POHeaderID int
	)

	declare @Processing table (PushToIRMAQueueID int)

	--we'll use this table variable to hold the PO's we're pushing
	insert into @Processing
	select Q.PushToIRMAQueueID
	from PushToIRMAQueue Q 
	inner join POHeader H on Q.POHeaderID = H.POHeaderID
	where Q.ProcessingFlag = 0
	and H.RegionID = @RegionID
	
	--first, set the ProcessingFlag to 1 to indicate we're going to work on this PO
	update PushToIRMAQueue set ProcessingFlag = 1
	from PushToIRMAQueue Q 
	inner join @Processing P on Q.PushToIRMAQueueID = P.PushToIRMAQueueID

	--now, we'll begin the push
	insert into #POQueue(POHeaderID) 
	select Q.POHeaderID 
	from 
	@Processing P
	inner join PushToIRMAQueue Q on P.PushToIRMAQueueID = Q.PushToIRMAQueueID
	inner join POHeader H on Q.POHeaderID = H.POHeaderID
	where H.RegionID = @RegionID

	declare @record_counter int
	declare @loop_counter int
	set @loop_counter = isnull((select count(*) from #POQueue),0)
	set @record_counter = 1
	
		--change for Sent date in IRMA as per the time zvone offset... BUG # 2054
		DECLARE @CentralTimeZoneOffset int
		declare @Sentdate as datetime
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Regions where regionID = @regionID

	set @Sentdate =  DATEADD(HOUR, @CentralTimeZoneOffset, GETDATE()) 
	--end change

	while @loop_counter > 0 AND @record_counter <= @loop_counter

	BEGIN
		BEGIN TRY
			declare @POHeaderID int
			select @POHeaderID = POHeaderID from #POQueue where POQueueID = @record_counter
			declare @PONumber varchar(9)
				
		    select @PONumber = cast(PONumber as varchar(9)) from 
															PONumber N inner join POHeader H on N.PONumberID = H.PONumberID 
															where POHeaderID = @POHeaderID
			--20090813 - Dave Stacey - Added functionality to add user from POET as derived from IRMA username or generic MultiPOTool ID
			declare @sql nvarchar(max)
			declare @multipotool_ID int

				set @sql = N'select @multipotool_IDOUT = max(User_ID) from ' + @DBString + '[Users] IU
								where  IU.UserName = ''multipotool'''	
				exec sp_executesql @sql															--query
					, N'@multipotool_IDOUT int OUTPUT'				--variable definitions
					, @multipotool_IDOUT = @multipotool_ID OUTPUT		--variable values
			
					
			select @sql = 
					N'insert into ' + @DBString + '[OrderHeader]
						(OrderHeaderDesc
						, Vendor_ID
						, PurchaseLocation_ID
						, ReceiveLocation_ID
						, CreatedBy
						, OrderDate
						, Sent
						, Fax_Order
						, Expected_Date
						, SentDate
						, QuantityDiscount
						, DiscountType
						, Transfer_To_Subteam
						, Return_Order
						, WarehouseSent
						, OrderType_ID
						, ProductType_ID
						, FromQueue
						, OrderExternalSourceID
						, OrderExternalSourceOrderID
						, CurrencyID
						, ReasonCodeDetailID
						, OrderedCost)
					select 
						@PONumber + '' from POET '' + isnull(H.Notes,'''') as OrderHeaderDesc
						, H.IRMAVendor_ID as Vendor_ID
						, IVS.Vendor_ID as PurchaseLocation_ID
						, IVS.Vendor_ID as ReceiveLocation_ID
						, ISNULL(IU.User_ID, @multipotool_ID) as CreatedBy
						, cast(H.CreatedDate as smalldatetime) as OrderDate
						, 1 as Sent
						, 0 as Fax_Order
						, cast(H.ExpectedDate as smalldatetime) as Expected_Date
						, cast(''' + cast(@Sentdate as varchar) + ''' as smalldatetime) as SentDate 
						, H.DiscountAmount as QuantityDiscount
						, CASE WHEN H.DiscountType = ''%'' THEN 2 ELSE 0 END as DiscountType
						, H.Subteam as Transfer_To_SubTeam
						, 0 as Return_Order
						, 0 as WarehouseSent
						, 1 as OrderType_ID
						, 1 as ProductType_ID
						, 0 as FromQueue
						, 5
						, @PONumber
						, V.CurrencyID
						, H.ReasonCode
						, I.OrderedCost
					from 
						POHeader H
						join (select POHeaderID, sum((OrderQuantity - isnull(FreeQuantity,0)) * UnitCost) as OrderedCost
						      from POItem 
						      group by POHeaderID
						      having POHeaderID = @POHeaderID 
						      ) I on H.POHeaderID = I.POHeaderID
						join PONumber N on H.PONumberID = N.PONumberID
						join Users U on U.UserID = H.PushedByUserID
						inner join ' + @DBString + '[Store] IST on H.BusinessUnit = IST.BusinessUnit_ID
						inner join ' + @DBString + '[Vendor] IVS on IVS.Store_No = IST.Store_No
						inner join ' + @DBString + '[Vendor] V on V.Vendor_ID = H.IRMAVendor_ID
						left join ' + @DBString + '[Users] IU on U.UserName = IU.Username
					where H.POHeaderID = @POHeaderID'
					exec sp_executesql @sql	
						, N'@PONumber varchar(9),@POHeaderID int, @multipotool_ID int'
						, @PONumber = @PONUmber, @POHeaderID = @POHeaderID, @multipotool_ID = @multipotool_ID
					

			declare @OrderHeader_ID int

				set @sql = N'select @OrderHeader_IDOUT = max(OrderHeader_ID) from ' + @DBString + '[OrderHeader]
								where OrderExternalSourceOrderID = @PONumber '	
				exec sp_executesql @sql															--query--anj--OrderHeaderDesc like @PONumber + '' from POET %''
					, N'@OrderHeader_IDOUT int OUTPUT, @PONumber varchar(9)'				--variable definitions
					, @OrderHeader_IDOUT = @OrderHeader_ID OUTPUT, @PONumber = @PONumber		--variable values

			--20090903 - Dave Stacey - Added functionality to send the correct Vendor Pack size to IRMA w/the order items.. 
			-- A cursor below calls a proc w/Package_desc1 output for each item in the order - these are inserted into a temp table which is joined to the insert query
	----insert order item

			select @sql = 
				N'
DECLARE @ItemKey INT, @Package_Desc1 decimal(9, 4), @IRMAVendor_ID INT, @Store_No INT

DECLARE @tblVendorPack TABLE
(Item_Key int, Package_Desc1 decimal(9, 4))

DECLARE @UnitCostOUT money, @UnitFreightOUT money, @MSRPOUT money, @Package_Desc1OUT decimal(9,4), @Date Datetime
Select @Date = GETDATE()
DECLARE vp_cursor CURSOR FOR 
				select I.Item_Key, H.IRMAVendor_ID, S.Store_No
				from dbo.POHeader H (NOLOCK)
				inner join dbo.POItem I (NOLOCK) on H.POHeaderID = I.POHeaderID
				inner join dbo.PONumber N (NOLOCK) on H.PONumberID = N.PONumberID
				inner join ' + @DBString + '[Vendor] IV on H.VendorPSNumber = IV.PS_Export_Vendor_ID
				inner join ' + @DBString + '[Item] II on I.Item_Key = II.Item_Key
				inner join ' + @DBString + '[Store] S on  S.StoreAbbr = H.StoreAbbr
				where I.OrderQuantity > 0 and H.POHeaderID = @POHeaderID
				GROUP BY I.Item_Key, H.IRMAVendor_ID, S.Store_No
OPEN vp_cursor

FETCH NEXT FROM vp_cursor 
INTO @ItemKey, @IRMAVendor_ID, @Store_No 

WHILE @@FETCH_STATUS = 0
BEGIN

exec ' + @DBString + '[GetVendorCost] @ItemKey, @IRMAVendor_ID, @Store_No, 0, @Date, @UnitCost = @UnitCostOUT OUTPUT, @UnitFreight = @UnitFreightOUT OUTPUT, @MSRP = @MSRPOUT OUTPUT, @Package_Desc1 = @Package_Desc1OUT OUTPUT


INSERT @tblVendorPack (Item_Key, Package_Desc1) SELECT @ItemKey, @Package_Desc1OUT

FETCH NEXT FROM vp_cursor 
INTO @ItemKey, @IRMAVendor_ID, @Store_No
END 
CLOSE vp_cursor
DEALLOCATE vp_cursor

				insert into ' + @DBString + '[OrderItem]
					(OrderHeader_ID, Item_Key, QuantityOrdered, QuantityUnit, Cost
					, UnitCost, UnitExtCost, LineItemCost, LandedCost, MarkupCost
					, CostUnit, QuantityDiscount, DiscountType, HandlingUnit
					, FreightUnit, Package_Desc1, Package_Desc2, Package_Unit_ID
					, Retail_Unit_ID, NetVendorItemDiscount, Freight3Party, LineItemFreight3Party
					, HandlingCharge,ReasonCodeDetailID)
				select distinct 
					@OrderHeader_ID, I.Item_Key as Item_Key, I.OrderQuantity as QuantityOrdered
					, I.UnitCostUOM as QuantityUnit, Cost = I.OrigUnitCost 
					, UnitCost =
						((I.OrderQuantity - isnull(I.FreeQuantity,0))
							* I.UnitCost)
						/I.OrderQuantity
					, UnitExtCost =
						((I.OrderQuantity - isnull(I.FreeQuantity,0))
							* I.UnitCost)
						/I.OrderQuantity
					, LineItemCost = 
						(I.OrderQuantity - isnull(I.FreeQuantity,0))
						* I.UnitCost
					, LandedCost = 
						((I.OrderQuantity - isnull(I.FreeQuantity,0))
							* I.UnitCost)
						/I.OrderQuantity
					, MarkupCost = 
						((I.OrderQuantity - isnull(I.FreeQuantity,0))
							* I.UnitCost)
						/I.OrderQuantity
					, I.UnitCostUOM as CostUnit, I.DiscountAmount as QuantityDiscount
					, case 
						when I.DiscountType = ''$'' then 1 
						when I.DiscountType = ''%'' then 2
						when I.DiscountType = ''FF'' then 3
						else 0 end as DiscountType
 					, I.UnitCostUOM as HandlingUnit, I.UnitCostUOM as FreightUnit
					, ISNULL(VP.Package_Desc1, ISNULL(II.Package_Desc1, 1)), II.Package_Desc2, II.Package_Unit_ID, II.Retail_Unit_ID
					, 0 as NetVendorItemDiscount, 0 as Freight3Party, 0 as LineItemFreight3Party, 0 as HandlingCharge
					, I.ReasonCode
					
					
				from dbo.POHeader H (NOLOCK)
				inner join dbo.POItem I (NOLOCK) on H.POHeaderID = I.POHeaderID
				inner join dbo.PONumber N (NOLOCK) on H.PONumberID = N.PONumberID
				inner join ' + @DBString + '[Vendor] IV on H.VendorPSNumber = IV.PS_Export_Vendor_ID
				inner join ' + @DBString + '[Item] II on I.Item_Key = II.Item_Key
				LEFT JOIN @tblVendorPack VP ON VP.Item_Key = II.Item_Key
				where I.OrderQuantity > 0 and H.POHeaderID = @POHeaderID'

			exec sp_executesql @sql	
				, N'@OrderHeader_ID int, @POHeaderID int'
				, @OrderHeader_ID = @OrderHeader_ID,@POHeaderID = @POHeaderID
				
			set @sql  = N'
				INSERT INTO ' + @DBString + '[ExternalOrderInformation] ( OrderHeader_Id ,ExternalSource_Id ,ExternalOrder_Id)
				SELECT TOP 1 @OrderHeader_ID, oes.id,  @PONumber
				FROM ' + @DBString + '[OrderExternalSource] oes WHERE Description = ''POET'' '

			EXEC sp_executesql @sql
				, N'@OrderHeader_ID int, @PONumber int'
				, @OrderHeader_ID = @OrderHeader_ID,@PONumber = @PONumber


			update POHeader 
			set PushedToIRMADate = getdate() 
			--NEW updating with IRMA PO Number
			    ,IRMAPONumber  = @OrderHeader_ID
			    --END NEW
			where POHeaderID = @POHeaderID

		END TRY
		BEGIN CATCH

				declare @ErrorMessage nvarchar(4000);
				set	@ErrorMessage = 
				'SQL error while pushing to RegionID ' + isnull(cast(@RegionID as char(2)),'unknown') 
				+ ', POHeaderID ' + isnull(cast(@POHeaderID as char(9)),'unknown') 
				+ ' - '  + ERROR_MESSAGE() + ' - ' + @sql

				insert into ErrorLog (Timestamp, ErrorMessage) values(getdate(), @ErrorMessage)

			set @ErrorStatus = 1
		END CATCH;

		set @record_counter = @record_counter + 1

	END

	if @ErrorStatus = 1 RAISERROR('SQL error entered in ErrorLog table',11,1)

	delete from PushToIRMAQueue 
	where PushToIRMAQueueID in 
	(select PushToIRMAQueueID from @Processing)
	and ProcessingFlag = 1
END
GO
