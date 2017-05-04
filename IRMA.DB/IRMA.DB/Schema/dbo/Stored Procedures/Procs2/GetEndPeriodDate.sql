﻿CREATE PROCEDURE dbo.GetEndPeriodDate
    @InDate datetime,
    @EP_Date datetime OUTPUT

AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @Error_No int, @Work_Date datetime
    
    SELECT @Error_No = 0
       
    SELECT @Work_Date = MAX(Date_Key)
    FROM Date
    INNER JOIN (SELECT Year, Period 
                FROM dbo.Date As Date
                WHERE Date_Key = CONVERT(smalldatetime, CONVERT(char(10), @InDate, 101))) T1
        ON T1.Year = Date.Year AND T1.Period = Date.Period
        
    SELECT @Error_No = @@ERROR
        
    IF @Error_No = 0
    BEGIN
        SELECT @EP_Date = DATEADD(mi, -1, DATEADD(day, 1, @Work_Date))
        SELECT @Error_No = @@ERROR
    END
    
    IF (@Error_No <> 0)
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        SET NOCOUNT OFF
        RAISERROR ('GetEndPeriodDate failed with Error = %d', @Severity, 1, @Error_No)
        RETURN
    END
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetEndPeriodDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetEndPeriodDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetEndPeriodDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetEndPeriodDate] TO [IRMAReportsRole]
    AS [dbo];

