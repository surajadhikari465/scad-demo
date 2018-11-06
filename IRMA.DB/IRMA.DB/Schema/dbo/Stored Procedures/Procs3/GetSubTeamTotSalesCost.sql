CREATE PROCEDURE dbo.GetSubTeamTotSalesCost
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @SubTeam_No int,
    @Store_No int
AS
BEGIN

/*
DECLARE @BeginDate varchar(10),
        @EndDate varchar(10),
        @SubTeam_No int,
        @Store_No int
SELECT @BeginDate = '20050504',
        @EndDate = '20050510',
        @SubTeam_No = 4200,
        @Store_No = 101
*/
    SET NOCOUNT ON
    
    DECLARE @TmpItem TABLE(Item_Key int primary key, Weight_Unit bit, Package_Desc1 Decimal(9, 4), SubTeamAvgCost numeric(9,4), SubTeamSales numeric(9,4))
    
    INSERT INTO @TmpItem 
    SELECT I.Item_Key, IU.Weight_Unit, I.Package_Desc1, null, null
    FROM Item I (NOLOCK)
        INNER JOIN
            Sales_SumByItem SSI (NOLOCK)
            ON SSI.Item_Key = I.Item_KEy
        INNER JOIN
            ItemUnit IU (NOLOCK)
            ON IU.Unit_ID = I.Retail_Unit_ID
    WHERE SSI.SubTeam_No = ISNULL(@SubTeam_No, SSI.Subteam_No)
          AND Date_Key >= CONVERT(smalldatetime, @BeginDate) 
          AND Date_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))
    GROUP BY I.Item_Key, IU.Weight_Unit, I.Package_Desc1


 -- The next field is calculated SalesQuantity * AvgCost
    
    SELECT ISNULL(SUM(SF.Sales_Amount - SF.Return_Amount - SF.Markdown_Amount - SF.Promotion_Amount), 0) As SubTeamSales,
           ISNULL(SUM(SF.AvgCost * dbo.Fn_ItemSalesQty('', SF.Weight_Unit, SF.Price_Level, SF.Sales_Quantity, 
                                                 SF.Return_Quantity, SF.Package_Desc1, SF.Weight)), 0) As SubTeamAvgCost 

    FROM (SELECT TI.*, SSI.Sales_Quantity, SSI.Return_Quantity, SSI.Weight, SSI.Price_Level,
                 SSI.Sales_Amount, SSI.Return_Amount, SSI.MarkDown_Amount, SSI.Promotion_Amount,
                 ISNULL(dbo.fn_AvgCostHistory(SSI.Item_Key, SSI.Store_No, 
                                              SSI.SubTeam_No, SSI.Date_Key), 0) As AvgCost
	     FROM Sales_SumByItem SSI (nolock)
             INNER JOIN
                @TmpItem TI
                ON TI.Item_Key = SSI.Item_Key
         WHERE SSI.Store_No = ISNULL(@Store_No, SSI.Store_No)
               AND Date_Key >= CONVERT(smalldatetime, @BeginDate) 
               AND Date_Key < DATEADD(day, 1, CONVERT(smalldatetime, @EndDate))
         ) SF 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamTotSalesCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamTotSalesCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamTotSalesCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamTotSalesCost] TO [IRMAReportsRole]
    AS [dbo];

