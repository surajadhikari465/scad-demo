CREATE PROCEDURE dbo.GetAvgCostAdjustmentReasons

	@Filter bit,
	@Active bit

AS 

DECLARE @SQL varchar(MAX)

SELECT @SQL = 'SELECT ID, Description, Active FROM	AvgCostAdjReason '

IF @Filter = 1
	BEGIN
		SELECT @SQL = @SQL + 'WHERE Active = ' + CAST(@Active As varchar)
	END

SELECT @SQL = @SQL + + ' ORDER BY Description '
	
EXEC (@SQL)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostAdjustmentReasons] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostAdjustmentReasons] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostAdjustmentReasons] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostAdjustmentReasons] TO [IRMAReportsRole]
    AS [dbo];

