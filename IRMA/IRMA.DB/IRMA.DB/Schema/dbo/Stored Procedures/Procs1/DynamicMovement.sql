CREATE PROCEDURE [dbo].[DynamicMovement]
    @StartDate		varchar(10),
    @EndDate		varchar(10),
    @StoreList		varchar(1000),
	@SubteamList	varchar(1000),
	@ClassList		varchar(1000),
	@ItemList		varchar(1000),
	@CommodityList	varchar(1000),
	@VenueList		varchar(1000),
	@MovementTable	int,
	@TopNumber		varchar(30),
	@BreakBy		varchar(100),
	@RankBy			varchar(100),
	@SortOrder		varchar(100)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	--*******************************************************
	--Separated this out into 3 different sql statements 
	--instead of a large query.  The string was too long.
	--*******************************************************
	DECLARE @ItemListSqlString varchar(8000)
	DECLARE @MovementSqlString varchar(8000)
	DECLARE @RankSqlString varchar (8000)

	--*******************************************************
	--Create a temp table of items and some dimension data
	--based off of the users selection of
	--subteam, class, idenifiers, or generic item attributes
	--*******************************************************
	CREATE TABLE #ItemList 
	(
		SubTeam_No int,		
		SubTeam_Name varchar(100),
		Item_Key int,
		Identifier varchar(13),
		Item_Description varchar(60),
		Brand_Name varchar(25),
		Package_Desc1 decimal(9,4),
		Package_Desc2 decimal(9,4),
		Unit_Name varchar(25),
		Text_1 varchar(50),
		Weight_Unit bit
	)
	CREATE INDEX idxItemKeyItemList ON #ItemList (Item_Key)
	CREATE INDEX idxSubTeamNoItemList ON #ItemList (SubTeam_No)

	SET @ItemListSqlString = ''
	SET @ItemListSqlString = '
	     INSERT INTO #ItemList			
			SELECT
				i.SubTeam_No,
			   st.SubTeam_Name,
				i.item_key,
			   ii.Identifier,
				i.Item_Description,
			   ib.Brand_Name,
				i.Package_Desc1,
				i.Package_Desc2,
			   iu.Unit_Name,
		       ia.Text_1,
			   iu.Weight_Unit				
			FROM
				Item i
				INNER JOIN ItemIdentifier ii  
					ON i.item_key = ii.item_key and
					   ii.Default_Identifier = 1
				INNER JOIN ItemUnit iu
					ON i.Package_Unit_Id = iu.Unit_Id
				LEFT OUTER JOIN ItemAttribute ia
					ON i.item_key = ia.item_key
					AND ia.ItemAttribute_ID = (SELECT MAX(ia2.ItemAttribute_ID)
											   FROM ItemAttribute ia2
											   WHERE ia.Item_Key = ia2.Item_Key)
				INNER JOIN ItemBrand ib
					ON i.Brand_ID = ib.Brand_ID
				INNER JOIN Subteam st
					ON i.Subteam_no = st.Subteam_no'

	IF @ItemList IS NOT NULL
		BEGIN
			SELECT @ItemListSqlString = @ItemListSqlString + '
				INNER JOIN dbo.fn_ParseStringList(''' + @ItemList + ''', '','') il 
					ON il.Key_Value = ii.Identifier '
		END

	IF @CommodityList IS NOT NULL
		BEGIN
			SELECT @ItemListSqlString = @ItemListSqlString + '
				INNER JOIN dbo.fn_ParseStringList(''' + @CommodityList + ''', '','') cl 
					ON cl.Key_Value = ia.Text_1 '
		END

	IF @VenueList IS NOT NULL
		BEGIN
			SELECT @ItemListSqlString = @ItemListSqlString + '
				INNER JOIN dbo.fn_ParseStringList(''' + @VenueList + ''', '','') vl 
					ON vl.Key_Value = ia.Text_5 '
		END

	IF @SubTeamList IS NOT NULL
		BEGIN
			SELECT @ItemListSqlString = @ItemListSqlString + '
				INNER JOIN dbo.fn_Parse_List(''' + @SubteamList + ''', '','') stl  
					 ON stl.Key_Value = i.SubTeam_No '
		END

	IF @ClassList IS NOT NULL
		BEGIN
			SELECT @ItemListSqlString = @ItemListSqlString + '
				INNER JOIN dbo.fn_Parse_List(''' + @Classlist + ''', '','') csl  
					 ON csl.Key_Value = i.Category_ID '
		END

    SELECT @ItemListSqlString = @ItemListSqlString + ';'

	EXEC (@ItemListSqlString)

	--*******************************************************
	--Create a temp table with movement data for the above
	--base on the date range and stores selected
	--*******************************************************
	CREATE TABLE #DynamicMovementStores 
	(
		SubTeam_No int,		
		SubTeam_Name varchar(100),
		Item_Key int,
		Identifier varchar(13),
		Item_Description varchar(60),
		Brand_Name varchar(25),
		Package_Desc2 decimal(9,4),
		Unit_Name varchar(25),
		Commodity varchar(50),
		Store_No int,
		SalesQuantity int,
		SalesAmount decimal(9,2),
		SalesCost decimal(9,2),
		SalesNetCost decimal(9,2),
		GMDollars decimal(9,2),
		GMNetDollars decimal(9,2),		
		Margin decimal(9,2),
		NetMargin decimal(9,2),
		Rank int
	)
	CREATE INDEX idxDynamicMovementStores  ON #DynamicMovementStores  (Item_Key, Store_no)

	SET @MovementSqlString = ''
	SET @MovementSqlString = '
	INSERT INTO #DynamicMovementStores		
			SELECT
				i.SubTeam_No,
				i.SubTeam_Name,
				i.Item_Key,
				i.Identifier,
				i.Item_Description,
				i.Brand_Name,
				i.Package_Desc2,
				i.Unit_Name,
				i.Text_1,
				sales.Store_No as Store_No,
			    sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) as SalesQuantity,
			    sum((sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount)) as SalesAmount,
			   (dbo.fn_GetAverageCostDateRange(i.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''') 
						* sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity))) as SalesCost,
			   (dbo.fn_GetAvgNetCostDateRange(i.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''') 
						* sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity))) as SalesNetCost,
				sum((sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount))
						- (dbo.fn_GetAverageCostDateRange(i.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''') 
								* sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity))) as GMDollars,
				sum((sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount))
						-(dbo.fn_GetAvgNetCostDateRange(i.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''') 
								* sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity))) as GMNetDollars,		
				dbo.fn_GetMargin(case
									when sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) is null then 0
									when sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) = 0 then 0 
									else sum((sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount))/sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity))
								 end, 1, dbo.fn_GetAverageCostDateRange(i.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''')) as Margin,
				dbo.fn_GetMargin(case
									when sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) is null then 0
									when sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) = 0 then 0 
									else sum((sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount))/sum(dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity))
								 end, 1, dbo.fn_GetAvgNetCostDateRange(i.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''')) as NetMargin,
				''''
			FROM #ItemList i'

	IF @MovementTable = 1
		BEGIN
			SELECT @MovementSqlString = @MovementSqlString + '
				INNER JOIN Sales_SumByItemWkly sales
					 ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate + ''') + 6 and
						sales.Date_Key <= CONVERT(datetime, ''' +  @EndDate + ''') '
		END
	ELSE
		BEGIN
			SELECT @MovementSqlString = @MovementSqlString + '
				INNER JOIN Sales_SumByItem sales
					ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate + ''') and 
						sales.Date_Key <= CONVERT(datetime, ''' + @EndDate + ''') '
		END

		
		SELECT @MovementSqlString = @MovementSqlString + '
		GROUP BY
				i.SubTeam_No,
				i.SubTeam_Name,
				i.Item_Key,
				i.Identifier,
				i.Item_Description,
				i.Brand_Name,
				i.Package_Desc2,
				i.Unit_Name,
				i.Text_1,
				sales.Store_No;

		UPDATE #DynamicMovementStores
		   SET #DynamicMovementStores.rank = 
						(SELECT 
							r.rank 
						 FROM (SELECT 
									item_key, 
									store_no, '
		IF @BreakBy = 'Region'
			BEGIN
				SELECT @MovementSqlString = @MovementSqlString + '
									Dense_Rank() OVER (PARTITION BY Store_no ORDER BY ' + @RankBy +  ' ' + @SortOrder + ') as Rank'
			END
		ELSE
			BEGIN
				SELECT @MovementSqlString = @MovementSqlString + '
									Dense_Rank() OVER (PARTITION BY Store_no, ' + @BreakBy + ' ORDER BY ' + @RankBy +  ' ' + @SortOrder + ') as Rank'
			END

		SELECT @MovementSqlString = @MovementSqlString + '			 
							   FROM 
									#DynamicMovementStores) as r
						 WHERE 
							#DynamicMovementStores.item_key = r.item_key
							AND #DynamicMovementStores.store_no = r.store_no);'

    EXEC (@MovementSqlString)
	
	--*******************************************************
	--Create a temp table with movement data for the above
	--base on the date range and stores selected
	--*******************************************************
	CREATE TABLE #DynamicMovementSum
	(
		SubTeam_No int,		
		Item_Key int,
		SalesQuantity int,
		SalesAmount decimal(9,2),
		SalesCost decimal(9,2),
		SalesNetCost decimal(9,2),
		GMDollars decimal(9,2),
		GMNetDollars decimal(9,2),
		Margin decimal(9,2),
		NetMargin decimal(9,2)
	)

	INSERT INTO #DynamicMovementSum
		SELECT
			SubTeam_No,			
			Item_Key,
		    sum(SalesQuantity),
		    sum(SalesAmount),
		    sum(SalesCost),
		    sum(SalesNetCost),
			sum(SalesAmount) - sum(SalesCost),
			sum(SalesAmount) - sum(SalesNetCost),
			(case
				when sum(SalesAmount) is null then 0
				when sum(SalesAmount) = 0 then 0 
				else (sum(SalesAmount)-sum(SalesCost))/sum(SalesAmount)
			end)*100,
			(case
				when sum(SalesAmount) is null then 0
				when sum(SalesAmount) = 0 then 0
				else (sum(SalesAmount)-sum(SalesNetCost))/sum(SalesAmount)
			 end)*100
		FROM
			#DynamicMovementStores
		GROUP BY
			SubTeam_No,
			Item_Key;

	SET @RankSqlString =''
	SET @RankSqlString ='
		WITH Ranking AS
		(		
			SELECT
				''Region'' as RegionalTotal,
				SubTeam_No,
				Item_Key,'

	IF @BreakBy = 'Region'
		BEGIN
			SELECT @RankSqlString = @RankSqlString + '
				Dense_Rank() OVER (PARTITION BY ''' + @BreakBy + ''' ORDER BY ' + @RankBy +  ' ' + @SortOrder + ') as HR_Rank,'
        END
	ELSE
		BEGIN
			SELECT @RankSqlString = @RankSqlString + '
				Dense_Rank() OVER (PARTITION BY ' + @BreakBy + ' ORDER BY ' + @RankBy +  ' ' + @SortOrder + ') as HR_Rank,'
		END

			SELECT @RankSqlString = @RankSqlString + '
			    SalesQuantity,
			    SalesAmount,
			    SalesCost,
			    SalesNetCost,
				GMDollars,
				GMNetDollars,
				Margin,
				NetMargin
			FROM 
				#DynamicMovementSum
			GROUP BY
				SubTeam_No,
				Item_Key,
			    SalesQuantity,
			    SalesAmount,
			    SalesCost,
			    SalesNetCost,
				GMDollars,
				GMNetDollars,
				Margin,
				NetMargin
		)
		SELECT
			Ranking.HR_Rank,
			s.SubTeam_No,
			ss.Store_Name,
			s.SubTeam_Name,
			s.Item_Key,
			s.Identifier,
			s.Item_Description,
			s.Brand_Name,
			s.Package_Desc2,
			s.Unit_Name,
			s.Commodity,
			Ranking.SalesQuantity as RegionalSalesQuantity,
			Ranking.SalesAmount as RegionalSalesAmount,
			Ranking.SalesCost as RegionalSalesCost,
			Ranking.SalesNetCost as RegionalSalesNetCost,
			Ranking.GMDollars,
			Ranking.GMNetDollars,
			Ranking.Margin as RegionalMargin,
			Ranking.NetMargin as RegionalNetMargin,
			s.Store_No,
			s.Rank,
			s.SalesQuantity,
			s.SalesAmount,
			s.SalesCost,
			s.SalesNetCost,
			s.Margin,
			s.NetMargin,
			[AveragePrice] = (CASE WHEN Ranking.SalesQuantity = 0 THEN 0 ELSE (Ranking.SalesAmount/Ranking.SalesQuantity) END)
		FROM
			Ranking
				INNER JOIN #DynamicMovementStores s
					ON s.item_key = Ranking.item_key
				INNER JOIN Store ss
					ON s.Store_No = ss.Store_No'

	IF @StoreList IS NOT NULL
		BEGIN
			SELECT @RankSqlString = @RankSqlString + '
				INNER JOIN dbo.fn_Parse_List(''' + @StoreList + ''', '','') sl
					ON sl.Key_Value = s.Store_No '
		END	

	IF @TopNumber IS NOT NULL
		BEGIN
			SELECT @RankSqlString = @RankSqlString + '
		WHERE
			Ranking.HR_Rank <= ' + @TopNumber
		END

	SELECT @RankSqlString = @RankSqlString + '
		ORDER BY Ranking.SubTeam_No, Ranking.HR_Rank'

	EXEC (@RankSqlString)

--	PRINT (@ItemListSqlString)
--	PRINT (@MovementSqlString)
--	PRINT (@RankSqlString)

DROP TABLE #ItemList
DROP TABLE #DynamicMovementStores
DROP TABLE #DynamicMovementSum

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DynamicMovement] TO [IRMAReportsRole]
    AS [dbo];

