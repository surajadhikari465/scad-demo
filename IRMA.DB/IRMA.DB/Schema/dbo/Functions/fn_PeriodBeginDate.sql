CREATE FUNCTION dbo.fn_PeriodBeginDate 
	(@InDate datetime)
RETURNS datetime
AS
BEGIN
    DECLARE @Result datetime

    SELECT @Result = MIN(Date_Key)
    FROM Date
    INNER JOIN (SELECT Year, Period 
                FROM dbo.Date As Date
                WHERE Date_Key = CONVERT(smalldatetime, CONVERT(char(10), @InDate, 101))) T1
        ON T1.Year = Date.Year AND T1.Period = Date.Period

    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PeriodBeginDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PeriodBeginDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PeriodBeginDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PeriodBeginDate] TO [IRMAReportsRole]
    AS [dbo];

