CREATE PROCEDURE dbo.GetPeriodDates 
    @Year int,
    @Quarter int,
    @Period int,
    @Week int,
    @BeginDate datetime OUTPUT,
    @EndDate datetime OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @BeginDate = MIN(Date_Key)
    FROM Date (NOLOCK)
    WHERE Year = ISNULL(@Year, Year)
    AND Quarter = ISNULL(@Quarter, Quarter)
    AND Period = ISNULL(@Period, Period)
    AND Week = ISNULL(@Week, Week)

    SELECT @EndDate = MAX(Date_Key)
    FROM Date (NOLOCK)
    WHERE Year = ISNULL(@Year, Year)
    AND Quarter = ISNULL(@Quarter, Quarter)
    AND Period = ISNULL(@Period, Period)
    AND Week = ISNULL(@Week, Week)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPeriodDates] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPeriodDates] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPeriodDates] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPeriodDates] TO [IRMAReportsRole]
    AS [dbo];

