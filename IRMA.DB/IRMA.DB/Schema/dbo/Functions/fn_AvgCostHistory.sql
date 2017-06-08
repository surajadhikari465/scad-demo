CREATE FUNCTION dbo.fn_AvgCostHistory 
	(@Item_Key int, 
	 @Store_No int,
     @SubTeam_No int,
     @Effective_Date datetime)
RETURNS smallmoney
AS
BEGIN
    DECLARE @AvgCost smallmoney

        SELECT @AvgCost = CASE WHEN Multiple > 0 THEN Price / Multiple ELSE Price END * ISNULL(CostFactor, 0)
        FROM Price (nolock)
        INNER JOIN
            StoreSubTeam (nolock)
            ON StoreSubTeam.Store_No = Price.Store_No AND StoreSubTeam.SubTeam_No = @SubTeam_No
        WHERE Price.Item_Key = @Item_Key AND Price.Store_No = @Store_No

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

