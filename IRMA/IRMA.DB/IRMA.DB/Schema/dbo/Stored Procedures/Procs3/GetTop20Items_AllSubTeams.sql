-- =============================================
-- Author:		Hussain Hashim
-- Create date: 08/06/2007
-- Description:	Returns Top 20 Items for every Sub Team for a given date range
-- =============================================
CREATE PROCEDURE [dbo].[GetTop20Items_AllSubTeams]
	@SubTeam_No	varchar(1000),
	@Store_No	varchar(1000),
	@StartDate	smalldatetime,
	@EndDate	smalldatetime	

AS
BEGIN
	
	DECLARE		@SQLString		NVARCHAR(3500)
	SET @SQLString = ''

	SET NOCOUNT ON
	
	SET @SQLString = @SQLString + ''

	SET @SQLString = @SQLString + 'SELECT     SubTeam_No, Item_Key, SUM(Sales_Amount) + SUM(Return_Amount) AS Net_Sales_Amount, ROW_NUMBER() OVER (PARTITION BY '
	SET @SQLString = @SQLString + '						  SubTeam_No ORDER BY SubTeam_No, SUM(Sales_Amount) + SUM(Return_Amount) DESC) AS TopItems '
	SET @SQLString = @SQLString + 'INTO #Temp1 '
	SET @SQLString = @SQLString + 'FROM         dbo.Sales_SumByItem '
	SET @SQLString = @SQLString + 'WHERE     (Date_Key BETWEEN ''' + CONVERT(VARCHAR, @StartDate , 101) + ''' AND ''' + CONVERT(VARCHAR, @EndDate, 101) + ''') '
	
	IF CHARINDEX(' All', @SubTeam_No) = 0 
		SET @SQLString = @SQLString + '			AND     (SubTeam_No IN (SELECT Param FROM dbo.fn_MVParam(''' + @SubTeam_No + ''', '','') AS fn_MVParam_1)) '

	SET @SQLString = @SQLString + 'GROUP BY SubTeam_No, Item_Key '
	SET @SQLString = @SQLString + 'ORDER BY SubTeam_No, TopItems '

--	PRINT @SQLString
--	EXECUTE SP_EXECUTESQL @SQLString



	--SET @SQLString = ''
	SET @SQLString = @SQLString + 'Select #Temp1.SubTeam_No, #Temp1.Item_Key, dbo.Sales_SumByItem.Store_No, '
	SET @SQLString = @SQLString + '		SUM(dbo.Sales_SumByItem.Sales_Quantity) - SUM(dbo.Sales_SumByItem.Return_Quantity) AS Net_Sale_Quantity, '
	SET @SQLString = @SQLString + 'SUM(dbo.Sales_SumByItem.Sales_Amount) + SUM(dbo.Sales_SumByItem.Return_Amount) AS Net_Sales_Amount, '
	SET @SQLString = @SQLString + '		TopItems, dbo.Item.Item_Description, dbo.SubTeam.SubTeam_Name, dbo.Store.Store_Name, AVG(Price) AS Average_Price, '
	SET @SQLString = @SQLString + '			dbo.fn_GetCurrentMarginPercent(AVG(Price), AVG(dbo.Price.Multiple), dbo.fn_GetCurrentNetCost(#Temp1.Item_Key, dbo.Sales_SumByItem.Store_No), (select TOP 1 Package_Desc1 from dbo.fn_VendorsCost(#Temp1.Item_Key, dbo.Sales_SumByItem.Store_No, GetDate()))) AS Margin, '
	SET @SQLString = @SQLString + '			dbo.fn_GetCurrentNetCost(#Temp1.Item_Key, dbo.Sales_SumByItem.Store_No) / (select TOP 1 Package_Desc1 from dbo.fn_VendorsCost(#Temp1.Item_Key, dbo.Sales_SumByItem.Store_No, GetDate())) AS Cost '
	SET @SQLString = @SQLString + '		INTO #Temp2	'
	SET @SQLString = @SQLString + '		FROM #Temp1  INNER JOIN '
	SET @SQLString = @SQLString + '                      dbo.Item ON #Temp1.Item_Key = dbo.Item.Item_Key INNER JOIN '
	SET @SQLString = @SQLString + '                      dbo.SubTeam ON #Temp1.SubTeam_No = dbo.SubTeam.SubTeam_No INNER JOIN '
	SET @SQLString = @SQLString + '					  dbo.Sales_SumByItem ON #Temp1.SubTeam_No = dbo.Sales_SumByItem.SubTeam_No AND '
	SET @SQLString = @SQLString + '					  #Temp1.Item_Key = dbo.Sales_SumByItem.Item_Key INNER JOIN '
	SET @SQLString = @SQLString + '                      dbo.Store ON dbo.Sales_SumByItem.Store_No = dbo.Store.Store_No INNER JOIN '
	SET @SQLString = @SQLString + '					  dbo.Price ON dbo.Sales_SumByItem.Store_No = dbo.Price.Store_No AND '
	SET @SQLString = @SQLString + '					  #Temp1.Item_Key = dbo.Price.Item_Key '

	SET @SQLString = @SQLString + 'Where TopItems <= 20 '
	SET @SQLString = @SQLString + 'AND     (Date_Key BETWEEN ''' + CONVERT(VARCHAR, @StartDate , 101) + ''' AND ''' + CONVERT(VARCHAR, @EndDate, 101) + ''') '
	
	
	IF CHARINDEX(' All', @Store_No) = 0 
		SET @SQLString = @SQLString + 'AND	dbo.Sales_SumByItem.Store_No IN (SELECT Param FROM dbo.fn_MVParam(''' + @Store_No+ ''', '','') AS fn_MVParam_1)  '

	SET @SQLString = @SQLString + 'GROUP BY #Temp1.Item_Key, #Temp1.SubTeam_No, #Temp1.TopItems, dbo.Item.Item_Description, dbo.SubTeam.SubTeam_Name, '
	SET @SQLString = @SQLString + 'dbo.Sales_SumByItem.Store_No, dbo.Store.Store_Name '
	SET @SQLString = @SQLString + 'order by subteam_no, TopItems, Store_No, Net_Sales_Amount DESC '

--	RETURN
--
--	SET NOCOUNT OFF

	SET @SQLString = @SQLString + 'SELECT '
	SET @SQLString = @SQLString + 'SubTeam_No, Item_Key, Store_No, Net_Sale_Quantity, Net_Sales_Amount, '
	SET @SQLString = @SQLString + '	TopItems, Item_Description, SubTeam_Name, Store_Name, Average_Price, Margin, '
	SET @SQLString = @SQLString + '	Cost, AVG_TopItems '
	SET @SQLString = @SQLString + 'FROM '
	SET @SQLString = @SQLString + '( '
	SET @SQLString = @SQLString + 'Select *, #Temp2.TopItems AS AVG_TopItems '
	SET @SQLString = @SQLString + 'FROM #Temp2 '
	SET @SQLString = @SQLString + 'UNION '
	SET @SQLString = @SQLString + 'Select SubTeam_No, Item_Key, 10000 AS Store_No, SUM(Net_Sale_Quantity) AS Net_Sale_Quantity, SUM(Net_Sales_Amount) AS Net_Sales_Amount, '
	SET @SQLString = @SQLString + '	TopItems AS TopItems, Item_Description, SubTeam_Name, ''Total'' AS Store_Name, AVG(Average_Price) AS Average_Price, AVG(Margin) AS Margin, '
	SET @SQLString = @SQLString + '	AVG(Cost) AS Cost, AVG(TopItems) AS AVG_TopItems '
	SET @SQLString = @SQLString + 'FROM #Temp2 '
	SET @SQLString = @SQLString + 'GROUP BY SubTeam_No, Item_Key, TopItems, Item_Description, SubTeam_Name '
	SET @SQLString = @SQLString + ') T '
	SET @SQLString = @SQLString + 'ORDER BY subteam_no, TopItems, Store_No, Net_Sales_Amount DESC '



	SET @SQLString = @SQLString + 'Drop Table #Temp1 '
	SET @SQLString = @SQLString + 'Drop Table #Temp2 '


	PRINT @SQLString
	EXECUTE SP_EXECUTESQL @SQLString
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTop20Items_AllSubTeams] TO [IRMAReportsRole]
    AS [dbo];

