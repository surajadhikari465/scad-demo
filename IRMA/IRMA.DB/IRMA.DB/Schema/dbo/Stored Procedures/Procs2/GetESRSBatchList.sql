CREATE PROCEDURE [dbo].[GetESRSBatchList]
		@PriceBatchStatusID		INT = 0,
		@BatchDescription		VARCHAR(255) = '',
		@StartDate				VARCHAR(10) = '',
		@Store_No				INT = 0,
		/* New VARS added 3/28/2008 */
		@StartDateRangeTop		VARCHAR(10) = '',
		@StartDateRangeBottom	VARCHAR(10) = '',
		@PriceChgTypeID			VARCHAR(10) = ''
AS

BEGIN
	/* Modified by Bryce Bartley on 3/28/2008 */
    SET NOCOUNT ON
	SELECT @BatchDescription = RTRIM(@BatchDescription)
	DECLARE @SQL varchar(2000)
				
	SELECT @SQL = '
	SELECT DISTINCT
		PD.Store_No,
		PD.SubTeam_No,
		PD.PriceBatchHeaderID,
		PH.PriceBatchStatusID,
		PH.ItemChgTypeID,
		PD.PriceChgTypeID,
		PH.BatchDescription,
		PH.StartDate,
		PH.SentDate,
		PH.PrintedDate,
		PH.ProcessedDate,
		PH.ApplyDate,
		PBS.PriceBatchStatusDesc,
		PCT.PriceChgTypeDesc,
		PCT.On_Sale,
		PCT.MSRP_Required,
		PCT.LineDrive
	FROM
		PriceBatchDetail (nolock) PD
	INNER JOIN
		PriceBatchHeader (nolock) PH
	ON
		PD.PriceBatchHeaderID = PH.PriceBatchHeaderID
	INNER JOIN
		PriceBatchStatus (nolock) PBS
	ON
		PH.PriceBatchStatusID = PBS.PriceBatchStatusID
	INNER JOIN
		PriceChgType (nolock) PCT
	ON
		PH.PriceChgTypeID = PCT.PriceChgTypeID
	'
	IF @Store_No <> 0
		BEGIN
			SELECT @SQL = @SQL + 'WHERE PD.Store_No = ' + CONVERT(VARCHAR(20), @Store_No)
		END
	ELSE
		BEGIN
			SELECT @SQL = @SQL + 'WHERE 1=1 '
		END
	IF (@BatchDescription <> '') SELECT @SQL = @SQL + 'AND PH.BatchDescription LIKE ''%' + @BatchDescription + '%'' '
	IF (@PriceBatchStatusID <> 0) SELECT @SQL = @SQL + ' AND PH.PriceBatchStatusID = ' + CONVERT(VARCHAR(20), @PriceBatchStatusID)
	IF (@PriceChgTypeID <> '') SELECT @SQL = @SQL + ' AND PH.PriceChgTypeID = ' + @PriceChgTypeID
	IF @StartDate <> '' 
		BEGIN
			IF @StartDate <> ''
				BEGIN
					SELECT @SQL = @SQL + ' AND PH.StartDate = ''' + @StartDate	+ ''''		
				END
		END
	ELSE
		BEGIN
			IF @StartDateRangeTop <> '' AND @StartDateRangeBottom <> ''
				BEGIN
					SELECT @SQL = @SQL + ' AND PH.StartDate >= ''' + @StartDateRangeTop	+ ''''	
					SELECT @SQL = @SQL + ' AND PH.StartDate <= ''' + @StartDateRangeBottom	+ ''''
				END
		END
	EXECUTE(@SQL)	
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetESRSBatchList] TO [IRMAClientRole]
    AS [dbo];

