Create  FUNCTION dbo.fn_IsAvgCostAdjReasonActive
	(@ID int
)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
    
    SELECT @return = Active FROM AvgCostAdjReason WHERE ID = @ID
        
	RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsAvgCostAdjReasonActive] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsAvgCostAdjReasonActive] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsAvgCostAdjReasonActive] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsAvgCostAdjReasonActive] TO [IRMAReportsRole]
    AS [dbo];

