CREATE PROCEDURE dbo.GetBeginPeriodDateRS
    @Date datetime = null

AS

BEGIN
    SET NOCOUNT ON

	DECLARE @Error_No int, @PeriodBeginDate datetime
    SELECT @Error_No = 0
	        

    SELECT @PeriodBeginDate = dbo.fn_PeriodBeginDate(ISNULL(@Date, GETDATE()))
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        SET NOCOUNT OFF
        RAISERROR ('GetBeginPeriodDateRS, GetBeginPeriodDate, failed with Error = %d', 16, 1, @Error_No)
        RETURN
    END
    ELSE
        SELECT @PeriodBeginDate As PeriodBeginDate

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDateRS] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDateRS] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDateRS] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginPeriodDateRS] TO [IRMAReportsRole]
    AS [dbo];

