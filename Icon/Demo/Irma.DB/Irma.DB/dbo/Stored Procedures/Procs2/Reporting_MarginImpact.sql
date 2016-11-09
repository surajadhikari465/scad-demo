CREATE PROCEDURE dbo.Reporting_MarginImpact
    @SubTeam_No int = NULL,
    @Category_ID int = NULL,
    @Vendor varchar(50) = NULL,
    @Vendor_ID varchar(20) = NULL,
    @Item_Description varchar(60) = NULL,
    @Identifier varchar(13) = NULL,
    @Discontinue_Item int = NULL,
    @WFM_Item int = NULL,
    @Not_Available int = NULL,
    @HFM_Item tinyint = NULL,
    @IncludeDeletedItems tinyint = NULL,
    @Brand_ID int = NULL,
	@DistSubTeam_No int = NULL,
	@LinkCodeID int = NULL,
    @ProdHierarchyLevel3_ID int = NULL,
    @ProdHierarchyLevel4_ID int = NULL,
    @Chain_ID int = NULL,
    @Vendor_PS varchar(40) = NULL,
	@Store_No int = NULL,
	@CompetitivePriceTypeID int = NULL,
	@CompetitorStoreIDs varchar(500) = NULL,
	@FiscalYear int,
	@FiscalPeriod int,
	@PeriodWeek int
AS 
BEGIN

DECLARE @SQL varchar(MAX)
DECLARE @CompetitorSelectSQL varchar(500)
DECLARE @CompetitorFromSQL varchar(500)
DECLARE @StartDate smalldatetime
DECLARE @EndDate smalldatetime

SET @CompetitorSelectSQL = ''
SET @CompetitorFromSQL = ''

SELECT
	@StartDate = StartDate,
	@EndDate = EndDate
FROM
	FiscalWeek
WHERE
	FiscalYear = @FiscalYear
	AND
	FiscalPeriod = @FiscalPeriod
	AND
	PeriodWeek = @PeriodWeek

IF @Discontinue_Item IS NULL
	SET @Discontinue_Item = 0
IF @WFM_Item IS NULL
	SET @WFM_Item = 1
IF @Not_Available IS NULL
	SET @Not_Available = 0
IF @HFM_Item IS NULL
	SET @HFM_Item = 1
IF @IncludeDeletedItems IS NULL
	SET @IncludeDeletedItems = 0

-- First get the price search results for the given criteria
SET @SQL = 'DECLARE @Prices AS TABLE (Item_Key int,
	Store_No int,
	Store_Name varchar(50), 
	SubTeam_Name varchar(50),
	Category_Name varchar(50),
	Brand_Name varchar(50),
	Identifier varchar(13),
	Item_Description varchar(60),
	FIACategory varchar(50),
	Multiple tinyint,
	Price smallmoney,
	CompetitivePriceTypeID int,
	BandwidthPercentageHigh tinyint,
	BandwidthPercentageLow tinyint) 
INSERT INTO @Prices 
	EXEC [dbo].[GetPriceSearch] '

	IF @SubTeam_No IS NOT NULL
		SET @SQL = @SQL + '@SubTeam_No = ' + CONVERT(varchar(20), @SubTeam_No) + ', '

	IF @Category_ID IS NOT NULL
		SET @SQL = @SQL + '@Category_ID = ' + CONVERT(varchar(20), @Category_ID) + ', '

	IF @Vendor IS NOT NULL
		SET @SQL = @SQL + '@Vendor = ''' + @Vendor + ''', '

	IF @Vendor_ID IS NOT NULL
		SET @SQL = @SQL + '@Vendor_ID = ''' + @Vendor_ID + ''', '

	IF @Item_Description IS NOT NULL
		SET @SQL = @SQL + '@Item_Description = ''' + @Item_Description + ''', '

	IF @Identifier IS NOT NULL
		SET @SQL = @SQL + '@Identifier = ''' + @Identifier + ''', '

	SET @SQL = @SQL + '@Discontinue_Item = ' + CONVERT(varchar(20), @Discontinue_Item) + ', 
		@WFM_Item = ' + CONVERT(varchar(20), @WFM_Item) + ',
		@Not_Available = ' + CONVERT(varchar(20), @Not_Available) + ',
		@HFM_Item = ' + CONVERT(varchar(20), @HFM_Item) + ',
		@IncludeDeletedItems = ' + CONVERT(varchar(20), @IncludeDeletedItems) + ', '

	IF @Brand_ID IS NOT NULL
		SET @SQL = @SQL + '@Brand_ID = ' + CONVERT(varchar(20), @Brand_ID) + ', '

    IF @DistSubTeam_No IS NOT NULL
		SET @SQL = @SQL + '@DistSubTeam_No = ' + CONVERT(varchar(20), @DistSubTeam_No) + ', '

	IF @LinkCodeID IS NOT NULL
		SET @SQL = @SQL + '@LinkCodeID = ' + CONVERT(varchar(20), @LinkCodeID) + ', '

	IF @ProdHierarchyLevel3_ID IS NOT NULL
		SET @SQL = @SQL + '@ProdHierarchyLevel3_ID = ' + CONVERT(varchar(20), @ProdHierarchyLevel3_ID) + ', '

	IF @ProdHierarchyLevel4_ID IS NOT NULL
		SET @SQL = @SQL + '@ProdHierarchyLevel4_ID = ' + CONVERT(varchar(20), @ProdHierarchyLevel4_ID) + ', '

	IF @Chain_ID IS NOT NULL
		SET @SQL = @SQL + '@Chain_ID = ' + CONVERT(varchar(20), @Chain_ID) + ', '

	IF @Vendor_PS IS NOT NULL
		SET @SQL = @SQL + '@Vendor_PS = ''' + @Vendor_PS + ''', '

	IF @Store_No IS NOT NULL
		SET @SQL = @SQL + '@Store_No = ' + CONVERT(varchar(20), @Store_No) + ', '

	IF @CompetitivePriceTypeID IS NOT NULL
		SET @SQL = @SQL + '@CompetitivePriceTypeID = ' + CONVERT(varchar(20), @CompetitivePriceTypeID) + ', '

	IF @Store_No IS NULL
		SET @SQL = @SQL + '@ItemSearch = 1 '

-- Remove trailing comma and space
IF RIGHT(@SQL, 2) = ', '
	SET @SQL = SUBSTRING(@SQL, 0, LEN(@SQL)) + ' '

-- Create the SQL Related to the dynamic set of competitor stores
DECLARE @CSIDs TABLE (CompetitorStoreID int)

IF @CompetitorStoreIDs IS NOT NULL
BEGIN
	INSERT INTO @CSIDs
		(CompetitorStoreID)
		SELECT Key_Value FROM fn_Parse_List(@CompetitorStoreIDs, ',') X1

	IF (SELECT COUNT(*) FROM @CSIDs) > 1
	BEGIN
		DECLARE @CompetitorStoreID int
		DECLARE @CSString varchar(20)
		DECLARE @ColumnName varchar(20)
		DECLARE @FirstColumnExpression varchar(100)
		DECLARE @Index int
		DECLARE CSCursor CURSOR FOR
			SELECT CompetitorStoreID FROM @CSIDs

		SET @Index = 1

		OPEN CSCursor
		FETCH NEXT FROM CSCursor INTO @CompetitorStoreID
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @CSString = '[' + CONVERT(varchar(20), @CompetitorStoreID) + ']'
			SET @ColumnName = 'C' + CONVERT(varchar(20), @Index)

			IF @Index = 1
			BEGIN
				SET @FirstColumnExpression = @CSString
			END
			ELSE
			BEGIN
				SET @FirstColumnExpression = 'ISNULL(' + @FirstColumnExpression + ', ' + @CSString + ') '
				SET @CompetitorSelectSQL = @CompetitorSelectSQL + @CSString + ' as ' + @ColumnName + ', '
			END

			SET @CompetitorFromSQL = @CompetitorFromSQL + @CSString + ','
			SET @Index = @Index + 1

			FETCH NEXT FROM CSCursor INTO @CompetitorStoreID
		END
		CLOSE CSCursor
		DEALLOCATE CSCursor

		SET @CompetitorSelectSQL = @FirstColumnExpression + ' AS C1, ' + @CompetitorSelectSQL
		SET @CompetitorSelectSQL = SUBSTRING(@CompetitorSelectSQL, 0, LEN(@CompetitorSelectSQL))
		SET @CompetitorFromSQL = SUBSTRING(@CompetitorFromSQL, 0, LEN(@CompetitorFromSQL))
	END
	ELSE
	BEGIN
		SET @CompetitorFromSQL = '[' + CONVERT(varchar(20), (SELECT CompetitorStoreID FROM @CSIDs)) + '] '
		SET @CompetitorSelectSQL = @CompetitorFromSQL + ' AS C1 '
	END
END


-- Now get all of the related info for the report

SET @SQL = @SQL + 
'SELECT
	I.SubTeam_No,
	ST.SubTeam_Name,
	I.Item_key,
	P.Identifier,
	I.Item_Description,
	I.Package_Desc2,
	IU.Unit_Abbreviation,
	P.Price,
	P.CompetitivePriceTypeID,
	CASE P.BandwidthPercentageHigh WHEN 0 THEN 0 ELSE CONVERT(decimal, P.BandwidthPercentageHigh) / 100 END AS BandwidthPercentageHigh,
	CASE P.BandwidthPercentageLow WHEN 0 THEN 0 ELSE CONVERT(decimal, P.BandwidthPercentageLow) / 100 END AS BandwidthPercentageLow,
	CPT.Description AS CPTDescription, '

IF @Store_No IS NOT NULL
BEGIN
	SET @SQL = @SQL +
	'ISNULL(STS.SubTeam_Sales, 0) AS SubTeam_Sales,
	SUM(Sales_Quantity) AS Quantity,
	SUM(Sales_Amount) AS Amount '
END
ELSE
BEGIN
	SET @SQL = @SQL +
	'0 AS SubTeam_Sales,
	0 AS Quantity,
	0 AS Amount '
END

IF @CompetitorStoreIDs IS NULL
BEGIN
	-- Only WFM Store
	SET @SQL = @SQL + 
	'FROM
		@Prices P
		INNER JOIN Item I (nolock) ON P.Item_Key = I.Item_Key
		INNER JOIN SubTeam ST (nolock) ON I.SubTeam_No = ST.SubTeam_No
		INNER JOIN ItemUnit IU (nolock) ON I.Package_Unit_ID = IU.Unit_ID
		LEFT OUTER JOIN 
			(SELECT SubTeam_No, SUM(Sales_Amount) AS SubTeam_Sales
				FROM
					Sales_SumBySubDept
				WHERE
					Store_No = ' + CONVERT(varchar(20), @Store_No) + '
					AND
					Date_Key BETWEEN ''' + CONVERT(varchar(20), @StartDate) + ''' AND ''' + CONVERT(varchar(20), @EndDate) + '''
				GROUP BY
					SubTeam_No) STS ON ST.SubTeam_No = STS.SubTeam_No
		LEFT OUTER JOIN CompetitivePriceType CPT (nolock) ON P.CompetitivePriceTypeID = CPT.CompetitivePriceTypeID
		LEFT OUTER JOIN Sales_SumByItem SBI ON I.Item_Key = SBI.Item_Key
			AND SBI.Date_Key BETWEEN ''' + CONVERT(varchar(20), @StartDate) + ''' AND ''' + CONVERT(varchar(20), @EndDate) + '''
	GROUP BY
		I.SubTeam_No,
		ST.SubTeam_Name,
		I.Item_key,
		P.Identifier,
		I.Item_Description,
		I.Package_Desc2,
		IU.Unit_Abbreviation,
		P.Price,
		P.CompetitivePriceTypeID,
		P.BandwidthPercentageHigh,
		P.BandwidthPercentageLow,
		CPT.Description,
		STS.SubTeam_Sales
	ORDER BY 
		I.SubTeam_No,
		I.Item_key'
END
ELSE
BEGIN
	SET @SQL = @SQL +
	', ' + @CompetitorSelectSQL +
	' FROM 
		(	SELECT 
				CP.Item_key,
				CP.CompetitorStoreID,
				ISNULL(CP.Sale, CP.Price) AS CPrice,
				ISNULL(P.Identifier,
					(SELECT TOP 1 Identifier FROM ItemIdentifier WHERE ItemIdentifier.Item_Key = CP.Item_Key ORDER BY Default_Identifier DESC)) AS [Identifier],
				P.Price,
				P.CompetitivePriceTypeID,
				P.BandwidthPercentageHigh,
				P.BandwidthPercentageLow
			FROM '

	IF @Store_No IS NULL
	BEGIN
		-- Only CompetitorStores
		SET @SQL = @SQL + 
				'CompetitorPrice CP (nolock) 
				INNER JOIN @Prices P ON CP.Item_Key = P.Item_Key '
	END
	ELSE
	BEGIN
		-- Both WFM Store and Competitor Stores
		SET @SQL = @SQL + 
				'@Prices P 
				LEFT OUTER JOIN CompetitorPrice CP (nolock) ON CP.Item_Key = P.Item_Key '
	END

	SET @SQL = @SQL + 
			'WHERE
			CP.FiscalYear = ' + CONVERT(varchar(20), @FiscalYear) + '
			AND
			CP.FiscalPeriod = ' + CONVERT(varchar(20), @FiscalPeriod) + '
			AND
			CP.PeriodWeek = ' + CONVERT(varchar(20), @PeriodWeek) + '
			AND
			CP.CompetitorStoreID IN (' + @CompetitorStoreIDs + ')	) CompetitorPivot
		PIVOT ( AVG(CPrice) FOR CompetitorStoreID IN (' + @CompetitorFromSQL + ') ) as P
		INNER JOIN Item I (nolock) ON P.Item_Key = I.Item_Key
		INNER JOIN SubTeam ST (nolock) ON I.SubTeam_No = ST.SubTeam_No
		INNER JOIN ItemUnit IU (nolock) ON I.Package_Unit_ID = IU.Unit_ID '
	
	IF @Store_No IS NOT NULL
	BEGIN
		SET @SQL = @SQL + 
		'LEFT OUTER JOIN 
			(SELECT SubTeam_No, SUM(Sales_Amount) AS SubTeam_Sales
				FROM
					Sales_SumBySubDept
				WHERE
					Store_No = ' + CONVERT(varchar(20), @Store_No) + '
					AND
					Date_Key BETWEEN ''' + CONVERT(varchar(20), @StartDate) + ''' AND ''' + CONVERT(varchar(20), @EndDate) + '''
				GROUP BY
					SubTeam_No) STS ON ST.SubTeam_No = STS.SubTeam_No '
	END

	SET @SQL = @SQL + 
		'LEFT OUTER JOIN CompetitivePriceType CPT (nolock) ON P.CompetitivePriceTypeID = CPT.CompetitivePriceTypeID
		LEFT OUTER JOIN Sales_SumByItem SBI ON I.Item_Key = SBI.Item_Key
			AND SBI.Date_Key BETWEEN ''' + CONVERT(varchar(20), @StartDate) + ''' AND ''' + CONVERT(varchar(20), @EndDate) + '''
	GROUP BY
		I.SubTeam_No,
		ST.SubTeam_Name,
		I.Item_key,
		P.Identifier,
		I.Item_Description,
		I.Package_Desc2,
		IU.Unit_Abbreviation,
		P.Price,
		P.CompetitivePriceTypeID,
		P.BandwidthPercentageHigh,
		P.BandwidthPercentageLow, 
		CPT.Description, '

	IF @Store_No IS NOT NULL
	BEGIN
		SET @SQL = @SQL + 'STS.SubTeam_Sales, ' 
	END

	SET @SQL = @SQL + @CompetitorFromSQL + 
	' ORDER BY 
		I.SubTeam_No,
		I.Item_Description'
END

EXEC(@SQL)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_MarginImpact] TO [IRMAReportsRole]
    AS [dbo];

