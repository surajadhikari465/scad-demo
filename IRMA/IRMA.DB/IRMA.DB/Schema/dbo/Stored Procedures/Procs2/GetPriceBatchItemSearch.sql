
CREATE PROCEDURE [dbo].[GetPriceBatchItemSearch]
    @StoreList			varchar(MAX),
    @StoreListSeparator char(1),
    @ItemChgTypeID		tinyint,
    @PriceChgTypeID		tinyint,
    @SubTeam_No			int,
    @StartDate			smalldatetime,
    @Identifier			varchar(255),
    @Item_Description	varchar(255),
    @IncScaleItems		bit,
	@IncNonRetailItems	bit,
	@MaxRowsToReturn	int = 0

AS

-- ****************************************************************************************************************
-- Procedure: GetPriceBatchItemSearch()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from PricingBatchItemSearch.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012/12/25	KM		8780	Add update history template; Search ItemOverride for Item_Description and Brand_ID values;
-- 2013/03/12	KM		11534	Change 'I' join from LEFT to INNER;
-- 2014/04/29	DN		15000	Added @ValidatedScanCode table to hold the validated scan codes. This table replaced
--								ItemIdentifiers in the query.
-- 2014/06/25	DN		15220	Updated logic to handle new value from dbo.fn_ReceiveUPCPLUUpdateFromIcon()
-- 2014/08/15	DN		15371	Included Removed / Deleted Non-validated identifiers
-- 2014/11/07	DN		15504	Remove I.Deleted_Item = 1 to block batching for non-validated items with a previously
--								deleted scan code.
-- 2015/03/26	DN		15957	Added condition in the WHERE clause to return only retail sale items (I.Retail_Sale = 1)
-- 2015/08/15	DN		16346	Allow non-retail items to be included in the #ValidatedScanCode table regardless of their
--								validation statuses.
-- 2015/10/16	DN		17446	Added an extra condition in the WHERE clause to prevent non-validated retail items that were
--								previously deleted non-retail items from batching
-- 2017/05/08	EM		21759	Prevent non-retail, non-validated items in the 46/48 ranges from batching
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON

	DECLARE 
		@MaxRows						int,
		@Required_VendorForBatching		bit,
		@UseStoreJurisdictions			bit,
		@On_Sale						bit,
		@Priority						int
	
	-----------------------------------
	-- Limit the number of rows returned
	-----------------------------------
	IF ISNULL(@MaxRowsToReturn, 0) > 0
		SELECT @MaxRows = @MaxRowsToReturn
	ELSE
		-- no limit specified; set to max int value (2,147,483,647)
		SELECT @MaxRows = 2147483647
    
	-----------------------------------
    -- DETERMINE IF REGIONAL FLAG REQUIRES THAT ALL BATCHABLE ITEMS HAVE A VENDOR ASSIGNED TO THEM
	-----------------------------------
	SELECT @Required_VendorForBatching = dbo.fn_InstanceDataValue('Required_VendorForBatching', NULL)

	SELECT @UseStoreJurisdictions = dbo.fn_InstanceDataValue('UseStoreJurisdictions', NULL)

	
	-----------------------------------
	-- Get the price priority
	-----------------------------------
	-- Get the On_Sale value separately because we only want re-authed records when this flag is 0
	-- for the PCT and the PCT for the detail record matches or is NULL (so we can't join to test the flag there)
	
	SELECT 
		@Priority = Priority,
		@On_Sale = On_Sale
	FROM 
		PriceChgType (NOLOCK)
	WHERE 
		PriceChgTypeId = @PriceChgTypeId
		
	-----------------------------------
	-- Create list of stores to be included
	-----------------------------------
	DECLARE @tblStoreList table 
							(
								Store_No int PRIMARY KEY CLUSTERED, 
								Store_Name varchar(50),
								StoreJurisdictionID int,
								BatchUnvalidatedIngredients bit
							)
	
	INSERT INTO @tblStoreList (
		Store_No,
		Store_Name,
		StoreJurisdictionID,
		BatchUnvalidatedIngredients)
	SELECT DISTINCT
		S.Store_No,
		RTRIM(S.Store_Name),
		S.StoreJurisdictionID,
		ISNULL(dbo.fn_InstanceDataValue('BatchNonValidatedIngredients', S.Store_No), 0)
	FROM 
		Store S (NOLOCK)
		INNER JOIN dbo.fn_Parse_List(@StoreList, @StoreListSeparator) LIST ON LIST.Key_Value = S.Store_No
	ORDER BY 
		S.Store_No

	IF OBJECT_ID('tempdb..#ValidatedScanCode') IS NOT NULL
	BEGIN
		DROP TABLE #ValidatedScanCode
	END 

	CREATE TABLE #ValidatedScanCode
	(
	Id			INT,
	ScanCode	VARCHAR(13),
	InsertDate	DATETIME,
	Validated	BIT
	)

	-- Check the app config value of ReceiveUPCPLUFromIcon
	DECLARE @Status SMALLINT
	SET @Status = dbo.fn_ReceiveUPCPLUUpdateFromIcon()
	
	IF @Status = 0 -- Validated UPC & PLU flags have not been turned on for the region.
	BEGIN
		INSERT INTO #ValidatedScanCode
		SELECT II.Item_Key AS Id, II.Identifier AS ScanCode, GETDATE() AS InsertDate, 1 AS Validated 
		FROM ItemIdentifier II (NOLOCK) 
			INNER JOIN Item I (NOLOCK) ON I.Item_Key = II.Item_Key 
		WHERE  I.SubTeam_No = ISNULL(@SubTeam_No, I.SubTeam_No)
				AND I.Deleted_Item=0 AND II.Deleted_Identifier=0 AND
				I.Retail_Sale = CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END
	END
	ELSE
	IF @Status = 1 -- Only validated UPCs are passing from Icon to IRMA
	BEGIN
		INSERT INTO #ValidatedScanCode
		SELECT
			CASE WHEN VSC.Id IS NULL THEN II.Item_Key ELSE VSC.Id END AS Id,
			CASE WHEN VSC.ScanCode IS NULL THEN II.Identifier ELSE VSC.ScanCode END AS ScanCode,
			CASE WHEN VSC.InsertDate IS NULL THEN GETDATE() ELSE VSC.InsertDate END AS InsertDate,
			CASE WHEN VSC.ScanCode IS NULL THEN 0 ELSE 1 END AS Validated
		FROM ItemIdentifier II (NOLOCK) 
			LEFT OUTER JOIN ValidatedScanCode VSC (NOLOCK) ON VSC.ScanCode=II.Identifier
			INNER JOIN Item I (NOLOCK) ON I.Item_Key = II.Item_Key
		WHERE I.SubTeam_No = ISNULL(@SubTeam_No, I.SubTeam_No) AND (
			(VSC.Id IS NOT NULL AND I.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END))
			OR (VSC.Id IS NULL AND I.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END) 
				AND (LEN(Identifier) < 7 OR Identifier LIKE '2%00000'))
			OR ( VSC.Id IS NULL AND I.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END) 
				AND (I.Remove_Item = 1 OR II.Remove_Identifier = 1)))
	END
	ELSE
	IF @Status = 2 -- Only validated PLUs are passing from Icon to IRMA
	BEGIN
		INSERT INTO #ValidatedScanCode
		SELECT
			CASE WHEN VSC.Id IS NULL THEN II.Item_Key ELSE VSC.Id END AS Id,
			CASE WHEN VSC.ScanCode IS NULL THEN II.Identifier ELSE VSC.ScanCode END AS ScanCode,
			CASE WHEN VSC.InsertDate IS NULL THEN GETDATE() ELSE VSC.InsertDate END AS InsertDate,
			CASE WHEN VSC.ScanCode IS NULL THEN 0 ELSE 1 END AS Validated
		FROM ItemIdentifier II (NOLOCK) 
			LEFT OUTER JOIN ValidatedScanCode VSC (NOLOCK) ON VSC.ScanCode=II.Identifier
			INNER JOIN Item I (NOLOCK) ON I.Item_Key = II.Item_Key
		WHERE (I.SubTeam_No = ISNULL(@SubTeam_No, I.SubTeam_No)
				AND I.Deleted_Item=0 AND II.Deleted_Identifier=0) AND ( 	
				( I.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END) 
					AND (LEN(Identifier) < 7 OR Identifier LIKE '2%00000') AND VSC.Id IS NOT NULL)
				OR ( I.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END) 
					AND (NOT (LEN(Identifier) < 7 OR Identifier LIKE '2%00000')))
				OR ( I.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END) 
					AND (I.Remove_Item = 1 OR II.Remove_Identifier = 1 )))
	END
	ELSE 
	IF @Status = 3 -- Both Validated UPC & PLU are passing from Icon to IRMA
	BEGIN				
		INSERT INTO #ValidatedScanCode
		SELECT
			CASE WHEN VSC.Id IS NULL THEN II.Item_Key ELSE VSC.Id END AS Id,
			CASE WHEN VSC.ScanCode IS NULL THEN II.Identifier ELSE VSC.ScanCode END AS ScanCode,
			CASE WHEN VSC.InsertDate IS NULL THEN GETDATE() ELSE VSC.InsertDate END AS InsertDate,
			CASE WHEN VSC.ScanCode IS NULL THEN 0 ELSE 1 END AS Validated
		FROM ItemIdentifier II  (NOLOCK)
			LEFT OUTER JOIN ValidatedScanCode VSC (NOLOCK) ON VSC.ScanCode=II.Identifier
			LEFT OUTER JOIN Item I (NOLOCK) ON I.Item_Key = II.Item_Key
		WHERE ( I.SubTeam_No = ISNULL(@SubTeam_No, I.SubTeam_No)
				AND I.Deleted_Item=0 AND II.Deleted_Identifier=0)
			OR ( I.SubTeam_No = ISNULL(@SubTeam_No, I.SubTeam_No) 
				AND (I.Remove_Item = 1 OR II.Remove_Identifier = 1)
				AND I.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I.Retail_Sale END)
				AND I.Deleted_Item=0 AND II.Deleted_Identifier=0) 
			OR ( I.SubTeam_No = ISNULL(@SubTeam_No, I.SubTeam_No)
				AND II.Remove_Identifier = 0 AND II.Deleted_Identifier=0
				AND I.Remove_Item = 0 AND I.Deleted_Item = 0 AND I.Retail_Sale = 0)
	END	

	-----------------------------------
	-- Return the search data
	-----------------------------------
	
    IF @ItemChgTypeID IS NULL OR @ItemChgTypeID = 0 -- Price Changes (include item changes)
	----------------------------------------------------------------------------------------------------
	BEGIN 
		--------------
		-- Build TempTable Sub Queries
		--------------
		SELECT 
			D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId, D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID, PCT1.PriceChgTypeDesc
		INTO #PriceData
		FROM 
			PriceBatchDetail D2 (NOLOCK)
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = D2.PriceChgTypeId
		WHERE 
			D2.PriceChgTypeID = ISNULL(@PriceChgTypeID, D2.PriceChgTypeID)			-- MAKE SURE THIS MATCHES THE PRICE CHANGE TYPE ENTERED BY THE USER
			AND NOT EXISTS (SELECT 
								D.PriceBatchDetailId 								-- THERE IS NOT AN UNEXPIRED, NEW ITEM OR GREATER PRIORITY SALE PBD RECORD FOR THE ITEM-STORE 
							FROM 
								PriceBatchDetail D (NOLOCK)	
								LEFT JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = D.PriceChgTypeId
							WHERE 
								D.Item_Key = D2.Item_Key 
								AND D.Store_No = D2.Store_No
								AND ISNULL(D.Expired, 0) = 0
								AND ((D.ItemChgTypeID = 1 AND D.PriceBatchHeaderID IS NULL)
									OR (D.Sale_End_Date	>= D2.StartDate
										AND @StartDate BETWEEN D.StartDate AND D.Sale_End_Date
										AND (PCT.Priority > PCT1.Priority OR (PCT.Priority =  ISNULL(@Priority, PCT.Priority) AND D.StartDate > D2.StartDate)))))
										
			AND D2.PriceBatchHeaderID IS NULL	-- THE PBD RECORD IS NOT ALREADY ASSIGNED TO A BATCH
			AND @StartDate BETWEEN D2.StartDate AND ISNULL(D2.Sale_End_Date, @StartDate)	-- THE SALE IS STILL ONGOING BASED ON THE BATCH START DATE
			AND D2.Expired = 0	-- THE RECORD IS NOT EXPIRED

		CREATE NONCLUSTERED INDEX #PriceData_Store_No on #PriceData (Store_No ASC);

		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #ItemData
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key
				AND II.Default_Identifier = 1 
				AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)
				
		CREATE NONCLUSTERED INDEX #ItemData_ItemKey on #ItemData (Item_Key ASC);

		SELECT 
			iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
		INTO #ItemOverrideData
		FROM
			ItemOverride iov 
		WHERE 
			iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
		
		CREATE NONCLUSTERED INDEX #ItemOverrideData_ItemKey_StoreJuris on #ItemOverrideData (Item_Key ASC, StoreJurisdictionID ASC);

		SELECT TOP(@MaxRows) 
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PBD.PriceChgTypeDesc,
			[ItemChgTypeID] = 0, 
			[ItemChgTypeDesc] = 'PRC',
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN #PriceData PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #ItemData I ON I.Item_Key = PBD.Item_Key 
			LEFT JOIN #ItemOverrideData iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			INNER JOIN StoreItem SI (NOLOCK) ON SI.Store_No = PBD.Store_No AND SI.Item_Key = PBD.Item_Key AND SI.Authorized = 1
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
			
		WHERE 
			(@Required_VendorForBatching = 0 OR (@Required_VendorForBatching = 1 AND SIV.Vendor_ID IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0	OR dbo.fn_IsItemInStoreJurisdiction(ISNULL(I.Item_Key, iov.Item_Key), I.Identifier, S.Store_No) = 1)
		
		ORDER BY 
			VSC.Id, CONVERT(bigint, ISNULL(PBD.Identifier, I.Identifier)), S.Store_Name 
	END
	
	ELSE IF @ItemChgTypeID = 5 -- Price Changes (include item changes
	BEGIN
	---------------------------------------------------------------------------------------------------)
		SELECT 
			D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId, D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID, PCT1.PriceChgTypeDesc
		INTO #Union1PriceData
		FROM
			PriceBatchDetail D2 (NOLOCK)
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = D2.PriceChgTypeId
		
		WHERE 
			D2.PriceChgTypeID = ISNULL(@PriceChgTypeID, D2.PriceChgTypeID)		-- MAKE SURE THIS MATCHES THE PRICE CHANGE TYPE ENTERED BY THE USER
			AND NOT EXISTS (SELECT 
								D.PriceBatchDetailId 							-- THERE IS NOT AN UNEXPIRED, NEW ITEM OR GREATER PRIORITY SALE PBD RECORD FOR THE ITEM-STORE 
							FROM 
								PriceBatchDetail D (NOLOCK)	
								LEFT JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = D.PriceChgTypeId
							WHERE 
								D.Item_Key = D2.Item_Key 
								AND D.Store_No = D2.Store_No
								AND ISNULL(D.Expired, 0) = 0
								AND ((D.ItemChgTypeID = 1 AND D.PriceBatchHeaderID IS NULL)
									OR (D.Sale_End_Date	>= D2.StartDate
										AND @StartDate BETWEEN D.StartDate AND D.Sale_End_Date
										AND (PCT.Priority > ISNULL(@Priority, -1) 
											OR (PCT.Priority =  ISNULL(@Priority, PCT.Priority) AND D.StartDate > D2.StartDate)))))
			
			AND D2.PriceBatchHeaderID IS NULL	-- THE PBD RECORD IS NOT ALREADY ASSIGNED TO A BATCH
			AND @StartDate BETWEEN D2.StartDate AND ISNULL(D2.Sale_End_Date, @StartDate)	-- THE SALE IS STILL ONGOING BASED ON THE BATCH START DATE
			AND D2.Expired = 0	-- THE RECORD IS NOT EXPIRED

		CREATE NONCLUSTERED INDEX #Union1PriceData_Store_No on #Union1PriceData (Store_No ASC);

		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #Union1ItemData
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key AND II.Default_Identifier = 1 AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

		CREATE NONCLUSTERED INDEX #Union1ItemData_ItemKey on #Union1ItemData (Item_Key);

		SELECT 
			D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId, D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID, PCT1.PriceChgTypeDesc
		INTO #Union2PriceData
		FROM 
			PriceBatchDetail D2 (NOLOCK)
			LEFT JOIN Price P (NOLOCK) ON P.Store_No = D2.Store_No AND P.Item_Key = D2.Item_Key
			LEFT JOIN SignQueue SQ (NOLOCK) ON SQ.Store_No = D2.Store_No AND SQ.Item_Key = D2.Item_Key
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = D2.PriceChgTypeId
		WHERE 
			D2.PriceBatchHeaderID IS NULL						-- THE PBD RECORD IS NOT ALREADY ASSIGNED TO A BATCH
			AND EXISTS (SELECT 
							D.PriceBatchDetailId				-- THERE IS AN UNEXPIRED, NEW ITEM CHANGE PBD RECORD FOR THE ITEM-STORE
						FROM 
							PriceBatchDetail D (NOLOCK)
						WHERE 
							D.Item_Key = D2.Item_Key 
							AND D.Store_No = D2.Store_No
							AND D.PriceBatchHeaderID IS NULL
							AND ISNULL(D.Expired, 0) = 0
							AND D.ItemChgTypeID = 1)
			
			AND NOT EXISTS (SELECT 
								D.PriceBatchDetailId			-- THERE IS NOT AN UNEXPIRED, SALE PBD RECORD FOR THE ITEM-STORE THAT HAS A GREATER
							FROM 
								PriceBatchDetail D (NOLOCK)		-- PRICE PRIORITY THAN THE CURRENT PRICE CHANGE TYPE THE USER IS SEARCHING FOR
								INNER JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = D.PriceChgTypeId
							WHERE 
								D.Item_Key = D2.Item_Key 
								AND D.Store_No = D2.Store_No
								AND ISNULL(D.Expired, 0) = 0
								AND D.Sale_End_Date	>= ISNULL(D2.StartDate, D.Sale_End_Date)
								AND @StartDate BETWEEN D.StartDate AND D.Sale_End_Date
										AND (PCT.Priority > ISNULL(@Priority, -1) 
											OR (PCT.Priority =  ISNULL(@Priority, PCT.Priority)  AND D.StartDate > D2.StartDate)))
			
			AND @StartDate <= ISNULL(D2.Sale_End_Date, @StartDate)	-- IF THE USER IS SEARCHING FOR A SALE, IT STARTED BEFORE THE BATCH START DATE
			AND D2.Expired = 0										-- THE RECORD IS NOT EXPIRED
			AND (
				-- For items that are actually NEW items to IRMA (not a store authorization for an 
				-- existing item), it is required that a PBD record exists to set the price for the
				-- item before the NEW item can be batched. 
				(D2.ReAuthFlag = 0
					AND D2.PriceChgTypeID = ISNULL(@PriceChgTypeID, D2.PriceChgTypeID)
					AND D2.StartDate <= @StartDate)
				
				-- For items that are store authorizations for an existing item, a PBD record that sets the 
				-- price for the item is not required.  The existing Price in IRMA will be used. Verify that there
				-- is a price in the SignQueue or Price table for the item before allowing it to be batched.
				OR (D2.ReAuthFlag = 1
					
					-- For Sale PCTs, exact match; For Regular PCTs, exact match or Price or SignQueue value
					AND (ISNULL(D2.PriceChgTypeID, ISNULL(P.PriceChgTypeID, SQ.PriceChgTypeID)) =  ISNULL(@PriceChgTypeID,ISNULL(D2.PriceChgTypeID, ISNULL(P.PriceChgTypeID, SQ.PriceChgTypeID))))
					AND (P.Item_Key IS NOT NULL OR SQ.Item_Key IS NOT NULL))
				)

		CREATE NONCLUSTERED INDEX #Union2PriceData_Store_No on #Union2PriceData (Store_No ASC);

		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #Union2ItemData
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key AND II.Default_Identifier = 1 AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

		CREATE NONCLUSTERED INDEX #Union2ItemData_ItemKey on #Union2ItemData (Item_Key);

		SELECT 
			D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId, D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID, PCT1.PriceChgTypeDesc
		INTO #Union3PriceData
		FROM 
			PriceBatchDetail D2 (NOLOCK)
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = D2.PriceChgTypeId
		WHERE 
			D2.ItemChgTypeID IN (2,6)	-- ITEM CHANGES ALSO INCLUDE 'OFF PROMO COST' CHANGES; OFF COST RECORDS SUBJECT TO DATE RESTRICTION
										-- IF ITEM CHANGE HAS STARTDATE THEN LIMIT BY IT (COST CHANGES)                    
			AND NOT EXISTS (SELECT 
								D.PriceBatchDetailId 
							FROM 
								PriceBatchDetail D (NOLOCK)
								LEFT JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = D.PriceChgTypeId
							WHERE 
								D.Item_Key = D2.Item_Key 
								AND D.Store_No = D2.Store_No
								AND ISNULL(D.Expired, 0) = 0
								AND ((D.PriceChgTypeID IS NOT NULL
										AND D.PriceBatchHeaderID IS NULL
										AND D.StartDate <= @StartDate)
									OR (D.Sale_End_Date	>= D2.StartDate
										AND @StartDate BETWEEN D.StartDate AND D.Sale_End_Date
										AND (PCT.Priority > ISNULL(@Priority, -1) 
											OR (PCT.Priority = ISNULL(@Priority, PCT.Priority) AND D.StartDate > D2.StartDate)))))
										AND D2.PriceBatchHeaderID IS NULL
			AND @StartDate BETWEEN ISNULL(D2.StartDate, @StartDate) AND ISNULL(D2.Sale_End_Date, @StartDate)
			AND D2.Expired = 0

		CREATE NONCLUSTERED INDEX #Union3PriceData_Store_No on #Union3PriceData (Store_No ASC);

		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #Union3ItemData
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key AND II.Default_Identifier = 1 AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

		CREATE NONCLUSTERED INDEX #Union3ItemData_ItemKey on #Union3ItemData (Item_Key);

		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #Union4ItemData
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key AND II.Default_Identifier = 1 AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

		CREATE NONCLUSTERED INDEX #Union4ItemData_ItemKey on #Union4ItemData (Item_Key ASC);

		SELECT 
			PBD.PriceBatchDetailID, S.Store_No, S.Store_Name, [Identifier] = PO.ReferenceCode, [Item_Description] = PO.Description, PO.StartDate, [Sale_End_Date] = PO.EndDate, PO.Offer_Id, IB.Brand_Name, PBD.PriceChgTypeID, S.BatchUnvalidatedIngredients, I.Retail_Sale
		INTO #Union5PriceData
		FROM 
			@tblStoreList S
			INNER JOIN PriceBatchDetail PBD (NOLOCK) ON PBD.Store_No = S.Store_No
			INNER JOIN PromotionalOffer PO (NOLOCK) ON PO.Offer_ID = PBD.Offer_ID
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = PBD.Item_Key AND II.Default_Identifier = 1
			INNER JOIN Item I (NOLOCK) ON I.Item_Key = PBD.Item_Key
			INNER JOIN ItemBrand IB (NOLOCK) ON IB.Brand_ID = I.Brand_ID
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1										
		WHERE 
			PBD.PriceBatchHeaderID IS NULL
			AND PBD.Expired = 0
			AND ISNULL(PBD.Sale_End_Date, @StartDate) >= @StartDate
			AND PBD.Offer_ID IS NOT NULL
			AND (@Required_VendorForBatching = 0 
				OR (@Required_VendorForBatching = 1 AND SIV.Vendor_Id IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0 
				OR dbo.fn_IsItemInStoreJurisdiction(II.Item_Key, II.Identifier, S.Store_No) = 1)
			AND dbo.fn_PriceSuperseded(PBD.Item_Key, PBD.Store_No, PBD.PriceChgTypeId, PBD.StartDate, @StartDate) = 0
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

		SELECT TOP(@MaxRows) 
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PBD.PriceChgTypeDesc,
			[ItemChgTypeID] = 0, 
			[ItemChgTypeDesc] = 'PRC',
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN #Union1PriceData PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #Union1ItemData I ON I.Item_Key = PBD.Item_Key
			LEFT JOIN	(SELECT 
							iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
						FROM
							ItemOverride iov 
						WHERE 
							iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
						) iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			INNER JOIN StoreItem SI (NOLOCK) ON SI.Store_No = PBD.Store_No AND SI.Item_Key = PBD.Item_Key AND SI.Authorized = 1
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
		WHERE 
			(@Required_VendorForBatching = 0 OR (@Required_VendorForBatching = 1 AND SIV.Vendor_ID IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0	OR dbo.fn_IsItemInStoreJurisdiction(ISNULL(I.Item_Key, iov.Item_Key), I.Identifier, S.Store_No) = 1)

		UNION 
		-- New or Re-Auth Items
        SELECT TOP(@MaxRows) 
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PBD.PriceChgTypeDesc,
			[ItemChgTypeID] = 1, 
			[ItemChgTypeDesc] = 'NEW',
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN #Union2PriceData PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #Union2ItemData I ON I.Item_Key = PBD.Item_Key
			LEFT JOIN	(SELECT 
							iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
						FROM
							ItemOverride iov 
						WHERE 
							iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
						) iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			INNER JOIN StoreItem SI (NOLOCK) ON SI.Store_No = PBD.Store_No AND SI.Item_Key = PBD.Item_Key AND SI.Authorized = 1
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
		WHERE 
			(@Required_VendorForBatching = 0 OR (@Required_VendorForBatching = 1 AND SIV.Vendor_ID IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0	OR dbo.fn_IsItemInStoreJurisdiction(ISNULL(I.Item_Key, iov.Item_Key), I.Identifier, S.Store_No) = 1)

		UNION 
		-- Item Changes only
        SELECT TOP(@MaxRows)
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PBD.PriceChgTypeDesc,
			[ItemChgTypeID] = 2, 
			[ItemChgTypeDesc] = 'ITM',
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN #Union3PriceData PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #Union3ItemData I ON I.Item_Key = PBD.Item_Key
			LEFT JOIN	(SELECT 
							iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
						FROM
							ItemOverride iov 
						WHERE 
							iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
						) iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			INNER JOIN Price P (NOLOCK) ON P.Item_Key = PBD.Item_Key AND P.Store_No = PBD.Store_No AND P.PriceChgTypeID  = ISNULL(@PriceChgTypeID, P.PriceChgTypeID)
			INNER JOIN StoreItem SI (NOLOCK) ON SI.Store_No = PBD.Store_No AND SI.Item_Key = PBD.Item_Key AND SI.Authorized = 1
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
		WHERE 
			(@Required_VendorForBatching = 0 OR (@Required_VendorForBatching = 1 AND SIV.Vendor_ID IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0	OR dbo.fn_IsItemInStoreJurisdiction(ISNULL(I.Item_Key, iov.Item_Key), I.Identifier, S.Store_No) = 1)

		UNION  
		--3 'Item Deletes'
		----------------------------------------------------------------------------------------------------
		-- 04/05/2010 - AZ TFS 12406 - Changed to pull ALL item deletes regardless of vendor authorizations 
		----------------------------------------------------------------------------------------------------
        SELECT TOP(@MaxRows) 
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PCT1.PriceChgTypeDesc,
			[ItemChgTypeID] = 3, 
			[ItemChgTypeDesc] = 'DEL',
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN (SELECT 
							D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId,	D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID
						FROM 
							PriceBatchDetail D2 (NOLOCK)
						WHERE 
							D2.ItemChgTypeID = 3  -- 3 = Item Deletes 
							AND D2.PriceBatchHeaderID IS NULL
							AND @StartDate BETWEEN D2.StartDate AND ISNULL(D2.Sale_End_Date, @StartDate)
							AND D2.Expired = 0
					) PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #Union4ItemData I ON I.Item_Key = PBD.Item_Key
			LEFT JOIN	(SELECT 
							iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
						FROM
							ItemOverride iov 
						WHERE 
							iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
						) iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = PBD.PriceChgTypeId
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
		WHERE 
			dbo.fn_PriceSuperseded(PBD.Item_Key, PBD.Store_No, PBD.PriceChgTypeId, PBD.StartDate, @StartDate) = 0
			AND	(@UseStoreJurisdictions = 0	OR dbo.fn_IsItemInStoreJurisdiction(ISNULL(I.Item_Key, iov.Item_Key), I.Identifier, S.Store_No) = 1)

		UNION  
		--4 -- Promotional Offers
		SELECT TOP(@MaxRows)
			Store_No,
			Store_Name, 
			Identifier,
			Brand_Name, 
			[Item_Description] = RTRIM(Item_Description), 
			[PriceBatchDetailID] = MAX(PriceBatchDetailID), 
			StartDate, 
			Sale_End_Date, 
			Offer_ID, 
			PCT1.PriceChgTypeID, 
			PCT1.PriceChgTypeDesc,
			[ItemChgTypeID] = 4, 
			[ItemChgTypeDesc] = 'OFR',
			CASE WHEN ( VSC.Validated = 0 AND details.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, details.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, details.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND details.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			#Union5PriceData details
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = details.PriceChgTypeId
			LEFT JOIN #ValidatedScanCode VSC ON details.Identifier = VSC.ScanCode 
		GROUP BY 
			Store_No, Store_Name, Identifier, Item_Description, StartDate, Sale_End_Date, Offer_Id, Brand_Name, PCT1.PriceChgTypeID,
					PCT1.PriceChgTypeDesc, VSC.Id, details.PriceChgTypeID, details.BatchUnvalidatedIngredients, details.Retail_Sale, VSC.Validated
	
	END

	ELSE IF @ItemChgTypeID = 1 -- New or Re-Auth Items
	BEGIN
	----------------------------------------------------------------------------------------------------
		SELECT 
			D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId, D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID, PCT1.PriceChgTypeDesc
		INTO #AuthChangePriceData
		FROM 
			PriceBatchDetail D2 (NOLOCK)
			LEFT JOIN Price P (NOLOCK) ON P.Store_No = D2.Store_No AND P.Item_Key = D2.Item_Key
			LEFT JOIN SignQueue SQ (NOLOCK) ON SQ.Store_No = D2.Store_No AND SQ.Item_Key = D2.Item_Key
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = D2.PriceChgTypeId
		WHERE 
			D2.PriceBatchHeaderID IS NULL						-- THE PBD RECORD IS NOT ALREADY ASSIGNED TO A BATCH
			AND EXISTS (SELECT 
							D.PriceBatchDetailId				-- THERE IS AN UNEXPIRED, NEW ITEM CHANGE PBD RECORD FOR THE ITEM-STORE
						FROM 
							PriceBatchDetail D (NOLOCK)
						WHERE 
							D.Item_Key = D2.Item_Key 
							AND D.Store_No = D2.Store_No
							AND D.PriceBatchHeaderID IS NULL
							AND ISNULL(D.Expired, 0) = 0
							AND D.ItemChgTypeID = 1)
			AND NOT EXISTS (SELECT 1							-- THERE IS NOT AN UNEXPIRED, SALE PBD RECORD FOR THE ITEM-STORE THAT HAS A GREATER
							FROM
								PriceBatchDetail D (NOLOCK)		-- PRICE PRIORITY THAN THE CURRENT PRICE CHANGE TYPE THE USER IS SEARCHING FOR
								INNER JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = D.PriceChgTypeId
							WHERE 
								D.Item_Key = D2.Item_Key 
								AND D.Store_No = D2.Store_No
								AND ISNULL(D.Expired, 0) = 0
								AND D.Sale_End_Date	>= ISNULL(D2.StartDate, D.Sale_End_Date)
								AND @StartDate BETWEEN D.StartDate AND D.Sale_End_Date
								AND PCT.Priority >= ISNULL(@Priority, PCT.Priority) 
								AND D.StartDate > D2.StartDate
							)
			AND @StartDate <= ISNULL(D2.Sale_End_Date, @StartDate) -- IF THE USER IS SEARCHING FOR A SALE, IT STARTED BEFORE THE BATCH START DATE
			AND D2.Expired = 0									-- THE RECORD IS NOT EXPIRED
			AND (
			-- For items that are actually NEW items to IRMA (not a store authorization for an 
			-- existing item), it is required that a PBD record exists to set the price for the
			-- item before the NEW item can be batched. 
			(D2.ReAuthFlag = 0
				AND D2.PriceChgTypeID = ISNULL(@PriceChgTypeID, D2.PriceChgTypeID)
				AND D2.StartDate <= @StartDate)
			
			-- For items that are store authorizations for an existing item, a PBD record that sets the 
			-- price for the item is not required.  The existing Price in IRMA will be used. Verify that there
			-- is a price in the SignQueue or Price table for the item before allowing it to be batched.
			OR (D2.ReAuthFlag = 1
				
				-- For Sale PCTs, exact match; For Regular PCTs, exact match or Price or SignQueue value
				--2009.12.18 - Dave Stacey - TFS 11351 - Add search for ALL price change types - took out redundant @On_Sale switch
				AND (ISNULL(D2.PriceChgTypeID, ISNULL(P.PriceChgTypeID, SQ.PriceChgTypeID)) =  ISNULL(@PriceChgTypeID,ISNULL(D2.PriceChgTypeID, ISNULL(P.PriceChgTypeID, SQ.PriceChgTypeID))))
				AND (P.Item_Key IS NOT NULL 
					OR SQ.Item_Key IS NOT NULL))
			)
		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #AuthChangeItemData
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key AND II.Default_Identifier = 1 AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)
		
        SELECT TOP(@MaxRows) 
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PBD.PriceChgTypeDesc,
			[ItemChgTypeID] = 1, 
			[ItemChgTypeDesc] = 'NEW',
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN #AuthChangePriceData	PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #AuthChangeItemData I ON I.Item_Key = PBD.Item_Key
			LEFT JOIN	(SELECT 
							iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
						FROM
							ItemOverride iov 
						WHERE 
							iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
						) iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			INNER JOIN StoreItem SI (NOLOCK) ON SI.Store_No = PBD.Store_No AND SI.Item_Key = PBD.Item_Key AND SI.Authorized = 1
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
		WHERE 
			(@Required_VendorForBatching = 0 OR (@Required_VendorForBatching = 1 AND SIV.Vendor_ID IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0	OR dbo.fn_IsItemInStoreJurisdiction(ISNULL(I.Item_Key, iov.Item_Key), I.Identifier, S.Store_No) = 1)
		
		ORDER BY 
			VSC.Id, CONVERT(bigint, ISNULL(PBD.Identifier, I.Identifier)), S.Store_Name
	END

	ELSE IF @ItemChgTypeID = 2 -- Item Changes only
	BEGIN
	----------------------------------------------------------------------------------------------------
		SELECT 
			D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId, D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID, PCT1.PriceChgTypeDesc
		INTO #ItemChgOnlyPriceData
		FROM 
			PriceBatchDetail D2 (NOLOCK)
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = D2.PriceChgTypeId
		WHERE 
			D2.ItemChgTypeID IN (2,6)	-- ITEM CHANGES ALSO INCLUDE 'OFF PROMO COST' CHANGES; OFF COST RECORDS SUBJECT TO DATE RESTRICTION
										-- IF ITEM CHANGE HAS STARTDATE THEN LIMIT BY IT (COST CHANGES)                    
			AND NOT EXISTS (SELECT 
								D.PriceBatchDetailId 
							FROM 
								PriceBatchDetail D (NOLOCK)
								LEFT JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = D.PriceChgTypeId
							WHERE 
								D.Item_Key = D2.Item_Key 
								AND D.Store_No = D2.Store_No
								AND ISNULL(D.Expired, 0) = 0
								AND ((D.PriceChgTypeID IS NOT NULL
										AND D.PriceBatchHeaderID IS NULL
										AND D.StartDate <= @StartDate)
									OR (D.Sale_End_Date	>= D2.StartDate
										AND @StartDate BETWEEN D.StartDate AND D.Sale_End_Date
										AND (PCT.Priority > ISNULL(@Priority, -1) 
											OR (PCT.Priority = ISNULL(@Priority, PCT.Priority) AND D.StartDate > D2.StartDate)))))
										AND D2.PriceBatchHeaderID IS NULL
			AND @StartDate BETWEEN ISNULL(D2.StartDate, @StartDate) AND ISNULL(D2.Sale_End_Date, @StartDate)
			AND D2.Expired = 0

		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #ItemChgOnlyItemData
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key AND II.Default_Identifier = 1 AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

		SELECT TOP(@MaxRows) 
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PBD.PriceChgTypeDesc,
			[ItemChgTypeID] = 2, 
			[ItemChgTypeDesc] = 'ITM',	
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN #ItemChgOnlyPriceData PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #ItemChgOnlyItemData I ON I.Item_Key = PBD.Item_Key
			LEFT JOIN	(SELECT 
							iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
						FROM
							ItemOverride iov 
						WHERE 
							iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
						) iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			INNER JOIN Price P (NOLOCK) ON P.Item_Key = PBD.Item_Key AND P.Store_No = PBD.Store_No AND P.PriceChgTypeID  = ISNULL(@PriceChgTypeID, P.PriceChgTypeID)
			INNER JOIN StoreItem SI (NOLOCK) ON SI.Store_No = PBD.Store_No AND SI.Item_Key = PBD.Item_Key AND SI.Authorized = 1
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
		WHERE (@Required_VendorForBatching = 0 OR (@Required_VendorForBatching = 1 AND SIV.Vendor_ID IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0	OR dbo.fn_IsItemInStoreJurisdiction(ISNULL(I.Item_Key, iov.Item_Key), I.Identifier, S.Store_No) = 1)
		
		ORDER BY 
			VSC.Id, CONVERT(bigint, ISNULL(PBD.Identifier, I.Identifier)), S.Store_Name
	END
	ELSE IF @ItemChgTypeID = 3 -- Item deletes
	BEGIN
	----------------------------------------------------------------------------------------------------
	-- 04/05/2010 - AZ TFS 12406 - Changed to pull ALL item deletes regardless of vendor authorizations 
	----------------------------------------------------------------------------------------------------

		SELECT 
			D2.Store_No, D2.Item_Key, D2.PriceBatchDetailID, D2.PriceChgTypeId,	D2.StartDate, D2.Sale_End_Date, D2.Identifier, D2.Offer_ID
		INTO #Type3Query_1
		FROM 
			PriceBatchDetail D2 (NOLOCK)
		WHERE 
			D2.ItemChgTypeID = @ItemChgTypeID  -- 3 = Item Deletes 
			AND D2.PriceBatchHeaderID IS NULL
			AND @StartDate BETWEEN D2.StartDate AND ISNULL(D2.Sale_End_Date, @StartDate)
			AND D2.Expired = 0
		SELECT 
			II.Item_Key, II.Identifier, I2.Item_Description, I2.Brand_ID, I2.Retail_Sale
		INTO #type3Query2
		FROM 
			Item I2 (NOLOCK)
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I2.Item_Key AND II.Default_Identifier = 1 AND I2.Retail_Sale = (CASE WHEN @IncNonRetailItems = 0 THEN 1 ELSE I2.Retail_Sale END)
		WHERE 
			I2.Item_Description LIKE ISNULL('%' + @Item_Description + '%', I2.Item_Description)
			AND I2.SubTeam_No = ISNULL(@SubTeam_No, I2.SubTeam_No)
			AND II.Identifier LIKE ISNULL('%' + @Identifier + '%', II.Identifier)
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

		SELECT TOP(@MaxRows) 
			S.Store_No,
			S.Store_Name, 
			[Identifier] = ISNULL(PBD.Identifier, I.Identifier),
			ISNULL(ibo.Brand_Name, ib.Brand_Name) AS Brand_Name,
			[Item_Description] = ISNULL(RTRIM(iov.Item_Description), RTRIM(I.Item_Description)),
			PBD.PriceBatchDetailID, 
			PBD.StartDate, 
			PBD.Sale_End_Date, 
			PBD.Offer_ID, 
			PBD.PriceChgTypeID, 
			PCT1.PriceChgTypeDesc,
			[ItemChgTypeID] = 3, 
			[ItemChgTypeDesc] = 'DEL',
			CASE WHEN ( VSC.Validated = 0 AND S.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, I.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, I.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND I.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			@tblStoreList S
			INNER JOIN #Type3Query_1 PBD ON S.Store_No = PBD.Store_No
			INNER JOIN #type3Query2 I ON I.Item_Key = PBD.Item_Key
			LEFT JOIN	(SELECT 
							iov.Item_Description, iov.Item_Key, iov.StoreJurisdictionID, iov.Brand_ID
						FROM
							ItemOverride iov 
						WHERE 
							iov.Item_Description like ISNULL('%' + @Item_Description + '%', iov.Item_Description)
						) iov on PBD.Item_Key = iov.Item_Key AND S.StoreJurisdictionID = iov.StoreJurisdictionID
			LEFT JOIN ItemBrand ib (NOLOCK) ON I.Brand_ID = ib.Brand_ID
			LEFT JOIN ItemBrand ibo (NOLOCK) ON iov.Brand_ID = ibo.Brand_ID
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = PBD.PriceChgTypeId
			LEFT JOIN #ValidatedScanCode VSC ON I.Identifier = VSC.ScanCode 
		WHERE 
			dbo.fn_PriceSuperseded(PBD.Item_Key, PBD.Store_No, PBD.PriceChgTypeId, PBD.StartDate, @StartDate) = 0
		
		ORDER BY 
			VSC.Id, CONVERT(bigint, ISNULL(PBD.Identifier, I.Identifier)), S.Store_Name
	END
	ELSE IF @ItemChgTypeID = 4 -- Promotional Offers
	----------------------------------------------------------------------------------------------------
		--only displaying 1 PriceBatchDetail record for a Store_No/Offer_ID combo; user will be batching
		--an entire offer, rather than individual items contained within the offer.
		--when detail is added to a batch, all details w/ the same Store_No/Offer_ID will have the PriceBatchHeaderID updated
	BEGIN 

		SELECT 
			PBD.PriceBatchDetailID, S.Store_No, S.Store_Name, [Identifier] = PO.ReferenceCode, [Item_Description] = PO.Description, PO.StartDate, [Sale_End_Date] = PO.EndDate, PO.Offer_Id, IB.Brand_Name, PBD.PriceChgTypeID, S.BatchUnvalidatedIngredients, I.Retail_Sale
		INTO #details
		FROM 
			@tblStoreList S
			INNER JOIN PriceBatchDetail PBD (NOLOCK) ON PBD.Store_No = S.Store_No
			INNER JOIN PromotionalOffer PO (NOLOCK) ON PO.Offer_ID = PBD.Offer_ID
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = PBD.Item_Key AND II.Default_Identifier = 1
			INNER JOIN Item I (NOLOCK) ON I.Item_Key = PBD.Item_Key
			INNER JOIN ItemBrand IB (NOLOCK) ON IB.Brand_ID = I.Brand_ID
			LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = PBD.Store_No AND SIV.Item_Key = PBD.Item_Key AND SIV.PrimaryVendor = 1 AND @Required_VendorForBatching = 1										
		WHERE 
			PBD.PriceBatchHeaderID IS NULL
			AND PBD.Expired = 0
			AND ISNULL(PBD.Sale_End_Date, @StartDate) >= @StartDate
			AND PBD.Offer_ID IS NOT NULL
			AND (@Required_VendorForBatching = 0 
				OR (@Required_VendorForBatching = 1 AND SIV.Vendor_Id IS NOT NULL))
			AND	(@UseStoreJurisdictions = 0 
				OR dbo.fn_IsItemInStoreJurisdiction(II.Item_Key, II.Identifier, S.Store_No) = 1)
			AND dbo.fn_PriceSuperseded(PBD.Item_Key, PBD.Store_No, PBD.PriceChgTypeId, PBD.StartDate, @StartDate) = 0
			AND (dbo.fn_IsScaleItem(II.Identifier) = 0 OR dbo.fn_IsScaleItem(II.Identifier) = @IncScaleItems)

    	SELECT TOP(@MaxRows) 
			Store_No,
			Store_Name, 
			Identifier,
			Brand_Name, 
			[Item_Description] = RTRIM(Item_Description), 
			[PriceBatchDetailID] = MAX(PriceBatchDetailID), 
			StartDate, 
			Sale_End_Date, 
			Offer_ID, 
			PCT1.PriceChgTypeID, 
			PCT1.PriceChgTypeDesc,
			[ItemChgTypeID] = 4, 
			[ItemChgTypeDesc] = 'OFR',
			CASE WHEN ( VSC.Validated = 0 AND details.BatchUnvalidatedIngredients = 0 AND
					(  CONVERT(FLOAT, details.Identifier) BETWEEN 46000000000 AND 46999999999 
					OR CONVERT(FLOAT, details.Identifier) BETWEEN 48000000000 AND 48999999999)
				) OR (VSC.Validated = 0 AND details.Retail_Sale=1) THEN NULL 
			ELSE VSC.Id END AS Id
		FROM 
			#details details
			LEFT JOIN PriceChgType PCT1 (NOLOCK) ON PCT1.PriceChgTypeID = details.PriceChgTypeId
			LEFT JOIN #ValidatedScanCode VSC ON details.Identifier = VSC.ScanCode 
		GROUP BY 
			Store_No, Store_Name, Identifier, Item_Description, StartDate, Sale_End_Date, Offer_Id, Brand_Name, PCT1.PriceChgTypeID,
					PCT1.PriceChgTypeDesc, VSC.Id, details.PriceChgTypeID, details.BatchUnvalidatedIngredients, details.Retail_Sale, VSC.Validated
		ORDER BY 
			VSC.Id, CONVERT(bigint, Identifier), Store_Name


    OPTION (RECOMPILE)
	END

	IF OBJECT_ID('tempdb..#ValidatedScanCode') IS NOT NULL
	BEGIN
		DROP TABLE #ValidatedScanCode
	END 

    SET NOCOUNT OFF
END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [dbo.GetPriceBatchItemSearch.sql]'
GO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchItemSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchItemSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchItemSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchItemSearch] TO [IRMAReportsRole]
    AS [dbo];

