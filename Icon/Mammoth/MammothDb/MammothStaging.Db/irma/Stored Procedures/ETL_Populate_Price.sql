CREATE PROCEDURE [irma].[ETL_Populate_Price] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start datetime;

	SET @table_name = N'Price'
	SET @sqlParams = N''
	SET @runtime_start = getdate();

	SELECT @LinkedServer = LinkedServer 
		, @DB = [Database]
	FROM [etl].DataSources
	WHERE Environment = @Environment 
	AND Region = @region

	PRINT '...Populating [irma]. ' + QUOTENAME(@table_name) + ' (' + @Region + ')';

	SET @sqlText = N'DELETE FROM irma.' + QUOTENAME(@table_name) + ' WHERE Region = @iRegion'
	SET @sqlParams = N'@iRegion NCHAR(2)'

	EXECUTE sp_executesql @sqlText, @sqlParams, @region;

	SET @sqlParams = N''
	SET @sqlText = 'INSERT INTO [irma].' + QUOTENAME(@table_name) + ' ( 
			Region
			, Item_Key
			, Store_No
			, Price
			, Age_Restrict
			, CompFlag
			, Discountable
			, IBM_Discount
			, Multiple
			, NotAuthorizedForSale
			, POSPrice
			, Restricted_Hours
			, Sale_End_Date
			, Sale_Multiple
			, Sale_Start_Date
			, Sale_Price
			, SrCitizenDiscount
			, PriceChgTypeId
			, MSRPPrice
			, AgeCode
			, ElectronicShelfTag
			, LinkedItem
			, LocalItem
		)
		SELECT ' + QUOTENAME(@Region, '''') + '
			, i.Item_Key
			, i.Store_no
			, i.Price
			, i.Age_Restrict
			, i.CompFlag
			, i.Discountable
			, i.IBM_Discount
			, i.Multiple
			, i.NotAuthorizedForSale
			, i.POSPrice
			, i.Restricted_Hours
			, i.Sale_End_Date
			, i.Sale_Multiple
			, i.Sale_Start_Date
			, i.Sale_Price
			, i.SrCitizenDiscount
			, i.PriceChgTypeId
			, i.MSRPPrice
			, i.AgeCode
			, i.ElectronicShelfTag
			, i.LinkedItem
			, i.LocalItem
		FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_name) + ' i
		INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].Store s ON (i.Store_No = s.Store_No)
		INNER JOIN ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].StoreRegionMapping srm ON (s.Store_No = srm.Store_No)
		WHERE
			(s.WFM_Store = 1 OR s.Mega_Store = 1 )
			AND (srm.Region_Code = ' + QUOTENAME(@Region,'''') + ')
            AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
';

	PRINT @sqlText;
	EXECUTE sp_executesql @sqlText, @sqlParams;

	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END