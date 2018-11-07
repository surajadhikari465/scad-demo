CREATE PROCEDURE dbo.Replenishment_Tlog_House_CheckTlogExists
	@storeNo int,
    @tlogDate datetime
AS

BEGIN
	SET NOCOUNT ON;
	
	SELECT Tlog_Count = COUNT(*)
      FROM dbo.Sales_SumByItem (NOLOCK)
     WHERE Store_No = @storeNo
       AND Date_Key = CONVERT(date, @tlogDate)
     GROUP BY Date_Key, Store_No
		
	SET NOCOUNT OFF;		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_CheckTlogExists] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_CheckTlogExists] TO [IRMASchedJobsRole]
    AS [dbo];

