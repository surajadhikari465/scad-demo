CREATE PROCEDURE [irma].[ETL_Populate_ItemOverride] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start datetime;

	SET @table_name = N'ItemOverride'
	SET @sqlParams = N''
	SET @runtime_start = getdate();

	SELECT @LinkedServer = LinkedServer 
		, @DB = [Database]
	FROM [etl].DataSources
	WHERE Environment = @Environment 
	AND Region = @region

	PRINT '...Populating [irma]. ' + QUOTENAME(@table_name) + ' (' + @Region + ')';

	SET @sqlText = 'DELETE FROM irma.' + QUOTENAME(@table_name) + ' WHERE Region = @iRegion'
	SET @sqlParams = N'@iRegion NCHAR(2)'

	EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @region

	SET @sqlParams = N''
	SET @sqlText = '
		INSERT INTO [irma].' + QUOTENAME(@table_name) + ' ( 
			  Region  
			, Item_Key, StoreJurisdictionID
			, Item_Description, Sign_Description, Package_Desc1, Package_Desc2, Package_Unit_ID
			, Retail_Unit_ID, Vendor_Unit_ID, Distribution_Unit_ID, POS_Description
			, Food_Stamps, Price_Required, Quantity_Required, Manufacturing_Unit_ID, QtyProhibit
			, GroupList, Case_Discount, Coupon_Multiplier, Misc_Transaction_Sale, Misc_Transaction_Refund
			, Ice_Tare, Brand_ID, Origin_ID, CountryProc_ID, SustainabilityRankingRequired, SustainabilityRankingID, LabelType_ID
			, CostedByWeight, Average_Unit_Weight, Ingredient, Recall_Flag, LockAuth, Not_Available, Not_AvailableNote, FSA_Eligible
			, Product_Code, Unit_Price_Category, LastModifiedUser_ID
		)
		SELECT    ' + QUOTENAME(@Region,'''') + '
			, i.Item_Key, i.StoreJurisdictionID
			, i.Item_Description, i.Sign_Description, i.Package_Desc1, i.Package_Desc2, i.Package_Unit_ID
			, i.Retail_Unit_ID, i.Vendor_Unit_ID, i.Distribution_Unit_ID, i.POS_Description
			, i.Food_Stamps, i.Price_Required, i.Quantity_Required, i.Manufacturing_Unit_ID, i.QtyProhibit
			, i.GroupList, i.Case_Discount, i.Coupon_Multiplier, i.Misc_Transaction_Sale, i.Misc_Transaction_Refund
			, i.Ice_Tare, i.Brand_ID, i.Origin_ID, i.CountryProc_ID, i.SustainabilityRankingRequired, i.SustainabilityRankingID, i.LabelType_ID
			, i.CostedByWeight, i.Average_Unit_Weight, i.Ingredient, i.Recall_Flag, i.LockAuth, i.Not_Available, i.Not_AvailableNote, i.FSA_Eligible
			, i.Product_Code, i.Unit_Price_Category, i.LastModifiedUser_ID

		FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_Name) + ' i'

	EXECUTE sp_executesql @sqlText, @sqlParams;

	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END