CREATE PROCEDURE dbo.GetDaysInFiscalMonth
@Month int,
@Year int
AS

SELECT Date.Day_Of_Month, Date.Date_Key 
FROM Date (NOLOCK) 
WHERE Period = @Month AND Year = @Year
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDaysInFiscalMonth] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDaysInFiscalMonth] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDaysInFiscalMonth] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDaysInFiscalMonth] TO [IRMAReportsRole]
    AS [dbo];

