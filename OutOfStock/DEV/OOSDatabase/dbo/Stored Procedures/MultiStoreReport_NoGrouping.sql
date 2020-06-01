

CREATE PROCEDURE dbo.MultiStoreReport_NoGrouping
    
  @Region VARCHAR(5) ,
    @startdate DATETIME ,
    @enddate DATETIME ,
    @StoreIds varchar(MAX) ,
    @TeamIds VARCHAR(MAX) ,
    @SubTeamIds VARCHAR(MAX) ,
    @Debug INT ,
    @TestUPC VARCHAR(13)
AS
BEGIN



  -- Variables
    DECLARE @LogMsg VARCHAR(255)
    DECLARE @createIndexes BIT
    DECLARE @days INT
  
 -- -- defaults
  --SET @Region = 'MA' -- varchar(5)
 --   SET @startdate = '3/15/2018 10:54:00 AM' -- datetime
 --   SET @enddate = '3/22/2018 10:54:00 AM' -- datetime
 --   SET @StoreIds = '907,408,574,414,873,558,748,632,554,619,795,549,898,862,419,561,880,955,611,569,405,742,932,475,461,808,532,538,940,447,654,430,879,948,537,911,433,591,539,489,533,427,881,579,924,473,962,596,422,479,428,921,415,923,796,431,753,908' -- varchar(max)
 --   SET @TeamIds = NULL -- varchar(max)
 --   SET @SubTeamIds = NULL -- varchar(max)
 --   SET @Debug = 0 -- int,
 --   SET  @testUPC = '0075106380003'
    
    
-- ## SET FLAGS
        SET @createIndexes = 1;


  
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
              MatchTeam BIT ,
              MatchSubTeam BIT ,
              IsActive BIT
            )


    DECLARE @StoreTable TABLE (StoreId INT PRIMARY KEY)

    
      
        DECLARE @TeamsTable TABLE
            (
              TeamId VARCHAR(50) PRIMARY KEY
            ) 


        DECLARE @SubTeamsTable TABLE
            (
              SubTeamId VARCHAR(50) PRIMARY KEY 
            ) 


    IF @storeids IS NOT NULL 
      BEGIN
        INSERT  INTO @StoreTable
            SELECT  *
            FROM    dbo.fn_CSVToTable(@storeids)
      END
    ELSE 
      BEGIN
        INSERT  INTO @StoreTable
            SELECT s.id
            FROM    store s
                INNER JOIN region r ON s.REGION_ID = r.ID
            WHERE   r.REGION_ABBR = @region
      END 

    

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
            MatchSubTeam=0,
            MatchTeam=0,
                        IsActive=0
                FROM    dbo.REPORT_HEADER rh
                        INNER JOIN dbo.REPORT_DETAIL rd ON rh.id = rd.REPORT_HEADER_ID
                        INNER JOIN store s ON rh.STORE_ID = s.ID
            INNER JOIN @StoreTable st ON st.StoreId = s.ID
                        INNER JOIN region r ON s.REGION_ID = r.ID
                WHERE   rh.OffsetCorrectedCreateDate >= @startdate
                        AND rh.OffsetCorrectedCreateDate <= @enddate
                        AND r.REGION_ABBR = @region
                        AND rh.EXCLUDE_FLAG IS NULL
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
        
    RAISERROR('here',0,1) WITH nowait 
                   
          
          UPDATE    #itemdata
          SET       isactive = 1
          WHERE     matchsubteam = 1
                    AND matchteam = 1

        


 
        IF @createIndexes = 1 
      begin
            CREATE INDEX IX_ItemData_Stores ON #itemdata(upc, storeabbr,isactive) 
      CREATE INDEX IX_ItemData_id ON #itemdata(id) 
      END 

      
      

-- ## Get a count of scans for eac upc
        CREATE TABLE #CountByUPCStore
            (
              upc VARCHAR(25) ,
        StoreAbbr VARCHAR(10),
              CountByUPC INT
            )

        INSERT  INTO #CountByUPCStore
                ( upc ,
            StoreAbbr,
                  CountByUPC 
                )
                SELECT  upc ,
            storeabbr,
                        COUNT(upc) AS CountByUPC
                FROM    #itemdata d
                WHERE   d.isactive = 1
                GROUP BY d.upc, d.StoreAbbr

        IF @createIndexes = 1 
            CREATE INDEX IX_CountByUPCStore_UPC ON #CountByUPCStore(upc, StoreAbbr) INCLUDE (CountByUPC)

      --IF @Debug = 1       
      --SELECT * FROM  #CountByUPCStore

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



        CREATE TABLE #MostRecentIdByUPCStore
            (
              UPC VARCHAR(25) ,
        storeabbr VARCHAR(25) ,
              MostRecentId INT ,
            )

        INSERT  INTO #MostRecentIdByUPCStore
                ( UPC ,
          storeabbr,
                  MostRecentId 
                )
                SELECT  upc ,
            storeabbr,
                        id
                FROM    ( SELECT    upc ,
                  storeabbr,
                                    id ,
                                    RANK() OVER ( PARTITION BY upc , storeabbr ORDER BY id DESC ) AS rk
                          FROM      #itemdata
                          WHERE     isactive = 1
                        ) part1
                WHERE   part1.rk = 1

  IF @createIndexes = 1 
            CREATE INDEX IX_MostRecentIdByUPCstore ON #MostRecentIdByUPCStore(upc,storeabbr) INCLUDE (MostRecentId)



        SET @LogMsg = 'updating null or zero values'
        IF @debug = 1 
            RAISERROR(@LogMsg,0,1) WITH NOWAIT

        UPDATE  #itemdata
        SET     EFF_Cost = -1
        WHERE   ( EFF_Cost IS NULL
                  OR EFF_Cost = 0
                );


        UPDATE  #itemdata
        SET     eff_price = -1
        WHERE   ( eff_price IS NULL
                  OR eff_price = 0
                );


        UPDATE  #itemdata
        SET     Movement = 0
        WHERE   ( Movement IS NULL
                  OR Movement = 0
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
        SalesByStore DECIMAL(18,3) ,
              LastDateSold DATETIME
            )


      

      INSERT  INTO #AveragesByUPCandStore
                ( UPC ,
                  StoreAbbr ,
                  AvgEffCost ,
                  AvgEffPrice ,
                  AvgMovement ,
          SalesByStore ,
                  LastDateSold
                )
                SELECT  upc ,
                        StoreAbbr ,
                        AvgEffCost = AVG(EFF_Cost) ,
                        AvgEffPrice = AVG(EFF_Price) ,
                        AvgMovement = AVG(NULLIF(Movement,0)) ,
            SalesByStore = SUM(Movement * EFF_Price),
                        LastDateSold = MAX(lastDateSOld)
                FROM    #itemdata
                WHERE   isactive = 1 AND movement <> -1 
                GROUP BY upc ,
                        StoreAbbr



        
        --IF @Debug = 1       
        --SELECT * FROM #averagesbyupcandstore


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
        GROUP BY UPC,StoreAbbr

    
  IF @createindexes = 1
    CREATE INDEX ix_CumulativeSalesData_upc ON #CumulativeSalesData(upc)

    --IF @debug = 1 
    --SELECT * FROM #CumulativeSalesData

        SELECT  id.UPC ,
                Region ,
                ps.ProductStatus
        INTO    #productstatus
        FROM    dbo.ProductStatus ps
                INNER JOIN #MostRecentIdByUPC id ON ps.UPC = id.UPC
                                             AND ps.Region = @Region
        WHERE   ps.ExpirationDate IS NULL
                OR ps.ExpirationDate > GETDATE() 

    IF @debug = 1 
    SELECT * FROM #productstatus

        SELECT  rd.PS_SUBTEAM ,
                i.UPC ,
                rd.brand ,
                rd.BRAND_NAME ,
                rd.LONG_DESCRIPTION ,
                rd.ITEM_SIZE ,
                rd.ITEM_UOM ,
                rd.VENDOR_KEY ,
                rd.VIN ,
                Sales = cs.CumulativeSalesOpportunity ,
                times_scanned = cnt.CountByUPC ,
                rd.notes ,
        cs.movement,
        cs.avgcost,
        Cost =   CASE WHEN  ISNULL(rd.CASE_SIZE, 0) = 0
            THEN cs.Movement * ISNULL(cs.AvgCost, 0.0)
            ELSE 
            cs.Movement * (ISNULL(cs.AvgCost, 0.0)  / ISNULL(rd.CASE_SIZE, 1))
                        END,
        Margin = CASE WHEN cs.AvgPrice = 0
                  OR rd.CASE_SIZE = 0 THEN 0
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
        --StoresList =sbu.StoreList,
                LAST_DATE_SOLD = rd.LASTDATEOFSALES ,
                DAYS_WITH_SALES = rd.DAYSOFMOVEMENT, 
        i.StoreAbbr
        INTO    #ItemDetail
        FROM    #MostRecentIdByUPCstore r INNER JOIN 
        #itemdata i ON r.MostRecentId = i.id
                INNER JOIN dbo.REPORT_DETAIL rd ON i.id = rd.ID
                INNER JOIN #CountByUPCstore cnt ON i.upc = cnt.upc  AND i.storeabbr = cnt.storeabbr 
                INNER JOIN #CumulativeSalesData cs ON cs.upc = rd.upc AND cs.StoreAbbr = i.storeabbr
        WHERE   i.isactive = 1

    

           

    
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
          --id.StoreList ,
          id.NOTES ,
          Cost=ROUND(ISNULL(id.Cost,0),2) ,
          margin= ROUND(id.margin,0) ,
          id.Case_Size ,
          id.Eff_Price ,
          id.Eff_Cost ,
          id.EFF_PRICETYPE ,
          id.CATEGORY_NAME ,
          id.CLASS_NAME ,
          Avg_daily_Units = ROUND(ISNULL(id.Avg_daily_Units,0),2) ,
          Avg_Mov_Sales=ROUND(ISNULL(id.Avg_Mov_Sales,0),2) ,
          id.LAST_DATE_SOLD ,
          id.DAYS_WITH_SALES ,
          StoresList = id.storeabbr,
          Product_Status = ISNULL(stat.ProductStatus,'')
      FROM    #ItemDetail id
          OUTER  APPLY (SELECT * FROM #productstatus ps WHERE ps.upc = id.upc) stat
        ORDER BY upc,storeslist



    DROP TABLE #itemdata
        DROP TABLE #CountByUPCStore
    DROP TABLE #MostRecentIdByUPCStore
        DROP TABLE #MostRecentIdByUPC
        DROP TABLE #AveragesByUPCandStore
        DROP TABLE #CumulativeSalesData
    DROP TABLE #productstatus
        DROP TABLE #ItemDetail
END