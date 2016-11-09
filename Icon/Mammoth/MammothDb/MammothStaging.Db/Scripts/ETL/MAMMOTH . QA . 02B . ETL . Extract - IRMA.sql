USE [Staging]
GO
SET NOCOUNT ON
GO

DECLARE @SQL NVARCHAR(MAX);
DECLARE @Params NVARCHAR(MAX);
DECLARE @DataSrc NVARCHAR(100);
DECLARE @Environment NVARCHAR(5);
DECLARE @Region NCHAR(2);
DECLARE @ProjectName NVARCHAR(255);
DECLARE @ScriptName NVARCHAR(255);
DECLARE @Msg NVARCHAR(255);
DECLARE @MsgWidth INT;
DECLARE @TableName NVARCHAR(128);
DECLARE @Operator sysname;
DECLARE @DataSource sysname;
DECLARE @runtime_start DATETIME;
DECLARE @now DATETIME;
DECLARE @SpecifiedRegions table 
(
	Region nvarchar(2)
)

SET @runtime_start = getdate();
SET @ProjectName = 'MAMMOTH';
SET @ScriptName = '02B. ETL. Extract - IRMA.sql';
SELECT @Operator = SUSER_NAME();
SET @MsgWidth = 80;

PRINT 'Project: ' + @ProjectName;
PRINT 'Environment: ' + @Environment;
PRINT 'Script: ' + @ScriptName;
PRINT REPLICATE('-', @MsgWidth);
PRINT 'Data sources: [QA - IRMA ()] = XXX';

SET @Environment = 'QA';

INSERT INTO @SpecifiedRegions 
VALUES ('');

DECLARE @InvalidRegions nvarchar(50)
SELECT 
	@InvalidRegions =  Region + '|' + @InvalidRegions
FROM 
	@SpecifiedRegions r
WHERE NOT EXISTS
(
	SELECT 1 FROM Staging.etl.Datasources WHERE Environment = @Environment AND Region = r.Region
)

IF LEN(@InvalidRegions) > 0 BEGIN
	PRINT 'ERROR: Specified regions ' + @InvalidRegions + ' are not part of the ' + @Environment + ' environment!'

	RETURN;

END

BEGIN -- Populate static tables

	SET @Msg = 'Populating static tables';
	SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg));
	PRINT @Msg;

	BEGIN -- Populate irma.currency
		SET @Msg = 'Populating irma.Currency';
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg));
		PRINT @Msg;
	
		TRUNCATE TABLE Staging.irma.Currency

		INSERT INTO Staging.irma.Currency (CurrencyCode, CurrencyName) VALUES
			  ('USD', 'US Dollar')
			, ('CAD', 'Canadian Dollar')
			, ('GBP', 'Pound Sterling')

	END -- Populate irma.currency

END -- Populate static tables

BEGIN -- Populate regional tables

	SET @Msg = 'Populating regional tables';
	SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg));
	PRINT @Msg;

	DECLARE cRegion CURSOR FAST_FORWARD FOR
	SELECT Region
	FROM Staging.etl.DataSources
	WHERE Environment = @Environment
	ORDER BY Region;

	OPEN cRegion
	FETCH NEXT FROM cRegion INTO @Region;

	WHILE @@FETCH_STATUS = 0 BEGIN
	
		IF EXISTS (SELECT * FROM @SpecifiedRegions regions WHERE regions.Region = @Region)
		BEGIN
			
			PRINT 'Populating regional tables for region ' + @Region

			EXECUTE Staging.irma.ETL_Populate_Item @Environment = @Environment, @region = @Region;
			EXECUTE Staging.irma.ETL_Populate_ItemIdentifier @Environment = @Environment, @region = @Region;
			EXECUTE Staging.irma.ETL_Populate_ItemOverride @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_ItemUnit @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_LabelType @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_Price @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_PriceBatchDetail @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_PriceChgType @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_Store @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_StoreJurisdiction @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_ValidatedScanCode @Environment = @Environment, @region = @region;
			EXECUTE Staging.irma.ETL_Populate_ItemOrigin @Environment = @Environment, @region = @region
			EXECUTE Staging.irma.ETL_Populate_ItemScale @Environment = @Environment, @region = @region
			EXECUTE Staging.irma.ETL_Populate_ItemScaleOverride @Environment = @Environment, @region = @region
			EXECUTE Staging.irma.ETL_Populate_ItemSignAttribute @Environment = @Environment, @region = @region
			EXECUTE Staging.irma.ETL_Populate_ScaleExtraText @Environment = @Environment, @region = @region
			EXECUTE Staging.irma.ETL_Populate_StoreItem @Environment = @Environment, @region = @region
			EXECUTE Staging.irma.ETL_Populate_StoreItemVendor @Environment = @Environment, @region = @Region
		END

		FETCH NEXT FROM cRegion INTO @Region;

	END

	CLOSE cRegion

	DEALLOCATE cRegion

END -- Populate regional tables

	PRINT 'IRMA Staging complete'

