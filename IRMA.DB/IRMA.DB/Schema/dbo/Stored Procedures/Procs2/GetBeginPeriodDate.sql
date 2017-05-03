CREATE PROCEDURE dbo.GetBeginPeriodDate
    @InDate datetime,
    @BP_Date datetime OUTPUT

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @BP_Date = dbo.fn_PeriodBeginDate(@InDate)
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDate] TO [IRMAReportsRole]
    AS [dbo];

