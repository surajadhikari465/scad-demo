
		CREATE    PROCEDURE [dbo].[SingleStoreReport]
			@Region VARCHAR(5) ,
			@startdate DATETIME ,
			@enddate DATETIME ,
			@StoreId INT ,
			@TeamIds VARCHAR(MAX) ,
			@SubTeamIds VARCHAR(MAX) ,
			@Debug INT ,
			@TestUPC VARCHAR(13)
		AS 
			BEGIN
	
				DECLARE @LogMsg VARCHAR(255)
				DECLARE @createIndexes BIT
				DECLARE @days INT

		-- ## SET FLAGS
				SET @createIndexes = 1;
	
		-- ## SET DEFAULTS
			--SET @Region = 'NC';
			--SET @startdate = '02/4/2014';
			--SET @enddate = '02/5/2014';
			--SET @startdate = '03/17/2014';
			--SET @enddate = '03/26/2014';
	

				IF @enddate > GETDATE() 
					SET @days = DATEDIFF(d, @startdate, GETDATE())
		
				ELSE 
					SET @days = DATEDIFF(d, @startdate, @enddate)
  
            


				SELECT  @LogMsg = 'Region:' + @Region 
				IF @debug = 1 
					RAISERROR(@LogMsg,0,1) WITH NOWAIT


				SELECT  @LogMsg = 'StartDate:' + CAST(@startdate AS VARCHAR(100))
				IF @debug = 1 
					RAISERROR(@LogMsg,0,1) WITH NOWAIT


				SELECT  @LogMsg = 'EndDate:' + CAST(@enddate AS VARCHAR(100))
				IF @debug = 1 
					RAISERROR(@LogMsg,0,1) WITH NOWAIT

			
				SELECT  @LogMsg = 'Days:' + CAST(@days AS VARCHAR(100))
				IF @debug = 1 
					RAISERROR(@LogMsg,0,1) WITH NOWAIT


				CREATE TABLE #itemdata
					(
					  id INT ,
					  upc VARCHAR(25) ,
					  StoreAbbr VARCHAR(10) ,
					  EFF_Cost DECIMAL(18, 3) ,
					  EFF_Price DECIMAL(18, 3) ,
					  Movement DECIMAL(9, 3) ,
					  LastDateSold DATETIME ,
					  Team VARCHAR(50) ,
					  SubTeam VARCHAR(50) ,
					  MatchTeam BIT, 
					  MatchSubTeam BIT,
					  IsActive BIT
					)


		--CREATE TABLE #data
		--    (
		--      id INT ,
		--      Store_Name VARCHAR(50) ,
		--      Store_Abbreviation VARCHAR(10) ,
		--      UPC VARCHAR(25) ,
		--      PS_Team VARCHAR(50) ,
		--      PS_SubTeam VARCHAR(50) ,
		--      EFF_Cost DECIMAL(18, 3) ,
		--      EFF_Price DECIMAL(18, 3) ,
		--      EFF_PriceType VARCHAR(5) ,
		--      Notes VARCHAR(2000) ,
		--      Vendor_Key VARCHAR(10) ,
		--      VIN VARCHAR(25) ,
		--      Brand VARCHAR(50) ,
		--      Brand_Name VARCHAR(50) ,
		--      Long_Description VARCHAR(50) ,
		--      Item_Size VARCHAR(10) ,
		--      Item_UOM VARCHAR(5) ,
		--      Category_Name VARCHAR(50) ,
		--      Class_Name VARCHAR(50) ,
		--      TimesScaned INT
		--    )

		---- ## if no stores are passed in default to ALL stores.
		--IF @StoreIds IS NULL
		--    OR LEN(@StoreIds) <= 1 
		--    BEGIN
		--        SELECT  @LogMsg = 'No StoreIds passed as parameters. Defaulting to all OPEN stores for ['
		--                + @Region + ']'
		--        IF @debug = 1 
		--            RAISERROR(@LogMsg,0,1) WITH NOWAIT

		--        SELECT  @StoreIds = STUFF(( SELECT  ',' + CAST(Id AS VARCHAR(10)) AS [text()]
		--                                    FROM    ( SELECT DISTINCT
		--                                                        s.id
		--                                              FROM      dbo.STORE s
		--                                                        INNER JOIN dbo.REGION r ON s.REGION_ID = r.ID
		--                                                        INNER JOIN dbo.STATUS st ON s.STATUS_ID = st.ID
		--                                                              AND r.REGION_ABBR = @Region
		--                                                              AND st.STATUS IN ('OPEN','SOON')
		--                                            ) x
		--                                  FOR
		--                                    XML PATH('')
		--                                  ), 1, 1, '')
    
		--    END
  

		--SELECT  @LogMsg = @storeIds;
		--IF @debug = 1 
		--    RAISERROR(@LogMsg,0,1) WITH NOWAIT




		--DECLARE @StoresTable TABLE
		--    (
		--      StoreId INT PRIMARY KEY
		--    ) 

				DECLARE @TeamsTable TABLE
					(
					  TeamId VARCHAR(50) PRIMARY KEY
					) 


				DECLARE @SubTeamsTable TABLE
					(
					  SubTeamId VARCHAR(50) PRIMARY KEY
					) 




		

				INSERT  INTO #itemdata
						( id ,
						  upc ,
						  StoreAbbr ,
						  EFF_Cost ,
						  EFF_Price ,
						  Movement ,
						  LastDateSold ,
						  team ,
						  SubTeam ,
						  MatchSubTeam,
						  MatchTeam,
						  IsActive
						)
						SELECT  rd.id ,
								rd.upc ,
								s.STORE_ABBREVIATION ,
								rd.EFF_COST ,
								rd.EFF_PRICE ,
								rd.MOVEMENT ,
								LASTDATEOFSALES ,
								PS_TEAM ,
								PS_SUBTEAM ,
								0,
								0,
								0
						FROM    dbo.REPORT_HEADER rh
								INNER JOIN dbo.REPORT_DETAIL rd ON rh.id = rd.REPORT_HEADER_ID
								INNER JOIN store s ON rh.STORE_ID = s.ID
								INNER JOIN region r ON s.REGION_ID = r.ID
						WHERE   rh.OffsetCorrectedCreateDate >= @startdate
								AND rh.OffsetCorrectedCreateDate <= @enddate
								AND r.REGION_ABBR = @region
								AND rh.EXCLUDE_FLAG IS NULL
								AND s.id = @storeid
								AND ( @TestUPC IS NULL
									  OR upc = @TestUPC
									)

				BEGIN TRY 


					IF @teamIds IS NOT NULL 
						BEGIN
							INSERT  INTO @teamstable
									SELECT  *
									FROM    dbo.fn_CSVToTable(@teamIds)
						END
		
				END TRY
				BEGIN CATCH


					SELECT  @LogMsg = 'Unable to parse Teams: ' + @TeamIds
					IF @debug = 1 
						RAISERROR(@LogMsg,0,1) WITH NOWAIT

					SELECT  @LogMsg = 'Defaulting to all teams'
					IF @debug = 1 
						RAISERROR(@LogMsg,0,1) WITH NOWAIT

				END CATCH

				BEGIN TRY 


					IF @subTeamIds IS NOT NULL 
						BEGIN
							INSERT  INTO @SubTeamsTable
									SELECT  *
									FROM    dbo.fn_CSVToTable(@subTeamIds)
						END
		
				END TRY
				BEGIN CATCH


					SELECT  @LogMsg = 'Unable to parse SubTeams: ' + @subTeamIds
					IF @debug = 1 
						RAISERROR(@LogMsg,0,1) WITH NOWAIT

					SELECT  @LogMsg = 'Defaulting to all subteams'
					IF @debug = 1 
						RAISERROR(@LogMsg,0,1) WITH NOWAIT

				END CATCH




				IF EXISTS ( SELECT  1
							FROM    @teamstable ) 
					BEGIN
						PRINT 'teams'
						UPDATE  #itemdata
						SET     MatchTeam = 1
						FROM    ( SELECT    d.id
								  FROM      #itemdata d
											INNER JOIN @teamstable t ON d.team = t.teamid
								) p1
						WHERE   p1.id = #itemdata.id 

					END
				ELSE 
					BEGIN
						UPDATE  #itemdata
						SET     MatchTeam = 1          
					END          






				IF EXISTS ( SELECT  1
							FROM    @SubTeamsTable ) 
					BEGIN
					PRINT 'subteams'
						UPDATE  #itemdata
						SET     MatchSubTeam = 1
						FROM    ( SELECT    d.id
								  FROM      #itemdata d
											INNER JOIN @SubTeamsTable st ON d.subteam = st.subteamid
								) p1
						WHERE   p1.id = #itemdata.id 

					END 
					ELSE
					BEGIN
							UPDATE #itemdata SET MatchSubTeam = 1          
					END          
        

		               
          
				  UPDATE    #itemdata
				  SET       isactive = 1
				  WHERE     matchsubteam = 1
							AND matchteam = 1

  
			--	SELECT * FROM #itemdata
 
				IF @createIndexes = 1 
					CREATE INDEX IX_ItemData_Stores ON #itemdata(upc, storeabbr,isactive) 


					IF @debug = 1 
					SELECT * FROM #itemdata

		---- ## Get a comma delimited list of Stores for each UPC
		--CREATE TABLE #StoresByUPC ( upc VARCHAR(25) , StoreList VARCHAR(MAX) )
 


		--INSERT INTO #StoresByUPC
		--        ( upc, StoreList )
		--SELECT  upc ,
		--        STUFF(( SELECT DISTINCT
		--                        ',' + id2.StoreAbbr
		--                FROM    #itemdata id2
		--                WHERE   id.upc = id2.upc
		--                ORDER BY ',' + id2.StoreAbbr
		--              FOR
		--                XML PATH('')
		--              ), 1, 1, '') AS StoreList
		--FROM    #itemdata id
		--GROUP BY upc

		--IF @createIndexes = 1  
		--CREATE INDEX IX_StoresByUPC_UPC ON #StoresByUPC(upc) INCLUDE (StoreList)
    




		-- ## Get a count of scans for eac upc
				CREATE TABLE #CountByUPC
					(
					  upc VARCHAR(25) ,
					  CountByUPC INT
					)

				INSERT  INTO #CountByUPC
						( upc ,
						  CountByUPC 
						)
						SELECT  upc ,
								COUNT(upc) AS CountByUPC
						FROM    #itemdata
						WHERE   isactive = 1
						GROUP BY upc

				IF @createIndexes = 1 
					CREATE INDEX IX_CountByUPC_UPC ON #CountByUPC(upc) INCLUDE (CountByUPC)

       

		-- ## Find most recent Report_Detail record for each UPC
				CREATE TABLE #MostRecentIdByUPC
					(
					  UPC VARCHAR(25) ,
					  MostRecentId INT ,
					)

				INSERT  INTO #MostRecentIdByUPC
						( UPC ,
						  MostRecentId 
						)
						SELECT  upc ,
								id
						FROM    ( SELECT    upc ,
											id ,
											RANK() OVER ( PARTITION BY upc ORDER BY id DESC ) AS rk
								  FROM      #itemdata
								  WHERE     isactive = 1
								) part1
						WHERE   part1.rk = 1


				IF @createIndexes = 1 
					CREATE INDEX IX_MostRecentIdByUPC_UPC ON #MostRecentIdByUPC(upc) INCLUDE (MostRecentId)



				SET @LogMsg = 'updating null or zero values'
				IF @debug = 1 
					RAISERROR(@LogMsg,0,1) WITH NOWAIT

				UPDATE  #itemdata
				SET     EFF_Cost = 0
				WHERE   ( EFF_Cost IS NULL
					  
						);


				UPDATE  #itemdata
				SET     eff_price = 0
				WHERE   ( eff_price IS NULL
					  
						);


				UPDATE  #itemdata
				SET     Movement = 0
				WHERE   ( Movement IS NULL
					  
						);


				SET @LogMsg = 'updates done'
				IF @debug = 1 
					RAISERROR(@LogMsg,0,1) WITH NOWAIT


				CREATE TABLE #AveragesByUPCandStore
					(
					  UPC VARCHAR(25) ,
					  StoreAbbr VARCHAR(10) ,
					  AvgEffCost DECIMAL(18, 3) ,
					  AvgEffPrice DECIMAL(18, 3) ,
					  AvgMovement DECIMAL(18, 3) ,
					  SalesByStore DECIMAL(18, 3) ,
					  LastDateSold DATETIME
					)

			

				INSERT  INTO #AveragesByUPCandStore
						( UPC ,
						  StoreAbbr ,
						  AvgEffCost ,
						  AvgEffPrice ,
						  AvgMovement ,
						  SalesByStore,
						  LastDateSold
						)
						SELECT  upc ,
								StoreAbbr ,
								AvgEffCost = AVG(EFF_Cost) ,
								AvgEffPrice = AVG(EFF_Price) ,
								AvgMovement = AVG(Movement) ,
								salesByStore = SUM(Movement * EFF_Price),
								LastDateSold = MAX(lastDateSOld)
								
						FROM    #itemdata
						WHERE   isactive = 1
						GROUP BY upc ,
								StoreAbbr


	IF @debug = 1 
	SELECT * FROM #AveragesByUPCandStore

				IF @createIndexes = 1 
					CREATE INDEX IX_AveragesByUPCandStore_UPCStore ON #AveragesByUPCandStore(upc, StoreAbbr) 

				SELECT  upc ,
						StoreAbbr ,
						CumulativeSalesOpportunity = SUM(salesByStore) ,
						Movement = SUM(AvgMovement) ,
						AvgPrice = AVG(AvgEffPrice) ,
						AvgCost = AVG(AvgEffCost) ,
						LastDateSold = MAX(LastDateSold)
				INTO    #CumulativeSalesData
				FROM    #AveragesByUPCandStore
				GROUP BY UPC ,
						StoreAbbr

						IF @debug =1
						SELECT * FROM  #CumulativeSalesData


		   --     SELECT  recent.upc ,
		   --             detail.PS_SUBTEAM ,
		   --             detail.PS_TEAM ,
		   --             detail.BRAND ,
		   --             detail.BRAND_NAME ,
		   --             detail.LONG_DESCRIPTION ,
		   --             detail.ITEM_SIZE ,
		   --             detail.ITEM_UOM ,
		   --             detail.VENDOR_KEY ,
		   --             detail.VIN ,
		   --             TimesScanned = cbu.CountByUPC ,
		   --             Notes = '' ,
		   --             Cost = csd.AvgCost ,
		   --             Price = csd.AvgPrice ,
		   --             Margin = CASE WHEN csd.AvgPrice = 0
		   --                                OR detail.CASE_SIZE = 0 THEN 0
		   --                           ELSE ( ( csd.AvgPrice - ( csd.AvgCost
		   --                                                     / detail.CASE_SIZE ) )
		   --                                  / csd.AvgPrice ) * 100
		   --                      END ,
		   --             Sales = csd.CumulativeSalesOpportunity * @days ,
		   --             AvgSalesOpportinity = csd.Movement * csd.AvgPrice ,
		   --             AvgUnitOpportinity = csd.Movement ,
		   --             detail.CASE_SIZE
		   --     FROM    #itemdata d
		   --             INNER JOIN #MostRecentIdByUPC recent ON d.id = recent.MostRecentId
		   --             INNER JOIN dbo.REPORT_DETAIL detail ON recent.MostRecentId = detail.ID
		   --             INNER JOIN #CumulativeSalesData csd ON csd.upc = detail.upc
		   --                                                    AND csd.storeabbr = d.storeabbr
		   --             INNER JOIN #CountByUPC cbu ON cbu.upc = detail.UPC
					--WHERE d.isactive = 1



				SELECT  rd.PS_SUBTEAM ,
						r.UPC ,
						rd.brand ,
						rd.BRAND_NAME ,
						rd.LONG_DESCRIPTION ,
						rd.ITEM_SIZE ,
						rd.ITEM_UOM ,
						rd.VENDOR_KEY ,
						rd.VIN ,
						Sales = cs.CumulativeSalesOpportunity ,
						times_scanned = cnt.CountByUPC ,
						StoresList = 'N/A' ,
						rd.notes ,
						Cost =	 CASE WHEN  ISNULL(rd.CASE_SIZE, 0) = 0
						THEN cs.Movement * ISNULL(cs.AvgCost, 0.0)
						ELSE 
						cs.Movement * (ISNULL(cs.AvgCost, 0.0)  / ISNULL(rd.CASE_SIZE, 1))
											   END,
						Margin = CASE WHEN ISNULL(cs.AvgPrice,0) = 0
										   OR ISNULL(rd.CASE_SIZE, 0) = 0 THEN 0
									  ELSE ( ( cs.AvgPrice - ( cs.AvgCost
															   / rd.CASE_SIZE ) )
											 / cs.AvgPrice ) * 100
								 END ,
						Case_Size = ISNULL(rd.CASE_SIZE, 0) ,
						Eff_Price = ISNULL(cs.AvgPrice, 0) ,
						Eff_Cost = ISNULL(cs.AvgCost, 0) ,
						rd.EFF_PRICETYPE ,
						rd.CATEGORY_NAME ,
						rd.CLASS_NAME ,
						Avg_daily_Units = cs.Movement ,
						Avg_Mov_Sales = cs.Movement * cs.AvgPrice, 
				--Product_Status ='',
						LAST_DATE_SOLD = rd.LASTDATEOFSALES ,
						DAYS_WITH_SALES = rd.DAYSOFMOVEMENT
				INTO    #ItemDetail
				FROM    #MostRecentIdByUPC r
						INNER JOIN #itemdata i ON r.MostRecentId = i.id
						INNER JOIN dbo.REPORT_DETAIL rd ON i.id = rd.ID
						INNER JOIN #CountByUPC cnt ON r.upc = cnt.upc
						INNER JOIN #CumulativeSalesData cs ON cs.upc = rd.upc
															  AND cs.storeabbr = i.storeabbr
				WHERE   i.isactive = 1



				SELECT  id.UPC ,
						Region ,
						ps.ProductStatus
				INTO    #productstatus
				FROM    dbo.ProductStatus ps
						INNER JOIN #ItemDetail id ON ps.UPC = id.UPC
													 AND ps.Region = @Region
				WHERE   ps.ExpirationDate IS NULL
						OR ps.ExpirationDate > GETDATE() 


				--CREATE CLUSTERED INDEX ix_productstatus_upc ON #productstatus (upc)
		
		

				SELECT  id.PS_SUBTEAM ,
						id.UPC ,
						id.BRAND ,
						id.BRAND_NAME ,
						id.LONG_DESCRIPTION ,
						id.ITEM_SIZE ,
						id.ITEM_UOM ,
						id.VENDOR_KEY ,
						id.VIN ,
						Sales=ROUND(id.Sales,2) ,
						id.times_scanned ,
						id.StoresList ,
						id.NOTES ,
						Cost=ROUND(id.Cost,2) ,
						margin= ROUND(id.margin,0) ,
						id.Case_Size ,
						id.Eff_Price ,
						id.Eff_Cost ,
						id.EFF_PRICETYPE ,
						id.CATEGORY_NAME ,
						id.CLASS_NAME ,
						Avg_daily_Units = ROUND(id.Avg_daily_Units,2) ,
						Avg_Mov_Sales=ROUND(id.Avg_Mov_Sales,2) ,
						id.LAST_DATE_SOLD ,
						id.DAYS_WITH_SALES ,
						Product_Status = ps.ProductStatus
				FROM    #ItemDetail id
						LEFT JOIN #productstatus ps ON id.UPC = ps.UPC
				ORDER BY sales DESC 


		


		--SELECT MAX(part1.case_size) AS case_size, AVG(eff_price) AS eff_price, AVG(eff_Cost) AS eff_Cost, SUM(movement) AS movement, MAX(LASTdatesold),
		--Cost = AVG(ROUND(CASE WHEN ISNULL(CASE_SIZE,0) = 0 THEN 0 ELSE MOVEMENT * ( EFF_COST / CASE_SIZE ) END,2)),
		--Margin = AVG(CASE WHEN (EFF_PRICE = 0 OR CASE_SIZE =0) THEN 0 ELSE ((EFF_PRICE - (EFF_COST/CASE_SIZE) ) / EFF_PRICE) * 100 END) ,
		--avgUnitOpportunity = avg(ROUND(MOVEMENT,2)),
		--avgSalesOpportunity = AVG(ROUND(movement * Eff_Price,2)),
		--Sales=avg(ISNULL(CumulativeSalesOpportunity,0) * @days),
		--ProductStatus = '',
		--MAX(timesScanned),
		--upc
		-- FROM ( 

		--SELECT  r.upc ,
		--       -- rd.PS_TEAM ,
		--        --rd.PS_SUBTEAM ,
		--        ISNULL(rd.CASE_SIZE, 0) Case_Size ,
		--        ISNULL(cs.AvgPrice, 0) Eff_Price ,
		--        ISNULL(cs.AvgCost, 0) Eff_Cost ,
		--        --rd.EFF_PRICETYPE ,
		--        --rd.notes ,
		--        --rd.VENDOR_KEY ,
		--        --rd.vin ,
		--        --rd.brand ,
		--        --rd.BRAND_NAME ,
		--        --rd.LONG_DESCRIPTION ,
		--        --rd.ITEM_SIZE ,
		--        --rd.ITEM_UOM ,
		--        --rd.CATEGORY_NAME ,
		--        --rd.CLASS_NAME ,
		--        cnt.CountByUPC timesScanned ,
		--        --s.StoreList ,
		--        cs.CumulativeSalesOpportunity ,
		--        cs.LastDateSold
		--FROM    #MostRecentIdByUPC r
		--        INNER JOIN #itemdata i ON r.MostRecentId = i.id
		--        INNER JOIN dbo.REPORT_DETAIL rd ON i.id = rd.ID
		--        INNER JOIN #CountByUPC cnt ON r.upc = cnt.upc
		--        INNER JOIN #CumulativeSalesData cs ON r.upc = cs.upc
		--        --INNER JOIN #StoresByUPC s ON r.UPC = s.upc
		--		) part1
		--		GROUP BY part1.UPC
		

		--DROP TABLE #data
				DROP TABLE #itemdata
		--DROP TABLE #StoresByUPC
				DROP TABLE #CountByUPC
				DROP TABLE #MostRecentIdByUPC
				DROP TABLE #AveragesByUPCandStore
				DROP TABLE #CumulativeSalesData
				DROP TABLE #ItemDetail
				DROP TABLE #productstatus


			END