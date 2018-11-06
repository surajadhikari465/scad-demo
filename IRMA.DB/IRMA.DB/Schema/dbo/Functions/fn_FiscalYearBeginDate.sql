CREATE FUNCTION dbo.fn_FiscalYearBeginDate 
	(@InDate datetime)
RETURNS datetime
AS
BEGIN
    DECLARE @Result datetime

    SELECT @Result = MIN(Date.Date_Key)
    FROM Date
    INNER JOIN (SELECT Date_Key 
                FROM dbo.Date As Date
                WHERE Year = Year(@InDate) AND Week = 1 AND Quarter = 1) T1
        ON T1.Date_Key = Date.Date_Key
    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_FiscalYearBeginDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_FiscalYearBeginDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_FiscalYearBeginDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_FiscalYearBeginDate] TO [IRMAReportsRole]
    AS [dbo];

