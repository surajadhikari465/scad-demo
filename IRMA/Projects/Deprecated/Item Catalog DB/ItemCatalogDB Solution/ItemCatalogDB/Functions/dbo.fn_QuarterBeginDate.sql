IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_QuarterBeginDate')
	DROP FUNCTION fn_QuarterBeginDate
GO

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