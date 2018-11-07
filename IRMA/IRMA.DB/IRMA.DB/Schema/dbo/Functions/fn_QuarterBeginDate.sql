CREATE FUNCTION dbo.fn_QuarterBeginDate 
	(@InDate datetime)
RETURNS datetime
AS
BEGIN
    DECLARE @Result datetime

    SELECT @Result = MIN(Date_Key)
    FROM Date
    INNER JOIN (SELECT Year, Quarter 
                FROM dbo.Date As Date
                WHERE Date_Key = CONVERT(smalldatetime, CONVERT(char(10), @InDate, 101))) T1
        ON T1.Year = Date.Year AND T1.Quarter = Date.Quarter

    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_QuarterBeginDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_QuarterBeginDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_QuarterBeginDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_QuarterBeginDate] TO [IRMAReportsRole]
    AS [dbo];

