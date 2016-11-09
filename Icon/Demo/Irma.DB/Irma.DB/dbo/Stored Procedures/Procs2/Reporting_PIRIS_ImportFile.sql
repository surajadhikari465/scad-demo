----------------------------------------------
-- Imports the same PIRIS files that were used for data conversion from IRIS to IRMA
----------------------------------------------
CREATE PROCEDURE dbo.[Reporting_PIRIS_ImportFile]
    @FilePath varchar(200), 
	@FileName varchar(100)
AS

BEGIN
	-- works from local directory

	DECLARE @SQL varchar(500),
		@ServerFilePath varchar(200),
		@ServerName varchar(100),
		@XMLFormatFile varchar(50),
		@StartDateTime datetime,
		@ErrNum int,
		@RowsAffected int

    SET NOCOUNT ON

	SELECT @StartDateTime = GetDate()

	------------------------------------------------------------------------
	SELECT TOP 1 @ServerName = RTRIM([srvnetname]) 
	FROM master.dbo.sysservers
	
	SELECT @ServerFilePath = '\\' + @ServerName + '\irma$\PIRISAudit\'

	SELECT @XMLFormatFile = 'FormatFile_BEL000_SAL.xml'
	------------------------------------------------------------------------

	PRINT 'Loading file ''' + @FileName + '''...'

	IF @FilePath IS NULL
		SELECT @FilePath = @ServerFilePath

	IF OBJECT_ID('tempdb..#tblBulkLoad') IS NOT NULL
		DROP TABLE #tblBulkLoad

	-- create a temp staging table to hold the file data (all fields as text)
	CREATE TABLE #tblBulkLoad
		(
			[Store] varchar(10),
			[Barcode] varchar(14),
			[ProductCode] varchar(12),
			[Department] varchar(10),
			[Section] varchar(10),
			[SubSection] varchar(10),
			[VATCode] varchar(10),
			[LongDescription] varchar(60),
			[ItemSize] varchar(20),
			[CaseSize] varchar(10),
			[Base] varchar(10),
			[Discount] varchar(10),
			[CaseCost] varchar(10),
			[UnitPrice1] varchar(10),
			[Supplier] varchar(15),
			[Item] varchar(20)
		)

	-- add data from the file into the staging table
	SELECT @SQL = 'BULK INSERT #tblBulkLoad'
		+ ' FROM ''' + @FilePath + @FileName + ''''
		+ ' WITH ('
            + ' ERRORFILE = ''' + @ServerFilePath + 'BulkInsertErrors ' + REPLACE(CONVERT(varchar(20), @StartDateTime, 120), ':', '.') + '.txt'','
            + ' FORMATFILE=''' + @ServerFilePath + @XMLFormatFile + ''''
		+ ');'

	EXEC (@SQL)

	SELECT @ErrNum = @@ERROR, @RowsAffected = @@ROWCOUNT

	-- remove all existing rows for that store; read the 1st row of the data to determine the store
	DELETE FROM dbo.[Reporting_PIRIS_Audit]
	WHERE Store = (SELECT TOP 1 Store FROM #tblBulkLoad)

	-- add all rows from the staging table into the regular table; convert (implicit) text values to numeric values
	INSERT INTO [dbo].[Reporting_PIRIS_Audit]
           ([Store] , [Barcode], [ProductCode], [Department], [Section], [SubSection], [VATCode], [LongDescription], 
			[ItemSize], [CaseSize], [Base], [Discount], [Cost], [UnitPrice1], [Supplier], [Item])
	SELECT [Store], CAST([Barcode] AS bigint), [ProductCode], [Department], [Section], [SubSection], [VATCode], [LongDescription], 
			[ItemSize], [CaseSize], [Base], [Discount], [CaseCost], [UnitPrice1], RTRIM([Supplier]), RTRIM([Item])
	FROM #tblBulkLoad

	-- check for errors
	SELECT @ErrNum = @@ERROR, @RowsAffected = @@ROWCOUNT

	IF OBJECT_ID('tempdb..#tblBulkLoad') IS NOT NULL
		DROP TABLE #tblBulkLoad

	-- output information to the user
	IF @ErrNum = 0
		PRINT '   Rows added: ' + CAST(@RowsAffected AS varchar) + ' [' + @FileName + '] (Elapsed time: ' + CAST(DATEDIFF(second, @StartDateTime, GetDate()) AS varchar) + ' seconds)'
	ELSE
		PRINT '   Import errors occurred! [' + @FileName + '] (Elapsed time: ' + CAST(DATEDIFF(second, @StartDateTime, GetDate()) AS varchar) + ' seconds)'

	RETURN @ErrNum
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_PIRIS_ImportFile] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_PIRIS_ImportFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_PIRIS_ImportFile] TO [IRMAReportsRole]
    AS [dbo];

