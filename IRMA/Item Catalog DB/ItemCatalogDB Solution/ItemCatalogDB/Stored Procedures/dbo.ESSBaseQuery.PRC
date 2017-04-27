SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ESSBaseQuery]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ESSBaseQuery]
GO


CREATE PROCEDURE dbo.ESSBaseQuery
    @RunDate datetime
AS 
BEGIN
    SET NOCOUNT ON

    DECLARE @DaysBack as int

    SET DATEFIRST 1

    SELECT @DaysBack = -1 * (DATEPART(dw, ISNULL(@RunDate, GETDATE())) + 6)

    SELECT Store.BusinessUnit_ID AS Unit, Convert(varchar(4),StoreSubTeam.Team_No) AS Dept, Convert(varchar(4),StoreSubTeam.SubTeam_No) AS Prod, '400000' AS Account, 
           CONVERT(CHAR(10), Date_Key, 101) AS Trans_Date,
           -1 * (SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount)) AS Amount	  
    FROM 
        Store(nolock)
    INNER JOIN
        Sales_SumByItem (nolock)
        ON Store.Store_No = Sales_SumByItem.Store_No 
    INNER JOIN
        StoreSubTeam (nolock)
        ON Sales_SumByItem.SubTeam_No = StoreSubTeam.SubTeam_No AND Sales_SumByItem.Store_No = StoreSubTeam.Store_No
    INNER JOIN 
	    Item (nolock) 
        ON Sales_SumByItem.Item_Key = Item.Item_Key 
    WHERE Date_Key >= CONVERT(CHAR(10),DATEADD(DAY,@DaysBack,GetDate()),101) AND 
          Date_Key < CONVERT(CHAR(10),DATEADD(DAY,@DaysBack+7,GetDate()),101) AND
          Sales_Account IS NULL AND Sales_SumByItem.SubTeam_No NOT LIKE '9%'
    GROUP BY Store.BusinessUnit_ID, Convert(varchar(4),StoreSubTeam.Team_No), Convert(varchar(4),StoreSubTeam.SubTeam_No), 
             CONVERT(CHAR(10), Date_Key, 101), Store.Store_Name
    UNION
    SELECT Store.BusinessUnit_ID AS Unit, 'No_Team' AS Dept, 'No_SubTeam' AS Prod, '950000' AS Account,
           CONVERT(CHAR(10), Date_Key, 101) AS Trans_Date,
           SUM(Transaction_Count) AS Amount
    FROM 
        Store (nolock)
    INNER JOIN 
        Buggy_SumByRegister(nolock) 
        ON (Store.Store_No = Buggy_SumByRegister.Store_No)
    WHERE Date_Key >= CONVERT(CHAR(10),DATEADD(DAY,@DaysBack,GetDate()),101) AND 
          Date_Key < CONVERT(CHAR(10),DATEADD(DAY,@DaysBack+7,GetDate()),101)
    GROUP BY Store.BusinessUnit_ID, CONVERT(CHAR(10), Date_Key, 101), Store.Store_Name
    UNION
    SELECT Store.BusinessUnit_ID AS Unit, 'No_Team' AS Dept, 'No_SubTeam' AS Prod, '951000' AS Account,
           CONVERT(CHAR(10), Date_Key, 101) AS Trans_Date,
           SUM(Sales_Quantity) - SUM(Return_Quantity) AS Amount
    FROM 
        Store (nolock)
    INNER JOIN
        Sales_SumByItem (nolock)
        ON Store.Store_No = Sales_SumByItem.Store_No 
    INNER JOIN
        StoreSubTeam (nolock)
        ON Sales_SumByItem.SubTeam_No = StoreSubTeam.SubTeam_No AND Sales_SumByItem.Store_No = StoreSubTeam.Store_No
    INNER JOIN 
	    Item (nolock) 
        ON Sales_SumByItem.Item_Key = Item.Item_Key 
    WHERE Date_Key >= CONVERT(CHAR(10),DATEADD(DAY,@DaysBack,GetDate()),101) AND 
          Date_Key < CONVERT(CHAR(10),DATEADD(DAY,@DaysBack+7,GetDate()),101) AND
          Sales_Account IS NULL AND Sales_SumByItem.SubTeam_No NOT LIKE '9%'
    GROUP BY Store.BusinessUnit_ID, CONVERT(CHAR(10), Date_Key, 101), Store.Store_Name
    ORDER BY Unit, Dept, Prod, Account, Trans_date

    SET DATEFIRST 1

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

