CREATE PROCEDURE PullReport
    (
      @BeginDate DATETIME ,
      @EndDate DATETIME,
	  @Region VARCHAR(5),
	  @Rounding INT
    )
AS 
    BEGIN
        SELECT  *
        INTO    #temp
        FROM    ( SELECT
                            s.STORE_NAME ,
                            rd.ps_team ,
                            COUNT(rd.upc) cnt ,
                            MAX(sku.numberOfSKUs) skus
                  FROM      dbo.REPORT_HEADER rh
                            INNER JOIN store s ON rh.STORE_ID = s.ID
                            INNER JOIN dbo.REGION r ON s.REGION_ID = r.ID
                            INNER JOIN dbo.REPORT_DETAIL rd ON rd.REPORT_HEADER_ID = rh.ID
                            INNER JOIN dbo.TEAM_Interim t ON t.teamName = rd.PS_TEAM
                            LEFT JOIN dbo.SKUCount sku ON sku.STORE_PS_BU = s.PS_BU --AND t.idTeam = sku.TEAM_ID
                  WHERE     rh.CREATED_DATE >=  @BeginDate
                            AND rh.CREATED_DATE <= @EndDate
                            AND r.REGION_ABBR = @Region
                  GROUP BY  s.STORE_NAME ,
                            rd.PS_TEAM
                 
                ) p1 PIVOT ( SUM(cnt) FOR PS_TEAM IN ( [Grocery], [Whole Body] ) ) AS p 


		
        SELECT  s.STORE_NAME ,
                grocery ,
                [whole body] ,
                ROUND(CAST(grocery AS DECIMAL) / CAST(skus AS DECIMAL) * 100, @Rounding) AS GroceryPercentage ,
                ROUND(CAST([whole body] AS DECIMAL) / CAST(skus AS DECIMAL) * 100, @Rounding) AS WBPercentage ,
                ( SELECT    COUNT(rh1.ID)
                  FROM      dbo.REPORT_HEADER rh1
                            INNER JOIN store s1 ON rh1.STORE_ID = s1.ID
                  WHERE     s1.STORE_NAME = t.store_name
                            AND rh1.CREATED_DATE >= @BeginDate
                            AND rh1.CREATED_DATE <= @EndDate
                ) timesScanned
				--, GROUPING(store_name) AS 'Grouping'
				INTO #temp2
        FROM    #temp t
			RIGHT JOIN store s ON t.STORE_NAME = s.STORE_NAME
			INNER JOIN region r ON s.REGION_ID = r.ID
			WHERE r. REGION_ABBR = @Region
     
		ORDER BY s.STORE_NAME
		

		SELECT 'Data' AS rowtype,STORE_NAME AS StoreName, ISNULL(grocery, 0) AS GroceryCnt, ISNULL([whole body],0) AS WBCnt, ISNULL(grocerypercentage,0) AS GroceryPerc, ISNULL(WBPercentage,0) AS WBPerc, ISNULL(timesScanned,0) AS TimesScanned FROM #temp2

		UNION 

		SELECT 'Totals' AS rowtype,'', ISNULL(SUM(grocery),0) AS GroceryCnt, ISNULL(SUM([whole body]),0) AS WBCnt, ISNULL(AVG(grocerypercentage),0) AS GroceryPerc, ISNULL(AVG(WBPercentage),0) AS WBPerc, ISNULL(SUM(timesscanned),0) AS TimesScanned
		FROM #temp2

		ORDER BY rowtype, Store_Name
		DROP TABLE #temp
        DROP TABLE #temp2

    END