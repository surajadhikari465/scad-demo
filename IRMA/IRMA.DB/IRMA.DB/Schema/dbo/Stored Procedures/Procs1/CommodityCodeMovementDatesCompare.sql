CREATE PROCEDURE [dbo].[CommodityCodeMovementDatesCompare]
    @StartDate1 varchar(10),
    @EndDate1 varchar(10),
	@StartDate2 varchar(10),
	@EndDate2 varchar(10),
    @StoreList varchar(1000),
	@CommodityList varchar(1000),
	@SubteamList varchar (1000),
	@ClassList varchar (1000),	
	@MovementTable varchar (300)
AS
BEGIN
	CREATE TABLE #ItemList 
	(
		SubTeam_No int,		
		Item_Key int,
		Identifier varchar(13),
		Item_Description varchar(60),
		Brand_Name varchar(25),
		Package_Desc1 decimal(9,4),
		Package_Desc2 decimal(9,4),
		Unit_Name varchar(25),
		Text_1 varchar(50),
		Weight_Unit bit,
		InsertDate smalldatetime
	)
	CREATE INDEX idxItemKeyItemList ON #ItemList (Item_Key)
	CREATE INDEX idxSubTeamNoItemList ON #ItemList (SubTeam_No)

	CREATE TABLE #CommodityCodeMovement
	(
		DateRange int,
		Date_Key smalldatetime,
		Item_Key int,
		Store_No int,
		SalesQuantity decimal(9,2),
		SalesAmount decimal(9,2),
		SalesCost decimal(9,2),
		SalesNetCost decimal(9,2),
		Margin decimal(9,2),
		NetMargin decimal(9,2)
	)

	DECLARE @SqlString varchar(8000)
	DECLARE @MvmtString varchar(8000)
	DECLARE @PvtQuery varchar(8000)

	SET @SqlString = ''
	SET @SqlString = '
     INSERT INTO #ItemList			
		SELECT
			i.SubTeam_No,
			i.item_key,
		   ii.Identifier,
			i.Item_Description,
		   ib.Brand_Name,
			i.Package_Desc1,
			i.Package_Desc2,
		   iu.Unit_Name,
	       ia.Text_1,
		   iu.Weight_Unit,
			i.Insert_Date				
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
				ON i.Brand_ID = ib.Brand_ID'

	IF @CommodityList IS NOT NULL
		BEGIN
			SELECT @SqlString = @SqlString + '
				INNER JOIN dbo.fn_ParseStringList(''' + @CommodityList + ''', '','') cl 
					ON cl.Key_Value = ia.Text_1 '
		END

	IF @SubTeamList IS NOT NULL
		BEGIN
			SELECT @SqlString = @SqlString + '
				INNER JOIN dbo.fn_Parse_List(''' + @SubteamList + ''', '','') stl  
					 ON stl.Key_Value = i.SubTeam_No '
		END

	IF @ClassList IS NOT NULL
		BEGIN
			SELECT @SqlString = @SqlString + '
				INNER JOIN dbo.fn_Parse_List(''' + @Classlist + ''', '','') csl  
					 ON csl.Key_Value = i.Category_ID '
		END

	SET @MvmtString = ''
	SET @MvmtString = '
	INSERT INTO #CommodityCodeMovement
	SELECT
		1,
		sales.Date_Key,
		sales.Item_Key,
		sales.Store_No,
		dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) as SalesQuantity,
	   (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount) as SalesAmount,
	   (dbo.fn_GetAverageCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate1 + ''', ''' + @EndDate1+ ''') 
				* dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) as SalesCost,
	   (dbo.fn_GetAvgNetCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate1 + ''', ''' + @EndDate1+ ''') 
				* dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) as SalesNetCost,
		dbo.fn_GetMargin(case
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) is null then 0
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) = 0 then 0 
							else (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount)/dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)
						 end, 1, dbo.fn_GetAverageCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate1 + ''', ''' + @EndDate1+ ''')) as Margin,
		dbo.fn_GetMargin(case
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) is null then 0
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) = 0 then 0 
							else (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount)/dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)
						 end, 1, dbo.fn_GetAvgNetCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate1 + ''', ''' + @EndDate1+ ''')) as NetMargin
	FROM
		#ItemList i'

	IF @MovementTable = 1
		BEGIN
			SELECT @MvmtString = @MvmtString + '
				INNER JOIN Sales_SumByItemWkly sales
					 ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate1 + ''') + 6 and
						sales.Date_Key <= CONVERT(datetime, ''' +  @EndDate1 + ''') '
		END
	ELSE
		BEGIN
			SELECT @MvmtString = @MvmtString + '
				INNER JOIN Sales_SumByItem sales
					ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate1 + ''') and 
						sales.Date_Key <= CONVERT(datetime, ''' + @EndDate1 + ''') '
		END

	IF @StoreList IS NOT NULL
		BEGIN
			SELECT @MvmtString = @MvmtString + '
				INNER JOIN dbo.fn_Parse_List(''' + @StoreList + ''', '','') sl
					ON sl.Key_Value = sales.Store_No '
		END	

	SET @MvmtString = @MvmtString + '
	INSERT INTO #CommodityCodeMovement
	SELECT
		2,
		sales.Date_Key,
		sales.Item_Key,
		sales.Store_No,
		dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) as SalesQuantity,
	   (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount) as SalesAmount,
	   (dbo.fn_GetAverageCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate2 + ''', ''' + @EndDate2+ ''') 
				* dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) as SalesCost,
	   (dbo.fn_GetAvgNetCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate2 + ''', ''' + @EndDate2+ ''') 
				* dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) as SalesNetCost,
		dbo.fn_GetMargin(case
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) is null then 0
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) = 0 then 0 
							else (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount)/dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)
						 end, 1, dbo.fn_GetAverageCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate2 + ''', ''' + @EndDate2 + ''')) as Margin,
		dbo.fn_GetMargin(case
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) is null then 0
							when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) = 0 then 0 
							else (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount)/dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)
						 end, 1, dbo.fn_GetAvgNetCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate2 + ''', ''' + @EndDate2 + ''')) as NetMargin
	FROM
		#ItemList i'

	IF @MovementTable = 1
		BEGIN
			SELECT @MvmtString = @MvmtString + '
				INNER JOIN Sales_SumByItemWkly sales
					 ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate2 + ''') + 6 and
						sales.Date_Key <= CONVERT(datetime, ''' +  @EndDate2 + ''') '
		END
	ELSE
		BEGIN
			SELECT @MvmtString = @MvmtString + '
				INNER JOIN Sales_SumByItem sales
					ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate2 + ''') and 
						sales.Date_Key <= CONVERT(datetime, ''' + @EndDate2 + ''') '
		END

	IF @StoreList IS NOT NULL
		BEGIN
			SELECT @MvmtString = @MvmtString + '
				INNER JOIN dbo.fn_Parse_List(''' + @StoreList + ''', '','') sl
					ON sl.Key_Value = sales.Store_No '
		END		
		
	SET @PvtQuery  = ''
	SET @PvtQuery  = '
	SELECT
		s.DateRange,
		s.Item_Key,
		i.SubTeam_No,		
		i.Identifier,
		i.Item_Description,
		i.Brand_Name,
		i.Package_Desc1,
		i.Package_Desc2,
		i.Unit_Name,
		i.Text_1,
		i.Weight_Unit,
		i.InsertDate,
		st.Store_Name,
		s.SalesQuantity,
		s.SalesAmount,
		s.SalesCost,
		s.SalesNetCost,
		s.Margin,
		s.NetMargin
	FROM
		#ItemList i
		INNER JOIN #CommodityCodeMovement s
			ON i.item_key = s.item_key
		INNER JOIN Store st
			ON st.Store_No = s.Store_No'

	EXEC (@SqlString)
    EXEC (@MvmtString)
	EXEC dbo.pivot_query @PvtQuery, 'SubTeam_No, Identifier, Item_Description, Brand_Name, Package_Desc1, Package_Desc2, Unit_Name, Text_1, Store_Name, InsertDate', 'DateRange', 'sum(SalesQuantity) SalesQuantity, sum(SalesAmount) SalesAmount,sum(SalesCost) SalesCost,sum(SalesNetCost) SalesNetCost,sum(Margin) Margin,sum(NetMargin) NetMargin';

--	PRINT (@PvtQuery)
--	PRINT (@SqlString)
--  PRINT (@MvmtString)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommodityCodeMovementDatesCompare] TO [IRMAReportsRole]
    AS [dbo];

