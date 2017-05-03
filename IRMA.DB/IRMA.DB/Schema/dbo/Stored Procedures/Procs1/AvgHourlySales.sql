CREATE PROCEDURE dbo.AvgHourlySales
@Team_No int,
@SubTeam_No int,
@Store_No int,
@Zone_Id int,
@StartDate datetime,
@EndDate datetime,
@SubtractNumber smallint,
@Identifier varchar(13),
@FamilyCode varchar(5)
AS

SET NOCOUNT ON

DECLARE @BeginDateDt SMALLDATETIME, @EndDateDt SMALLDATETIME
SELECT @BeginDateDt = CONVERT(SMALLDATETIME, @StartDate), @EndDateDt = CONVERT(SMALLDATETIME, @EndDate)



SELECT @Identifier = ISNULL(@identifier,@familycode) + CASE WHEN @familycode IS NULL THEN '' ELSE '%' END

SELECT DATEPART(hh,Sales_Fact.Time_Key) AS HOUR,
 SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) AS TOTALPRICE
, sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                           Sales_Quantity, Return_Quantity, Package_Desc1, Weight))as Qnty
FROM Item (NOLOCK) 
    INNER JOIN  
        Sales_Fact (NOLOCK) 
        ON Item.Item_Key = Sales_Fact.Item_Key
    INNER JOIN 
        StoreSubTeam (NOLOCK) 
        ON StoreSubTeam.SubTeam_No = Sales_Fact.SubTeam_No 
            AND Sales_Fact.Store_No = StoreSubTeam.Store_No 
    INNER JOIN 
        Time (NOLOCK) 
        ON Sales_Fact.Time_Key = Time.Time_Key       
    INNER JOIN
        ItemIdentifier (NOLOCK)
        ON ItemIdentifier.item_key = Item.Item_Key and Default_Identifier = case when @Identifier is null then 1 else default_identifier end
    INNER JOIN 
        ItemUnit (NOLOCK)
        on ItemUnit.Unit_ID = Item.Retail_Unit_ID
WHERE Date_Key >= DATEADD(d,-@Subtractnumber, @BeginDateDt) AND Date_Key < DATEADD(d,1 - @Subtractnumber ,@EndDateDt)  AND
      ISNULL(@Team_No, StoreSubteam.Team_No) = StoreSubteam.Team_No AND 
      ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No) = StoreSubTeam.SubTeam_No AND
      ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No AND Sales_Account IS NULL
      AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier, ItemIdentifier.Identifier) 
GROUP BY DATEPART(hh,Sales_Fact.Time_Key)
ORDER BY DATEPART(hh,Sales_Fact.Time_Key)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AvgHourlySales] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AvgHourlySales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AvgHourlySales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AvgHourlySales] TO [IRMAReportsRole]
    AS [dbo];

