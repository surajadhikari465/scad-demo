CREATE FUNCTION dbo.fn_GetCurrentAvgCost 
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

    RETURN @AvgCost
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentAvgCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentAvgCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentAvgCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentAvgCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentAvgCost] TO [IRMAReportsRole]
    AS [dbo];

