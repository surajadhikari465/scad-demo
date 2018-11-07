CREATE PROCEDURE dbo.GetBeginFiscalYearDate
    @InDate datetime,
    @BP_Date datetime OUTPUT

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @BP_Date = dbo.fn_FiscalYearBeginDate(@InDate)
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDate] TO [IRMAReportsRole]
    AS [dbo];

