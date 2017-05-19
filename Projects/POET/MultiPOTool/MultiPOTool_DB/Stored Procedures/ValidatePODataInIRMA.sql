if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ValidatePODataInIRMA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ValidatePODataInIRMA]
GO

CREATE PROCEDURE [dbo].[ValidatePODataInIRMA]
	@RegionID int
AS
BEGIN

BEGIN TRY

	IF object_id('tempdb..#ValidationItems') IS NOT NULL
	DROP TABLE #ValidationItems

	IF object_id('tempdb..#ValidationResult') IS NOT NULL
	DROP TABLE #ValidationResult
	
	IF object_id('tempdb..#MyReasonCodes') IS NOT NULL
	DROP TABLE #MyReasonCodes


	CREATE TABLE #ValidationItems
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
		, HeaderReasonCode nchar(3)
		, ItemReasonCode nchar(3)
		, UploadSessionHistoryID int
		, ExpectedDate datetime
		, Item_Key int
		, UnitCostUOM int
		, UnitCost money
		, StoreAbbr varchar(5)
		, VendorName varchar(50)
		, IRMAVendor_ID int
	)

	CREATE INDEX IX_Validation_ItemId ON #ValidationItems ( ValidationItemsID )
	CREATE INDEX IX_Validation_Identifier ON #ValidationItems ( Identifier )


	CREATE TABLE #ValidationResult 
	(
		ValidationItemsID int,
		ExceptionID int
	)
	
	 CREATE TABLE #MyReasonCodes 
	(
		ReasonCodeDetailID int,
		ReasonCode nchar(3),
		ReasonCodeDesc nvarchar(50),
		ReasonCodeExtDesc varchar(max)
	)

	/*
		1	Identifier doesnt exist
		2	VIN doesnt exist for this vendor
		3	Vendor doesnt exist
		4	Store doesnt exist
		5	Item is not authorized for this Store AND Vendor
		6	Subteam doesnt match
		7	PO can't be validated because it's already been successful, or is still waiting to be validated
		8	Cost data is not in IRMA for the expected date for this item
		9   Invalid Reason Code
	*/


	DECLARE @IRMAServer varchar(6)
	DECLARE @IRMADatabase varchar(50)
	DECLARE @DBString varchar(max)

	SELECT @IRMAServer = IRMAServer, @IRMADatabase = IRMADataBase FROM Regions WHERE RegionID = @RegionID
	SET @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'

	DECLARE @UploadSessionHistoryID int
	DECLARE @POHeaderID int
	DECLARE @ValidationItemsID int
	DECLARE @PONumberID int
	DECLARE @Identifier varchar(13)
	DECLARE @VIN varchar(20)
	DECLARE @BusinessUnit int
	DECLARE @VendorPSNumber varchar(10)
	DECLARE @Subteam int
    DECLARE @ExceptionID int
	DECLARE @ExpectedDate datetime
	DECLARE @UnitCostUOM int
	DECLARE @UnitCost smallmoney

	DECLARE @IRMAItem_Key int
	DECLARE @IRMAStore_No int
	DECLARE @IRMAStoreAbbr varchar(5)
	DECLARE @IRMAVendor_ID int
	DECLARE @IRMAStoreItemVendorID int
	DECLARE @IRMACompanyName varchar(50)
	
	DECLARE @DSDVendorStoreID int

	DECLARE @Processing TABLE (ValidationQueueID int)

	--we'll use this TABLE variable to hold the sessions we're validating
	INSERT INTO @Processing
	SELECT TOP 1 Q.ValidationQueueID --grabbing TOP 1 to avoid clogging up the validation queue
	FROM ValidationQueue Q 
	INNER JOIN POHeader H ON Q.UploadSessionHistoryID = H.UploadSessionHistoryID
	INNER JOIN PONumber N ON H.PONumberID = N.PONumberID
	WHERE Q.ProcessingFlag = 0 AND N.RegionID = @RegionId

	--SET the ProcessingFlag to 1 to indicate we're going to work ON these sessions
	UPDATE ValidationQueue SET ProcessingFlag = 1
	FROM ValidationQueue Q 
	INNER JOIN @Processing P ON Q.ValidationQueueID = P.ValidationQueueID

	--now, we'll BEGIN the validation

	INSERT INTO #ValidationItems (
		Identifier
		, VIN
		, StoreAbbr
		, VendorPSNumber
		, Subteam
		, PONumberID
		, POItemID
		, POHeaderID
		, HeaderReasonCode
		, ItemReasonCode
		, UploadSessionHistoryID
		, ExpectedDate )
	SELECT
		I.Identifier
		, I.VendorItemNumber
		, H.StoreAbbr
		, H.VendorPSNumber
		, H.Subteam
		, H.PONumberID
		, I.POItemID
		, H.POHeaderID
		, H.ReasonCode
		, I.ReasonCode
		, H.UploadSessionHistoryID
		, H.ExpectedDate
	FROM 
	@Processing P 
	INNER JOIN ValidationQueue Q ON Q.ValidationQueueID = P.ValidationQueueID
	INNER JOIN POHeader H ON Q.UploadSessionHistoryID = H.UploadSessionHistoryID
	INNER JOIN POItem I ON H.POHeaderID = I.POHeaderID
	INNER JOIN PONumber N ON H.PONumberID = N.PONumberID
	WHERE N.RegionID = @RegionId

	DECLARE @record_counter int
	DECLARE @HeaderReasonCode int
	DECLARE @ItemReasonCode int
	DECLARE @loop_counter int
	SET @loop_counter = isnull((SELECT count(*) FROM #ValidationItems),0)
	SET @record_counter = 1

	--!!!BEGIN loop!!!
    while @loop_counter > 0 AND @record_counter <= @loop_counter
    BEGIN
		SET @ExceptionID = 0

        SELECT 
		@UploadSessionHistoryID = UploadSessionHistoryID
		, @POHeaderID = POHeaderID
		, @ValidationItemsID = ValidationItemsID
		, @PONumberID = PONumberID
		, @Identifier = Identifier
		, @VIN = VIN
		, @HeaderReasonCode = HeaderReasonCode
		, @ItemReasonCode = ItemReasonCode
		, @IRMAStoreAbbr = StoreAbbr
		, @VendorPSNumber = VendorPSNumber
		, @Subteam = Subteam
		, @ExpectedDate = ExpectedDate
		FROM #ValidationItems
        WHERE ValidationItemsID = @record_counter

		--check to see IF the PO Number can be re-uploaded
		DECLARE @PreventUpload int
		
		SELECT @PreventUpload = sum(cast(U.ValidationSuccessful AS int))
		FROM #ValidationItems V
		INNER JOIN POHeader H ON V.PONumberID = H.PONumberID
		INNER JOIN UploadSessionHistory U ON H.UploadSessionHistoryID = U.UploadSessionHistoryID
		WHERE V.PONumberID = @PONumberID		

		IF @PreventUpload > 0
		BEGIN 
			SET @ExceptionID = 7
			INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
			VALUES (@ValidationItemsID , @ExceptionID)
		END

		DECLARE @sql nvarchar(2000)

		--try to grab the Item_Key BY Identifier.	
		SET @IRMAItem_Key = null

		SET @sql = 'SELECT @IRMAItem_KeyOUT = Item_Key FROM ' + @DBString + '[ItemIdentifier] WHERE Identifier = @Identifier AND Deleted_Identifier = 0'
		EXEC sp_executesql @sql
			, N'@IRMAItem_KeyOUT int OUTPUT, @Identifier varchar(13)'
			, @IRMAItem_KeyOUT=@IRMAItem_Key OUTPUT, @Identifier=@Identifier

		IF @IRMAItem_Key is null
		BEGIN
			SET @ExceptionID = 1
			INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
			VALUES (@ValidationItemsID , @ExceptionID)

			-- IF it doesnt match, than we will try matching the VIN
			SET @sql = 'SELECT @IRMAItem_KeyOUT = Item_Key FROM ' + @DBString + '[ItemVendor] WHERE Item_ID = @VIN AND Vendor_ID = @IRMAVendor_ID'
			EXEC sp_executesql @sql
				, N'@IRMAItem_KeyOUT int OUTPUT, @VIN varchar(20), @IRMAVendor_ID int'
				, @IRMAItem_KeyOUT=@IRMAItem_Key OUTPUT, @VIN=@VIN, @IRMAVendor_ID=@IRMAVendor_ID
						
			IF @IRMAItem_Key is null
			BEGIN
				SET @ExceptionID = 2
				INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
				VALUES (@ValidationItemsID , @ExceptionID)				
			END
		END

		UPDATE #ValidationItems SET Item_Key = @IRMAItem_Key WHERE ValidationItemsID = @ValidationItemsID

		--try to SET Vendor_ID		
		SET @IRMAVendor_ID = null

		SET @sql = N'SELECT @IRMAVendor_IDOUT = Vendor_ID FROM ' + @DBString + '[Vendor] WHERE PS_Export_Vendor_ID = @VendorPSNumber'
		EXEC sp_executesql @sql																				--query
					   , N'@IRMAVendor_IDOUT int OUTPUT, @VendorPSNumber varchar(10)'							--variable definitions
					   , @IRMAVendor_IDOUT=@IRMAVendor_ID OUTPUT, @VendorPSNumber = @VendorPSNumber     --variable VALUES
	
		IF @IRMAVendor_ID is null
		BEGIN 
			SET @ExceptionID = 3
			INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
			VALUES (@ValidationItemsID , @ExceptionID)			
		END

		--try to SET BusinessUnit
		SET @IRMAStore_No = null 
		SET @BusinessUnit = null

		SET @sql = 'SELECT @IRMAStore_NoOUT = Store_No, @BusinessUnitOUT = BusinessUnit_ID FROM ' + @DBString + '[Store] WHERE StoreAbbr = @IRMAStoreAbbr'
		EXEC sp_executesql @sql
			, N'@IRMAStore_NoOUT int OUTPUT, @BusinessUnitOUT int OUTPUT, @IRMAStoreAbbr varchar(5)'
			, @IRMAStore_NoOUT=@IRMAStore_No OUTPUT, @BusinessUnitOUT =@BusinessUnit OUTPUT, @IRMAStoreAbbr=@IRMAStoreAbbr

		UPDATE #ValidationItems SET BusinessUnit = @BusinessUnit WHERE ValidationItemsID = @ValidationItemsID
			
		IF @IRMAStore_No is null
		BEGIN 
			SET @ExceptionID = 4
			INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
			VALUES (@ValidationItemsID , @ExceptionID)			
		END
				
		--check for DSD Vendor
		IF @IRMAVendor_ID is not null AND @IRMAStore_No is not null
		BEGIN
			SET @SQL = 'SELECT @DSVendorDStore_IDOUT = dvs.DSDVendorStoreID FROM ' + @DBString + '[DSDVendorStore] dvs WITH (nolock) '
			SET @SQL = @SQL + ' WHERE dvs.Vendor_ID = @IRMAVendor_ID AND dvs.Store_No = @IRMAStore_No'
			
			EXEC sp_executesql @SQL
					, N'@DSVendorDStore_IDOUT int OUTPUT, @IRMAVendor_ID int, @IRMAStore_No int'
					, @DSVendorDStore_IDOUT = @DSDVendorStoreID OUTPUT, @IRMAVendor_ID = @IRMAVendor_ID, @IRMAStore_No = @IRMAStore_No
					
			IF @DSDVendorStoreID is not null
			BEGIN
				SET @ExceptionID = 10
				INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
				VALUES (@ValidationItemsID , @ExceptionID)				
			END
		END
		
		--new code for task # 2910
		INSERT INTO #MyReasonCodes(ReasonCodeDetailID,
				ReasonCode,
				ReasonCodeDesc,
				ReasonCodeExtDesc) EXEC (@DBString + 'reasoncodes_getdetailsfortype ''CA''')

		DECLARE @RHCode AS nchar(3)
		DECLARE @IRCode AS nchar(3)
		
		IF (@HeaderReasonCode is not null) 
		BEGIN
			SELECT @RHCode = hr.ReasonCode  FROM #MyReasonCodes hr WHERE ReasonCodeDetailID = @HeaderReasonCode

			IF @RHCode is null
			BEGIN
				SET @ExceptionID = 9
				INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
				VALUES (@ValidationItemsID , @ExceptionID)
			END
		END
		
		IF (@ItemReasonCode is not null) 
		BEGIN
			SELECT @IRCode = ir.ReasonCode FROM #MyReasonCodes ir WHERE ReasonCodeDetailID = @ItemReasonCode
			
			IF @IRCode is null
			BEGIN
				SET @ExceptionID = 9
				INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
				VALUES (@ValidationItemsID , @ExceptionID)
			END
		END
		--END of-new code for task # 2910

		--try to SET StoreItemVendorID - also make sure the item is authorized per StoreItem.
		--but only IF we know the vendor, item AND store in IRMA

		IF @IRMAVendor_ID is not null AND @IRMAItem_Key is not null AND @IRMAStore_No is not null
		BEGIN
			--the @IRMAVendor_ID we SET earlier just checked to see IF the vendor existed, now we're making sure it's the one associated with the stores
			SET @IRMAVendor_ID = null
			SET @IRMACompanyName = null
			SET @IRMAStoreItemVendorID = null

			SET @sql = 'SELECT @IRMAVendor_IDOUT = SIV.Vendor_ID '
			SET @sql = @sql + ', @IRMACompanyNameOUT=CompanyName '
			SET @sql = @sql + ', @IRMAStoreItemVendorIDOUT = StoreItemVendorID '
			SET @sql = @sql + 'FROM ' + @DBString + '[StoreItemVendor] SIV '
			SET @sql = @sql + 'INNER JOIN ' + @DBString + '[StoreItem] SI ON SIV.Item_Key = SI.Item_Key AND SIV.Store_No = SI.Store_No '
			SET @sql = @sql + 'INNER JOIN ' + @DBString + '[Vendor] V ON SIV.Vendor_ID = V.Vendor_ID '
			SET @sql = @sql + 'WHERE @IRMAItem_Key = SI.Item_Key AND @IRMAStore_No = SIV.Store_No '
			SET @sql = @sql + 'AND V.PS_Export_Vendor_ID = @VendorPSNumber '
			SET @sql = @sql + 'AND SI.Authorized = 1 AND SIV.DeleteDate is null '
			EXEC sp_executesql @sql
				, N'@IRMAVendor_IDOUT int OUTPUT, @IRMACompanyNameOUT varchar(50) OUTPUT, @IRMAStoreItemVendorIDOUT int OUTPUT, @IRMAItem_Key int, @IRMAVendor_ID int, @IRMAStore_No int, @VendorPSNumber varchar(10)'
				, @IRMAVendor_IDOUT=@IRMAVendor_ID OUTPUT
				, @IRMACompanyNameOUT=@IRMACompanyName OUTPUT
				, @IRMAStoreItemVendorIDOUT=@IRMAStoreItemVendorID OUTPUT
				, @IRMAItem_Key=@IRMAItem_Key
				, @IRMAVendor_ID=@IRMAVendor_ID
				, @IRMAStore_No=@IRMAStore_No
				, @VendorPSNumber = @VendorPSNumber
			
		UPDATE #ValidationItems SET VendorName = @IRMACompanyName, IRMAVendor_ID = @IRMAVendor_ID WHERE ValidationItemsID = @ValidationItemsID
					
			IF @IRMAStoreItemVendorID is null
			BEGIN
				SET @ExceptionID = 5
				INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
				VALUES (@ValidationItemsID , @ExceptionID)							
			END
		END

		--see IF the subteam matches
		DECLARE @SubTeam_NOmatch int
		DECLARE @MySubTeamType AS int
		SET @sql = 'SELECT @SubTeam_NoOUT  = Subteam_No FROM ' + @DBString + '[Item] WHERE Item_Key = @IRMAItem_Key'
		EXEC sp_executesql @sql
			,N'@SubTeam_NoOUT int OUTPUT, @IRMAItem_Key int'
			,@SubTeam_NoOUT=@SubTeam_NOmatch OUTPUT, @IRMAItem_Key=@IRMAItem_Key

			--NEW code to check IF item belongs to Retail AND Retail/Manufacturing sub team typse.
							SET @sql = 'SELECT @MySubTeamType_OUT = SubTeamType_ID FROM ' + @DBString + '[subteam] WHERE SubTeam_No = @SubTeam'
							EXEC sp_executesql @sql
							,N'@MySubTeamType_OUT int OUTPUT,@SubTeam int'
							,@MySubTeamType_OUT = @MySubTeamType OUTPUT, @SubTeam = @SubTeam
						
							--------------------
							IF 	@MySubTeamType is  null
									BEGIN
										SET @ExceptionID = 6
											INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
											VALUES (@ValidationItemsID , @ExceptionID)
									END
							--------------		
							else
							BEGIN
										IF @MySubTeamType not in  (2,3)
										BEGIN
											IF @Subteam != @SubTeam_NOmatch
												BEGIN
													SET @ExceptionID = 6
													INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
													VALUES (@ValidationItemsID , @ExceptionID)
													
												END
														
										END
							END
						
							
		-- try to grab some cost data
		-----NEW NEW this change is to aviod cost validation when identifier, store etc doesnt exist
		IF @IRMAItem_Key is not null 
		BEGIN
			DECLARE @Cost TABLE (Item_Key int, CostUnit_ID int, NetCost smallmoney)
			DELETE FROM @Cost

			SET @sql = 'EXEC ' + @DBString + '[GetNetCostByDate] @IRMAItem_Key, @IRMAStore_No, @IRMAVendor_ID, @ExpectedDate'
			INSERT INTO @Cost 
				EXEC sp_executeSQL @sql
				,N'@IRMAItem_Key int, @IRMAStore_No int, @IRMAVendor_ID int, @ExpectedDate datetime'
				, @IRMAItem_Key=@IRMAItem_Key, @IRMAStore_No=@IRMAStore_No, @IRMAVendor_ID=@IRMAVendor_ID, @ExpectedDate=@ExpectedDate


			IF not exists (SELECT TOP 1 * FROM @Cost)
			BEGIN
				SET @ExceptionID = 8
				INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
					VALUES (@ValidationItemsID , @ExceptionID)			
			END

			UPDATE #ValidationItems 
			SET UnitCostUOM = CostUnit_ID, UnitCost = NetCost 
			FROM (SELECT TOP 1 CostUnit_ID, NetCost FROM @Cost) a 
			WHERE ValidationItemsID = @ValidationItemsID
		END

		--IF it passed all validations...
		IF @ExceptionID = 0
		BEGIN
			INSERT INTO #ValidationResult (ValidationItemsID , ExceptionID)
			VALUES (@ValidationItemsID , @ExceptionID)
		END

		SET @record_counter = @record_counter + 1
	END

	--!!END of loop!!--
		
	--UPDATE the Tool tables
	--first, expire previous tries...
	UPDATE POHeader
	SET Expired = 1
	FROM POHeader H
	INNER JOIN #ValidationItems V ON H.PONumberID = V.PONumberID
	INNER JOIN UploadSessionHistory U ON H.UploadSessionHistoryID = U.UploadSessionHistoryID
	WHERE H.UploadSessionHistoryID != V.UploadSessionHistoryID 
	AND U.ValidationSuccessful != 1

	--...now, go ahead AND enter some data
	UPDATE POItem
	SET Item_Key = V.Item_Key
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
	FROM POItem I
	INNER JOIN #ValidationItems V ON I.POItemID = V.POItemID
	INNER JOIN POHeader H ON H.POHeaderID = I.POHeaderID

	UPDATE POHeader
	SET ValidationAttemptDate = getdate()
	, OrderItemCount = isnull(I.OrderItemCount, 0)
	, ExceptionItemCount = isnull(E.Exceptions, 0)
	, StoreAbbr = V.StoreAbbr
	, BusinessUnit = V.BusinessUnit -- ADDED
	, VendorName = V.VendorName
	, IRMAVendor_ID = V.IRMAVendor_ID
	, TotalPOCost = C.TotalPOCost 
	FROM
	POHeader H 
	INNER JOIN #ValidationItems V ON H.POHeaderID = V.POHeaderID
	INNER JOIN 
		(SELECT V.POHeaderID, count(V.POItemID) AS OrderItemCount
		FROM #ValidationItems V
		INNER JOIN POItem I ON V.POItemID = I.POItemID
		WHERE I.OrderQuantity > 0
		GROUP BY V.POHeaderID) I ON H.POHeaderID = I.POHeaderID 
	LEFT OUTER JOIN
		(SELECT
		I.POHeaderID, count(R.ExceptionID) AS Exceptions 
		FROM #ValidationResult R 
		INNER JOIN #ValidationItems I ON R.ValidationItemsID = I.ValidationItemsID
		WHERE R.ExceptionID != 0
		GROUP BY I.POHeaderID) E ON H.POHeaderID = E.POHeaderID
	INNER JOIN
		(SELECT I.POHeaderID, sum((I.OrderQuantity - isnull(I.FreeQuantity,0)) * I.UnitCost) AS TotalPOCost
		FROM #ValidationItems V
		INNER JOIN POItem I ON V.POItemID = I.POItemID
		GROUP BY I.POHeaderID) C ON H.POHeaderID = C.POHeaderID


	--INSERT the new exceptions
	INSERT INTO POItemException (POItemID, ExceptionID)
	SELECT 
	V.POItemID
	, R.ExceptionID
	FROM #ValidationResult R 
	INNER JOIN #ValidationItems V ON R.ValidationItemsID = V.ValidationItemsID
	WHERE R.ExceptionID != 0

	--first, mark all sessions AS unsuccessful (just to replace the null that shows the attempt hasn't been made)
	UPDATE UploadSessionHistory
	SET ValidationSuccessful = 0
	FROM UploadSessionHistory U 
	INNER JOIN #ValidationItems V ON U.UploadSessionHistoryID = V.UploadSessionHistoryID

	--IF all POs within an Upload Session were successful, than we mark Validation successful
	--, AND those POs can be pushed...
	UPDATE UploadSessionHistory
	SET ValidationSuccessful = 1
	WHERE UploadSessionHistoryID in
		(SELECT H.UploadSessionHistoryID
		FROM POHeader H
		INNER JOIN #ValidationItems V ON H.UploadSessionHistoryID = V.UploadSessionHistoryID
		GROUP BY H.UploadSessionHistoryID
		HAVING sum(H.ExceptionItemCount) = 0)

	--clear processed records FROM queue
	DELETE FROM ValidationQueue 
	WHERE ValidationQueueID in 
		(SELECT ValidationQueueID FROM @Processing)
	AND ProcessingFlag = 1

END TRY
BEGIN CATCH

    DECLARE @ErrorMessage nvarchar(4000);
    SELECT 
        @ErrorMessage = 
	'SQL error while validating RegionID ' + isnull(cast(@RegionID AS char(2)),'unknown') 
	+ ', UploadSessionHistoryID ' + isnull(cast(@UploadSessionHistoryID AS char(9)),'unknown')
	+ ', POHeaderID ' + isnull(cast(@POHeaderID AS char(9)),'unknown') 
	+ ' - '  + ERROR_MESSAGE()

	INSERT INTO ErrorLog (Timestamp, ErrorMessage) VALUES(getdate(), @ErrorMessage)
	
	RAISERROR('SQL error entered in ErrorLog TABLE',11,1)

	--clear processed records FROM queue
	DELETE FROM ValidationQueue 
	WHERE ValidationQueueID in 
		(SELECT ValidationQueueID FROM @Processing)
	AND ProcessingFlag = 1

END CATCH;

		
END
GO