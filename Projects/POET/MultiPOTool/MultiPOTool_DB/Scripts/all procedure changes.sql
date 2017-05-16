/****** Object:  StoredProcedure [dbo].[GetPOsPushedToIRMA]    Script Date: 06/02/2011 10:15:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPOsPushedToIRMA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPOsPushedToIRMA]
GO

CREATE PROCEDURE [dbo].[GetPOsPushedToIRMA]
	

	@UserID int
	--,@Top int 
	
AS
	
BEGIN

--DECLARE @sql VARCHAR(1000)

SET NOCOUNT ON 

	-- SET @sql = 'SELECT TOP ' + CONVERT(VARCHAR, @Top) + '
	SELECT 
	
	U.UploadSessionHistoryID
	, H.POHeaderID
	, H.RegionID
	, R.RegionName
	, N.PONumber
	, H.BusinessUnit
	, H.StoreAbbr
	, H.Subteam
	, H.VendorName
	, H.VendorPSNumber
	, H.OrderItemCount
	, H.TotalPOCost
	, H.ExpectedDate
	, H.CreatedDate
	, H.PushedToIRMADate
	, H.ConfirmedInIRMADate
	, H.Notes
	from
	POHeader H
	inner join Regions R on H.RegionID = R.RegionID
	inner join PONumber N on H.PONumberID = N.PONumberID
	inner join UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
	where U.ValidationSuccessful = 1
	and isnull(H.Expired,0) = 0
	and H.DeletedDate is null
	and H.PushedToIRMADate is not null 
	and U.UploadUserID =CONVERT(VARCHAR, @UserID)  

--EXEC(@sql)

END



GO

/****** Object:  StoredProcedure [dbo].[GetRegionsByUser]    Script Date: 06/02/2011 10:15:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRegionsByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRegionsByUser]
GO

/****** Object:  StoredProcedure [dbo].[GetRegionsByUser]    Script Date: 06/02/2011 10:15:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetRegionsByUser]
	@UserID int

AS
BEGIN
	declare @GlobalBuyer bit
	select @GlobalBuyer = GlobalBuyer from Users where UserID = @UserID
	
	if @GlobalBuyer = 1
		begin
			select *
			from Regions
			where RegionName != 'Global' 
		end
	else
		begin
			select R.RegionName, R.RegionID
			from Regions R inner join Users U on U.RegionID = R.RegionID
			where U.UserID = @UserID
		end
END
GO


/****** Object:  StoredProcedure [dbo].[GetStoreNamesbyRegion]    Script Date: 06/02/2011 10:15:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStoreNamesbyRegion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStoreNamesbyRegion]
GO

/****** Object:  StoredProcedure [dbo].[GetStoreNamesbyRegion]    Script Date: 06/02/2011 10:15:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Anjana M
-- Create date: 3/30/2011
-- Description:	to get the list of Regions based on the user login region.
-- =============================================
CREATE PROCEDURE [dbo].[GetStoreNamesbyRegion] 
	@UserID as int
	, @DBString varchar(250) = null
AS
DECLARE @RegionID as int,
@IRMAServer varchar(6),
          @IRMADatabase varchar(50),
       --   @DBString varchar(max),
          @Sql nvarchar(max)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		set @RegionID = (SELECT RegionID from users where userID = @UserID)
		
		
	IF object_id('tempdb..#TEMPMySelect') IS NOT NULL
	DROP TABLE #TEMPMySStores
	
	create table #TEMPMySStores
	(
		Store_No int 
		,Store_Name varchar(50)
		, StoreAbbr varchar(10)
	)

insert into #TEMPMySStores(Store_No,Store_Name,StoreAbbr) values (0,'--Select One--','')

if @DBString is null
begin
	select @IRMAServer = IRMAServer , @IRMADatabase = IRMADataBase
	 from Regions where RegionID = @RegionID
	
	set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'
end	
	
	set @sql = 'select [Store_No]
      ,[Store_Name],[StoreAbbr] from ' + @DBString + '[Store]'
	
insert into #TEMPMySStores exec sp_executesql @sql
	
select * from #TEMPMySStores

END

GO


/****** Object:  StoredProcedure [dbo].[GetSubTeamNamesbyRegion]    Script Date: 06/02/2011 10:15:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubTeamNamesbyRegion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSubTeamNamesbyRegion]
GO

/****** Object:  StoredProcedure [dbo].[GetSubTeamNamesbyRegion]    Script Date: 06/02/2011 10:15:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Anjana M
-- Create date: 3/31/2011
-- Description:	to get the list of Sub-Teams based on the user login region.
-- =============================================
CREATE PROCEDURE [dbo].[GetSubTeamNamesbyRegion] 
	@UserID as int
	, @DBString varchar(250) = null
	
AS
DECLARE @RegionID as int,
@IRMAServer varchar(6),
          @IRMADatabase varchar(50),
      --    @DBString varchar(max),
          @Sql nvarchar(max)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		set @RegionID = (SELECT RegionID from users where userID = @UserID)
		
		IF object_id('tempdb..#TEMPMySubTeam') IS NOT NULL
	DROP TABLE #TEMPMySubTeam
	
	create table #TEMPMySubTeam
	(
		SubTeam_No int 
		,SubTeam_Name varchar(50)
		
	)
	
	insert into #TEMPMySubTeam(SubTeam_No,SubTeam_Name) values (0,'--Select One--')


if @DBString is null
begin

	select @IRMAServer = IRMAServer , @IRMADatabase = IRMADataBase
	 from Regions where RegionID = @RegionID
	 

		
	set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'
	
end
	
	set @sql = 'select SubTeam_No,
      SubTeam_Name from ' + @DBString + '[SubTeam]'
	
	
insert into #TEMPMySubTeam exec sp_executesql @sql
	
select * from #TEMPMySubTeam
	
	


END

GO


/****** Object:  StoredProcedure [dbo].[GetVendorNamesbyRegion]    Script Date: 06/02/2011 10:15:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVendorNamesbyRegion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVendorNamesbyRegion]
GO

/****** Object:  StoredProcedure [dbo].[GetVendorNamesbyRegion]    Script Date: 06/02/2011 10:15:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Anjana M
-- Create date: 3/31/2011
-- Description:	to get the list of vendors based on the user login region.
-- =============================================
CREATE PROCEDURE [dbo].[GetVendorNamesbyRegion] 
	@UserID as int
	, @DBString varchar(250) = null
	
	
AS
DECLARE @RegionID as int,
@IRMAServer varchar(6),
          @IRMADatabase varchar(50),
        --  @DBString varchar(max),
          @Sql nvarchar(max)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		set @RegionID = (SELECT RegionID from users where userID = @UserID)
		
		IF object_id('tempdb..#TEMPMySelect') IS NOT NULL
	DROP TABLE #TEMPMyVendor
	
	create table #TEMPMyVendor
	(
		Vendor_ID int 
		,CompanyName varchar(50)
		
	)
	
	insert into #TEMPMyVendor(Vendor_ID,CompanyName) values (0,'--Select One--')


if @DBString is null
begin
	select @IRMAServer = IRMAServer , @IRMADatabase = IRMADataBase
	 from Regions where RegionID = @RegionID
	 
	
	set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'
end


	
	
	set @sql = 'select Vendor_ID,
      CompanyName from ' + @DBString + '[Vendor]'
	
	
insert into #TEMPMyVendor exec sp_executesql @sql
	
select * from #TEMPMyVendor
	


END

GO

/****** Object:  StoredProcedure [dbo].[PushPOsToIRMA]    Script Date: 06/02/2011 10:15:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PushPOsToIRMA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PushPOsToIRMA]
GO

/****** Object:  StoredProcedure [dbo].[PushPOsToIRMA]    Script Date: 06/02/2011 10:15:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PushPOsToIRMA]
	@RegionID int

AS
BEGIN
	declare @ErrorStatus bit
	declare @IRMAServer varchar(6)
	declare @IRMADatabase varchar(50)
	declare @DBString varchar(max)
	
	


	select @IRMAServer = IRMAServer, @IRMADatabase = IRMADataBase from Regions where RegionID = @RegionID
	set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'


	IF object_id('tempdb..#POQueue') IS NOT NULL
	DROP TABLE #POQueue

	create table #POQueue
	(
		POQueueID int identity (1,1) not null 
		, POHeaderID int
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
						, CurrencyID)
					select 
						@PONumber + '' from POET '' + isnull(H.Notes,'''') as OrderHeaderDesc
						, H.IRMAVendor_ID as Vendor_ID
						, IVS.Vendor_ID as PurchaseLocation_ID
						, IVS.Vendor_ID as ReceiveLocation_ID
						, ISNULL(IU.User_ID, @multipotool_ID) as CreatedBy
						, H.CreatedDate as OrderDate
						, 1 as Sent
						, 0 as Fax_Order
						, H.ExpectedDate as Expected_Date
						,cast(''' + cast(@Sentdate as varchar) + ''' as smalldatetime) as SentDate 
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
					from 
						POHeader H
						join PONumber N on H.PONumberID = N.PONumberID
						join Users U on U.UserID = N.AssignedByUserID
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
					, HandlingCharge)
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
	
	--select @sentdate as mydaate
	--select @dbstring as mystring

END
-- H.CreatedDate as SentDate
GO


/****** Object:  StoredProcedure [dbo].[ValidatePODataInIRMA]    Script Date: 06/02/2011 10:15:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValidatePODataInIRMA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ValidatePODataInIRMA]
GO

/****** Object:  StoredProcedure [dbo].[ValidatePODataInIRMA]    Script Date: 06/02/2011 10:15:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[ValidatePODataInIRMA]
	@RegionID int

AS
BEGIN

BEGIN TRY


	IF object_id('tempdb..#ValidationItems') IS NOT NULL
	DROP TABLE #ValidationItems

	IF object_id('tempdb..#ValidationResult') IS NOT NULL
	DROP TABLE #ValidationResult


	create table #ValidationItems
	(
		ValidationItemsID int identity (1,1) not null 
		, Identifier varchar(13)
		, VIN varchar(20)
		, BusinessUnit int
		, VendorPSNumber varchar(10)
		, Subteam int
		, PONumberID int
		, POItemID int
		, POHeaderID int
		, UploadSessionHistoryID int
		, ExpectedDate datetime
		, Item_Key int
		, UnitCostUOM int
		, UnitCost money
		, StoreAbbr varchar(5)
		, VendorName varchar(50)
		, IRMAVendor_ID int
	)

	create index IX_Validation_ItemId on #ValidationItems ( ValidationItemsID )
	create index IX_Validation_Identifier on #ValidationItems ( Identifier )


	create table #ValidationResult 
	(
		ValidationItemsID int,
		ExceptionID int
	)

	/*
		1	Identifier doesnt exist
		2	VIN doesnt exist for this vendor
		3	Vendor doesnt exist
		4	Store doesnt exist
		5	Item is not authorized for this Store and Vendor
		6	Subteam doesnt match
		7	PO can't be validated because it's already been successful, or is still waiting to be validated
		8	Cost data is not in IRMA for the expected date for this item
	*/


	declare @IRMAServer varchar(6)
	declare @IRMADatabase varchar(50)
	declare @DBString varchar(max)

	select @IRMAServer = IRMAServer, @IRMADatabase = IRMADataBase from Regions where RegionID = @RegionID
	set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'

	declare @UploadSessionHistoryID int
	declare @POHeaderID int
	declare @ValidationItemsID int
	declare @PONumberID int
	declare @Identifier varchar(13)
	declare @VIN varchar(20)
	declare @BusinessUnit int
	declare @VendorPSNumber varchar(10)
	declare @Subteam int
    declare @ExceptionID int
	declare @ExpectedDate datetime
	declare @UnitCostUOM int
	declare @UnitCost smallmoney

	declare @IRMAItem_Key int
	declare @IRMAStore_No int
	declare @IRMAStoreAbbr varchar(5)
	declare @IRMAVendor_ID int
	declare @IRMAStoreItemVendorID int
	declare @IRMACompanyName varchar(50)

	declare @Processing table (ValidationQueueID int)

	--we'll use this table variable to hold the sessions we're validating
	insert into @Processing
	select top 1 Q.ValidationQueueID --grabbing top 1 to avoid clogging up the validation queue
	from ValidationQueue Q 
	inner join POHeader H on Q.UploadSessionHistoryID = H.UploadSessionHistoryID
	inner join PONumber N on H.PONumberID = N.PONumberID
	where Q.ProcessingFlag = 0 and N.RegionID = @RegionId

	--set the ProcessingFlag to 1 to indicate we're going to work on these sessions
	update ValidationQueue set ProcessingFlag = 1
	from ValidationQueue Q 
	inner join @Processing P on Q.ValidationQueueID = P.ValidationQueueID

	--now, we'll begin the validation

	insert into #ValidationItems (
		Identifier
		, VIN
		--, BusinessUnit
		, StoreAbbr
		, VendorPSNumber
		, Subteam
		, PONumberID
		, POItemID
		, POHeaderID
		, UploadSessionHistoryID
		, ExpectedDate )
	select
		I.Identifier
		, I.VendorItemNumber
		--, H.BusinessUnit
		, H.StoreAbbr
		, H.VendorPSNumber
		, H.Subteam
		, H.PONumberID
		, I.POItemID
		, H.POHeaderID
		, H.UploadSessionHistoryID
		, H.ExpectedDate
	from 
	@Processing P 
	inner join ValidationQueue Q on Q.ValidationQueueID = P.ValidationQueueID
	inner join POHeader H on Q.UploadSessionHistoryID = H.UploadSessionHistoryID
	inner join POItem I on H.POHeaderID = I.POHeaderID
	inner join PONumber N on H.PONumberID = N.PONumberID
	where N.RegionID = @RegionId

--	select * from #validationitems

	declare @record_counter int
	declare @loop_counter int
	set @loop_counter = isnull((select count(*) from #ValidationItems),0)
	set @record_counter = 1

    while @loop_counter > 0 AND @record_counter <= @loop_counter

--!!!begin loop!!!
    begin
		set @ExceptionID = 0

        select 
		@UploadSessionHistoryID = UploadSessionHistoryID
		, @POHeaderID = POHeaderID
		, @ValidationItemsID = ValidationItemsID
		, @PONumberID = PONumberID
		, @Identifier = Identifier
		, @VIN = VIN
		--, @BusinessUnit = BusinessUnit
		, @IRMAStoreAbbr = StoreAbbr
		, @VendorPSNumber = VendorPSNumber
		, @Subteam = Subteam
		, @ExpectedDate = ExpectedDate
		from #ValidationItems
        where ValidationItemsID = @record_counter

--check to see if the PO Number can be re-uploaded
		
		declare @PreventUpload int
		select @PreventUpload = sum(cast(U.ValidationSuccessful as int))
		from #ValidationItems V
		inner join POHeader H on V.PONumberID = H.PONumberID
		inner join UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
		where V.PONumberID = @PONumberID		

		if @PreventUpload > 0
		begin 
			set @ExceptionID = 7
			insert into #ValidationResult (ValidationItemsID , ExceptionID)
			values (@ValidationItemsID , @ExceptionID)
			
set @record_counter = @record_counter + 1
		end

		declare @sql nvarchar(2000)

--try to grab the Item_Key by Identifier.	
		set @IRMAItem_Key = null

		set @sql = 'select @IRMAItem_KeyOUT = Item_Key from ' + @DBString + '[ItemIdentifier] where Identifier = @Identifier'
		exec sp_executesql @sql
			, N'@IRMAItem_KeyOUT int OUTPUT, @Identifier varchar(13)'
			, @IRMAItem_KeyOUT=@IRMAItem_Key OUTPUT, @Identifier=@Identifier


		if @IRMAItem_Key is null
		begin
			set @ExceptionID = 1
			insert into #ValidationResult (ValidationItemsID , ExceptionID)
			values (@ValidationItemsID , @ExceptionID)

set @record_counter = @record_counter + 1

		-- if it doesnt match, than we will try matching the VIN
			set @sql = 'select @IRMAItem_KeyOUT = Item_Key from ' + @DBString + '[ItemVendor] where Item_ID = @VIN and Vendor_ID = @IRMAVendor_ID'
			exec sp_executesql @sql
				, N'@IRMAItem_KeyOUT int OUTPUT, @VIN varchar(20), @IRMAVendor_ID int'
				, @IRMAItem_KeyOUT=@IRMAItem_Key OUTPUT, @VIN=@VIN, @IRMAVendor_ID=@IRMAVendor_ID
			
			
			if @IRMAItem_Key is null
			begin
				set @ExceptionID = 2
				insert into #ValidationResult (ValidationItemsID , ExceptionID)
				values (@ValidationItemsID , @ExceptionID)
				
--set @record_counter = @record_counter + 1
				
			end
		end

		update #ValidationItems set Item_Key = @IRMAItem_Key where ValidationItemsID = @ValidationItemsID

--try to set Vendor_ID		
		set @IRMAVendor_ID = null

		set @sql = N'select @IRMAVendor_IDOUT = Vendor_ID from ' + @DBString + '[Vendor] where PS_Export_Vendor_ID = @VendorPSNumber'
		exec sp_executesql @sql																				--query
					   , N'@IRMAVendor_IDOUT int OUTPUT, @VendorPSNumber varchar(10)'							--variable definitions
					   , @IRMAVendor_IDOUT=@IRMAVendor_ID OUTPUT, @VendorPSNumber = @VendorPSNumber     --variable values

			
		if @IRMAVendor_ID is null
		begin 
			set @ExceptionID = 3
			insert into #ValidationResult (ValidationItemsID , ExceptionID)
			values (@ValidationItemsID , @ExceptionID)
			
set @record_counter = @record_counter + 1
			
		end

--try to set BusinessUnit
		set @IRMAStore_No = null 
		--set @IRMAStoreAbbr = null
		set @BusinessUnit = null

--		set @sql = 'select @IRMAStore_NoOUT = Store_No, @IRMAStoreAbbrOUT = StoreAbbr from ' + @DBString + '[Store] where BusinessUnit_ID = @BusinessUnit'
--		exec sp_executesql @sql
--			, N'@IRMAStore_NoOUT int OUTPUT, @IRMAStoreAbbrOUT varchar(5) OUTPUT, @BusinessUnit int'
--			, @IRMAStore_NoOUT=@IRMAStore_No OUTPUT, @IRMAStoreAbbrOUT =@IRMAStoreAbbr OUTPUT, @BusinessUnit=@BusinessUnit

		set @sql = 'select @IRMAStore_NoOUT = Store_No, @BusinessUnitOUT = BusinessUnit_ID from ' + @DBString + '[Store] where StoreAbbr = @IRMAStoreAbbr'
		exec sp_executesql @sql
			, N'@IRMAStore_NoOUT int OUTPUT, @BusinessUnitOUT int OUTPUT, @IRMAStoreAbbr varchar(5)'
			, @IRMAStore_NoOUT=@IRMAStore_No OUTPUT, @BusinessUnitOUT =@BusinessUnit OUTPUT, @IRMAStoreAbbr=@IRMAStoreAbbr

		update #ValidationItems set BusinessUnit = @BusinessUnit where ValidationItemsID = @ValidationItemsID


			
		if @IRMAStore_No is null
		begin 
			set @ExceptionID = 4
			insert into #ValidationResult (ValidationItemsID , ExceptionID)
			values (@ValidationItemsID , @ExceptionID)
			
set @record_counter = @record_counter + 1
			
		end

--try to set StoreItemVendorID - also make sure the item is authorized per StoreItem.
--but only if we know the vendor, item and store in IRMA

		if @IRMAVendor_ID is not null and @IRMAItem_Key is not null and @IRMAStore_No is not null
		begin
			--the @IRMAVendor_ID we set earlier just checked to see if the vendor existed, now we're making sure it's the one associated with the stores
			set @IRMAVendor_ID = null
			set @IRMACompanyName = null
			set @IRMAStoreItemVendorID = null

			set @sql = 'select @IRMAVendor_IDOUT = SIV.Vendor_ID '
			set @sql = @sql + ', @IRMACompanyNameOUT=CompanyName '
			set @sql = @sql + ', @IRMAStoreItemVendorIDOUT = StoreItemVendorID '
			set @sql = @sql + 'from ' + @DBString + '[StoreItemVendor] SIV '
			set @sql = @sql + 'inner join ' + @DBString + '[StoreItem] SI on SIV.Item_Key = SI.Item_Key and SIV.Store_No = SI.Store_No '
			set @sql = @sql + 'inner join ' + @DBString + '[Vendor] V on SIV.Vendor_ID = V.Vendor_ID '
			set @sql = @sql + 'where @IRMAItem_Key = SI.Item_Key and @IRMAStore_No = SIV.Store_No '
			set @sql = @sql + 'and V.PS_Export_Vendor_ID = @VendorPSNumber '
			set @sql = @sql + 'and SI.Authorized = 1 and SIV.DeleteDate is null '
			exec sp_executesql @sql
				, N'@IRMAVendor_IDOUT int OUTPUT, @IRMACompanyNameOUT varchar(50) OUTPUT, @IRMAStoreItemVendorIDOUT int OUTPUT, @IRMAItem_Key int, @IRMAVendor_ID int, @IRMAStore_No int, @VendorPSNumber varchar(10)'
				, @IRMAVendor_IDOUT=@IRMAVendor_ID OUTPUT
				, @IRMACompanyNameOUT=@IRMACompanyName OUTPUT
				, @IRMAStoreItemVendorIDOUT=@IRMAStoreItemVendorID OUTPUT
				, @IRMAItem_Key=@IRMAItem_Key
				, @IRMAVendor_ID=@IRMAVendor_ID
				, @IRMAStore_No=@IRMAStore_No
				, @VendorPSNumber = @VendorPSNumber
			
		update #ValidationItems set VendorName = @IRMACompanyName, IRMAVendor_ID = @IRMAVendor_ID where ValidationItemsID = @ValidationItemsID


			
			if @IRMAStoreItemVendorID is null
			begin
				set @ExceptionID = 5
				insert into #ValidationResult (ValidationItemsID , ExceptionID)
				values (@ValidationItemsID , @ExceptionID)
				
set @record_counter = @record_counter + 1
				
			end
		end

--see if the subteam matches
		declare @SubTeam_NOmatch int
		declare @MySubTeamType as int
		set @sql = 'select @SubTeam_NoOUT  = Subteam_No from ' + @DBString + '[Item] where Item_Key = @IRMAItem_Key'
		exec sp_executesql @sql
			,N'@SubTeam_NoOUT int OUTPUT, @IRMAItem_Key int'
			,@SubTeam_NoOUT=@SubTeam_NOmatch OUTPUT, @IRMAItem_Key=@IRMAItem_Key

			--NEW code to check if item belongs to Retail and Retail/Manufacturing sub team typse.
							set @sql = 'select @MySubTeamType_OUT = SubTeamType_ID from ' + @DBString + '[subteam] where SubTeam_No = @SubTeam'
							exec sp_executesql @sql
							,N'@MySubTeamType_OUT int OUTPUT,@SubTeam int'
							,@MySubTeamType_OUT = @MySubTeamType OUTPUT, @SubTeam = @SubTeam
						
							--------------------
							if 	@MySubTeamType is  null
									begin
										set @ExceptionID = 6
											insert into #ValidationResult (ValidationItemsID , ExceptionID)
											values (@ValidationItemsID , @ExceptionID)
set @record_counter = @record_counter + 1
									end
							--------------		
							else
							begin
										if @MySubTeamType not in  (2,3)
										begin
											if @Subteam != @SubTeam_NOmatch
												begin
													set @ExceptionID = 6
													insert into #ValidationResult (ValidationItemsID , ExceptionID)
													values (@ValidationItemsID , @ExceptionID)
set @record_counter = @record_counter + 1
													
												end
														
										end
							end
						
							
							 
							
								

-- try to grab some cost data
-----NEW NEW this change is to aviod cost validation when identifier, store etc doesnt exist
if @IRMAItem_Key is not null 
	begin
		declare @Cost table (Item_Key int, CostUnit_ID int, NetCost smallmoney)
		delete from @Cost

		set @sql = 'exec ' + @DBString + '[GetNetCostByDate] @IRMAItem_Key, @IRMAStore_No, @IRMAVendor_ID, @ExpectedDate'
		insert into @Cost 
		exec sp_executeSQL @sql
		,N'@IRMAItem_Key int, @IRMAStore_No int, @IRMAVendor_ID int, @ExpectedDate datetime'
		, @IRMAItem_Key=@IRMAItem_Key, @IRMAStore_No=@IRMAStore_No, @IRMAVendor_ID=@IRMAVendor_ID, @ExpectedDate=@ExpectedDate


			


		if not exists (select top 1 * from @Cost)
		begin
			
					set @ExceptionID = 8
					insert into #ValidationResult (ValidationItemsID , ExceptionID)
					values (@ValidationItemsID , @ExceptionID)
			
			set @record_counter = @record_counter + 1
		end


		

		update #ValidationItems set UnitCostUOM = CostUnit_ID, UnitCost = NetCost 
		from (select top 1 CostUnit_ID, NetCost from @Cost) a 
		where ValidationItemsID = @ValidationItemsID
end
--NEW NEW
--if it passed all validations...
		if @ExceptionID = 0
		begin
			insert into #ValidationResult (ValidationItemsID , ExceptionID)
			values (@ValidationItemsID , @ExceptionID)
		end

		set @record_counter = @record_counter + 1
	end

--!!end of loop!!--
	
--update the Tool tables
--first, expire previous tries...
	update POHeader
	set Expired = 1
	from POHeader H
	inner join #ValidationItems V on H.PONumberID = V.PONumberID
	inner join UploadSessionHistory U on H.UploadSessionHistoryID = U.UploadSessionHistoryID
	where H.UploadSessionHistoryID != V.UploadSessionHistoryID 
	and U.ValidationSuccessful != 1

--...now, go ahead and enter some data

	update POItem
	set Item_Key = V.Item_Key
	, UnitCost = CASE WHEN I.DiscountType = 'FF' THEN
					V.UnitCost
				WHEN I.DiscountType = '%' THEN
					V.UnitCost * ((100 - I.DiscountAmount)/100)
				WHEN I.DiscountType = '$' THEN
					V.UnitCost - I.DiscountAmount
				WHEN H.DiscountType = '%' THEN
					V.UnitCost * ((100 - H.DiscountAmount)/100)
				ELSE V.UnitCost	END	
	, UnitCostUOM = V.UnitCostUOM
	, OrigUnitCost = V.UnitCost
	, FreeQuantity = CASE WHEN ISNULL(FreeQuantity,0) > OrderQuantity
					THEN OrderQuantity
					ELSE ISNULL(FreeQuantity,0) END
	from POItem I
	inner join #ValidationItems V on I.POItemID = V.POItemID
	inner join POHeader H on H.POHeaderID = I.POHeaderID

	update POHeader
	set ValidationAttemptDate = getdate()
	, OrderItemCount = isnull(I.OrderItemCount, 0)
	, ExceptionItemCount = isnull(E.Exceptions, 0)
	, StoreAbbr = V.StoreAbbr
	, BusinessUnit = V.BusinessUnit -- ADDED
	, VendorName = V.VendorName
	, IRMAVendor_ID = V.IRMAVendor_ID
	, TotalPOCost = C.TotalPOCost 
	from
	POHeader H 
	inner join #ValidationItems V on H.POHeaderID = V.POHeaderID
	inner join 
		(select V.POHeaderID, count(V.POItemID) as OrderItemCount
		from #ValidationItems V
		inner join POItem I on V.POItemID = I.POItemID
		where I.OrderQuantity > 0
		group by V.POHeaderID) I on H.POHeaderID = I.POHeaderID 
	left outer join
		(select
		I.POHeaderID, count(R.ExceptionID) as Exceptions 
		from #ValidationResult R 
		inner join #ValidationItems I on R.ValidationItemsID = I.ValidationItemsID
		where R.ExceptionID != 0
		group by I.POHeaderID) E on H.POHeaderID = E.POHeaderID
	inner join
		(select I.POHeaderID, sum((I.OrderQuantity - isnull(I.FreeQuantity,0)) * I.UnitCost) as TotalPOCost
		from #ValidationItems V
		inner join POItem I on V.POItemID = I.POItemID
		group by I.POHeaderID) C on H.POHeaderID = C.POHeaderID


--insert the new exceptions
	insert into POItemException (POItemID, ExceptionID)
	select 
	V.POItemID
	, R.ExceptionID
	from #ValidationResult R 
	inner join #ValidationItems V on R.ValidationItemsID = V.ValidationItemsID
	where R.ExceptionID != 0

--first, mark all sessions as unsuccessful (just to replace the null that shows the attempt hasn't been made)
	update UploadSessionHistory
	set ValidationSuccessful = 0
	from UploadSessionHistory U 
	inner join #ValidationItems V on U.UploadSessionHistoryID = V.UploadSessionHistoryID

--if all POs within an Upload Session were successful, than we mark Validation successful
--, and those POs can be pushed...
	update UploadSessionHistory
	set ValidationSuccessful = 1
	where UploadSessionHistoryID in
		(select H.UploadSessionHistoryID
		from POHeader H
		inner join #ValidationItems V on H.UploadSessionHistoryID = V.UploadSessionHistoryID
		group by H.UploadSessionHistoryID
		having sum(H.ExceptionItemCount) = 0)

--clear processed records from queue
	delete from ValidationQueue 
	where ValidationQueueID in 
		(select ValidationQueueID from @Processing)
	and ProcessingFlag = 1

END TRY
BEGIN CATCH

    declare @ErrorMessage nvarchar(4000);
    select 
        @ErrorMessage = 
	'SQL error while validating RegionID ' + isnull(cast(@RegionID as char(2)),'unknown') 
	+ ', UploadSessionHistoryID ' + isnull(cast(@UploadSessionHistoryID as char(9)),'unknown')
	+ ', POHeaderID ' + isnull(cast(@POHeaderID as char(9)),'unknown') 
	+ ' - '  + ERROR_MESSAGE()

	insert into ErrorLog (Timestamp, ErrorMessage) values(getdate(), @ErrorMessage)
	
	RAISERROR('SQL error entered in ErrorLog table',11,1)

--clear processed records from queue
	delete from ValidationQueue 
	where ValidationQueueID in 
		(select ValidationQueueID from @Processing)
	and ProcessingFlag = 1

END CATCH;

		
END
GO



