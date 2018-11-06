CREATE PROCEDURE dbo.GetBeginFiscalYearDateRS
    @Date datetime = null
AS

BEGIN
    SET NOCOUNT ON

	DECLARE @Error_No int, @FiscalYearBeginDate datetime
    SELECT @Error_No = 0
	        

    SELECT @FiscalYearBeginDate = dbo.fn_FiscalYearBeginDate(ISNULL(@Date, GETDATE()))
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        SET NOCOUNT OFF
        RAISERROR ('GetBeginFiscalYearDateRS, GetBeginPeriodDate, failed with Error = %d', 16, 1, @Error_No)
        RETURN
    END
    ELSE
        SELECT @FiscalYearBeginDate As FiscalYearBeginDate

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDateRS] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDateRS] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDateRS] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginFiscalYearDateRS] TO [IRMAReportsRole]
    AS [dbo];

