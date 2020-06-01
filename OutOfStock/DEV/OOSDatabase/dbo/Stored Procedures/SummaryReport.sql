
CREATE PROCEDURE dbo.SummaryReport
    @region AS VARCHAR(2) ,
    @start AS DATETIME ,
    @end AS DATETIME
AS 
    BEGIN 
	

        CREATE TABLE #stores ( ps_bu VARCHAR(10) ) 
        CREATE TABLE #reportHeaders ( id INT ) 

        INSERT  INTO #stores
                SELECT  PS_BU
                FROM    store s
                        INNER JOIN region r ON s.REGION_ID = r.ID
                WHERE   r.REGION_ABBR = @region
                        AND s.STATUS_ID = 3

        CREATE NONCLUSTERED INDEX ix_stores_temp ON #stores (ps_bu)



        SELECT  s.PS_BU ,
                COUNT(*) [count]
        INTO    #counts
        FROM    dbo.REPORT_HEADER h
                INNER JOIN store s ON h.STORE_ID = s.id
                INNER JOIN #stores s1 ON s.PS_BU = s1.ps_bu
        WHERE   OffsetCorrectedCreateDate >= @start
                AND OffsetCorrectedCreateDate <= @end
        GROUP BY s.PS_BU
                WITH ROLLUP

				

        SELECT  s.ps_bu ,
                PS_TEAM ,
                COUNT(*) [count]
        INTO    #items
        FROM    dbo.REPORT_HEADER h
                INNER JOIN dbo.REPORT_DETAIL rd ON h.ID = rd.REPORT_HEADER_ID
                INNER JOIN store s ON h.STORE_ID = s.id
                INNER JOIN #stores s1 ON s.PS_BU = s1.ps_bu
        WHERE   OffsetCorrectedCreateDate >= @start
                AND OffsetCorrectedCreateDate <= @end
                AND PS_TEAM IN ( 'Grocery', 'Whole Body' )
        GROUP BY s.ps_bu ,
                ps_team
        ORDER BY s.PS_BU;

        SELECT  *, (SELECT COUNT(*) FROM #items WHERE ps_team = p.ps_team) g
        FROM    ( SELECT    PS_TEAM ,
                            SUM(count) c 
                  FROM      #items i 
                  GROUP BY  PS_TEAM
                ) p

        ;WITH    cte
                  AS ( SELECT   s.ps_bu ,
                                s1.STORE_NAME ,
                                COALESCE(( SELECT   [count]
                                           FROM     #items i
                                           WHERE    i.PS_BU = s.ps_bu
                                                    AND PS_TEAM = 'Grocery'
                                         ), 0) GroceryCnt ,
                                COALESCE(( SELECT   [count]
                                           FROM     #items i
                                           WHERE    i.PS_BU = s.ps_bu
                                                    AND PS_TEAM = 'Whole Body'
                                         ), 0) WholeBodyCnt ,
                                ( SELECT    skuGrocery.numberOfSKUs
                                  FROM      dbo.SKUCount skuGrocery
                                  WHERE     skuGrocery.STORE_PS_BU = s.ps_bu
                                            AND skuGrocery.TEAM_ID = 1
                                ) GrocerySkuCnt ,
                                ( SELECT    skuGrocery.numberOfSKUs
                                  FROM      dbo.SKUCount skuGrocery
                                  WHERE     skuGrocery.STORE_PS_BU = s.ps_bu
                                            AND skuGrocery.TEAM_ID = 2
                                ) WholeBodySkuCnt
                       FROM     #stores s
                                INNER JOIN store s1 ON s.ps_bu = s1.PS_BU
                     )
            SELECT  cte.ps_bu ,
                    cte.STORE_NAME ,
                    cte.GroceryCnt ,
                    cte.WholeBodyCnt ,
                    GroceryPercent = CAST(CASE WHEN GrocerySkuCnt > 0
                                          THEN CAST(GroceryCnt AS DECIMAL)
                                               / CAST(GrocerySkuCnt AS DECIMAL)
                                               * 100
                                          ELSE 0
                                     END AS DECIMAL(18,5)),
                    WholeBodyPercent = CAST(CASE WHEN WholeBodySkuCnt > 0
                                            THEN CAST(WholeBodyCnt AS DECIMAL)
                                                 / CAST(WholeBodySkuCnt AS DECIMAL)
                                                 * 100
                                            ELSE 0
                                       END AS DECIMAL(18,5)),
                    ScanCount = c.count
            FROM    cte
                    INNER JOIN #counts c ON cte.ps_bu = c.PS_BU


        DROP TABLE #stores 
        DROP TABLE #counts
        DROP TABLE #reportHeaders
        DROP TABLE #items

    END