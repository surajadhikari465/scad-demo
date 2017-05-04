CREATE FUNCTION dbo.fn_AvgCostHistory 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int,
     @Effective_Date datetime)
RETURNS smallmoney
AS
BEGIN
    DECLARE @AvgCost smallmoney

    SELECT @AvgCost =   (SELECT TOP 1 AvgCost
                         FROM AvgCostHistory (nolock)
                         WHERE Item_Key = @Item_Key
                             AND Store_No = @Store_No
                             AND SubTeam_No = @SubTeam_No
                             AND Effective_Date <= @Effective_Date
                         ORDER BY Effective_Date DESC)

    IF ISNULL(@AvgCost, 0) <= 0
    BEGIN
        SELECT @AvgCost = CASE WHEN Multiple > 0 THEN Price / Multiple ELSE Price END * ISNULL(CostFactor, 0)
        FROM Price (nolock)
        INNER JOIN
            StoreSubTeam (nolock)
            ON StoreSubTeam.Store_No = Price.Store_No AND StoreSubTeam.SubTeam_No = @SubTeam_No
        WHERE Price.Item_Key = @Item_Key AND Price.Store_No = @Store_No
    END

	SELECT @AvgCost = ISNULL(@AvgCost, 0)

    RETURN @AvgCost
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_AvgCostHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_AvgCostHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_AvgCostHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_AvgCostHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_AvgCostHistory] TO [IRMAReportsRole]
    AS [dbo];

