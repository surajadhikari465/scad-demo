CREATE PROCEDURE dbo.MovementCompSalesOrg
@Month int,
@Year int,
@Team_No int,
@SubTeam_No int,
@Store_No int
AS

BEGIN

    DECLARE @Min_Date varchar(25), @Max_Date varchar(25)

    SELECT @Min_Date = MIN(Date_Key), @Max_Date = MAX(Date_Key)
    FROM Date 
    WHERE Period = @Month AND Year = @Year

    SELECT ISNULL(T1.Date_Key, ISNULL(T2.Date_Key, T3.Date_Key)) AS Date_Key, TotalPriceTY, TotalPriceLY, 'Product Price - Returns - Promotions - Markdowns - Gift Certificates' AS Formula 
    FROM (SELECT Date_Key, SUM(ISNULL(Sales_Amount,0)) + SUM(ISNULL(Return_Amount,0)) + SUM(ISNULL(Markdown_Amount,0)) + SUM(ISNULL(Promotion_Amount,0)) AS TotalPriceTY 
          FROM Item INNER JOIN (Store INNER JOIN (
                 Sales_SumByItem INNER JOIN SubTeam ON (Sales_SumByItem.SubTeam_No = SubTeam.SubTeam_No) 
               ) ON (Store.Store_No = Sales_sumByItem.Store_No)
             ) ON (Item.Item_Key = Sales_SumByItem.Item_Key)
	  WHERE Date_Key >= @Min_Date AND Date_Key <= @Max_Date AND 
                ISNULL(@Team_No, SubTeam.Team_No) = SubTeam.Team_No AND ISNULL(@SubTeam_No, SubTeam.SubTeam_No) = SubTeam.SubTeam_No AND
                ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND 
                ISNULL(Sales_SumByItem.Item_Key,0) <> 30124 AND Sales_Account IS NULL
          GROUP BY Date_Key) T1 FULL JOIN ( 
          SELECT DATEADD(d,364,Date_Key) AS Date_Key, SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) AS TotalPriceLY 
          FROM Item INNER JOIN (Store INNER JOIN (
                 Sales_SumByItem INNER JOIN SubTeam ON (Sales_SumByItem.SubTeam_No = SubTeam.SubTeam_No)
               ) ON (Store.Store_No = Sales_SumByItem.Store_No)
             ) ON (Item.Item_Key = Sales_SumByItem.Item_Key)
          WHERE Date_Key >= DATEADD(d,-364,@Min_Date) AND Date_Key <= DATEADD(d,-364,@Max_Date) AND 
                ISNULL(@Team_No, SubTeam.Team_No) = SubTeam.Team_No AND ISNULL(@SubTeam_No, SubTeam.SubTeam_No) = SubTeam.SubTeam_No AND
                ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND
                Sales_SumByItem.Item_Key <> 30124 AND Sales_Account IS NULL
          GROUP BY Date_Key) T2 ON (T1.Date_Key = T2.Date_Key) FULL JOIN (
          SELECT Date_Key 
          FROM Date 
          WHERE Date_Key >= @Min_Date AND Date_Key <= @Max_Date) T3 ON (ISNULL(T1.Date_Key,T2.Date_Key) = T3.Date_Key)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MovementCompSalesOrg] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MovementCompSalesOrg] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MovementCompSalesOrg] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MovementCompSalesOrg] TO [IRMAReportsRole]
    AS [dbo];

