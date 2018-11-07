

CREATE PROCEDURE [dbo].[GetDateRangeForCurrentWeek]
AS
BEGIN
	DECLARE @str varchar(128)
	
	Set DATEFIRST 1
		
	DECLARE @StartDate varchar(12)
	SET @StartDate = CONVERT(varchar(12), dateadd(dd, 1-datepart(dw,GETDATE()),GETDATE()), 101)

	DECLARE @EndDate varchar(12)
	SET @EndDate = convert(varchar(12), dateadd("d",6,@StartDate), 101)

	Select @startdate + ' - ' + @enddate as DateRange

END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDateRangeForCurrentWeek] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDateRangeForCurrentWeek] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDateRangeForCurrentWeek] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDateRangeForCurrentWeek] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDateRangeForCurrentWeek] TO [IRMAReportsRole]
    AS [dbo];

