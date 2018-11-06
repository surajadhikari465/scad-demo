SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MovementCompSalesNew]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[MovementCompSalesNew]
GO


CREATE PROCEDURE dbo.MovementCompSalesNew
@Month int,
@Year int,
@Team_No int,
@SubTeam_No int,
@Store_No int,
@Zone_ID int
AS

BEGIN
    DECLARE @Min_Date varchar(25),
            @Max_Date varchar(25)

    SELECT @Min_Date = MIN(Date_Key), 
           @Max_Date = MAX(Date_Key)
    FROM Date (nolock) 
    WHERE Period = @Month AND Year = @Year
          
    DECLARE @StoreSubTeam table(Store_no int, 
                                Team_No int, 
                                SubTeam_No int)
    INSERT INTO @StoreSubTeam
    SELECT Store_No, Team_No, SubTeam_No
    FROM StoreSubTeam(nolock)
    WHERE ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
          AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)= StoreSubTeam.SubTeam_No
          AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 
    
    
    SELECT ISNULL(T1.Date_Key, ISNULL(T2.Date_Key, T3.Date_Key)) AS Date_Key, 
           TotalPriceTY, 
           TotalPriceLY, 
           'Product Price - Returns - Promotions - Markdowns - Gift Certificates' AS Formula 
    FROM (
          SELECT Date_Key,
                 SUM(ISNULL(Sales_Amount,0)) - SUM(ISNULL(Return_Amount,0)) 
                    - SUM(ISNULL(Markdown_Amount,0)) - SUM(ISNULL(Promotion_Amount,0)) AS TotalPriceTY 
          FROM Item (nolock) 
            INNER JOIN 
                Sales_SumByItem (nolock) 
                ON Item.Item_Key = Sales_SumByItem.Item_Key
            INNER JOIN 
                Store (nolock)
                ON Store.Store_No = Sales_sumByItem.Store_No 
            INNER JOIN 
                @StoreSubTeam SST
                ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No 
                    and sales_sumbyitem.store_no = SST.store_no)                              
            WHERE Date_Key >= @Min_Date AND Date_Key <= @Max_Date     
                  AND ISNULL(@Zone_ID, Store.Zone_ID) = Store.Zone_ID 
                  AND Sales_Account IS NULL
          GROUP BY Date_Key
         ) T1 
        FULL JOIN ( 
                   SELECT DATEADD(d,364,Date_Key) AS Date_Key, 
                         SUM(Sales_Amount) - SUM(Return_Amount) - SUM(Markdown_Amount) 
                            - SUM(Promotion_Amount) AS TotalPriceLY 
                   FROM Item (nolock) 
                        INNER JOIN 
                            Sales_SumByItem (nolock) 
                            ON Item.Item_Key = Sales_SumByItem.Item_Key
                        INNER JOIN 
                            Store (nolock) 
                            ON Store.Store_No = Sales_sumByItem.Store_No 
                        INNER JOIN 
                            @StoreSubTeam SST
                            ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No 
                                AND Sales_SumByItem.store_no = SST.store_no)                              
                    WHERE Date_Key >= DATEADD(d,-364,@Min_Date) AND Date_Key <= DATEADD(d,-364,@Max_Date) 
                          AND ISNULL(@Zone_ID, Store.Zone_ID) = Store.Zone_ID 
                          AND Sales_Account IS NULL
                   GROUP BY Date_Key
                  ) T2 ON T1.Date_Key = T2.Date_Key
        FULL JOIN (
                   SELECT Date_Key 
                   FROM Date (nolock) 
                   WHERE Date_Key >= @Min_Date AND Date_Key <= @Max_Date
                  ) T3 ON ISNULL(T1.Date_Key,T2.Date_Key) = T3.Date_Key
        ORDER BY Date_Key Asc
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


