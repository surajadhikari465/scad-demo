CREATE PROCEDURE dbo.SalesPercentageCrossTab
@Store_No INT,
@ZoneID INT,
@Team_No INT,
@SubTeam_No INT,
@Identifier VARCHAR(13),
@FamilyCode VARCHAR(5),
@StartDate VARCHAR(20),
@EndDate VARCHAR(20)
as
-- Convert the dates to smalldatetime because this will run forever if not
-- The parameters are varchar because Crystal does not cooperate otherwise.
DECLARE @BeginDateDt smalldatetime, @EndDateDt smalldatetime
SELECT @BeginDateDt = CONVERT(smalldatetime, @StartDate), @EndDateDt = CONVERT(smalldatetime, @EndDate)

DECLARE @tmpSalesTY TABLE(Date_Key SMALLDATETIME, Store_Name VARCHAR(50), SubTeam_No INT, Team_No INT,Identifier VARCHAR(13), Sales decimal(9, 2) )
Insert into @tmpSalesTY 

	select Sales_SumByItem.Date_Key, Store.Store_Name, Sales_SumByItem.SubTeam_No, StoreSubTeam.Team_No, ItemIdentifier.Identifier,
		SUM(Sales_SumByItem.Sales_Amount) + SUM(Sales_SumByItem.Return_Amount) 
		+ SUM(Sales_SumByItem.Markdown_Amount) + SUM(Sales_SumByItem.Promotion_Amount) as Sales		
	FROM Store (NOLOCK) 
		INNER JOIN Sales_SumByItem (NOLOCK) ON Store.Store_No = Sales_SumByItem.Store_No 
		INNER JOIN StoreSubTeam  (NOLOCK) ON (Sales_SumByItem.SubTeam_No = StoreSubTeam.SubTeam_No 
			    			      AND Sales_SumByItem.Store_No = StoreSubTeam.Store_No) 
		INNER JOIN Item (NOLOCK) ON Sales_SumByItem.Item_Key = Item.Item_Key
		INNER JOIN ItemIdentifier (NOLOCK) ON Item.Item_Key = ItemIdentifier.Item_Key and ItemIdentifier.Default_Identifier = 1
	WHERE
		Date_Key >= @BeginDateDt AND Date_Key <= @EndDateDt  AND Sales_Account IS NULL AND --DATEADD(day, -364, @BeginDateDt) 
		ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND 
		ISNULL(@ZoneID, Store.Zone_ID) = Store.Zone_ID 
		--ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
		--ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No) = StoreSubTeam.SubTeam_No 
	GROUP BY Sales_SumByItem.Date_Key, Store.Store_Name, Sales_SumByItem.SubTeam_No, StoreSubTeam.Team_No, ItemIdentifier.Identifier
	Order by Sales_SumByItem.Date_Key, Sales_SumByItem.SubTeam_No,ItemIdentifier.Identifier

DECLARE @tmpSalesLY TABLE(Date_Key SMALLDATETIME, Store_Name VARCHAR(50), SubTeam_No INT, Team_No INT, Identifier VARCHAR(13), Sales decimal(9, 2)) 
Insert into @tmpSalesLY
	select Sales_SumByItem.Date_Key, Store.Store_Name, Sales_SumByItem.SubTeam_No, StoreSubTeam.Team_No, ItemIdentifier.Identifier,
		SUM(Sales_SumByItem.Sales_Amount) + SUM(Sales_SumByItem.Return_Amount) 
		+ SUM(Sales_SumByItem.Markdown_Amount) + SUM(Sales_SumByItem.Promotion_Amount) as Sales		
	FROM Store (NOLOCK) 
		INNER JOIN Sales_SumByItem (NOLOCK) ON Store.Store_No = Sales_SumByItem.Store_No 
		INNER JOIN StoreSubTeam  (NOLOCK) ON (Sales_SumByItem.SubTeam_No = StoreSubTeam.SubTeam_No 
			    			      AND Sales_SumByItem.Store_No = StoreSubTeam.Store_No) 
		INNER JOIN Item (NOLOCK) ON Sales_SumByItem.Item_Key = Item.Item_Key
		INNER JOIN ItemIdentifier (NOLOCK) ON Item.Item_Key = ItemIdentifier.Item_Key and ItemIdentifier.Default_Identifier = 1
	WHERE
		Date_Key >= DATEADD(day, -364, @BeginDateDt) AND Date_Key <= DATEADD(day, -364, @EndDateDt) AND Sales_Account IS NULL AND
		ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND 
		ISNULL(@ZoneID, Store.Zone_ID) = Store.Zone_ID 
		--ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
		--ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No) = StoreSubTeam.SubTeam_No 
	GROUP BY Sales_SumByItem.Date_Key, Store.Store_Name, Sales_SumByItem.SubTeam_No, StoreSubTeam.Team_No, ItemIdentifier.Identifier
	Order by Sales_SumByItem.Date_Key, Sales_SumByItem.SubTeam_No,ItemIdentifier.Identifier


    Select TY_SubTeam.Date_Key1 as TY_Date_Key, TY_SubTeam.Store_Name as TY_Store_Name, TY_SubTeam.SubTeamSales, 
	    TY_AllSubTeam.TotSubTeamSales, LY_SubTeam.LYSubTeamSales, LY_AllSubTeam.LYTotSubTeamSales
    From(
	 SELECT TY_S.Date_Key as Date_Key1, TY_S.Store_Name, SUM(TY_S.Sales) as SubteamSales -- 
	 FROM @tmpSalesTY TY_S 
	 Where  ISNULL(@Team_No, TY_S.Team_No) = TY_S.Team_No AND 
		ISNULL(@SubTeam_No, TY_S.SubTeam_No) = TY_S.SubTeam_No 
		AND TY_S.Identifier like 
		case when not(@Identifier is null) and @familyCode is null then
			@Identifier
		     when @Identifier is null and not(@familyCode is null) then
			@familycode + '%'
		     when @Identifier is null and @familyCode is null then
			TY_S.Identifier
		end 	
	 GROUP BY TY_S.DATE_KEY, TY_S.Store_Name
        ) as TY_SubTeam
       		INNER JOIN(
		           SELECT TY_AS.Date_Key as Date_Key2, TY_AS.Store_Name, SUM(TY_AS.Sales) AS TotSubTeamSales 
				--SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) As TotalSales
            	           FROM @tmpSalesTY TY_AS
			   GROUP BY TY_AS.DATE_KEY, TY_AS.Store_Name
                          ) as TY_AllSubTeam on TY_SubTeam.Date_Key1 = TY_AllSubTeam.Date_Key2 and TY_SubTeam.Store_Name = TY_AllSubTeam.Store_Name
        	INNER JOIN(	
			   SELECT DATEADD(day, 364, LY_S.Date_Key) as LYDate_Key1, LY_S.Store_Name, SUM(LY_S.Sales) As LYSubTeamSales		
			   FROM @tmpSalesLY LY_S
			   WHERE ISNULL(@Team_No, LY_S.Team_No) = LY_S.Team_No AND 
				 ISNULL(@SubTeam_No, LY_S.SubTeam_No) = LY_S.SubTeam_No 
				 AND LY_S.Identifier like 
					case when not(@Identifier is null) and @familyCode is null then
						@Identifier
			     		when @Identifier is null and not(@familyCode is null) then
						@familycode + '%'
			     		when @Identifier is null and @familyCode is null then
						LY_S.Identifier
					end 					
	 		   GROUP BY LY_S.DATE_KEY, LY_S.Store_Name
        		   ) as LY_SubTeam on LY_SubTeam.LYDate_Key1 = TY_SubTeam.Date_Key1 and LY_SubTeam.Store_Name = TY_SubTeam.Store_Name

    		INNER JOIN(
			   SELECT DATEADD(day, 364, LY_AS.Date_Key) as LYDate_Key2, LY_AS.Store_Name, SUM(LY_AS.Sales) As LYTotSubTeamSales
			   FROM @tmpSalesLY LY_AS
		 	   GROUP BY LY_AS.DATE_KEY, LY_AS.Store_Name
			  ) as LY_AllSubTeam  on LY_AllSubTeam.LYDate_Key2 = TY_AllSubTeam.Date_Key2 and LY_AllSubTeam.Store_Name = TY_AllSubTeam.Store_Name	
order by TY_SubTeam.Store_Name, TY_SubTeam.Date_Key1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesPercentageCrossTab] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesPercentageCrossTab] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesPercentageCrossTab] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesPercentageCrossTab] TO [IRMAReportsRole]
    AS [dbo];

