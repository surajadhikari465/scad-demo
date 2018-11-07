CREATE PROCEDURE dbo.Reporting_NotOnFile 
	@EndOfWeekDate DATETIME
AS
BEGIN

DECLARE @BeginningOfWeekDate DATETIME

SET @BeginningOfWeekDate = DATEADD(day, -7, @EndOfWeekDate)
	
	SELECT 
		DailyFrequency.Identifier,
		I.SubTeam_No,
		IB.Brand_Name,
		I.Item_Description,
		I.Package_Desc1,
		UOM.Unit_Abbreviation,
		CASE I.Deleted_Item
			WHEN 1 THEN 'Deleted'
			ELSE
				CASE ISNULL(DailyFrequency.Authorized, 0)
					WHEN 0 THEN 'Unauthorized'
					ELSE 
						ISNULL(
							(SELECT TOP 1
								ISNULL(PBS.PriceBatchStatusDesc, 'Pending - Unbatched')
							FROM
								PriceBatchDetail PBD
								LEFT OUTER JOIN PriceBatchHeader PBH (nolock) ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
								LEFT OUTER JOIN PriceBatchStatus PBS (nolock) ON PBH.PriceBatchStatusID = PBS.PriceBatchStatusID
							WHERE
								PBD.Item_Key = DailyFrequency.Item_Key
								AND 
								PBD.Store_No = DailyFrequency.Store_No
							ORDER BY
								PBH.PriceBatchStatusID), 
						'No Batch Records')
				END
			END AS Status,
		-- Count of days of the week with scans
		COUNT(DailyFrequency.Frequency) AS Frequency,
		S.StoreAbbr
	FROM
		(SELECT
			SI.Item_Key,
			SI.Store_No,
			II.Identifier,
			SI.Authorized,
			COUNT(Scan.Time_Key) AS Frequency
		FROM
			StoreItem SI
			INNER JOIN		ItemIdentifier II		(nolock) ON SI.Item_Key = II.Item_Key
			INNER JOIN		POSScan Scan			(nolock) ON SI.Store_No = Scan.Store_No AND II.Identifier = Scan.ScanCode
			LEFT OUTER JOIN	POSItem POI				(nolock) ON SI.Item_Key = POI.Item_Key AND SI.Store_No = POI.Store_No
		WHERE
			Scan.Time_Key BETWEEN @BeginningOfWeekDate AND @EndOfWeekDate
			AND
			II.Default_Identifier = 1
			AND
			POI.Item_Key IS NULL
		GROUP BY
			SI.Item_Key,
			SI.Store_No,
			SI.Authorized,
			II.Identifier,
			DatePart(weekday, Scan.Time_Key)) DailyFrequency
		INNER JOIN		Item I					(nolock) ON DailyFrequency.Item_Key = I.Item_Key
		INNER JOIN		ItemBrand IB			(nolock) ON I.Brand_ID = IB.Brand_ID
		INNER JOIN		ItemUnit UOM			(nolock) ON I.Package_Unit_ID = UOM.Unit_ID
		INNER JOIN		Store S					(nolock) ON DailyFrequency.Store_No = S.Store_No
	GROUP BY
		I.SubTeam_No,
		IB.Brand_Name,
		I.Item_Description,
		I.Package_Desc1,
		I.Deleted_Item,
		UOM.Unit_Abbreviation,
		S.StoreAbbr,
		DailyFrequency.Item_Key,
		DailyFrequency.Store_No,
		DailyFrequency.Authorized,
		DailyFrequency.Identifier
	ORDER BY
		S.StoreAbbr

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_NotOnFile] TO [IRMAReportsRole]
    AS [dbo];

