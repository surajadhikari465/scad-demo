--*******************************************************************************
--Title: iCon Auto-Match IRMA Items
--Author: Benjamin Loving
--Date: 2013-12-12
--Task # 1498 Auto Match Development
--Purpose: This script will match up identifiers shared by multiple regions to
--         create a single Product record for the item. A set of rules will 
--         determine which identifiers are matched up and how data elements 
--         will be chosen. The rules were specified as Acceptance criteria in 
--         the user story and are copied below for reference
--
--         The following rules were implemented:
--	        * Regional alphabetical ordering for single version of truth 
--	        * UPC = let’s assume for now this is an item that has identifier > 6 digits 
--	        * Exclude items whose identifier has last five zeros 
--	        * If only one region has a specific UPC – the first rule will not fully apply 
--
--		   The following rules were not immediately implemented due to larger design discussions
--	        * Exclude items that are weighed at the POS 
--	        * Exclude deleted items (what if deleted in one region and not deleted in another?
--*******************************************************************************
DECLARE @runTime DATETIME,
		@runUser VARCHAR(128),
		@runHost VARCHAR(128),
		@runDB VARCHAR(128),
		@CodeLocation VARCHAR(255)

SELECT	@runTime = GETDATE(),
		@runUser = SUSER_NAME(),
		@runHost = HOST_NAME(),
		@runDB = DB_NAME(),
		@CodeLocation = '1.0 Variable Initialization'

PRINT '---------------------------------------------------------------------------------------'
PRINT 'Current System Time: ' + CONVERT(VARCHAR, @runTime, 121)
PRINT 'User Name: ' + @runUser
PRINT 'Running From Host: ' + @runHost
PRINT 'Connected To DB Server: ' + @@SERVERNAME
PRINT 'DB Name: ' + @runDB
PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Begin [iCon].[dbo].[Product] Auto-Match Script'
PRINT '---------------------------------------------------------------------------------------'

SET @CodeLocation = '1.1 BEGIN TRY'
BEGIN TRY

	SET @CodeLocation = '1.2 BEGIN TRANSACTION'
	BEGIN TRANSACTION

		SET @CodeLocation = '2. Create pivot table of regional table with '

		----------------------------------------------------------------------
		-- Create a temp table with PIVOT information based on the 
		--
		-- BJL 2013-12-13 The following two auto-match rules were not able to 
		--	be applied at this time
		-- 		* Exclude items that are weighed at the POS 
		--		* Exclude deleted items (what if deleted in one region and 
		--		  not deleted in another?
		IF OBJECT_ID('tempdb..#amTmp') IS NOT NULL
			DROP TABLE #amTmp

		CREATE TABLE #amTmp (
			[Identifier] VARCHAR(13) NOT NULL PRIMARY KEY,
			[EU] BIT,
			[MA] BIT,
			[FL] BIT,
			[MW] BIT,
			[NA] BIT,
			[NC] BIT,
			[NE] BIT,
			[PN] BIT,
			[RM] BIT,
			[SO] BIT,
			[SP] BIT,
			[SW] BIT,
			[sourceRegion] CHAR(2) NULL,
			[regionCount] INT NULL DEFAULT 0,
			[matchCount] INT NULL DEFAULT 0			
		)
		INSERT INTO #amTmp (
			[Identifier],
			[EU],
			[MA],
			[FL],
			[MW],
			[NA],
			[NC],
			[NE],
			[PN],
			[RM],
			[SO],
			[SP],
			[SW]
		)
		SELECT DISTINCT
			[Identifier],
			CASE WHEN [EU] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [MA] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [FL] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [MW] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [NA] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [NC] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [NE] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [PN] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [RM] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [SO] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [SP] IS NULL THEN 0 ELSE 1 END,
			CASE WHEN [SW] IS NULL THEN 0 ELSE 1 END
		FROM
		(SELECT 
			[RegionCode],
			[Identifier],
			[valueIdentifier] = [Identifier]
			FROM dbo.[IRMAItemLoad]
			WHERE LEN([Identifier]) > 6
			AND [Identifier] NOT LIKE '%00000'
			) AS SourceTable
		PIVOT
		(
		MAX([valueIdentifier])
		FOR [regionCode] IN (
			[EU],
			[MA],
			[FL],
			[MW],
			[NA],
			[NC],
			[NE],
			[PN],
			[RM],
			[SO],
			[SP],
			[SW]
			)
		) AS PivotTable
		ORDER BY [Identifier] ASC;

		----------------------------------------------------------------------
		-- Determine the regional alphabetical ordering rule for the single version 
		-- of truth. This also takes into account any identifiers that are unique
		-- to a single region.
		SET @CodeLocation = '6.1 Before determine source region'
		UPDATE #amTmp
		SET [sourceRegion] = CASE 
								WHEN [EU] = 1 THEN 'EU' 
								WHEN [FL] = 1 THEN 'FL' 
								WHEN [MA] = 1 THEN 'MA' 
								WHEN [MW] = 1 THEN 'MW' 
								WHEN [NA] = 1 THEN 'NA' 
								WHEN [NC] = 1 THEN 'NC' 
								WHEN [NE] = 1 THEN 'NE' 
								WHEN [PN] = 1 THEN 'PN' 
								WHEN [RM] = 1 THEN 'RM' 
								WHEN [SO] = 1 THEN 'SO' 
								WHEN [SP] = 1 THEN 'SP' 
								WHEN [SW] = 1 THEN 'SW' ELSE ''
							 END
		SET @CodeLocation = '6.2 After determine source region'

		----------------------------------------------------------------------
		-- Get values for the various foreign keys in the Product table
		SET @CodeLocation = '7.1 Before get foreign key values'
		DECLARE @BrandHierarchyID INT,
				@DepartmentSaleID INT,
				@TaxHierarchyID INT,
				@MerchandisingHierarchyID INT		
		SELECT @BrandHierarchyID = (SELECT MIN(BrandHierarchyID) FROM BrandHierarchy)
		SELECT @DepartmentSaleID = (SELECT MIN(DepartmentSaleID) FROM DepartmentSale)
		SELECT @TaxHierarchyID = (SELECT MIN(TaxHierarchyID) FROM TaxHierarchy)
		SELECT @MerchandisingHierarchyID = (SELECT MIN(MerchandisingHierarchyID) FROM MerchandisingHierarchy)
		SET @CodeLocation = '7.2 After get foreign key values'

		----------------------------------------------------------------------
		-- Insert the auto-matched record into the Product to create a new MainId
		--  note: All fields in [dbo].[Product] are non-nullable
		SET @CodeLocation = '8.1 Before INSERT of auto-matched product records'
		INSERT INTO [dbo].[Product] (
			[ScanCode],					-- Identifier
			[Short],					-- VarChar (26) : irma pos description
			[Long],						-- VarChar (60) : irma item description
			[S-RomanceDescription],		-- VarChar (60) : irma sign description
			[L-RomanceDescription],		-- VarChar (60) : irma sign description
			[PackageSize],				-- Decimal (9,4) : 
			[PackageUnit],				-- VarChar (25) : 
			[RetailSize],				-- Decimal (9,4) : numerical size listed on product
			[RetailUnit],				-- VarChar (25) : retail unit of measure
			[BrandHierarchyID],			-- Int : foreign key
			[DepartmentSaleID],			-- Int : foreign key to get departmentsale info
			[MerchandisingHierarchyID], -- Int : foreign key to get merchandising info
			[TaxHierarchyID],			-- Int : foreign key
			[LastUpdated],				-- datetime of last update
			[UpdatedBy]					-- user/source of last update
		)
		SELECT 
			iil.[Identifier], -- ScanCode
			ISNULL(iil.[pos_description],''), -- Short
			ISNULL(iil.[item_description],''), -- Long
			ISNULL(iil.[sign_description],''), -- S-RomanceDescription
			ISNULL(iil.[sign_description],''), -- L-RomanceDescription
			ISNULL(iil.[package_size],0),
			ISNULL(iil.[package_unit],''),
			ISNULL(iil.[retail_size],0),
			ISNULL(iil.[retail_unit],''),
			@BrandHierarchyID,
			@DepartmentSaleID,
			@MerchandisingHierarchyID,
			@TaxHierarchyID,
			GETDATE(), -- LastUpdated
			'AutoMatch' -- UpdatedBy
		FROM #amTmp amt
		INNER JOIN [dbo].[IRMAItemLoad] iil on amt.[Identifier] = iil.[Identifier]
											and amt.[sourceRegion] = iil.[regioncode]
		WHERE NOT EXISTS (SELECT P.[ScanCode] FROM [dbo].[Product] P WHERE amt.[Identifier] = P.[ScanCode])
		SET @CodeLocation = '8.2 After INSERT of auto-matched product records'

		----------------------------------------------------------------------
		-- Clean up any temporary tables
		SET @CodeLocation = '9.1 Drop temporary tables'
		DROP TABLE #amTmp

	IF @@TRANCOUNT > 0
	BEGIN
		SET @CodeLocation = '10.1 Before transaction commit'
		COMMIT TRANSACTION
		SET @CodeLocation = '10.2 After transaction commit'
	END

	PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
		+ 'Successfully inserted Auto-Match product rows into iCon.dbo.Product!' + CHAR(13) + CHAR(10)
		+ REPLACE(SPACE(120), SPACE(1), '-')

END TRY

BEGIN CATCH
        
		IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION

		PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
				+ CHAR(9) + ' at statement  ''' + @CodeLocation + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'Database changes were rolled back.' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-')

	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage
END CATCH
GO