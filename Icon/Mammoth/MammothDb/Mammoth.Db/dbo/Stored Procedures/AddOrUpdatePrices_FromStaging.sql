CREATE PROCEDURE [dbo].[AddOrUpdatePrices_FromStaging]
	@Region NCHAR(2),
	@Timestamp DATETIME
AS
BEGIN

	-- =========================================
	-- Locale Variables
	-- =========================================
	DECLARE @region_local NCHAR(2);
	DECLARE @timestamp_local DATETIME;
	DECLARE @SQL NVARCHAR(MAX);

	SET @region_local = @Region;
	SET @timestamp_local = @Timestamp;

	-- =========================================
	-- Main
	-- =========================================
	PRINT 'Updating dbo.Price_' + @region_local + 'table'
	SET @SQL = '
		MERGE
			dbo.Price_' + @region_local + ' WITH (updlock, rowlock) p
		USING
			(
				SELECT
					' + QUOTENAME(@region_local, '''') + ' as Region,
					i.ItemID,
					sp.BusinessUnitId,
					sp.StartDate,
					sp.Price
				FROM
					Staging.dbo.Price	sp
					JOIN Items			i	on stg.ScanCode = i.ScanCode 
				WHERE
					sp.Region = ' + QUOTENAME(@region_local,'''') + '
					AND sp.Timestamp = ''' + CONVERT(nvarchar, @timestamp_local, 121) + '''
			) s
		ON
			p.ItemID = s.ItemID
			AND p.BusinessUnitID = s.BusinessUnitID
			AND p.StartDate = s.StartDate
		WHEN MATCHED THEN
			UPDATE
			SET
				p.Price = s.Price,
				p.ModifiedDate = ''' + CONVERT(nvarchar, @timestamp_local, 121) + '''
		WHEN NOT MATCHED THEN
			INSERT
			(
				ItemID,
				BusinessUnitID,
				StartDate,
				Price,
				AddedDate
			)
			VALUES
			(
				s.ItemID,
				s.BusinessUnitId,
				s.StartDate,
				s.Price,
				''' + CONVERT(nvarchar, @timestamp_local, 121) + '''
			);'

	PRINT @SQL
	EXEC sp_executesql @SQL
END