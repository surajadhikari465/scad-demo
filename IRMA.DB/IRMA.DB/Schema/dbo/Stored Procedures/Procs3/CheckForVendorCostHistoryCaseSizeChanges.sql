CREATE PROCEDURE CheckForVendorCostHistoryCaseSizeChanges
AS
BEGIN
	
	CREATE TABLE #VendorCostHistoryData
	  ([VendorCostHistoryID] [int] NOT NULL,
       [StoreItemVendorID] [int] NOT NULL,
       [Package_Desc1] [decimal](9, 4) NOT NULL,
       [OldPackage_Desc1] [decimal](9, 4) NULL,
	   [ItemKey] [int] NULL,
	   [StoreNo] [int] NULL,
	   [Processed] [bit] NULL
       )

	-- get current Case Size for records with start date today or with insert date yesterday and start date <today
	  INSERT INTO #VendorCostHistoryData 
           ([VendorCostHistoryID]
		   ,[StoreItemVendorID]
           ,[Package_Desc1]
           ,[OldPackage_Desc1]
		   ,[ItemKey]
		   ,[StoreNo],
		    [Processed])

       SELECT   [VendorCostHistoryID]
		       ,[StoreItemVendorID]
               ,[Package_Desc1]
			   ,Null
			   ,NULL
			   ,NULL
			   ,0
	  FROM VendorCostHistory vch
	  WHERE VendorCostHistoryID IN( SELECT MAX(VendorCostHistoryID) AS VendorCostHistoryID
									FROM VendorCostHistory vch
									JOIN StoreItemVendor siv ON vch.StoreItemVendorID = siv.StoreItemVendorID
									WHERE StartDate  = CAST(GETDATE() as DATE)
										  OR (StartDate < CAST(GETDATE() as DATE) 
											  AND InsertDate = CAST((GETDATE() - 1) as DATE)
											  )
										  AND siv.DeleteDate IS NULL
										  AND siv.PrimaryVendor = 1
									 GROUP BY vch.StoreItemVendorID)


	--- get previous day case size
		SELECT  [VendorCostHistoryID]
			   ,[StoreItemVendorID]  
			   ,[Package_Desc1]
			INTO #tmpPreviousDayVCHData
			FROM VendorCostHistory vch
			WHERE [VendorCostHistoryID] IN( SELECT MAX(vch.VendorCostHistoryID) AS VendorCostHistoryID
											FROM VendorCostHistory vch
											JOIN #VendorCostHistoryData vchmd ON vch.StoreItemVendorID = vchmd.StoreItemVendorID 
											WHERE vch.VendorCostHistoryID < vchmd.VendorCostHistoryID
											       AND StartDate <= CAST((GETDATE() - 1) as DATE)
											GROUP BY vch.StoreItemVendorID
										   )
	-- Join #VendorCostHistoryData and #tmpPreviousDayVCHData and populate old case size[OldPackage_Desc1]
			UPDATE vchData
			SET OldPackage_Desc1 = #tmpPreviousDayVCHData.Package_Desc1
			FROM #VendorCostHistoryData vchData
			INNER JOIN #tmpPreviousDayVCHData
			ON #tmpPreviousDayVCHData.StoreItemVendorID = vchData.StoreItemVendorID

			UPDATE vchData
			SET ItemKey = siv.Item_Key,
			     StoreNo = siv.Store_No
			FROM #VendorCostHistoryData vchData
			INNER JOIN StoreItemVendor siv
			ON siv.StoreItemVendorID = vchData .StoreItemVendorID 

	--see if [OldPackage_Desc1] is differenct from [Package_Desc1], if so create item locale queue

	DECLARE @Count INT = (SELECT COUNT(*) FROM #VendorCostHistoryData 
						  WHERE Processed= 0 AND ISNULL(OldPackage_Desc1,-1) <> Package_Desc1 )
	DECLARE @Item_Key INT 
	DECLARE @Store_No INT 
	DECLARE @StoreItemVendorID INT 

	WHILE (@Count > 0)
	BEGIN

		SELECT TOP 1 @Item_Key = ItemKey, @Store_No = StoreNo, @StoreItemVendorID = StoreItemVendorID
		FROM #VendorCostHistoryData  WHERE Processed = 0
	
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, @Store_No, 'ItemLocaleAddOrUpdate', NULL, NULL

		UPDATE #VendorCostHistoryData
		SET Processed = 1
		WHERE StoreItemVendorID = @StoreItemVendorID

		SET @Count = @Count -1
	END
   END