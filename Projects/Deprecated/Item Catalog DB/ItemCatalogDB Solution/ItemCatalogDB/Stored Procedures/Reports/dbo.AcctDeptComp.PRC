SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AcctDeptComp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AcctDeptComp]
GO


CREATE PROCEDURE dbo.AcctDeptComp   
@Store_No int, 
@ZoneID int,
@StartDate varchar(20),
@EndDate varchar(20)
As      

SET NOCOUNT ON

DECLARE @BeginDateDt smalldatetime, @EndDateDt smalldatetime
SELECT @BeginDateDt = CONVERT(smalldatetime, @StartDate), @EndDateDt = CONVERT(smalldatetime, @EndDate)

DECLARE @StoreSubTeam table(Store_no int, 
                            Team_No int, 
                            SubTeam_No int)

INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam
WHERE ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 


DECLARE  @tmpSalesTY table (Store_Name varchar(50), 
                            SubTeam_No int, 
                            SubTeam_Name varchar(100), 
                            Sales decimal(9, 2) )

INSERT INTO @tmpSalesTY 
	SELECT Store.Store_Name, 
        SubTeam.SubTeam_No, 
        SubTeam.SubTeam_Name,
		SUM(Sales_SumByItem.Sales_Amount) + SUM(Sales_SumByItem.Return_Amount) 
		    + SUM(Sales_SumByItem.Markdown_Amount) + SUM(Sales_SumByItem.Promotion_Amount) As Sales		
	FROM Sales_SumByItem (nolock) 
		INNER JOIN 
            Store (nolock) 
            ON Store.Store_No = Sales_SumByItem.Store_No 
		INNER JOIN 
            @StoreSubTeam SST
            ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No 
			    AND Sales_SumByItem.Store_No = SST.Store_No) 
		INNER JOIN 
            SubTeam (nolock) 
            ON Sales_SumByItem.SubTeam_No = SubTeam.SubTeam_No             
		INNER JOIN 
            Item (nolock)  
            ON Item.Item_Key = Sales_SumByItem.Item_Key
	WHERE
		Date_Key >= @BeginDateDt AND Date_Key <= @EndDateDt  
        AND Sales_Account IS NULL 
        AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No
        AND ISNULL(@ZoneID, Store.Zone_ID) = Store.Zone_ID 
	GROUP BY Store.Store_Name, SubTeam.SubTeam_No, SubTeam.SubTeam_Name 
	ORDER BY Store.Store_Name, SubTeam.SubTeam_No

DECLARE @tmpSalesLY table (Store_Name varchar(50), 
                           SubTeam_No int, 
                           SubTeam_Name varchar(100), 
                           Sales decimal(9, 2) )

INSERT INTO @tmpSalesLY 
	SELECT Store.Store_Name,
        SubTeam.SubTeam_No, 
        SubTeam.SubTeam_Name, 
		SUM(Sales_SumByItem.Sales_Amount) + SUM(Sales_SumByItem.Return_Amount) 
		    + SUM(Sales_SumByItem.Markdown_Amount) + SUM(Sales_SumByItem.Promotion_Amount) As Sales	
	FROM Sales_SumByItem (nolock) 
		INNER JOIN 
            Store (nolock) 
            ON Store.Store_No = Sales_SumByItem.Store_No 
		INNER JOIN 
            @StoreSubTeam  SST
            ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No 
			    AND Sales_SumByItem.Store_No = SST.Store_No) 
		INNER JOIN 
            SubTeam (nolock) 
            ON Sales_SumByItem.SubTeam_No = SubTeam.SubTeam_No             
		INNER JOIN 
            Item (nolock)  
            ON Item.Item_Key = Sales_SumByItem.Item_Key
	WHERE
		Date_Key >= DATEADD(day, -364, @BeginDateDt) AND Date_Key <= DATEADD(day, -364, @EndDateDt)
        AND Item.Sales_Account IS NULL 
        AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No 
        AND ISNULL(@ZoneID, Store.Zone_ID) = Store.Zone_ID 
	GROUP BY Store.Store_Name, SubTeam.SubTeam_No, SubTeam.SubTeam_Name 
	ORDER BY SubTeam.SubTeam_Name , SubTeam.SubTeam_No

    SELECT TY.Store_Name As Store_Name, 
        TY.SubTeam_Name, 
        TY.Sales As TY_Sales, 
	    LY.Sales As LY_Sales, 
	    CASE WHEN ISNULL(LY.Sales, 0) = 0
            THEN 0 
            ELSE CAST((TY.Sales / LY.Sales - 1)*100 As decimal(9,1))
		END As Diff
    FROM @tmpSalesTY TY 
        LEFT JOIN 
            @tmpSalesLY LY 
            ON TY.SubTeam_No = LY.SubTeam_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


