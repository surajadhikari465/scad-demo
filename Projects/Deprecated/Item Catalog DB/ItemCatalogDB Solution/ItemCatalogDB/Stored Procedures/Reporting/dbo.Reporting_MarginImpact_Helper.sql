if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_MarginImpact_Helper]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_MarginImpact_Helper]
GO

CREATE PROCEDURE dbo.Reporting_MarginImpact_Helper
		@CompetitorStoreIDs varchar(500) = NULL
AS
BEGIN

IF @CompetitorStoreIDs IS NOT NULL
BEGIN
	-- Create the SQL Related to the dynamic set of competitor stores
	DECLARE @SQL varchar(MAX)
	DECLARE @PivotInSQL varchar(250)
	DECLARE @CSIDs TABLE (CompetitorStoreID int, Priority int identity(1,1))
	DECLARE @CompetitorStoreID int
	DECLARE @Index int
	
	INSERT INTO @CSIDs
		(CompetitorStoreID)
		SELECT TOP 3 Key_Value FROM fn_Parse_List(@CompetitorStoreIDs, ',') X1

	SET @Index = 1
	SET @SQL = 'SELECT '
	SET @PivotInSQL = ''

	WHILE @Index <= 3
	BEGIN
		SET @CompetitorStoreID = NULL

		SELECT @CompetitorStoreID = CompetitorStoreID
			FROM @CSIDs
			WHERE Priority = @Index
	
		IF @CompetitorStoreID IS NULL
		BEGIN
			SET @SQL = @SQL + 'NULL'
		END
		ELSE
		BEGIN
			SET @SQL = @SQL + '[' + CONVERT(varchar(20), @CompetitorStoreID) + ']'
			SET @PivotInSQL = @PivotInSQL + '[' + CONVERT(varchar(20), @CompetitorStoreID) + '], '
		END
	
		SET @SQL = @SQL + ' AS C' + CONVERT(varchar(20), @Index) + ', '
		
		SET @Index = @Index + 1
	END
	
	-- Strip the trailing ', '
	SET @SQL = SUBSTRING(@SQL, 0, LEN(@SQL))
	SET @PivotInSQL = SUBSTRING(@PivotInSQL, 0, LEN(@PivotInSQL))
	
	SET @SQL = @SQL + ' FROM
		(SELECT
			CS.CompetitorStoreID,
			C.Name + '' - '' + CL.Name + '' - '' + CS.Name AS Name
			FROM 
			CompetitorStore CS
			INNER JOIN Competitor C ON CS.CompetitorID = C.CompetitorID
			INNER JOIN CompetitorLocation CL ON CS.CompetitorLocationID = CL.CompetitorLocationID) InnerPivot
	PIVOT (MAX(Name) FOR CompetitorStoreID IN (' + @PivotInSQL + ')) AS pvt'
	
	EXEC(@SQL)
END
ELSE
BEGIN
	SELECT NULL AS C1, NULL AS C2, NULL AS C3
END

END
GO 