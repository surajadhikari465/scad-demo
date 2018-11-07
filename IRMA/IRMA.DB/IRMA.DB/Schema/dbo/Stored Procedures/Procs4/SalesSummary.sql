CREATE PROCEDURE dbo.SalesSummary
@SubTeamLst varchar(200),
@Store_No int,
@StartDate varchar(20),
@EndDate varchar(20)
As     


SET NOCOUNT ON
--Parse out the Subteams from the subTeamLst param into the @SubTeams table variable
DECLARE @SubTeams table(SubTeam_No int)
DECLARE @iPos tinyint
DECLARE @iLastPos tinyint

SELECT @iLastPos = 0
SELECT @iPos = CHARINDEX('|', @SubTeamLst,@iLastPos) 
WHILE @iPos <> 0
  BEGIN
	INSERT INTO @SubTeams
	SELECT SUBSTRING(@SubTeamLst, @iLastPos + 1, @iPos - (@iLastPos + 1)) 

	SELECT @iLastPos = @iPos 

	SELECT @iPos = CHARINDEX('|', @SubTeamLst,@iLastPos + 1) 
  END

DECLARE @StoreSubTeam table(Store_no int, 
                            Team_No int, 
                            SubTeam_No int)
INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam
WHERE StoreSubTeam.SubTeam_No IN (SELECT SubTeam_No 
                                  FROM @SubTeams)
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No

DECLARE @BeginDateDt smalldatetime, 
        @EndDateDt smalldatetime

SELECT @BeginDateDt = CONVERT(smalldatetime, @StartDate), 
       @EndDateDt = CONVERT(smalldatetime, @EndDate)

SELECT SubTeam_Name, 
       SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) AS Total_Sales 
FROM Sales_SumByItem (nolock) 
	INNER JOIN
        @StoreSubTeam SST
        ON (SST.SubTeam_No = Sales_SumByItem.Subteam_No 
		    AND SST.Store_No = Sales_SumbyItem.Store_No)	
	INNER JOIN 
        Item (nolock) 
        ON Item.Item_Key = Sales_SumByItem.Item_Key
	INNER JOIN 
        SubTeam (nolock) 
        ON Sales_SumByItem.SubTeam_No = SubTeam.SubTeam_No 
	INNER JOIN 
        @SubTeams ST 
        ON SubTeam.SubTeam_No = ST.SubTeam_No
WHERE Sales_SumByItem.Date_Key >= @BeginDateDt AND Sales_SumByItem.Date_Key < DATEADD(day, 1, @EndDateDT) 
    AND ISNULL(@Store_No, Sales_SumByItem.Store_No) = Sales_SumByItem.Store_No 
    AND Sales_Account IS NULL
GROUP BY SubTeam_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesSummary] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesSummary] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesSummary] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SalesSummary] TO [IRMAReportsRole]
    AS [dbo];

