CREATE PROCEDURE dbo.Reporting_AdjustmentSummary
    @Store_No int,
    @SubTeam_No int,
    @BeginDate smalldatetime,
    @EndDate smalldatetime
AS
BEGIN
    SET NOCOUNT ON

    SELECT @Store_No, 
           (SELECT ISNULL(Vendor.CompanyName, '') FROM Vendor (nolock) WHERE Store_No = @Store_No) as StoreName, 
           ItemHistory.Item_Key, Quantity, Weight, 
           ISNULL(dbo.fn_AvgCostHistory(ItemHistory.Item_Key, ItemHistory.Store_No, ItemHistory.SubTeam_No, ItemHistory.DateStamp), 0) As AvgCost,
           Item_Description, Identifier, 
           (SELECT SubTeam_Name FROM SubTeam (nolock) WHERE SubTeam_No = ISNULL(@SubTeam_No, ItemHistory.SubTeam_No)) As SubTeam_Name, 
           Adjustment_ID, AdjustmentReason
    FROM ItemHistory (nolock) 
    INNER JOIN
        Item (nolock)
        ON ItemHistory.Item_Key = Item.Item_Key
    INNER JOIN 
        ItemIdentifier (nolock) 
        ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    WHERE DateStamp >= @BeginDate AND DateStamp < DATEADD(d,1,@EndDate) AND
          ItemHistory.SubTeam_No = ISNULL(@SubTeam_No, ItemHistory.SubTeam_No) AND 
          ItemHistory.Store_No = @Store_No AND
          ItemHistory.Adjustment_ID IN (1, 8, 14)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_AdjustmentSummary] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_AdjustmentSummary] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_AdjustmentSummary] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_AdjustmentSummary] TO [IRMAReportsRole]
    AS [dbo];

