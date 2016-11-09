﻿CREATE PROCEDURE [irma].[ETL_Populate_ItemUnit] @Environment NCHAR(3), @region NCHAR(2) AS BEGIN

	DECLARE @LinkedServer NVARCHAR(128)
	DECLARE @DB NVARCHAR(128) 
	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @table_name sysname
	DECLARE @runtime_start DATETIME;

	SET @table_name = N'ItemUnit'
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
			, Unit_ID, Unit_Name, Weight_Unit, User_ID, Unit_Abbreviation, UnitSysCode, IsPackageUnit, PlumUnitAbbr, EDISysCode, LastUpdateTimestamp
		)
		SELECT    ' + QUOTENAME(@Region,'''') + '
			, Unit_ID, Unit_Name, Weight_Unit, User_ID, Unit_Abbreviation, UnitSysCode, IsPackageUnit, PlumUnitAbbr, EDISysCode, LastUpdateTimestamp

		FROM ' + QUOTENAME(@LinkedServer) + '.' + QUOTENAME(@DB) + '.[dbo].' + QUOTENAME(@table_Name)

	EXECUTE sp_executesql @sqlText, @sqlParams;

	PRINT '   ...Elapsed time: ' + CONVERT(NVARCHAR(255), DATEDIFF(SECOND, @runtime_start, getdate()), 120) + 's';

END