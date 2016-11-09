SET NOCOUNT ON 
GO

-- Only make this change for the MA region.
IF EXISTS (SELECT RegionCode FROM Region WHERE RegionCode = 'MA')
BEGIN

	-- Create back up tables
	SELECT * INTO ItemCategory_Backup FROM ItemCategory
	
	SELECT * INTO ProdHierarchyLevel3_Backup FROM dbo.ProdHierarchyLevel3
	
	SELECT * INTO ProdHierarchyLevel4_Backup FROM dbo.ProdHierarchyLevel4
	
	SELECT * INTO NatHier_Class_Backup FROM NatHier_Class
	
	SELECT * INTO JDA_HierarchyMapping_Backup FROM JDA_HierarchyMapping

END
GO
-- Only make this change for the MA region.
IF EXISTS (SELECT RegionCode FROM Region WHERE RegionCode = 'MA')
BEGIN
	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'STA_MAHierarchyInsert')
	BEGIN
		DROP PROCEDURE STA_MAHierarchyInsert
	END
	ELSE 
	BEGIN
		PRINT 'Stored procedure STA_MAHierarchyInsert does not exist'
	END
END
ELSE
BEGIN
	PRINT 'Not in the MA region! No changes will be applied.'
END
GO

-- Only make this change for the MA region.
CREATE PROCEDURE STA_MAHierarchyInsert (
	@subTeamName VARCHAR(100),
	@itemCategoryName VARCHAR(35),
	@prodHierarchyLevel3 VARCHAR(50),
	@prodHierarchyLevel4 VARCHAR(50),
	@result VARCHAR(512) OUTPUT
)
AS
BEGIN
	
	IF NOT EXISTS (SELECT RegionCode FROM Region WHERE RegionCode = 'MA')
	BEGIN
		RAISERROR('Not in the MA region! No changes will be applied.', 16, 1);
	END

	SELECT @result = ''
	-- Parameter check
	IF (@subTeamName IS NULL OR @itemCategoryName IS NULL OR @prodHierarchyLevel3 IS NULL OR @prodHierarchyLevel4 IS NULL)
	BEGIN
		RAISERROR('Null parameter values are not allowed.', 16, 1);
	END

	-- Internal Variables
	-- Find out if the subteam, category, level 3 hierarchy, and level 4 hierarchy already exist
	DECLARE @subTeamNo INT = (SELECT [SubTeam_No]
							  FROM [SubTeam]
							  WHERE [SubTeam_Name] = @subTeamName),
			@subTeamPreFix CHAR(3) = RIGHT('000'+CAST(LEFT(@subTeamName,3) AS VARCHAR(3)),3)

	DECLARE @itemCategoryID INT = (SELECT ic.[Category_ID]
								   FROM [ItemCategory] ic
								   WHERE ic.[SubTeam_No] = @subTeamNo
								   AND ic.[Category_Name] = @itemCategoryName),
			@itemCategoryPrefix CHAR(3) = RIGHT('000'+CAST(LEFT(@itemCategoryName,3) AS VARCHAR(3)),3);
	
	DECLARE @prodHierarchyLevel3ID INT = (SELECT p3.[ProdHierarchyLevel3_ID]
										  FROM [ProdHierarchyLevel3] p3
										  WHERE p3.[Category_ID] = @itemCategoryID
										  AND p3.[Description] = @prodHierarchyLevel3),
			@prodLevel3Prefix CHAR(3) = RIGHT('000'+CAST(LEFT(@prodHierarchyLevel3,3) AS VARCHAR(3)),3);
				
	DECLARE @prodHierarchyLevel4ID INT = (SELECT p4.[ProdHierarchyLevel4_ID]
										  FROM [ProdHierarchyLevel4] p4 
										  WHERE p4.[ProdHierarchyLevel3_ID] = @prodHierarchyLevel3ID
										  AND p4.[Description] = @prodHierarchyLevel4),
			@prodLevel4Prefix CHAR(3) = RIGHT('000'+CAST(LEFT(@prodHierarchyLevel4,3) AS VARCHAR(3)),3);

	DECLARE @hierarchyRef VARCHAR(255)
BEGIN TRY
BEGIN TRANSACTION 

	IF (@subTeamNo IS NULL)
	BEGIN
		RAISERROR('SubTeam Does Not Exist!!!', 16, 1);
	END

	-- Insert the Item Category if it doesn't exist
	IF (@subTeamNo IS NOT NULL AND @itemCategoryID IS NULL)
	BEGIN
		-- Make sure no other Item Category exists with the same prefix
		IF NOT EXISTS (SELECT 1 FROM [ItemCategory] WHERE [Category_Name] LIKE @itemCategoryPrefix + '%' AND [SubTeam_No] = @subTeamNo)
		BEGIN
			INSERT INTO [dbo].[ItemCategory] ([Category_Name], [SubTeam_No])
			VALUES (@itemCategoryName, @subTeamNo);

			-- Get the ID of newly inserted category
			SELECT @itemCategoryID = (SELECT ic.[Category_ID]
									  FROM [dbo].[ItemCategory] ic
									  WHERE ic.[SubTeam_No] = @subTeamNo
									  AND ic.[Category_Name] = @itemCategoryName);

			-- Insert into NatHierClass
			SELECT @hierarchyRef  = @subTeamPreFix + @itemCategoryPrefix
			IF NOT EXISTS (SELECT 1 FROM [NatHier_Class] WHERE [HIERARCHY_REF] = @hierarchyRef)
			BEGIN
				INSERT INTO [NatHier_Class] ([HIERARCHY_REF], [HIER_FULL_NAME], [HIER_LEVEL], [HIER_LVL_ID], [HIER_PARENT])
				VALUES (@hierarchyRef, REPLACE(@itemCategoryName, @itemCategoryPrefix,''), 'CAT', 2, @subTeamPreFix)
			END

			-- Insert into JDA_HierarchyMapping
			IF NOT EXISTS (SELECT 1 FROM [JDA_HierarchyMapping] WHERE Subteam_No = @subTeamNo AND Category_ID = @itemCategoryID AND ProdHierarchyLevel3_ID = 0 AND ProdHierarchyLevel4_ID = 0)
			BEGIN
				INSERT INTO [JDA_HierarchyMapping] ([Subteam_No], [Category_ID], [ProdHierarchyLevel3_ID], [ProdHierarchyLevel4_ID], [JDA_Dept], [JDA_SubDept], [JDA_Class], [JDA_SubClass])
				VALUES (@subTeamNo, @itemCategoryID, 0, 0, CONVERT(INT, @subTeamPreFix), CONVERT(INT, @itemCategoryPrefix), 0, 0)
			END
		END
		ELSE
		BEGIN
			SELECT @result = @SubTeamName + ' already has an ItemCategory with prefix ' + @itemCategoryPrefix;
			RAISERROR('ItemCategory Prefix Already Exists!!!', 16, 1);
		END
	END

	-- Insert the Hierarchy Level 3 if it doesn't exist
	IF (@subTeamNo IS NOT NULL AND @itemCategoryID IS NOT NULL AND @prodHierarchyLevel3ID IS NULL)
	BEGIN
		-- Make sure no other hierarchy level 3 exists with the same prefix for the given ItemCategory
		IF NOT EXISTS (SELECT 1 FROM [ProdHierarchyLevel3] WHERE [Description] LIKE @prodLevel3Prefix + '%' AND [Category_ID] = @itemCategoryID)
		BEGIN
			INSERT INTO [dbo].[ProdHierarchyLevel3] ([Category_ID], [Description])
			VALUES (@itemCategoryID, @prodHierarchyLevel3);
		
			-- Get the ID of newly inserted level three hierarchy 
			SELECT @prodHierarchyLevel3ID = (SELECT p3.[ProdHierarchyLevel3_ID]
												FROM [dbo].[ProdHierarchyLevel3] p3 
												WHERE p3.[Category_ID] = @itemCategoryID
												AND p3.[Description] = @prodHierarchyLevel3);
		
			-- Insert into NatHierClass
			SELECT @hierarchyRef = @subTeamPreFix + @itemCategoryPrefix + @prodLevel3Prefix;
			IF NOT EXISTS (SELECT 1 FROM [NatHier_Class] WHERE [HIERARCHY_REF] = @hierarchyRef)
			BEGIN
				INSERT INTO [NatHier_Class] ([HIERARCHY_REF], [HIER_FULL_NAME], [HIER_LEVEL], [HIER_LVL_ID], [HIER_PARENT])
				VALUES (@hierarchyRef, REPLACE(@prodHierarchyLevel3, @prodLevel3Prefix,''), 'CLS', 3, @subTeamPreFix + @itemCategoryPrefix)
			END
			
			-- Insert into JDA_HierarchyMapping
			IF NOT EXISTS (SELECT 1 FROM [JDA_HierarchyMapping] WHERE Subteam_No = @subTeamNo AND Category_ID = @itemCategoryID AND ProdHierarchyLevel3_ID = @prodHierarchyLevel3ID AND ProdHierarchyLevel4_ID = 0)
			BEGIN
				INSERT INTO [JDA_HierarchyMapping] ([Subteam_No], [Category_ID], [ProdHierarchyLevel3_ID], [ProdHierarchyLevel4_ID], [JDA_Dept], [JDA_SubDept], [JDA_Class], [JDA_SubClass])
				VALUES (@subTeamNo, @itemCategoryID, @prodHierarchyLevel3ID, 0, CONVERT(INT, @subTeamPreFix), CONVERT(INT, @itemCategoryPrefix), CONVERT(INT, @prodLevel3Prefix), 0)
			END
		END
		ELSE
		BEGIN
			SELECT @result = @itemCategoryName + ' already has a level three hierarchy with prefix ' + @prodLevel3Prefix;
			RAISERROR('Prod Hierarchy Level 3 Prefix Already Exists!!!', 16, 1);
		END
	END

	-- Insert the Hierarchy Level 4, if it doesn't exist
	IF (@subTeamNo IS NOT NULL AND  @itemCategoryID IS NOT NULL AND @prodHierarchyLevel3ID IS NOT NULL AND @prodHierarchyLevel4ID IS NULL)
	BEGIN
		-- Make sure no other hierarchy level 4 exists with the same prefix for the given ItemCategory
		IF NOT EXISTS (SELECT 1 FROM [ProdHierarchyLevel4] WHERE [Description] LIKE @prodLevel4Prefix + '%' AND [ProdHierarchyLevel4_ID] = @prodHierarchyLevel4ID)
		BEGIN
			INSERT INTO [dbo].[ProdHierarchyLevel4] ([ProdHierarchyLevel3_ID], [Description])
			VALUES (@prodHierarchyLevel3ID, @prodHierarchyLevel4);

			-- Get the ID of newly inserted level four hierarchy 
			SELECT @prodHierarchyLevel4ID = (SELECT p4.[ProdHierarchyLevel4_ID]
										FROM [dbo].[ProdHierarchyLevel4] p4
										WHERE p4.[ProdHierarchyLevel3_ID] = @prodHierarchyLevel3ID
										AND p4.[Description] = @prodHierarchyLevel4);
			-- Insert into NatHierClass
			SELECT @hierarchyRef = @subTeamPreFix + @itemCategoryPrefix + @prodLevel3Prefix + @prodLevel4Prefix;
			IF NOT EXISTS (SELECT 1 FROM [NatHier_Class] WHERE [HIERARCHY_REF] = @hierarchyRef)
			BEGIN
				INSERT INTO [NatHier_Class] ([HIERARCHY_REF], [HIER_FULL_NAME], [HIER_LEVEL], [HIER_LVL_ID], [HIER_PARENT])
				VALUES (@hierarchyRef, REPLACE(@prodHierarchyLevel4, @prodLevel4Prefix,''), 'SCL', 4, @subTeamPreFix + @itemCategoryPrefix + @prodLevel3Prefix)
			END
			
			-- Insert into JDA_HierarchyMapping
			IF NOT EXISTS (SELECT 1 FROM [JDA_HierarchyMapping] WHERE Subteam_No = @subTeamNo AND Category_ID = @itemCategoryID AND ProdHierarchyLevel3_ID = @prodHierarchyLevel3ID AND ProdHierarchyLevel4_ID = @prodHierarchyLevel4ID)
			BEGIN
				INSERT INTO [JDA_HierarchyMapping] ([Subteam_No], [Category_ID], [ProdHierarchyLevel3_ID], [ProdHierarchyLevel4_ID], [JDA_Dept], [JDA_SubDept], [JDA_Class], [JDA_SubClass])
				VALUES (@subTeamNo, @itemCategoryID, @prodHierarchyLevel3ID, @prodHierarchyLevel4ID, CONVERT(INT, @subTeamPreFix), CONVERT(INT, @itemCategoryPrefix), CONVERT(INT, @prodLevel3Prefix), CONVERT(INT, @prodLevel4Prefix))
			END
		END
		ELSE
		BEGIN
			SELECT @result = @prodHierarchyLevel3 + ' already has a level four hierarchy with prefix ' + @prodLevel4Prefix;
			RAISERROR('Prod Hierarchy Level Four Prefix Already Exists!!!', 16, 1);
		END
	END

	-- output the result
	IF (@subTeamNo IS NOT NULL AND @itemCategoryID IS NOT NULL AND @prodHierarchyLevel3ID IS NOT NULL AND @prodHierarchyLevel4ID IS NOT NULL)
	BEGIN
		SELECT @result = @subTeamName + ' > ' + @itemCategoryName + ' > ' + @prodHierarchyLevel3 + ' > ' + @prodHierarchyLevel4 + ' hierarchy structure exists.';
	END
	ELSE
	BEGIN
		SELECT @result = 'UH-OH --> ' + @subTeamName + ' > ' + @itemCategoryName + ' > ' + @prodHierarchyLevel3 + ' > ' + @prodHierarchyLevel4 + ' hierarchy structure does not fully exist. Please review manually.';
	END

	IF @@TRANCOUNT > 0
	BEGIN
		COMMIT TRANSACTION
	END


END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
	BEGIN
		ROLLBACK TRANSACTION
	END

	PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
			+ 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
			+ CHAR(9) + ' at statement  ''' + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
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
END
GO

-- Only make these changes in the MA Region
IF EXISTS (SELECT RegionCode FROM Region WHERE RegionCode = 'MA')
BEGIN

	-- Perform the Mid-Atlantic Four Level Hierarchy Updates
	declare @result varchar(512)
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '150 NUTRITION BARS', '150 NUTRITION BARS', '150 NUTRITION BARS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '250 DO NOT USE', '250 DO NOT USE', '250 DO NOT USE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '910 REUSABLE BAGS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '102 MEAT', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '102 MEAT', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '102 MEAT', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '102 MEAT', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '102 MEAT', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '102 MEAT', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '104 PRODUCE', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '104 PRODUCE', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '104 PRODUCE', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '104 PRODUCE', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '104 PRODUCE', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '104 PRODUCE', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '105 FLORAL', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '105 FLORAL', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '105 FLORAL', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '105 FLORAL', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '105 FLORAL', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '105 FLORAL', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '107 SUSHI', '107 SUSHI', '107 SUSHI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '171 DOSATERIA', '171 DOSATERIA', '171 DOSATERIA', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '107 SUSHI', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '107 SUSHI', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '107 SUSHI', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '107 SUSHI', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '109 DAIRY', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '112 CHEESE', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '115 BODY CARE', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '115 BODY CARE', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '115 BODY CARE', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '115 BODY CARE', '150 NUTRITION BARS', '150 NUTRITION BARS', '150 NUTRITION BARS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '115 BODY CARE', '250 DO NOT USE', '250 DO NOT USE', '250 DO NOT USE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '910 REUSABLE BAGS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '121 COFFEE BAR', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '121 COFFEE BAR', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '121 COFFEE BAR', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '121 COFFEE BAR', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '130 JUICE BAR', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '130 JUICE BAR', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '130 JUICE BAR', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '130 JUICE BAR', '107 SUSHI', '107 SUSHI', '107 SUSHI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '130 JUICE BAR', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '130 JUICE BAR', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '130 JUICE BAR', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '140 ALCOHOL AND SPIRITS', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '140 ALCOHOL AND SPIRITS', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '920 FRONT END', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '920 FRONT END', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '920 FRONT END', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '920 FRONT END', '910 REUSABLE BAGS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', @result OUTPUT; PRINT @result; SET @result = NULL;

	-- Added as a result of unit testing with a test mapping...
	EXEC dbo.STA_MAHierarchyInsert '101 GROCERY', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '103 BEER', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '104 PRODUCE', '200 PROMOTION COUPON', '200 PROMOTION COUPON', '200 PROMOTION COUPON', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '106 DELI', '400 MISC.TRANSACTIONS', '400 MISC.TRANSACTIONS', '400 MISC.TRANSACTIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '108 SEAFOOD', '107 SUSHI', '107 SUSHI', '107 SUSHI', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '110 FROZEN', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '111 BAKERY', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '150 NUTRITION BARS', '150 NUTRITION BARS', '150 NUTRITION BARS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '198 GIFT CERTIFICATE', '198 GIFT CERTIFICATE', '198 GIFT CERTIFICATE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '114 VITAMINS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '160 VITAMIX', '160 VITAMIX', '160 VITAMIX', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '116 PUBLICATIONS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '117 BULK', '200 PROMOTION COUPON', '200 PROMOTION COUPON', '200 PROMOTION COUPON', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '200 PROMOTION COUPON', '200 PROMOTION COUPON', '200 PROMOTION COUPON', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', '300 MISC.TRANSCATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '118 WINE', '400 MISC.TRANSACTIONS', '400 MISC.TRANSACTIONS', '400 MISC.TRANSACTIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
	EXEC dbo.STA_MAHierarchyInsert '171 DOSATERIA', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;

END
ELSE
BEGIN
	PRINT 'Not in the MA region! No changes will be applied.'
END
go

IF EXISTS (SELECT RegionCode FROM Region WHERE RegionCode = 'MA')
BEGIN
	-- These already exist, but just need to be renamed...
	-- 106 DELI > 110 FROZEN > 110 FROZEN > 110 FROZEN

	UPDATE ITEMCATEGORY
	SET Category_Name = '110 FROZEN'
	FROM ItemCategory 
	INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
	WHERE SubTeam_Name = '106 DELI'
	AND Category_Name LIKE '110%'

	UPDATE PRODHIERARCHYLEVEL3
	SET [DESCRIPTION] = '110 FROZEN'
	FROM PRODHIERARCHYLEVEL3 
	INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
	INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
	WHERE SubTeam_Name = '106 DELI'
	AND Category_Name LIKE '110%'
	AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '110%'

	UPDATE PRODHIERARCHYLEVEL4
	SET [DESCRIPTION] = '110 FROZEN'
	FROM PRODHIERARCHYLEVEL4
	INNER JOIN PRODHIERARCHYLEVEL3 ON PRODHIERARCHYLEVEL3.PRODHIERARCHYLEVEL3_ID = PRODHIERARCHYLEVEL4.PRODHIERARCHYLEVEL3_ID
	INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
	INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
	WHERE SubTeam_Name = '106 DELI'
	AND Category_Name LIKE '110%'
	AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '110%'
	AND PRODHIERARCHYLEVEL4.[DESCRIPTION] LIKE '110%'

	-- UPDATE HIERARCHY... 115 BODY CARE > 101 GROCERY > 101 GROCERY > 101 GROCERY
	UPDATE ITEMCATEGORY
	SET Category_Name = '101 GROCERY'
	FROM ItemCategory 
	INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
	WHERE SubTeam_Name = '115 BODY CARE'
	AND Category_Name LIKE '101%'

	UPDATE PRODHIERARCHYLEVEL3
	SET [DESCRIPTION] = '101 GROCERY'
	FROM PRODHIERARCHYLEVEL3 
	INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
	INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
	WHERE SubTeam_Name = '115 BODY CARE'
	AND Category_Name LIKE '101%'
	AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '101%'

	UPDATE PRODHIERARCHYLEVEL4
	SET [DESCRIPTION] = '101 GROCERY'
	FROM PRODHIERARCHYLEVEL4
	INNER JOIN PRODHIERARCHYLEVEL3 ON PRODHIERARCHYLEVEL3.PRODHIERARCHYLEVEL3_ID = PRODHIERARCHYLEVEL4.PRODHIERARCHYLEVEL3_ID
	INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
	INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
	WHERE SubTeam_Name = '115 BODY CARE'
	AND Category_Name LIKE '101%'
	AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '101%'
	AND PRODHIERARCHYLEVEL4.[DESCRIPTION] LIKE '101%'

END
ELSE
BEGIN
	PRINT 'Not in the MA region! No changes will be applied.'
END
GO

-- Only make this change for the MA region.
IF EXISTS (SELECT RegionCode FROM Region WHERE RegionCode = 'MA')
BEGIN
	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'STA_MAHierarchyInsert')
	BEGIN
		DROP PROCEDURE STA_MAHierarchyInsert
	END
	ELSE 
	BEGIN
		PRINT 'Stored procedure STA_MAHierarchyInsert does not exist'
	END
END
ELSE
BEGIN
	PRINT 'Not in the MA region! No changes will be applied.'
END
GO

SET NOCOUNT OFF
GO