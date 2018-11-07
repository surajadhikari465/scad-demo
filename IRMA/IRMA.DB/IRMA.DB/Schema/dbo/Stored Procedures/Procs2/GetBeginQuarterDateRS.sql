CREATE PROCEDURE dbo.GetBeginQuarterDateRS
    @Date datetime = null
AS

BEGIN
    SET NOCOUNT ON

	DECLARE @Error_No int, @QuarterBeginDate datetime
    SELECT @Error_No = 0
	        

    SELECT @QuarterBeginDate = dbo.fn_QuarterBeginDate(ISNULL(@Date, GETDATE()))
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        SET NOCOUNT OFF
        RAISERROR ('GetBeginQuarterDateRS, GetBeginPeriodDate, failed with Error = %d', 16, 1, @Error_No)
        RETURN
    END
    ELSE
        SELECT @QuarterBeginDate As QuarterBeginDate

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginQuarterDateRS] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginQuarterDateRS] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginQuarterDateRS] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBeginQuarterDateRS] TO [IRMAReportsRole]
    AS [dbo];

