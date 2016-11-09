
CREATE PROCEDURE UpdateItemHistoryFromSalesSumByItem
@ProcessDate AS DATETIME = NULL

AS
-- Edit History
-- 12/18/2015	DN							Added a filter to include only R10 stores
-- 01/25/2016	DN	Bug 18204 (PBI: 13935)	Updated the condition to @TotalStore > 0
-- 01/29/2016	DN	Bug 18204 (PBI: 13935)	Added a condition to match only transactions with Adjustment_ID = 3,9,10

BEGIN
	DECLARE @Counter INT = 0
	DECLARE @TotalStore INT
	DECLARE @CurrentStore INT

	IF @ProcessDate IS NULL
		SET @ProcessDate = (SELECT CONVERT(DATE,DATEADD(D,-1,GETDATE())))

	IF Object_id('tempdb..#StoresList') IS NOT NULL
	      DROP TABLE #StoresList
      
	CREATE TABLE #StoresList
		(
	        StoreIndex	INT IDENTITY(1,1) NOT NULL,
			ProcessDate	DATETIME,
		    Store_No	INT      
		)
      
	INSERT INTO #StoresList 
		(ProcessDate,
		 Store_No
		 )
	SELECT 
		trr.Date_Key,
		trr.Store_No 
	FROM TlogReprocessRequest trr
	UNION
	SELECT
		@ProcessDate,
		s.Store_No
	FROM Store s (NOLOCK)
	INNER JOIN StorePOSConfig spc (NOLOCK) 
	ON s.Store_No = spc.Store_No
	INNER JOIN POSWriter pos (NOLOCK)
	ON spc.POSFileWriterKey = pos.POSFileWriterKey
	WHERE (s.Mega_Store = 1 OR s.WFM_Store = 1)
		  AND dbo.fn_GetCustomerType(s.Store_No, s.Internal, s.BusinessUnit_ID) = 3 -- Regional    
		  AND pos.POSFileWriterCode = 'R10'

	SET @TotalStore = (SELECT COUNT(Store_No) FROM #StoresList)

	BEGIN TRY
		BEGIN TRANSACTION

		IF @TotalStore > 0
			BEGIN
				SET @Counter = 1
				WHILE (@Counter <= @TotalStore)
					BEGIN
						SET @CurrentStore = (SELECT Store_No FROM #StoresList s WHERE s.StoreIndex = @Counter)
						SET @ProcessDate = (SELECT ProcessDate FROM #StoresList s WHERE s.StoreIndex = @Counter)
						
						DELETE itemhistory
				        FROM   itemhistory ih
				        WHERE  adjustment_id IN ( 3, 9, 10 )
						       AND datestamp >= @ProcessDate 
							   AND datestamp < Dateadd(day, 1, @ProcessDate)
		                       AND ih.store_no = @CurrentStore 

						; -- Need this for SQL Compatibility Level 90 because MERGE is NOT recognized as a reserved word at this level, resulting in a parsing error at compat level lower than 100.
						MERGE ItemHistory AS ih
						USING
						(

						SELECT
							Store_No, 
							Item_Key,
							RunDate AS DateStamp,
							Quantity,
							Weight,
							Adjustment_ID,
							CreatedBy,
							Subteam_No,
							ExtCost,
							Retail
						FROM dbo.fn_UpdateItemHistory_GetSalesSummary(@ProcessDate, @CurrentStore)
						GROUP BY 
							RunDate,
							Store_No,
							Item_Key,
							Subteam_No,
							Adjustment_ID,
							CreatedBy,
							Quantity,
							Weight,
							ExtCost,
							Retail
						) ssbi
						ON ih.DateStamp = ssbi.DateStamp AND
						ih.Store_No = ssbi.Store_No AND 
						ih.Adjustment_ID IN (3,9,10)
		
						WHEN NOT MATCHED THEN
							INSERT (
								Store_No, 
								Item_Key,
								DateStamp,
								Quantity,
								Weight,
								Adjustment_ID,
								CreatedBy,
								Subteam_No,
								ExtCost,
								Retail
								)
							VALUES (
								ssbi.Store_No, 
								ssbi.Item_Key,
								ssbi.DateStamp,
								ssbi.Quantity,
								ssbi.Weight,
								ssbi.Adjustment_ID,
								ssbi.CreatedBy,
								ssbi.Subteam_No,
								ssbi.ExtCost,
								ssbi.Retail
								);																				
						DELETE FROM #StoresList WHERE ProcessDate = @ProcessDate AND Store_No = @CurrentStore 
						DELETE FROM TlogReprocessRequest WHERE Date_Key = @ProcessDate AND Store_No = @CurrentStore
						SET @Counter = @Counter + 1
					END
			END
	
		IF Object_id('tempdb..#StoresList') IS NOT NULL
		            DROP TABLE #StoresList
		            
	
		COMMIT TRANSACTION
	END TRY
	
	BEGIN CATCH
		IF XACT_STATE() <> 0
			ROLLBACK TRANSACTION
	END CATCH	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemHistoryFromSalesSumByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemHistoryFromSalesSumByItem] TO [IRSUser]
    AS [dbo];

