SET NOCOUNT ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'STA_MAHierarchyBackout')
	DROP PROCEDURE STA_MAHierarchyBackout
GO

CREATE PROCEDURE STA_MAHierarchyBackout (
	@subTeamName VARCHAR(100),
	@itemCategoryName VARCHAR(35),
	@prodHierarchyLevel3 VARCHAR(50),
	@prodHierarchyLevel4 VARCHAR(50),
	@result VARCHAR(512) OUTPUT
)
AS
BEGIN
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

BEGIN TRY
BEGIN TRANSACTION 

	IF @prodHierarchyLevel4ID > 0
	BEGIN
		-- Check if items are associated to this fourth level
		IF NOT EXISTS (SELECT 1 FROM Item WHERE prodHierarchyLevel4_ID = @prodHierarchyLevel4ID)
		BEGIN
			DELETE FROM [ProdHierarchyLevel4]
			WHERE prodHierarchyLevel4_ID = @prodHierarchyLevel4ID

			DELETE FROM jda_hierarchymapping
			WHERE SUBTEAM_NO = @subTeamNo
			AND [Category_ID] = @itemCategoryID
			AND prodHierarchyLevel3_ID = @prodHierarchyLevel3ID
			AND prodHierarchyLevel4_ID = @prodHierarchyLevel4ID

			IF ((SELECT COUNT(*) FROM nathier_class WHERE HIERARCHY_REF = @subTeamPreFix + @itemCategoryPrefix + @prodLevel3Prefix + @prodLevel4Prefix AND HIER_LEVEL = 'SCL' AND HIER_LVL_ID = '4') = 1)
			BEGIN
				DELETE FROM nathier_class
				WHERE HIERARCHY_REF = @subTeamPreFix + @itemCategoryPrefix + @prodLevel3Prefix + @prodLevel4Prefix
				AND HIER_LEVEL = 'SCL'
				AND HIER_LVL_ID = '4'
			END

			SELECT @result = case when @result is not null then @result + '. ' else '' end  + 'prodHierarchyLevel4 = ''' + @prodHierarchyLevel4 + ' deleted';
		END
		ELSE
		BEGIN
			SELECT @result = case when @result is not null then @result + '. ' else '' end  + 'Items are associated to this fourth level. No delete occurred';
		END
	END

	IF @prodHierarchyLevel3ID > 0
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM [ProdHierarchyLevel4] WHERE [ProdHierarchyLevel3_ID] = @prodHierarchyLevel3ID)
		BEGIN
			DELETE FROM [ProdHierarchyLevel3]
			WHERE [ProdHierarchyLevel3_ID] = @prodHierarchyLevel3ID

			IF ((SELECT COUNT(*) FROM jda_hierarchymapping WHERE SUBTEAM_NO = @subTeamNo AND [Category_ID] = @itemCategoryID AND prodHierarchyLevel3_ID = @prodHierarchyLevel3ID) = 1)
			BEGIN
				DELETE FROM jda_hierarchymapping
				WHERE SUBTEAM_NO = @subTeamNo
				AND [Category_ID] = @itemCategoryID
				AND prodHierarchyLevel3_ID = @prodHierarchyLevel3ID
			END

			IF ((SELECT COUNT(*) FROM nathier_class WHERE HIERARCHY_REF = @subTeamPreFix + @itemCategoryPrefix + @prodLevel3Prefix AND HIER_LEVEL = 'CLS' AND HIER_LVL_ID = '3') = 1)
			BEGIN
				DELETE FROM nathier_class
				WHERE HIERARCHY_REF = @subTeamPreFix + @itemCategoryPrefix + @prodLevel3Prefix
				AND HIER_LEVEL = 'CLS'
				AND HIER_LVL_ID = '3'
			END

			SELECT @result = case when @result is not null then @result + '. ' else '' end  + 'prodHierarchyLevel3 = ''' + @prodHierarchyLevel3 + ''' deleted';
		END
		ELSE
		BEGIN
			SELECT @result = case when @result is not null then @result + '. ' else '' end  + 'Hierarchy 4th levels are associated to this third level. No delete occurred';
		END
	END

	IF @itemCategoryID IS NOT NULL
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM ProdHierarchyLevel3 WHERE [Category_ID] = @itemCategoryID)
		BEGIN
			DELETE FROM [ItemCategory]
			WHERE [Category_ID] = @itemCategoryID

			IF ((SELECT COUNT(*) FROM jda_hierarchymapping WHERE SUBTEAM_NO = @subTeamNo AND [Category_ID] = @itemCategoryID) = 1)
			BEGIN
				DELETE FROM jda_hierarchymapping
				WHERE SUBTEAM_NO = @subTeamNo
				AND [Category_ID] = @itemCategoryID;
				PRINT 'jda_hierarchymapping @subTeamNo = ' + CONVERT(VARCHAR, @subTeamNo) + ', @itemCategoryID = ' + CONVERT(VARCHAR, @itemCategoryID) + ' DELETED'
			END

			IF ((SELECT COUNT(*) FROM nathier_class WHERE HIERARCHY_REF = @subTeamPreFix + @itemCategoryPrefix AND HIER_LEVEL = 'CAT' AND HIER_LVL_ID = '2') = 1)
			BEGIN
				DELETE FROM nathier_class
				WHERE HIERARCHY_REF = @subTeamPreFix + @itemCategoryPrefix
				AND HIER_LEVEL = 'CAT'
				AND HIER_LVL_ID = '2'
			END

			SELECT @result = case when @result is not null then @result + '. ' else '' end  + 'ItemCategory = ''' + @itemCategoryName + ''' deleted';
		END
		ELSE
		BEGIN
			SELECT @result = case when @result is not null then @result + '. ' else '' end  + 'Hierarchy 3rd levels are associated to this category level. No delete occurred';
		END
	END

	IF @result is null or @result = ''
	BEGIN
		SELECT @result = @subTeamName + ' > ' + @itemCategoryName + ' > ' + @prodHierarchyLevel3 + ' > ' + @prodHierarchyLevel4 + ' hierarchy structure DOES NOT exist.';
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

-- Perform the Mid-Atlantic Four Level Hierarchy Updates
declare @result varchar(512);
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '150 NUTRITION BARS', '150 NUTRITION BARS', '150 NUTRITION BARS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '250 DO NOT USE', '250 DO NOT USE', '250 DO NOT USE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '101 GROCERY', '910 REUSABLE BAGS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '102 MEAT', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '102 MEAT', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '102 MEAT', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '102 MEAT', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '102 MEAT', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '102 MEAT', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '103 BEER', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '104 PRODUCE', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '104 PRODUCE', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '104 PRODUCE', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '104 PRODUCE', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '104 PRODUCE', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '104 PRODUCE', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '105 FLORAL', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '105 FLORAL', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '105 FLORAL', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '105 FLORAL', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '105 FLORAL', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '105 FLORAL', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '107 SUSHI', '107 SUSHI', '107 SUSHI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;

EXEC dbo.STA_MAHierarchyBackout '106 DELI', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '106 DELI', '171 DOSATERIA', '171 DOSATERIA', '171 DOSATERIA', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '107 SUSHI', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '107 SUSHI', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '107 SUSHI', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '107 SUSHI', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '108 SEAFOOD', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '108 SEAFOOD', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '108 SEAFOOD', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '108 SEAFOOD', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '108 SEAFOOD', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '108 SEAFOOD', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '109 DAIRY', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '110 FROZEN', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '111 BAKERY', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '102 MEAT', '102 MEAT', '102 MEAT', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '108 SEAFOOD', '108 SEAFOOD', '108 SEAFOOD', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '109 DAIRY', '109 DAIRY', '109 DAIRY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '112 CHEESE', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;

EXEC dbo.STA_MAHierarchyBackout '115 BODY CARE', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '115 BODY CARE', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '115 BODY CARE', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '115 BODY CARE', '150 NUTRITION BARS', '150 NUTRITION BARS', '150 NUTRITION BARS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '115 BODY CARE', '250 DO NOT USE', '250 DO NOT USE', '250 DO NOT USE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '114 VITAMINS', '114 VITAMINS', '114 VITAMINS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '117 BULK', '910 REUSABLE BAGS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '105 FLORAL', '105 FLORAL', '105 FLORAL', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '112 CHEESE', '112 CHEESE', '112 CHEESE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '117 BULK', '117 BULK', '117 BULK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', '119 GENERAL MERCHANDISE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', '140 ALCOHOL AND SPIRITS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '118 WINE', '170 HAPPY COOK', '170 HAPPY COOK', '170 HAPPY COOK', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '121 COFFEE BAR', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '121 COFFEE BAR', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '121 COFFEE BAR', '113 COFFEE', '113 COFFEE', '113 COFFEE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '121 COFFEE BAR', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '130 JUICE BAR', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '130 JUICE BAR', '104 PRODUCE', '104 PRODUCE', '104 PRODUCE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '130 JUICE BAR', '106 DELI', '106 DELI', '106 DELI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '130 JUICE BAR', '107 SUSHI', '107 SUSHI', '107 SUSHI', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '130 JUICE BAR', '110 FROZEN', '110 FROZEN', '110 FROZEN', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '130 JUICE BAR', '111 BAKERY', '111 BAKERY', '111 BAKERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '130 JUICE BAR', '121 COFFEE BAR', '121 COFFEE BAR', '121 COFFEE BAR', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '140 ALCOHOL AND SPIRITS', '103 BEER', '103 BEER', '103 BEER', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '140 ALCOHOL AND SPIRITS', '118 WINE', '118 WINE', '118 WINE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '920 FRONT END', '101 GROCERY', '101 GROCERY', '101 GROCERY', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '920 FRONT END', '115 BODY CARE', '115 BODY CARE', '115 BODY CARE', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '920 FRONT END', '116 PUBLICATIONS', '116 PUBLICATIONS', '116 PUBLICATIONS', @result OUTPUT; PRINT @result; SET @result = NULL;
EXEC dbo.STA_MAHierarchyBackout '920 FRONT END', '910 REUSABLE BAGS', '910 REUSABLE BAGS', '910 REUSABLE BAGS', @result OUTPUT; PRINT @result; SET @result = NULL;

GO

-- These already existed, but just need to be renamed...
-- 106 DELI > '110 BOT. DEP. / RETURN' > '110 BOT. DEP. / RETURN' > '110 BOT. DEP. / RETURN'

UPDATE ItemCategory
SET Category_Name = '110 BOT. DEP. / RETURN'
FROM ItemCategory 
INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
WHERE SubTeam_Name = '106 DELI'
AND Category_Name LIKE '110%'

UPDATE PRODHIERARCHYLEVEL3
SET [DESCRIPTION] = '110 BOT. DEP. / RETURN'
FROM PRODHIERARCHYLEVEL3 
INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
WHERE SubTeam_Name = '106 DELI'
AND Category_Name LIKE '110%'
AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '110%'

UPDATE PRODHIERARCHYLEVEL4
SET [DESCRIPTION] = '110 BOT. DEP. / RETURN'
FROM PRODHIERARCHYLEVEL4
INNER JOIN PRODHIERARCHYLEVEL3 ON PRODHIERARCHYLEVEL3.PRODHIERARCHYLEVEL3_ID = PRODHIERARCHYLEVEL4.PRODHIERARCHYLEVEL3_ID
INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
WHERE SubTeam_Name = '106 DELI'
AND Category_Name LIKE '110%'
AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '110%'
AND PRODHIERARCHYLEVEL4.[DESCRIPTION] LIKE '110%'

-- UPDATE HIERARCHY... '115 BODY CARE' > '101 BODY CARE COUPON's > '101 BODY CARE COUPON' > '101 BODY CARE COUPON'
UPDATE ItemCategory
SET Category_Name = '101 BODY CARE COUPON'
FROM ItemCategory 
INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
WHERE SubTeam_Name = '115 BODY CARE'
AND Category_Name LIKE '101%'

UPDATE PRODHIERARCHYLEVEL3
SET [DESCRIPTION] = '101 BODY CARE COUPON'
FROM PRODHIERARCHYLEVEL3 
INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
WHERE SubTeam_Name = '115 BODY CARE'
AND Category_Name LIKE '101%'
AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '101%'

UPDATE PRODHIERARCHYLEVEL4
SET [DESCRIPTION] = '101 BODY CARE COUPON'
FROM PRODHIERARCHYLEVEL4
INNER JOIN PRODHIERARCHYLEVEL3 ON PRODHIERARCHYLEVEL3.PRODHIERARCHYLEVEL3_ID = PRODHIERARCHYLEVEL4.PRODHIERARCHYLEVEL3_ID
INNER JOIN ItemCategory ON PRODHIERARCHYLEVEL3.Category_ID = ItemCategory.Category_ID
INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No
WHERE SubTeam_Name = '115 BODY CARE'
AND Category_Name LIKE '101%'
AND PRODHIERARCHYLEVEL3.[DESCRIPTION] LIKE '101%'
AND PRODHIERARCHYLEVEL4.[DESCRIPTION] LIKE '101%'
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'STA_MAHierarchyBackout')
	DROP PROCEDURE STA_MAHierarchyBackout
GO
SET NOCOUNT OFF
GO