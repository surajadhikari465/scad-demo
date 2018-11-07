CREATE PROCEDURE [dbo].[CommodityCodeMovement]
    @StartDate varchar(10),
    @EndDate varchar(10),
    @StoreList varchar(1000),
	@CommodityList varchar(1000),
	@SubteamList varchar (1000),
	@ClassList varchar (1000),	
	@MovementTable varchar (300)
AS

BEGIN
	DECLARE @SqlString varchar(8000)

	SET @SqlString = ''

	SET @SqlString = '
			SELECT
				i.SubTeam_No,
				ii.Identifier,
				i.Item_Description,
				ib.Brand_Name,
				i.Package_Desc2,
				iu.Unit_Name,
				ia.Text_1 as Commodity,
				sales.Date_Key,
				sales.Store_No,
				s.Store_Name,
			    dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) as SalesQuantity,
			   (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount) as SalesAmount,
			   (dbo.fn_GetAverageCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''') 
						* dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) as SalesCost,
			   (dbo.fn_GetAvgNetCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''') 
						* dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)) as SalesNetCost,
				dbo.fn_GetMargin(case
									when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) is null then 0
									when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) = 0 then 0 
									else (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount)/dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)
								 end, 1, dbo.fn_GetAverageCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''')) as Margin,
				dbo.fn_GetMargin(case
									when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) is null then 0
									when dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity) = 0 then 0 
									else (sales.Sales_Amount + sales.Return_Amount + sales.Markdown_Amount + sales.Promotion_Amount)/dbo.fn_GetWeightOrQtySold(Weight, Sales_Quantity, Return_Quantity)
								 end, 1, dbo.fn_GetAvgNetCostDateRange(sales.item_key, sales.store_no, ''' + @StartDate + ''', ''' + @EndDate+ ''')) as NetMargin
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

	IF @MovementTable = 1
		BEGIN
			SELECT @SqlString = @SqlString + '
				INNER JOIN Sales_SumByItemWkly sales
					 ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate + ''') + 6 and
						sales.Date_Key <= CONVERT(datetime, ''' +  @EndDate + ''') 
				INNER JOIN Store s
					 ON s.store_no = sales.store_no '
		END
	ELSE
		BEGIN
			SELECT @SqlString = @SqlString + '
				INNER JOIN Sales_SumByItem sales
					ON i.item_key = sales.item_key and
						sales.Date_Key >= CONVERT(datetime, ''' + @StartDate + ''') and 
						sales.Date_Key <= CONVERT(datetime, ''' + @EndDate + ''') 
				INNER JOIN Store s
					 ON s.store_no = sales.store_no '
		END

	IF @StoreList IS NOT NULL
		BEGIN
			SELECT @SqlString = @SqlString + '
				INNER JOIN dbo.fn_Parse_List(''' + @StoreList + ''', '','') sl
					ON sl.Key_Value = sales.Store_No '
		END			
    
--	PRINT(@SqlString)
	EXEC (@SqlString)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommodityCodeMovement] TO [IRMAReportsRole]
    AS [dbo];

