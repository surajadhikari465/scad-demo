CREATE PROCEDURE [dbo].[UpdateItemHistoryFromSalesSumByItem]
@ProcessDate AS DATETIME = NULL

AS
-- Edit History
-- 12/18/2015	DN							Added a filter to include only R10 stores
-- 01/25/2016	DN	Bug 18204 (PBI: 13935)	Updated the condition to @TotalStore > 0
-- 01/29/2016	DN	Bug 18204 (PBI: 13935)	Added a condition to match only transactions with Adjustment_ID = 3,9,10
-- 06/27/2016   JA	TFS (PBI: 16582) Refactored the code by removing the loop and batching the deletes and using a temp table for the inserts.
-- 07/05/2016   MZ  TFS 20090 (PBI: 16582)  Reduced the transaction scope, and saved resultsets of a couple of function calls into temp tables.

BEGIN
	DECLARE @Counter INT = 0
	DECLARE @StoreCountInStoresList INT
	DECLARE @TotalStore INT
	DECLARE @CurrentStore INT

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	IF @ProcessDate IS NULL
		SET @ProcessDate = (SELECT CONVERT(DATE,DATEADD(D,-1,GETDATE())))

	IF Object_id('tempdb..#StoresList') IS NOT NULL
	    DROP TABLE #StoresList
	IF Object_id('tempdb..#ItemHistory') IS NOT NULL
		DROP TABLE #ItemHistory
	IF Object_id('tempdb..#subquery') IS NOT NULL
		DROP TABLE #subquery
	IF Object_id('tempdb..#SSBI1') IS NOT NULL
		DROP TABLE #SSBI1
	IF Object_id('tempdb..#SalesSummary') IS NOT NULL
		DROP TABLE #SalesSummary
      
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
	FROM Store s
	INNER JOIN StorePOSConfig spc
	ON s.Store_No = spc.Store_No
	INNER JOIN POSWriter pos
	ON spc.POSFileWriterKey = pos.POSFileWriterKey
	WHERE (s.Mega_Store = 1 OR s.WFM_Store = 1)
		  AND dbo.fn_GetCustomerType(s.Store_No, s.Internal, s.BusinessUnit_ID) = 3 -- Regional    
		  AND pos.POSFileWriterCode = 'R10'
	
	CREATE INDEX IDXStoreIndex on #StoresList(Store_No, ProcessDate)
	
	SELECT @StoreCountInStoresList = SUM(ROW_COUNT) 
	FROM TEMPDB.sys.dm_db_partition_stats
	WHERE OBJECT_ID = OBJECT_ID('tempdb..#StoresList')    
	AND (index_id=0 or index_id=1);
	
	BEGIN TRY
	
		IF @StoreCountInStoresList > 0
			BEGIN

			CREATE TABLE #SSBI1(
				[Date_Key] [smalldatetime] NOT NULL,
				[Store_No] [int] NOT NULL,
				[Item_Key] [int] NOT NULL,
				[SubTeam_No] [int] NOT NULL,
				[Price_Level] [tinyint] NOT NULL,
				[Sales_Quantity] [int] NULL,
				[Return_Quantity] [int] NULL,
				[Weight] [decimal](10, 3) NULL,
				[Sales_Amount] [decimal](9, 2) NULL,
				[Return_Amount] [decimal](9, 2) NULL,
				[Markdown_Amount] [decimal](9, 2) NULL,
				[Promotion_Amount] [decimal](9, 2) NULL,
				[Store_Coupon_Amount] [decimal](9, 2) NULL,
				[UnitPrice] [decimal](9,2) Null,
				[SalesAmount] [decimal] (9,2) Null
			 )

			INSERT INTO #SSBI1		
			SELECT ssbi1.*,
					Isnull(dbo.Fn_pricehistoryprice(ssbi1.item_key,
							ssbi1.store_no,
							ssbi1.date_key),
					dbo.Fn_price(dbo.Fn_onsale(p1.pricechgtypeid),
					p1.multiple,
					p1.price,
					p1.pricingmethod_id,
					p1.sale_multiple, p1.sale_price)) AS UnitPrice,
					ssbi1.sales_amount - ssbi1.return_amount - ssbi1.markdown_amount - ssbi1.promotion_amount AS SalesAmount
			
			FROM   sales_sumbyitem ssbi1
			JOIN #StoresList sl ON ssbi1.Store_No = sl.Store_No 
			                   AND sl.ProcessDate = ssbi1.Date_Key
 LEFT OUTER JOIN price p1 ON p1.item_key = ssbi1.item_key
					     AND p1.store_no = ssbi1.store_no		

			CREATE INDEX IDX_SSBI ON #SSBI1(store_no, item_key)
			
			CREATE TABLE #subquery(
			[Store_No] [int] NOT NULL,
			[Item_Key] [int] NOT NULL,
			[runDate] DATE NOT Null,
			[weight_unit] bit NOT Null, 
			[costedbyweight] bit NOT Null,
			[Weight] [decimal](10, 3) NULL,
			[CreatedBy] int NOT Null,
			[SubTeam_No] [int] NOT NULL,
			[Amount] [decimal](9, 2) NULL,
			[ItemSalesQty] [decimal] (9,2) Null,
			[ExtCost] [smallmoney] NULL
			)

			INSERT INTO #subquery ( [Store_No],
									[Item_Key],
									[runDate],
									[weight_unit], 
									[costedbyweight],
									[Weight],
									[CreatedBy],
									[SubTeam_No],
									[Amount],
									[ItemSalesQty],
									[ExtCost])
			SELECT ssbi.store_no,
					ssbi.item_key,
					ssbi.Date_Key,
					iu.weight_unit,
					i.costedbyweight,
					Sum(CASE
							WHEN Isnull(iu.weight_unit, 0) > 0 THEN ssbi.weight
							ELSE
							CASE
								WHEN i.costedbyweight = 1 THEN
								ssbi.sales_quantity *
								dbo.Fn_getaverageunitweightbyitemkey(i.item_key)
								ELSE 0
							END
						END),
					0,
					ssbi.subteam_no,
					Sum(ssbi.sales_amount - ssbi.return_amount -
						ssbi.markdown_amount - ssbi.promotion_amount),
					Sum(dbo.Fn_ItemSalesQty2(i.item_key, 
											iu.weight_unit,
											ssbi.price_level,
											ssbi.sales_quantity, 
											ssbi.return_quantity, 
											Cast(dbo.Fn_getvendorpacksize(i.item_key, siv.vendor_id, ssbi.store_no, Getdate()) AS DECIMAL(9, 4)), 
											i.package_desc2, 
											ssbi.weight, 
											ssbi.salesamount,
											ssbi.unitprice)),
					Isnull(dbo.Fn_avgcosthistory(ssbi.item_key, 
												ssbi.store_no,
												ssbi.subteam_no,
												Getdate()),	0)
					FROM   #SSBI1 ssbi
						INNER JOIN item i 
								ON i.item_key = ssbi.item_key
						INNER JOIN itemunit iu 
								ON iu.unit_id = i.retail_unit_id
						INNER JOIN itemidentifier ii 
								ON ii.item_key = i.item_key
									AND ii.default_identifier = 1
						INNER JOIN price p 
								ON p.item_key = ssbi.item_key
									AND p.store_no = ssbi.store_no
						INNER JOIN storeitemvendor siv 
								ON siv.item_key = ssbi.item_key
									AND siv.primaryvendor = 1
									AND siv.store_no = ssbi.store_no
						INNER JOIN  #StoresList sl
						        ON sl.Store_No = ssbi.Store_No
									AND sl.ProcessDate = ssbi.Date_Key
				GROUP  BY ssbi.item_key,
							ssbi.subteam_no,
							ssbi.store_no,
							ssbi.Date_Key,
							iu.weight_unit,
							i.costedbyweight

				CREATE INDEX IDX_subquery ON #subquery(ItemSalesQty)

				CREATE TABLE #SalesSummary(
				[Store_No] [int] NOT NULL,
				[Item_Key] [int] NOT NULL,
				[runDate] DATE NOT Null,
				[Quantity] [decimal] (9,2) Null,
				[Weight] [decimal](10, 3) NULL,
				[Adjustment_ID] int NOT Null,
				[CreatedBy] int NOT Null,
				[SubTeam_No] [int] NOT NULL,
				[ExtCost] [smallmoney] NULL,
				[Retail] [smallmoney] NULL		
				)

				INSERT INTO #SalesSummary(
					[Store_No],
					[Item_Key],
					[runDate],
					[Quantity],
					[Weight],
					[Adjustment_ID],
					[CreatedBy],
					[SubTeam_No],
					[ExtCost],
					[Retail]		
				)
				SELECT store_no,
					   item_key,
					   rundate,
					   CASE
						 WHEN Isnull(weight_unit, 0) = 0
							  AND costedbyweight = 0 THEN ItemSalesQty
						 ELSE 0
					   END ,
					   weight,
					   3,
					   createdby,
					   subteam_no,
					   extcost,
					   amount / ItemSalesQty 
				FROM   #subquery subquery
				WHERE  ItemSalesQty <> 0

				CREATE INDEX IDX_SalesSummary ON #SalesSummary(rundate, store_no)

				SELECT @TotalStore = COUNT(ih.Store_no)
				FROM   itemhistory ih
				JOIN   #StoresList sl
				ON     ih.Store_no = sl.Store_No 							   
				WHERE  adjustment_id IN ( 3, 9, 10 )
				AND datestamp >=sl.ProcessDate 
				AND datestamp < Dateadd(day, 1, sl.ProcessDate)
				
				WHILE (@TotalStore > 0)
				BEGIN

					DELETE TOP(50000) itemhistory
					FROM   itemhistory ih
					JOIN   #StoresList sl
					ON     ih.Store_no = sl.Store_No 							   
					WHERE  adjustment_id IN ( 3, 9, 10 )
					AND datestamp >=sl.ProcessDate 
					AND datestamp < Dateadd(day, 1, sl.ProcessDate)
							
					SELECT @TotalStore = COUNT(ih.Store_no)
					FROM   itemhistory ih
					JOIN   #StoresList sl
					ON     ih.Store_no = sl.Store_No 							   
					WHERE  adjustment_id IN ( 3, 9, 10 )
					AND datestamp >=sl.ProcessDate 
					AND datestamp < Dateadd(day, 1, sl.ProcessDate)							
		        END  

				CREATE TABLE #ItemHistory(
					[Store_No] [int] NOT NULL,
					[Item_Key] [int] NOT NULL,
					[DateStamp] [datetime] NOT NULL,
					[Quantity] [decimal](18, 4) NULL,
					[Weight] [decimal](18, 4) NULL,
					[ExtCost] [smallmoney] NULL,
					[Retail] [smallmoney] NULL,
					[Adjustment_ID] [int] NOT NULL,
					[CreatedBy] [int] NOT NULL,
					[SubTeam_No] [int] NOT NULL					
				)

				INSERT INTO #ItemHistory (
					Store_No, 
					Item_Key,
					DateStamp,
					Quantity,
					Weight,
					Adjustment_ID,
					CreatedBy,
					Subteam_No,
					ExtCost,
					Retail)
				SELECT						
					ssbi.Store_No, 
					ssbi.Item_Key,
					RunDate,
					ssbi.Quantity,
					ssbi.Weight,
					ssbi.Adjustment_ID,
					ssbi.CreatedBy,
					ssbi.Subteam_No,
					ssbi.ExtCost,
					ssbi.Retail
				FROM #SalesSummary as ssbi
				GROUP BY 
					RunDate,
					ssbi.Store_No,
					ssbi.Item_Key,
					ssbi.Subteam_No,
					ssbi.Adjustment_ID,
					ssbi.CreatedBy,
					ssbi.Quantity,
					ssbi.Weight,
					ssbi.ExtCost,
					ssbi.Retail

			BEGIN TRANSACTION

				INSERT INTO ItemHistory 
						([Store_No]
					   ,[Item_Key]
					   ,[DateStamp]
					   ,[Quantity]
					   ,[Weight]          
					   ,[Adjustment_ID]           
					   ,[CreatedBy]
					   ,[SubTeam_No]          
					   ,[ExtCost]
					   ,[Retail])
					SELECT						 
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
					FROM #ItemHistory
							
				DELETE TlogReprocessRequest
				FROM TlogReprocessRequest tl
				JOIN #StoresList sl
				ON   tl.Store_No = sl.Store_No
				WHERE Date_Key = sl.ProcessDate 					

			COMMIT TRANSACTION

		END
		 
		IF Object_id('tempdb..#ItemHistory') IS NOT NULL
			DROP TABLE #ItemHistory
		IF Object_id('tempdb..#StoresList') IS NOT NULL
			DROP TABLE #StoresList
		IF Object_id('tempdb..#subquery') IS NOT NULL
			DROP TABLE #subquery
		IF Object_id('tempdb..#SSBI1') IS NOT NULL
			DROP TABLE #SSBI1
		IF Object_id('tempdb..#SalesSummary') IS NOT NULL
		    DROP TABLE #SalesSummary	 

	END TRY
	
	BEGIN CATCH
		Print 'Roll back transaction'
		IF XACT_STATE() <> 0
			ROLLBACK TRANSACTION

		DECLARE @ErrorMessage NVARCHAR(MAX);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = '[UpdateItemHistoryFromSalesSumByItem] failed with error: ' + ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE()
		print @ErrorMessage

	END CATCH	
END